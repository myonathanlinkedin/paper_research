using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;
using CodeSage.Core.Interfaces;
using CodeSage.Core.Models;
using CodeSage.Core.Models.Error;
using CodeSage.Core.Interfaces.MCP;
using CodeSage.Core.Remediation.Interfaces;

namespace CodeSage.Core.Middleware;

public class ErrorHandlingMiddlewareOptions
{
    public bool EnableErrorAnalysis { get; set; } = true;
    public bool EnableAutomatedRemediation { get; set; } = true;
    public bool EnableRequestCorrelation { get; set; } = true;
    public bool EnablePerformanceMonitoring { get; set; } = true;
    public TimeSpan AnalysisTimeout { get; set; } = TimeSpan.FromSeconds(30);
    public TimeSpan RemediationTimeout { get; set; } = TimeSpan.FromMinutes(5);
    public Dictionary<string, string[]> ExcludedPaths { get; set; } = new()
    {
        ["/health"] = new[] { "GET" },
        ["/metrics"] = new[] { "GET" },
        ["/swagger"] = new[] { "GET" }
    };
}

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly ICodeSageService _codeSageService;
    private readonly IMCPClient _mcpClient;
    private readonly IRemediationMetricsCollector _metricsCollector;
    private readonly ActivitySource _activitySource;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        ICodeSageService codeSageService,
        IMCPClient mcpClient,
        IRemediationMetricsCollector metricsCollector)
    {
        _next = next;
        _logger = logger;
        _codeSageService = codeSageService;
        _mcpClient = mcpClient;
        _metricsCollector = metricsCollector;
        _activitySource = new ActivitySource("CodeSage.ErrorHandling");
    }

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
        var errorContext = new ErrorContext
        {
            ServiceName = "UnknownService", // TODO: Get service name from config/context
            OperationName = context.Request.Path,
            CorrelationId = correlationId,
            Timestamp = DateTime.UtcNow,
            Exception = ex,
            AdditionalContext = new Dictionary<string, object>
            {
                { "HttpMethod", context.Request.Method },
                { "RequestPath", context.Request.Path }
                // Add more relevant context here
            },
            Severity = DetermineSeverity(ex),
            UserId = context.User?.Identity?.Name, // Get authenticated user ID
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        };

        // Analyze the error using CodeSageService
        var analysisResult = await _codeSageService.ProcessExceptionAsync(ex, errorContext);

        // Optionally attempt automated remediation (within paper scope for simple cases)
        RemediationResult? remediationResult = null;
        if (analysisResult.SuggestedActions.Any() && analysisResult.Confidence > 0.7) // Example condition for auto-remediation
        {
            remediationResult = await _codeSageService.ApplyRemediationAsync(analysisResult);
        }

        // Log the final outcome
        _logger.LogInformation("Error handled. CorrelationId: {CorrelationId}, RootCause: {RootCause}, RemediationStatus: {RemediationStatus}",
            correlationId,
            analysisResult.RootCause,
            remediationResult?.Status.ToString() ?? "Not Attempted");

        // Prepare and send the response
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new
        {
            CorrelationId = correlationId,
            Message = "An internal server error occurred.",
            Details = ex?.Message,
            Analysis = analysisResult, // Include analysis result in response
            Remediation = remediationResult // Include remediation result in response
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
    }

    private ErrorSeverity DetermineSeverity(Exception ex)
    {
        // Simple severity mapping (can be expanded)
        return ex switch
        {
            System.Data.SqlClient.SqlException sqlEx when sqlEx.Number == -2 => ErrorSeverity.Critical, // Timeout
            System.Data.SqlClient.SqlException => ErrorSeverity.Error,
            HttpRequestException => ErrorSeverity.Error,
            UnauthorizedAccessException => ErrorSeverity.Error,
            System.IO.IOException => ErrorSeverity.Error,
            OutOfMemoryException => ErrorSeverity.Critical,
            NullReferenceException => ErrorSeverity.Error,
            ArgumentException => ErrorSeverity.Error,
            _ => ErrorSeverity.Error,
        };
    }

    private async Task RecordRequestMetricsAsync(
        HttpContext context,
        TimeSpan? duration,
        ErrorContext? errorContext)
    {
        try
        {
            var metrics = new Dictionary<string, object>
            {
                ["request.path"] = context.Request.Path,
                ["request.method"] = context.Request.Method,
                ["request.status"] = context.Response.StatusCode,
                ["request.timestamp"] = DateTime.UtcNow
            };

            if (duration.HasValue)
            {
                metrics["request.duration"] = duration.Value.TotalMilliseconds;
            }

            if (errorContext != null)
            {
                metrics["error.type"] = errorContext.ErrorType;
                metrics["error.severity"] = errorContext.Severity;
                metrics["error.timestamp"] = errorContext.Timestamp;

                if (errorContext.Analysis != null)
                {
                    metrics["error.analysis"] = errorContext.Analysis.Summary;
                    metrics["error.remediation"] = errorContext.Analysis.RemediationPlan?.Summary;
                }
            }

            await _metricsCollector.CollectMetricsAsync(
                errorContext ?? new ErrorContext
                {
                    ServiceName = context.Request.Host.Value,
                    ErrorType = "RequestMetrics",
                    Severity = ErrorSeverity.Low,
                    Timestamp = DateTime.UtcNow
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording request metrics");
        }
    }
}

public class RequestContext
{
    public string Path { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string QueryString { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = new();
    public DateTime Timestamp { get; set; }
}

public class ErrorResponse
{
    public string ErrorId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public ErrorSeverity Severity { get; set; }
    public string? Analysis { get; set; }
    public string? Remediation { get; set; }
    public DateTime Timestamp { get; set; }
} 