using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Provides remediation strategies based on error context analysis.
/// </summary>
public class RemediationStrategyProvider : IRemediationStrategyProvider
{
    private readonly ILogger<RemediationStrategyProvider> _logger;
    private readonly IErrorContextAnalyzer _errorContextAnalyzer;
    private readonly IRemediationRegistry _registry;
    private readonly IRemediationValidator _validator;
    private readonly IRemediationMetricsCollector _metricsCollector;
    private readonly ILLMClient _llmClient;

    public bool IsEnabled { get; } = true;
    public string Name { get; } = "RuntimeErrorSage Remediation Strategy Provider";
    public string Version { get; } = "1.0.0";

    public RemediationStrategyProvider(
        ILogger<RemediationStrategyProvider> logger,
        IErrorContextAnalyzer errorContextAnalyzer,
        IRemediationRegistry registry,
        IRemediationValidator validator,
        IRemediationMetricsCollector metricsCollector,
        ILLMClient llmClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _errorContextAnalyzer = errorContextAnalyzer ?? throw new ArgumentNullException(nameof(errorContextAnalyzer));
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
        _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
    }

    /// <summary>
    /// Returns applicable remediation strategies for the given error context using graph-based and LLM analysis.
    /// </summary>
    public async Task<List<IRemediationStrategy>> GetApplicableStrategiesAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            // Analyze error context using the graph-based analyzer
            var graphAnalysis = await _errorContextAnalyzer.AnalyzeErrorContextAsync(context);

            // Get strategies from the registry for the error type
            var strategies = await _registry.GetStrategiesForErrorAsync(context);
            var applicable = new List<IRemediationStrategy>();

            foreach (var strategy in strategies)
            {
                // Validate the strategy for the current context
                var validationResult = await _validator.ValidateStrategyAsync(strategy, context);
                if (validationResult.IsValid)
                {
                    applicable.Add(strategy);
                }
            }

            // Sort by priority
            applicable.Sort((a, b) => b.Priority.CompareTo(a.Priority));

            _logger.LogInformation(
                "Found {Count} applicable strategies for error type '{ErrorType}'",
                applicable.Count,
                context.ErrorType);

            return applicable;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting applicable strategies for error type {ErrorType}", context.ErrorType);
            return new List<IRemediationStrategy>();
        }
    }

    /// <inheritdoc/>
    public async Task<IRemediationStrategy> GetStrategyAsync(string strategyName)
    {
        ArgumentNullException.ThrowIfNull(strategyName);

        try
        {
            var strategy = await _registry.GetStrategyAsync(strategyName);
            
            _logger.LogDebug("Retrieved strategy '{StrategyName}'", strategyName);
            
            return strategy;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving strategy {StrategyName}", strategyName);
            throw;
        }
    }

    public async Task<RemediationPlan> CreateRemediationPlanAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            var strategies = await GetApplicableStrategiesAsync(context);
            if (!strategies.Any())
            {
                return new RemediationPlan
                {
                    PlanId = Guid.NewGuid().ToString(),
                    Steps = new List<RemediationStep>(),
                    Status = RemediationPlanStatus.NoStrategiesAvailable,
                    Message = $"No applicable strategies found for error type '{context.ErrorType}'"
                };
            }

            var plan = new RemediationPlan
            {
                PlanId = Guid.NewGuid().ToString(),
                Steps = new List<RemediationStep>(),
                Status = RemediationPlanStatus.Created,
                Message = "Remediation plan created"
            };

            // Add steps for each strategy
            for (int i = 0; i < strategies.Count; i++)
            {
                var strategy = strategies[i];
                plan.Steps.Add(new RemediationStep
                {
                    Id = Guid.NewGuid().ToString(),
                    StrategyName = strategy.Name,
                    Context = context,
                    Parameters = strategy.Parameters,
                    Order = i + 1
                });
            }

            // Validate the plan
            var validationResult = await _validator.ValidatePlanAsync(plan, context);
            if (!validationResult.IsValid)
            {
                plan.Status = RemediationPlanStatus.Invalid;
                plan.Message = validationResult.ValidationMessage;
            }

            return plan;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating remediation plan for error type {ErrorType}", context.ErrorType);
            return new RemediationPlan
            {
                PlanId = Guid.NewGuid().ToString(),
                Steps = new List<RemediationStep>(),
                Status = RemediationPlanStatus.Failed,
                Message = $"Failed to create remediation plan: {ex.Message}"
            };
        }
    }

    public async Task<RemediationImpact> GetEstimatedImpactAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            var strategies = await GetApplicableStrategiesAsync(context);
            if (!strategies.Any())
            {
                return new RemediationImpact
                {
                    Severity = RemediationImpactSeverity.Low,
                    Scope = RemediationImpactScope.Local,
                    AffectedComponents = new List<string> { context.ComponentId },
                    EstimatedDuration = TimeSpan.Zero,
                    RiskLevel = RiskLevel.Low,
                    Confidence = 0.0
                };
            }

            var impact = new RemediationImpact
            {
                Severity = RemediationImpactSeverity.Low,
                Scope = RemediationImpactScope.Local,
                AffectedComponents = new List<string> { context.ComponentId },
                EstimatedDuration = TimeSpan.Zero,
                RiskLevel = RiskLevel.Low,
                Confidence = 0.0
            };

            // Aggregate impact from all strategies
            foreach (var strategy in strategies)
            {
                var strategyImpact = await strategy.GetEstimatedImpactAsync(new ErrorAnalysisResult
                {
                    Context = context,
                    Analysis = new Dictionary<string, object>()
                });

                // Update impact based on strategy impact
                impact.Severity = (RemediationImpactSeverity)Math.Max((int)impact.Severity, (int)strategyImpact.Severity);
                impact.Scope = (RemediationImpactScope)Math.Max((int)impact.Scope, (int)strategyImpact.Scope);
                impact.AffectedComponents = impact.AffectedComponents.Union(strategyImpact.AffectedComponents).ToList();
                impact.EstimatedDuration += strategyImpact.EstimatedDuration;
                impact.RiskLevel = (RiskLevel)Math.Max((int)impact.RiskLevel, (int)strategyImpact.RiskLevel);
                impact.Confidence = Math.Max(impact.Confidence, strategyImpact.Confidence);
            }

            return impact;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error estimating impact for error type {ErrorType}", context.ErrorType);
            return new RemediationImpact
            {
                Severity = RemediationImpactSeverity.Unknown,
                Scope = RemediationImpactScope.Unknown,
                AffectedComponents = new List<string> { context.ComponentId },
                EstimatedDuration = TimeSpan.Zero,
                RiskLevel = RiskLevel.Unknown,
                Confidence = 0.0
            };
        }
    }
} 