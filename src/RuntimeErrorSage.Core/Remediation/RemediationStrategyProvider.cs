using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using DomainIRemediationStrategy = RuntimeErrorSage.Domain.Interfaces.IRemediationStrategy;
using RuntimeErrorSage.Core.Remediation;

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
                    // Convert Domain IRemediationStrategy to Application IRemediationStrategy if needed
                    if (strategy is DomainIRemediationStrategy domainStrategy)
                    {
                        // Use adapter to convert
                        var appStrategy = RemediationStrategyAdapterExtensions.ToApplicationStrategy(domainStrategy);
                        if (appStrategy != null)
                        {
                            applicable.Add(appStrategy);
                        }
                    }
                    else if (strategy is IRemediationStrategy appStrategy)
                    {
                        applicable.Add(appStrategy);
                    }
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
    public async Task<List<IRemediationStrategy>> GetStrategiesAsync(ErrorContext context)
    {
        return await GetApplicableStrategiesAsync(context);
    }

    /// <inheritdoc/>
    public async Task<IRemediationStrategy> GetStrategyByIdAsync(string strategyId)
    {
        ArgumentNullException.ThrowIfNull(strategyId);

        try
        {
            var strategy = await _registry.GetStrategyAsync(strategyId);
            
            _logger.LogDebug("Retrieved strategy with ID '{StrategyId}'", strategyId);
            
            return strategy;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving strategy with ID {StrategyId}", strategyId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<IRemediationStrategy>> GetAllStrategiesAsync()
    {
        try
        {
            var strategies = await _registry.GetAllStrategiesAsync();
            var result = new List<IRemediationStrategy>();
            
            foreach (var strategy in strategies)
            {
                // Convert Domain IRemediationStrategy to Application IRemediationStrategy if needed
                if (strategy is DomainIRemediationStrategy domainStrategy)
                {
                    var appStrategy = RemediationStrategyAdapterExtensions.ToApplicationStrategy(domainStrategy);
                    if (appStrategy != null)
                    {
                        result.Add(appStrategy);
                    }
                }
                else if (strategy is IRemediationStrategy appStrategy)
                {
                    result.Add(appStrategy);
                }
            }
            
            _logger.LogDebug("Retrieved {Count} strategies", result.Count);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all strategies");
            return new List<IRemediationStrategy>();
        }
    }

    /// <inheritdoc/>
    public async Task<IRemediationStrategy> GetBestStrategyAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            var strategies = await GetApplicableStrategiesAsync(context);
            if (!strategies.Any())
            {
                return null;
            }

            // Return the highest priority strategy
            return strategies.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting best strategy for error type {ErrorType}", context.ErrorType);
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            var strategies = await GetApplicableStrategiesAsync(context);
            if (!strategies.Any())
            {
                return new RemediationPlan(
                    name: $"Remediation Plan - {context.ErrorType}",
                    description: $"No applicable strategies found for error type '{context.ErrorType}'",
                    actions: new List<RemediationAction>(),
                    parameters: new Dictionary<string, object>(),
                    estimatedDuration: TimeSpan.Zero)
                {
                    PlanId = Guid.NewGuid().ToString(),
                    Status = RemediationStatusEnum.Failed,
                    StatusInfo = $"No applicable strategies found for error type '{context.ErrorType}'"
                };
            }

            var plan = new RemediationPlan(
                name: $"Remediation Plan - {context.ErrorType}",
                description: $"Remediation plan for error type '{context.ErrorType}'",
                actions: new List<RemediationAction>(),
                parameters: new Dictionary<string, object>(),
                estimatedDuration: TimeSpan.FromMinutes(10))
            {
                PlanId = Guid.NewGuid().ToString(),
                Context = context,
                CreatedAt = DateTime.UtcNow,
                Status = RemediationStatusEnum.Pending,
                StatusInfo = "Remediation plan created"
            };

            return plan;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating remediation plan for error type {ErrorType}", context.ErrorType);
            return new RemediationPlan(
                name: $"Remediation Plan - {context.ErrorType}",
                description: $"Failed to create remediation plan: {ex.Message}",
                actions: new List<RemediationAction>(),
                parameters: new Dictionary<string, object>(),
                estimatedDuration: TimeSpan.Zero)
            {
                PlanId = Guid.NewGuid().ToString(),
                Status = RemediationStatusEnum.Failed,
                StatusInfo = $"Failed to create remediation plan: {ex.Message}"
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
                    Severity = SeverityLevel.Low.ToImpactSeverity().ToRemediationActionSeverity(),
                    Scope = ImpactScope.Component.ToRemediationActionImpactScope(),
                    AffectedComponents = new List<string> { context.ComponentId },
                    EstimatedRecoveryTime = TimeSpan.Zero,
                    RiskLevel = RiskLevel.Low,
                    Confidence = (double)ConfidenceLevel.Low
                };
            }

            var impact = new RemediationImpact
            {
                Severity = SeverityLevel.Low.ToImpactSeverity().ToRemediationActionSeverity(),
                Scope = ImpactScope.Component.ToRemediationActionImpactScope(),
                AffectedComponents = new List<string> { context.ComponentId },
                EstimatedRecoveryTime = TimeSpan.Zero,
                RiskLevel = RiskLevel.Low,
                Confidence = (double)ConfidenceLevel.Low
            };

            // Aggregate impact from all strategies
            foreach (var strategy in strategies)
            {
                // Create default impact based on strategy properties
                var strategyImpact = new RemediationImpact
                {
                    Severity = RemediationActionSeverity.Medium,
                    Scope = RemediationActionImpactScope.Module,
                    AffectedComponents = new List<string> { context.ComponentId },
                    EstimatedRecoveryTime = TimeSpan.FromMinutes(5),
                    RiskLevel = strategy.RiskLevel,
                    Confidence = 0.5
                };

                // Update impact based on strategy impact
                UpdateImpactSeverity(impact, strategyImpact);
                impact.AffectedComponents = impact.AffectedComponents.Union(strategyImpact.AffectedComponents).ToList();
                impact.EstimatedRecoveryTime += strategyImpact.EstimatedRecoveryTime;
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
                Severity = SeverityLevel.Unknown.ToImpactSeverity().ToRemediationActionSeverity(),
                Scope = ImpactScope.Component.ToRemediationActionImpactScope(),
                AffectedComponents = new List<string> { context.ComponentId },
                EstimatedRecoveryTime = TimeSpan.Zero,
                RiskLevel = RiskLevel.Unknown,
                Confidence = (double)ConfidenceLevel.Low
            };
        }
    }

    private RemediationImpact CreateDefaultImpact()
    {
        return new RemediationImpact
        {
            Severity = SeverityLevel.Low.ToImpactSeverity().ToRemediationActionSeverity(),
            Scope = ImpactScope.Component.ToRemediationActionImpactScope(),
            AffectedComponents = new List<string>(),
            EstimatedRecoveryTime = TimeSpan.FromMinutes(5)
        };
    }

    private RemediationImpact CreateUnknownImpact()
    {
        return new RemediationImpact
        {
            Severity = SeverityLevel.Unknown.ToImpactSeverity().ToRemediationActionSeverity(),
            Scope = ImpactScope.Component.ToRemediationActionImpactScope(),
            AffectedComponents = new List<string>(),
            EstimatedRecoveryTime = TimeSpan.Zero
        };
    }

    private void UpdateImpactSeverity(RemediationImpact impact, RemediationImpact strategyImpact)
    {
        var currentSeverity = impact.Severity.ToSeverityLevel();
        var strategySeverity = strategyImpact.Severity.ToSeverityLevel();
        impact.Severity = (currentSeverity > strategySeverity ? currentSeverity : strategySeverity).ToImpactSeverity().ToRemediationActionSeverity();
    }

    public async Task<RemediationPlan> CreatePlanAsync(ErrorContext context, IRemediationStrategy strategy)
    {
        var plan = new RemediationPlan(
            strategy.Name,
            strategy.Description,
            strategy.Actions,
            strategy.Parameters,
            TimeSpan.FromMinutes(10)); // Default estimated duration

        plan.Status = RemediationStatusEnum.NotStarted;
        plan.StatusInfo = "Plan created successfully";

        return plan;
    }
} 
