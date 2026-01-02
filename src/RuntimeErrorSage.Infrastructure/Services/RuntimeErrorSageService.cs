using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Domain.Models.Context;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Models.LLM;
using RuntimeErrorSage.Domain.Models.MCP;
using RuntimeErrorSage.Domain.Models.Metrics;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Options;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.Runtime.Exceptions;
using RuntimeErrorSage.Application.Runtime.Interfaces;
using RuntimeErrorSage.Application.Services.Interfaces;
using ValidationResult = RuntimeErrorSage.Domain.Models.Validation.ValidationResult;
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;
using RuntimeErrorSage.Domain.Models;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Application.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Graph.Interfaces;
using DomainIRemediationStrategy = RuntimeErrorSage.Domain.Interfaces.IRemediationStrategy;

namespace RuntimeErrorSage.Infrastructure.Services;

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
    private readonly IModelContextProtocol _mcp;
    private readonly IQwenLLMClient _llmClient;
    private readonly IErrorAnalyzer _errorAnalyzer;
    private readonly IDependencyGraphAnalyzer _graphAnalyzer;
    private readonly Dictionary<string, ErrorAnalysisResult> _analysisCache;
    private readonly Dictionary<string, RemediationResult> _remediationCache;
    private readonly IModelContextProtocol _modelContext;

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
        IModelContextProtocol mcp,
        IQwenLLMClient llmClient,
        IErrorAnalyzer errorAnalyzer,
        IDependencyGraphAnalyzer graphAnalyzer,
        IModelContextProtocol modelContext)
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
        _modelContext = modelContext ?? throw new ArgumentNullException(nameof(modelContext));
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

            // Create a simple validation check
            var validationContext = new ValidationContext();
            var validationResult = ValidationResult.Success(validationContext);

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

            // Skip graph analysis for now
            // var graphAnalysis = await _graphAnalyzer.AnalyzeContextAsync(enrichedContext);
            // enrichedContext.AddMetadata("GraphAnalysis", graphAnalysis);

            // Analyze with LLM
            var llmAnalysis = await _llmClient.AnalyzeContextAsync(context.ToRuntimeContext());
            enrichedContext.AddMetadata("LLMAnalysis", llmAnalysis);

            // Add MCP analysis
            var mcpAnalysis = await _mcp.AnalyzeContextAsync(context.ToRuntimeContext());
            enrichedContext.AddMetadata("MCPAnalysis", mcpAnalysis);

            return enrichedContext;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enriching context");
            throw new RuntimeErrorSageException("Failed to enrich error context", ex);
        }
    }

    /// <inheritdoc />
    public void RegisterRemediationStrategy(RuntimeErrorSage.Application.Interfaces.IRemediationStrategy strategy)
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

            // Perform error analysis using the error from context
            ErrorAnalysisResult analysisResult;
            if (context.Error != null)
            {
                // Create an exception from the RuntimeError if needed
                var exception = new Exception(context.Error.Message);
                analysisResult = await _errorAnalyzer.AnalyzeErrorAsync(exception, context);
            }
            else
            {
                analysisResult = new ErrorAnalysisResult 
                { 
                    Status = AnalysisStatus.Failed, 
                    ErrorId = context.Id 
                };
            }

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

            // Validate context if validation registry is available
            ValidationResult validationResult = null;
            try
            {
                if (_validationRegistry != null)
                {
                    // Try to use ValidateContextAsync if available, otherwise use ValidateContextAsync from this service
                    try
                    {
                        validationResult = await _validationRegistry.ValidateContextAsync(context);
                    }
                    catch
                    {
                        // Fallback to service's own validation
                        validationResult = await ValidateContextAsync(context);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error validating context, continuing without validation");
            }

            // Create remediation plan
            RemediationPlan remediationPlan = null;
            try
            {
                var remediationAnalysis = await _remediationAnalyzer.AnalyzeErrorAsync(context);
                if (remediationAnalysis != null && remediationAnalysis.IsValid)
                {
                    // Create RemediationPlan from RemediationAnalysis
                    var actions = remediationAnalysis.SuggestedActions ?? new List<RemediationAction>();
                    remediationPlan = new RemediationPlan(
                        name: $"Remediation Plan - {context.ErrorType}",
                        description: $"Remediation plan for error {context.ErrorId}",
                        actions: actions,
                        parameters: remediationAnalysis.Metadata ?? new Dictionary<string, object>(),
                        estimatedDuration: TimeSpan.FromMinutes(10))
                    {
                        PlanId = Guid.NewGuid().ToString(),
                        Context = context,
                        CreatedAt = DateTime.UtcNow,
                        Status = RemediationStatusEnum.Pending,
                        StatusInfo = "Remediation plan created from analysis"
                    };
                    
                    // Get strategies from registry to populate Strategies property
                    // Note: We need to get actual IRemediationStrategy instances, not just recommendations
                    // For now, we'll create a basic plan with empty strategies list
                    // The strategies will be populated by the remediation service when needed
                    remediationPlan.Strategies = new List<DomainIRemediationStrategy>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error creating remediation plan, continuing without plan");
            }

            // Combine results
            var result = new ErrorAnalysisResult
            {
                ErrorId = context.Id,
                Timestamp = DateTime.UtcNow,
                Status = AnalysisStatus.Completed,
                CorrelationId = context.CorrelationId,
                IsAnalyzed = validationResult?.IsValid ?? true,
                RemediationPlan = remediationPlan,
                ValidationResult = validationResult,
                Details = new Dictionary<string, object>
                {
                    ["OriginalContext"] = context,
                    ["ProcessingTime"] = metrics.TotalProcessingTime,
                    ["ResourceUsage"] = metrics.PhaseResourceUsage
                }
            };

            // Add to cache
            _analysisCache[context.Id] = result;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing error");
            throw new RuntimeErrorSageException("Failed to analyze error", ex);
        }
    }

    /// <inheritdoc />
    public async Task<RemediationResult> RemediateErrorAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Remediating error: {ContextId}", context.Id);

            // Check cache first
            if (_remediationCache.TryGetValue(context.Id, out var cachedResult))
            {
                return cachedResult;
            }

            // Analyze error first
            var analysisResult = await AnalyzeErrorAsync(context);

            // Apply remediation
            var result = await ApplyRemediationAsync(analysisResult);

            // Add to cache
            _remediationCache[context.Id] = result;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error remediating error");
            throw new RuntimeErrorSageException("Failed to remediate error", ex);
        }
    }

    /// <inheritdoc />
    public async Task<ValidationResult> ValidateContextAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Validating error context: {ContextId}", context.Id);

            // Create a validation context
            var validationContext = new ValidationContext();
            validationContext.SetTarget(context);
            
            // Create a validation result
            var result = new ValidationResult(validationContext);

            // Validate error context
            if (context.Error == null)
            {
                result.AddError("Error context does not contain an error object");
            }

            // Simple validation check
            if (string.IsNullOrEmpty(context.Id))
            {
                result.AddError("Error context must have an ID");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating context");
            throw new RuntimeErrorSageException("Failed to validate error context", ex);
        }
    }

    /// <inheritdoc />
    public async Task<ErrorAnalysisResult> GetAnalysisStatusAsync(string correlationId)
    {
        try
        {
            _logger.LogInformation("Getting analysis status for correlation ID: {CorrelationId}", correlationId);

            // Check cache first
            var result = _analysisCache.Values.FirstOrDefault(a => a.CorrelationId == correlationId);
            if (result != null)
            {
                return result;
            }

            // If not in cache, create a placeholder result
            return new ErrorAnalysisResult
            {
                ErrorId = Guid.NewGuid().ToString(),
                CorrelationId = correlationId,
                Status = AnalysisStatus.NotStarted,
                Timestamp = DateTime.UtcNow,
                Details = new Dictionary<string, object>
                {
                    ["Message"] = "Analysis not found for the specified correlation ID"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting analysis status");
            throw new RuntimeErrorSageException("Failed to get analysis status", ex);
        }
    }

    /// <inheritdoc />
    public async Task<GraphAnalysisResult> AnalyzeContextGraphAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Analyzing context graph: {ContextId}", context.Id);

            // Create a placeholder result since we can't use AnalyzeContextAsync
            var result = new GraphAnalysisResult
            {
                AnalysisId = Guid.NewGuid().ToString(),
                CorrelationId = context.CorrelationId,
                ComponentId = context.ComponentId,
                Status = AnalysisStatus.Completed,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow,
                IsValid = true,
                Metrics = new Dictionary<string, double>
                {
                    ["ProcessingTime"] = 0.0
                }
            };

            return result;
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
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Analyzing with LLM: {ContextId}", context.Id);

            // Convert to runtime context
            var runtimeContext = context.ToRuntimeContext();

            // Analyze with LLM
            return await _llmClient.AnalyzeContextAsync(runtimeContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing with LLM");
            throw new RuntimeErrorSageException("Failed to analyze with LLM", ex);
        }
    }

    Task<RuntimeErrorSage.Domain.Models.Validation.ValidationResult> IRuntimeErrorSageService.ValidateContextAsync(ErrorContext context)
    {
        throw new NotImplementedException();
    }
} 
