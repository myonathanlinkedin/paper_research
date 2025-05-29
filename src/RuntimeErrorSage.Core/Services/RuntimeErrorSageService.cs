using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Analysis.Interfaces;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.Models.Context;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.LLM;
using RuntimeErrorSage.Core.Models.MCP;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Options;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Runtime.Exceptions;
using RuntimeErrorSage.Core.Runtime.Interfaces;
using RuntimeErrorSage.Core.Services.Interfaces;
using ValidationResult = RuntimeErrorSage.Core.Models.Validation.ValidationResult;

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
    private readonly RuntimeErrorSage.Core.Analysis.Interfaces.IDependencyGraphAnalyzer _graphAnalyzer;
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
        RuntimeErrorSage.Core.Analysis.Interfaces.IDependencyGraphAnalyzer graphAnalyzer)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(errorAnalysisService);
        ArgumentNullException.ThrowIfNull(remediationService);
        ArgumentNullException.ThrowIfNull(contextEnrichmentService);
        ArgumentNullException.ThrowIfNull(validationRegistry);
        ArgumentNullException.ThrowIfNull(errorContextAnalyzer);
        ArgumentNullException.ThrowIfNull(remediationAnalyzer);
        ArgumentNullException.ThrowIfNull(remediationValidator);
        ArgumentNullException.ThrowIfNull(metricsCollector);
        ArgumentNullException.ThrowIfNull(mcp);
        ArgumentNullException.ThrowIfNull(llmClient);
        ArgumentNullException.ThrowIfNull(errorAnalyzer);
        ArgumentNullException.ThrowIfNull(graphAnalyzer);

        _logger = logger;
        _errorAnalysisService = errorAnalysisService;
        _remediationService = remediationService;
        _contextEnrichmentService = contextEnrichmentService;
        _validationRegistry = validationRegistry;
        _errorContextAnalyzer = errorContextAnalyzer;
        _remediationAnalyzer = remediationAnalyzer;
        _remediationValidator = remediationValidator;
        _metricsCollector = metricsCollector;
        _mcp = mcp;
        _llmClient = llmClient;
        _errorAnalyzer = errorAnalyzer;
        _graphAnalyzer = graphAnalyzer;
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
            var validationResult = await _validationRegistry.ValidateAsync(context.Id, enrichedContext);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Analysis validation failed: {Errors}", string.Join(", ", validationResult.Errors));
                analysisResult.Status = AnalysisStatus.Failed;
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
                return new RemediationResult(context, RemediationStatusEnum.Failed, "Failed to create valid remediation plan", Guid.NewGuid().ToString());
            }

            // Validate remediation plan
            var validationResult = await _remediationValidator.ValidateRemediationAsync(analysisResult, context);
            if (!validationResult.IsValid)
            {
                return new RemediationResult(context, RemediationStatusEnum.ValidationFailed, string.Join(", ", validationResult.Errors), Guid.NewGuid().ToString());
            }

            // Execute remediation
            var result = await _remediationService.ApplyRemediationAsync(context);

            // Collect metrics
            var metrics = await _metricsCollector.CollectMetricsAsync(context);
            result.Metrics = metrics;

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
            enrichedContext.AddMetadata("GraphAnalysis", graphAnalysis); // Use AddMetadata method to add data

            // Analyze with LLM
            var llmAnalysis = await _llmClient.AnalyzeContextAsync(context.ToRuntimeContext());
            enrichedContext.AddMetadata("LLMAnalysis", llmAnalysis); // Use AddMetadata method to add data

            // Add MCP analysis
            var mcpAnalysis = await _mcp.AnalyzeContextAsync(context.ToRuntimeContext());
            enrichedContext.AddMetadata("MCPAnalysis", mcpAnalysis); // Use AddMetadata method to add data

            return enrichedContext;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enriching context");
            throw new RuntimeErrorSageException("Failed to enrich error context", ex);
        }
    }

    /// <inheritdoc />
    public void RegisterRemediationStrategy(RuntimeErrorSage.Core.Models.Remediation.Interfaces.IRemediationStrategy strategy)
    {
        try
        {
            _logger.LogInformation("Registering remediation strategy: {StrategyName}", strategy.Name);
            
            // Register the strategy directly
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
            if (_errorAnalysisService is IConfigurable configurable)
                configurable.Configure(options);
            if (_remediationService is IConfigurable configurable2)
                configurable2.Configure(options);
            if (_contextEnrichmentService is IConfigurable configurable3)
                configurable3.Configure(options);
            if (_validationRegistry is IConfigurable configurable4)
                configurable4.Configure(options);
            if (_errorContextAnalyzer is IConfigurable configurable5)
                configurable5.Configure(options);
            if (_remediationAnalyzer is IConfigurable configurable6)
                configurable6.Configure(options);
            if (_remediationValidator is IConfigurable configurable7)
                configurable7.Configure(options);
            if (_metricsCollector is IConfigurable configurable8)
                configurable8.Configure(options);
            if (_mcp is IConfigurable configurable9)
                configurable9.Configure(options);
            if (_llmClient is IConfigurable configurable10)
                configurable10.Configure(options);
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
            var resourceUsage = await _metricsCollector.CollectMetricsAsync(context);
            metrics.PhaseResourceUsage = new Dictionary<string, MetricsResourceUsage>();
            foreach (var kvp in resourceUsage)
            {
                if (kvp.Value is MetricsResourceUsage usage)
                {
                    metrics.PhaseResourceUsage[kvp.Key] = new MetricsResourceUsage
                    {
                        CpuUsage = usage.CpuUsage,
                        MemoryUsage = usage.MemoryUsage,
                        Timestamp = DateTime.UtcNow
                    };
                }
            }

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
                MemoryUsage = metrics.PhaseResourceUsage["Analysis"].MemoryUsage,
                CpuUsage = metrics.PhaseResourceUsage["Analysis"].CpuUsage,
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
            var analysis = await _errorContextAnalyzer.AnalyzeContextAsync(context);
            
            // Convert RemediationAnalysis to GraphAnalysisResult
            return new GraphAnalysisResult
            {
                // TODO: Set the properties based on the analysis
                //AnalysisId = analysis.AnalysisId,
                //Context = new RuntimeContext
                //{
                //    ContextId = context.Id,
                //    ApplicationName = context.ComponentName,
                //    Environment = context.ServiceName,
                //    CorrelationId = context.CorrelationId,
                //    Timestamp = context.Timestamp,
                //    Metadata = context.Metadata.ToDictionary(kv => kv.Key, kv => kv.Value)
                //},
                Status = AnalysisStatus.Completed,
                StartTime = analysis.Timestamp,
                EndTime = DateTime.UtcNow,
                CorrelationId = context.CorrelationId,
                ComponentId = context.ComponentId,
                ComponentName = context.ComponentName,
                IsValid = true, // Default to true since the class doesn't have this property
                ErrorMessage = string.Empty, // Default to empty since the class doesn't have this property
                Metadata = new Dictionary<string, object>
                {
                    { "ConfidenceLevel", analysis.ConfidenceLevel },
                    { "Suggestions", analysis.Suggestions },
                    { "RelatedErrors", analysis.RelatedErrors },
                    { "DependencyGraph", analysis.DependencyGraph },
                    { "RootCause", analysis.RootCause },
                    { "AdditionalContext", analysis.AdditionalContext }
                }
            };
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
            _logger.LogInformation("Analyzing error context with LLM");

            // Analyze with LLM
            var llmAnalysis = await _llmClient.AnalyzeContextAsync(context.ToRuntimeContext());

            // Enrich with additional insights
            var enrichedAnalysis = await _errorAnalyzer.EnrichLLMAnalysisAsync(llmAnalysis);

            // Add graph analysis insights
            var graphAnalysis = await _graphAnalyzer.AnalyzeContextAsync(context);
            enrichedAnalysis.GraphInsights = graphAnalysis.Insights;

            return enrichedAnalysis;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing with LLM");
            throw new RuntimeErrorSageException("Failed to analyze with LLM", ex);
        }
    }

    //TODO
    Task<System.ComponentModel.DataAnnotations.ValidationResult> IRuntimeErrorSageService.ValidateContextAsync(ErrorContext context)
    {
        throw new NotImplementedException();
    }
} 
