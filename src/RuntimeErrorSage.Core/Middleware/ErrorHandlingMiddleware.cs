using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Application.Runtime.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Extensions;

namespace RuntimeErrorSage.Application.Middleware;

/// <summary>
/// Middleware for handling errors in the application.
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IRuntimeErrorSageService _RuntimeErrorSageService;
    private readonly IMCPClient _mcpClient;
    private readonly IRemediationMetricsCollector _metricsCollector;
    private readonly ActivitySource _activitySource;
    private readonly ErrorHandlingMiddlewareOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
    /// </summary>
    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IRuntimeErrorSageService RuntimeErrorSageService,
        IMCPClient mcpClient,
        IRemediationMetricsCollector metricsCollector,
        IOptions<ErrorHandlingMiddlewareOptions> options)
    {
        _next = next;
        _logger = logger;
        _RuntimeErrorSageService = RuntimeErrorSageService;
        _mcpClient = mcpClient;
        _metricsCollector = metricsCollector;
        _activitySource = new ActivitySource("RuntimeErrorSage.ErrorHandling");
        _options = options.Value;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;
        var method = context.Request.Method.ToUpperInvariant();

        // Skip excluded paths
        if (IsExcludedPath(path, method))
        {
            await _next(context);
            return;
        }

        // Start activity for request tracking
        using var activity = _activitySource.StartActivity(
            $"{method} {path}",
            ActivityKind.Server);

        // Add correlation ID
        if (context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationIds) &&
            correlationIds.Count > 0)
        {
            var correlationId = correlationIds[0];
            context.Response.Headers["X-Correlation-ID"] = correlationId;
            activity?.SetTag("correlation.id", correlationId);
        }

        // Add request context
        var requestContext = new RequestContext
        {
            Path = path,
            Method = method,
            QueryString = context.Request.QueryString.ToString(),
            Headers = context.Request.Headers
                .Where(h => !h.Key.StartsWith("Authorization", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(h => h.Key, h => h.Value.ToString()),
            Timestamp = DateTime.UtcNow
        };

        try
        {
            // Start performance monitoring
            var stopwatch = Stopwatch.StartNew();
            activity?.SetTag("request.start", DateTime.UtcNow);

            // Process request
            await _next(context);

            // Record successful request
            stopwatch.Stop();
            activity?.SetTag("request.duration", stopwatch.ElapsedMilliseconds);
            activity?.SetTag("request.status", context.Response.StatusCode);
            activity?.SetStatus(ActivityStatusCode.Ok);

            if (context.Response.StatusCode < 500)
            {
                if (context.Response.StatusCode == 400)
                {
                    _logger.LogWarning("Request resulted in BadRequest. CorrelationId: {CorrelationId}",
                        activity?.GetTag("correlation.id"));
                }
                else
                {
                    _logger.LogInformation("Request succeeded. CorrelationId: {CorrelationId}",
                        activity?.GetTag("correlation.id"));
                }

                if (context.Response.StatusCode == 200)
                {
                    await RecordRequestMetricsAsync(context, stopwatch.Elapsed, null);
                }
            }
            else
            {
                _logger.LogError("Request resulted in InternalServerError. CorrelationId: {CorrelationId}",
                    activity?.GetTag("correlation.id"));
                await HandleExceptionAsync(context, null);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred. CorrelationId: {CorrelationId}",
                activity?.GetTag("correlation.id"));
            await HandleExceptionAsync(context, ex);
        }
    }

    private bool IsExcludedPath(string path, string method)
    {
        return _options.ExcludedPaths.TryGetValue(path, out var methods) &&
            methods.Contains(method);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var correlationId = Guid.NewGuid().ToString();
        var runtimeError = ex != null ? new RuntimeError(ex.Message, ex.GetType().Name, ex.StackTrace) : null;
        
        var errorContext = new ErrorContext(
            runtimeError,
            context.Request.Path,
            DateTime.UtcNow)
        {
            ServiceName = "UnknownService", // TODO: Get service name from config/context
            OperationName = context.Request.Path,
            CorrelationId = correlationId,
            AdditionalContext = new Dictionary<string, string>
            {
                { "HttpMethod", context.Request.Method },
                { "RequestPath", context.Request.Path.ToString() }
                // Add more relevant context here
            }
        };
        
        // Set severity
        errorContext.Severity = DetermineSeverity(ex);
        
        // Set environment and user
        errorContext.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown";
        errorContext.ContextData["UserId"] = context.User?.Identity?.Name ?? "Anonymous";

        // Analyze the error using RuntimeErrorSageService
        var analysisResult = await _RuntimeErrorSageService.ProcessExceptionAsync(ex, errorContext);

        // Optionally attempt automated remediation (within paper scope for simple cases)
        Domain.Models.Remediation.RemediationResult? remediationResult = null;
        if (analysisResult.SuggestedActions.Any() && analysisResult.Confidence > 0.7) // Example condition for auto-remediation
        {
            remediationResult = await _RuntimeErrorSageService.ApplyRemediationAsync(analysisResult);
        }

        // Log the final outcome
        _logger.LogInformation("Error handled. CorrelationId: {CorrelationId}, RootCause: {RootCause}, RemediationStatus: {RemediationStatus}",
            correlationId,
            analysisResult.RootCause,
            remediationResult?.Status.ToString() ?? "Not Attempted");

        // Prepare and send the response
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new ErrorResponse
        {
            ErrorId = correlationId,
            Message = "An internal server error occurred.",
            Type = ex?.GetType().Name ?? "Unknown",
            Severity = DetermineSeverity(ex),
            Analysis = analysisResult.RootCause ?? "Unknown",
            Remediation = remediationResult?.Status.ToString(),
            Timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
    }

    private SeverityLevel DetermineSeverity(Exception ex)
    {
        return ex switch
        {
            System.Data.SqlClient.SqlException sqlEx when sqlEx.Number == -2 => SeverityLevel.Critical, // Timeout
            System.Data.SqlClient.SqlException => SeverityLevel.High,
            HttpRequestException => SeverityLevel.High,
            UnauthorizedAccessException => SeverityLevel.High,
            System.IO.IOException => SeverityLevel.High,
            OutOfMemoryException => SeverityLevel.Critical,
            NullReferenceException => SeverityLevel.High,
            ArgumentException => SeverityLevel.Medium,
            _ => SeverityLevel.High
        };
    }

    private async Task RecordRequestMetricsAsync(
        HttpContext context,
        TimeSpan? duration,
        ErrorContext? errorContext)
    {
        try
        {
            var remediationId = Guid.NewGuid().ToString(); // Generate a unique ID for this request
            
            // Record each metric individually using RecordMetricAsync
            await _metricsCollector.RecordMetricAsync(remediationId, "request.path", context.Request.Path.ToString());
            await _metricsCollector.RecordMetricAsync(remediationId, "request.method", context.Request.Method);
            await _metricsCollector.RecordMetricAsync(remediationId, "request.status", context.Response.StatusCode);
            await _metricsCollector.RecordMetricAsync(remediationId, "request.timestamp", DateTime.UtcNow);

            if (duration.HasValue)
            {
                await _metricsCollector.RecordMetricAsync(remediationId, "request.duration", duration.Value.TotalMilliseconds);
            }

            if (errorContext != null)
            {
                await _metricsCollector.RecordMetricAsync(remediationId, "error.type", errorContext.ErrorType);
                await _metricsCollector.RecordMetricAsync(remediationId, "error.severity", errorContext.SeverityLevel.ToString());
                await _metricsCollector.RecordMetricAsync(remediationId, "error.timestamp", errorContext.Timestamp);

                if (errorContext.AnalysisResult != null)
                {
                    await _metricsCollector.RecordMetricAsync(remediationId, "error.analysis", errorContext.AnalysisResult.RootCause ?? "Unknown");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to record request metrics");
        }
    }
} 

