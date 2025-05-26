using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Options;
using RuntimeErrorSage.Core.Services.Interfaces;
using RuntimeErrorSage.Core.Validation;
using RuntimeErrorSage.Core.Graph;
using RuntimeErrorSage.Core.LLM;
using RuntimeErrorSage.Core.MCP;
using RuntimeErrorSage.Core.Models.MCP;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.Exceptions;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.Context;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Services;

/// <summary>
/// Core service that orchestrates error analysis and remediation.
/// Implements the Model Context Protocol (MCP) as described in the research paper.
/// </summary>
public class RuntimeErrorSageService : IRuntimeErrorSageService
{
    private readonly ILogger<RuntimeErrorSageService> _logger;
    private readonly IErrorAnalysisService _errorAnalysisService;
    private readonly IRemediationService _remediationService;
    private readonly IContextEnrichmentService _contextEnrichmentService;
    private readonly IValidationRegistry _validationRegistry;
    private readonly IErrorContextAnalyzer _errorContextAnalyzer;
    private readonly IRemediationAnalyzer _remediationAnalyzer;
    private readonly IRemediationValidator _remediationValidator;
    private readonly IRemediationMetricsCollector _metricsCollector;
    private readonly ModelContextProtocol _mcp;
    private readonly IQwenLLMClient _llmClient;
    private readonly IErrorAnalyzer _errorAnalyzer;
    private readonly IDependencyGraphAnalyzer _graphAnalyzer;
    private readonly Dictionary<string, ErrorAnalysisResult> _analysisCache;
    private readonly Dictionary<string, RemediationResult> _remediationCache;

    public RuntimeErrorSageService(
        ILogger<RuntimeErrorSageService> logger,
        IErrorAnalysisService errorAnalysisService,
        IRemediationService remediationService,
        IContextEnrichmentService contextEnrichmentService,
        IValidationRegistry validationRegistry,
        IErrorContextAnalyzer errorContextAnalyzer,
        IRemediationAnalyzer remediationAnalyzer,
        IRemediationValidator remediationValidator,
        IRemediationMetricsCollector metricsCollector,
        ModelContextProtocol mcp,
        IQwenLLMClient llmClient,
        IErrorAnalyzer errorAnalyzer,
        IDependencyGraphAnalyzer graphAnalyzer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _errorAnalysisService = errorAnalysisService ?? throw new ArgumentNullException(nameof(errorAnalysisService));
        _remediationService = remediationService ?? throw new ArgumentNullException(nameof(remediationService));
        _contextEnrichmentService = contextEnrichmentService ?? throw new ArgumentNullException(nameof(contextEnrichmentService));
        _validationRegistry = validationRegistry ?? throw new ArgumentNullException(nameof(validationRegistry));
        _errorContextAnalyzer = errorContextAnalyzer ?? throw new ArgumentNullException(nameof(errorContextAnalyzer));
        _remediationAnalyzer = remediationAnalyzer ?? throw new ArgumentNullException(nameof(remediationAnalyzer));
        _remediationValidator = remediationValidator ?? throw new ArgumentNullException(nameof(remediationValidator));
        _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
        _mcp = mcp ?? throw new ArgumentNullException(nameof(mcp));
        _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
        _errorAnalyzer = errorAnalyzer ?? throw new ArgumentNullException(nameof(errorAnalyzer));
        _graphAnalyzer = graphAnalyzer ?? throw new ArgumentNullException(nameof(graphAnalyzer));
        _analysisCache = new Dictionary<string, ErrorAnalysisResult>();
        _remediationCache = new Dictionary<string, RemediationResult>();
    }

    /// <inheritdoc />
    public async Task<ErrorAnalysisResult> ProcessExceptionAsync(Exception exception, ErrorContext context)
    {
        try
        {
            _logger.LogInformation("Processing exception: {Message}", exception.Message);

            // Enrich context with additional runtime information
            var enrichedContext = await EnrichContextAsync(context);

            // Analyze the exception
            var analysisResult = await _errorAnalysisService.AnalyzeExceptionAsync(exception, enrichedContext);

            // Validate the analysis result
            var validationResult = await _validationRegistry.ValidateAsync(enrichedContext);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Analysis validation failed: {Errors}", string.Join(", ", validationResult.Errors));
                analysisResult.Status = AnalysisStatus.ValidationFailed;
                analysisResult.Details["ValidationErrors"] = validationResult.Errors;
            }

            return analysisResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing exception");
            throw new RuntimeErrorSageException("Failed to process exception", ex);
        }
    }

    /// <inheritdoc />
    public async Task<RemediationResult> ApplyRemediationAsync(ErrorAnalysisResult analysisResult)
    {
        try
        {
            _logger.LogInformation("Applying remediation for analysis: {AnalysisId}", analysisResult.ErrorId);

            // Get the original context
            var context = analysisResult.Details.GetValueOrDefault("OriginalContext") as ErrorContext;
            if (context == null)
            {
                throw new RuntimeErrorSageException("Original error context not available for remediation");
            }

            // Create remediation plan
            var plan = await _remediationAnalyzer.AnalyzeErrorAsync(context);
            if (!plan.IsValid)
            {
                return new RemediationResult
                {
                    RemediationId = Guid.NewGuid().ToString(),
                    Status = "Failed",
                    Message = "Failed to create valid remediation plan",
                    Timestamp = DateTime.UtcNow
                };
            }

            // Validate remediation plan
            var validationResult = await _remediationValidator.ValidateRemediationAsync(context);
            if (!validationResult.IsValid)
            {
                return new RemediationResult
                {
                    RemediationId = Guid.NewGuid().ToString(),
                    Status = "ValidationFailed",
                    Message = string.Join(", ", validationResult.Errors),
                    Timestamp = DateTime.UtcNow
                };
            }

            // Execute remediation
            var result = await _remediationService.ApplyRemediationAsync(context);

            // Collect metrics
            await _metricsCollector.CollectMetricsAsync(context, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying remediation");
            throw new RuntimeErrorSageException("Failed to apply remediation", ex);
        }
    }

    /// <inheritdoc />
    public async Task<ErrorContext> EnrichContextAsync(ErrorContext context)
    {
        try
        {
            _logger.LogInformation("Enriching error context");

            // Enrich with runtime information
            var enrichedContext = await _contextEnrichmentService.EnrichContextAsync(context);

            // Analyze context graph
            var graphAnalysis = await _errorContextAnalyzer.AnalyzeContextAsync(enrichedContext);
            enrichedContext.Metadata["GraphAnalysis"] = graphAnalysis;

            // Analyze with LLM
            var llmAnalysis = await _llmClient.AnalyzeContextAsync(enrichedContext);
            enrichedContext.Metadata["LLMAnalysis"] = llmAnalysis;

            // Add MCP analysis
            var mcpAnalysis = await _mcp.AnalyzeContextAsync(enrichedContext);
            enrichedContext.Metadata["MCPAnalysis"] = mcpAnalysis;

            return enrichedContext;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enriching context");
            throw new RuntimeErrorSageException("Failed to enrich error context", ex);
        }
    }

    /// <inheritdoc />
    public void RegisterRemediationStrategy(IRemediationStrategy strategy)
    {
        try
        {
            _logger.LogInformation("Registering remediation strategy: {StrategyName}", strategy.Name);
            _remediationService.RegisterStrategy(strategy);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering remediation strategy");
            throw new RuntimeErrorSageException("Failed to register remediation strategy", ex);
        }
    }

    /// <inheritdoc />
    public void Configure(RuntimeErrorSageOptions options)
    {
        try
        {
            _logger.LogInformation("Configuring RuntimeErrorSage service");
            // Configure services with options
            _errorAnalysisService.Configure(options);
            _remediationService.Configure(options);
            _contextEnrichmentService.Configure(options);
            _validationRegistry.Configure(options);
            _errorContextAnalyzer.Configure(options);
            _remediationAnalyzer.Configure(options);
            _remediationValidator.Configure(options);
            _metricsCollector.Configure(options);
            _mcp.Configure(options);
            _llmClient.Configure(options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error configuring service");
            throw new RuntimeErrorSageException("Failed to configure RuntimeErrorSage service", ex);
        }
    }

    /// <inheritdoc />
    public async Task<ErrorAnalysisResult> AnalyzeErrorAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Analyzing error context: {ContextId}", context.Id);

            // Check cache first
            if (_analysisCache.TryGetValue(context.Id, out var cachedResult))
            {
                return cachedResult;
            }

            var startTime = DateTime.UtcNow;
            var metrics = new PerformanceMetrics();

            // Perform error analysis
            var analysisResult = await _errorAnalyzer.AnalyzeErrorAsync(context.Exception, context);

            // Perform graph analysis
            var graphResult = await _graphAnalyzer.AnalyzeContextAsync(context);

            // Calculate metrics
            metrics.TotalProcessingTime = DateTime.UtcNow - startTime;
            metrics.PhaseResourceUsage = await _metricsCollector.CollectMetricsAsync();

            // Combine results
            var result = new ErrorAnalysisResult
            {
                ErrorId = context.Id,
                Timestamp = DateTime.UtcNow,
                Status = AnalysisStatus.Completed,
                ErrorType = analysisResult.ErrorType,
                RootCause = analysisResult.RootCause,
                Confidence = analysisResult.Confidence,
                Accuracy = analysisResult.Accuracy,
                Latency = metrics.TotalProcessingTime.TotalMilliseconds,
                MemoryUsage = metrics.PhaseResourceUsage["analysis"].MemoryUsageMB * 1024 * 1024,
                CpuUsage = metrics.PhaseResourceUsage["analysis"].CPUUsagePercent,
                DependencyGraph = graphResult.DependencyGraph,
                ImpactResults = graphResult.ImpactResults,
                RelatedErrors = graphResult.RelatedErrors,
                Metrics = graphResult.Metrics,
                Metadata = new Dictionary<string, object>
                {
                    { "PerformanceMetrics", metrics },
                    { "GraphAnalysis", graphResult }
                }
            };

            // Cache the result
            _analysisCache[context.Id] = result;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing context: {ContextId}", context.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<RemediationResult> RemediateErrorAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Remediating error context: {ContextId}", context.Id);

            // Check cache first
            if (_remediationCache.TryGetValue(context.Id, out var cachedResult))
            {
                return cachedResult;
            }

            // Get error analysis
            var analysis = await AnalyzeErrorAsync(context);

            // Perform remediation
            var result = await _remediationService.RemediateAsync(analysis, context);

            // Cache the result
            _remediationCache[context.Id] = result;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error remediating context: {ContextId}", context.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ValidationResult> ValidateContextAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Validating error context: {ContextId}", context.Id);

            var result = new ValidationResult
            {
                IsValid = true,
                Timestamp = DateTime.UtcNow
            };

            // Validate context properties
            if (string.IsNullOrEmpty(context.Id))
            {
                result.IsValid = false;
                result.Errors.Add("Context ID is required");
            }

            if (context.Exception == null)
            {
                result.IsValid = false;
                result.Errors.Add("Exception is required");
            }

            // Validate graph analyzer configuration
            var graphAnalyzerValid = await _graphAnalyzer.ValidateConfigurationAsync();
            if (!graphAnalyzerValid)
            {
                result.IsValid = false;
                result.Errors.Add("Graph analyzer configuration is invalid");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating context: {ContextId}", context.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ErrorAnalysisResult> GetAnalysisStatusAsync(string correlationId)
    {
        ArgumentNullException.ThrowIfNull(correlationId);

        try
        {
            _logger.LogInformation("Getting analysis status for correlation ID: {CorrelationId}", correlationId);

            // Check cache first
            if (_analysisCache.TryGetValue(correlationId, out var result))
            {
                return result;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting analysis status for correlation ID: {CorrelationId}", correlationId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GraphAnalysisResult> AnalyzeContextGraphAsync(ErrorContext context)
    {
        try
        {
            _logger.LogInformation("Analyzing context graph");
            return await _errorContextAnalyzer.AnalyzeContextAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing context graph");
            throw new RuntimeErrorSageException("Failed to analyze context graph", ex);
        }
    }

    /// <inheritdoc />
    public async Task<LLMAnalysisResult> AnalyzeWithLLMAsync(ErrorContext context)
    {
        try
        {
            _logger.LogInformation("Analyzing with LLM");
            return await _llmClient.AnalyzeContextAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing with LLM");
            throw new RuntimeErrorSageException("Failed to analyze with LLM", ex);
        }
    }
} 