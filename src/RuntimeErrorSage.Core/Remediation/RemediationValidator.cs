using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Domain.Models.Common;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Health;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using System.Collections.Concurrent;
using IRemediationValidator = RuntimeErrorSage.Application.Remediation.Interfaces.IRemediationValidator;
using ValidationResult = RuntimeErrorSage.Domain.Models.Validation.ValidationResult;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Graph;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Metrics;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Validation.Interfaces;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Interfaces;

namespace RuntimeErrorSage.Application.Remediation;

/// <summary>
/// Validates remediation actions and strategies.
/// </summary>
public class RemediationValidator : IRemediationValidator, IDisposable
{
    private readonly ILogger<RemediationValidator> _logger;
    private readonly IReadOnlyCollection<IValidationRule> _rules;
    private readonly IReadOnlyCollection<IValidationWarning> _warnings;
    private readonly RemediationValidatorOptions _options;
    private readonly CancellationTokenSource _globalCts;
    private bool _disposed;
    private readonly IErrorContextAnalyzer _errorContextAnalyzer;
    private readonly IRemediationMetricsCollector _metricsCollector;
    private readonly IRemediationRegistry _registry;
    private readonly ILLMClient _llmClient;
    private readonly Dictionary<string, IRemediationActionValidator> _supportedActions;
    private readonly EventId _validationErrorEventId;

    /// <inheritdoc/>
    public bool IsEnabled => !_disposed && _options.IsEnabled;

    /// <inheritdoc/>
    public string Name => "RuntimeErrorSage Remediation Validator";

    /// <inheritdoc/>
    public string Version => "1.0.0";

    public RemediationValidator(
        ILogger<RemediationValidator> logger,
        IOptions<RemediationValidatorOptions> options,
        IEnumerable<IRemediationValidationRule> rules,
        IEnumerable<IRemediationValidationWarning> warnings,
        IErrorContextAnalyzer errorContextAnalyzer,
        IRemediationMetricsCollector metricsCollector,
        IRemediationRegistry registry,
        ILLMClient llmClient,
        IEnumerable<string> supportedActions,
        EventId? validationErrorEventId = null)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(rules);
        ArgumentNullException.ThrowIfNull(warnings);
        ArgumentNullException.ThrowIfNull(errorContextAnalyzer);
        ArgumentNullException.ThrowIfNull(metricsCollector);
        ArgumentNullException.ThrowIfNull(registry);
        ArgumentNullException.ThrowIfNull(llmClient);
        ArgumentNullException.ThrowIfNull(supportedActions);

        _logger = logger;
        _options = options.Value;
        _rules = rules.ToList();
        _warnings = warnings.ToList();
        _globalCts = new CancellationTokenSource();
        _errorContextAnalyzer = errorContextAnalyzer;
        _metricsCollector = metricsCollector;
        _registry = registry;
        _llmClient = llmClient;
        _supportedActions = supportedActions.ToDictionary(action => action, action => (IRemediationActionValidator)null);
        _validationErrorEventId = validationErrorEventId ?? new EventId(1, nameof(ValidateRemediationAsync));
    }

    /// <inheritdoc/>
    public async Task<ValidationResult> ValidatePlanAsync(RemediationPlan plan, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        var result = new ValidationResult
        {
            StartTime = DateTime.UtcNow,
            Timestamp = DateTime.UtcNow
        };
        result.SetCorrelationId(context.CorrelationId);

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            // Validate plan properties
            if (string.IsNullOrEmpty(plan.Name))
            {
                result.AddError("Plan name is required");
            }

            if (string.IsNullOrEmpty(plan.Description))
            {
                result.AddError("Plan description is required");
            }

            if (plan.Actions == null || plan.Actions.Count == 0)
            {
                result.AddError("Plan must contain at least one action");
            }

            // Validate each action
            if (plan.Actions != null)
            {
                foreach (var action in plan.Actions)
                {
                    var actionResult = await ValidateActionAsync(action as RemediationAction, context);
                    if (!actionResult.IsValid)
                    {
                        result.IsValid = false;
                        foreach (var error in actionResult.Errors)
                        {
                            result.AddError(error);
                        }
                    }
                }
            }

            // Add warning if no rollback plan
            if (plan.RollbackPlan == null)
            {
                result.AddWarning("Plan does not have a rollback plan");
            }

            result.EndTime = DateTime.UtcNow;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating plan {PlanId}", plan.PlanId);
            return new ValidationResult
            {
                IsValid = false,
                Messages = new List<string> { $"Error validating plan: {ex.Message}" },
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ValidationResult> ValidateStrategyAsync(IRemediationStrategy strategy, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(strategy);
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        var result = new ValidationResult
        {
            StartTime = DateTime.UtcNow,
            Timestamp = DateTime.UtcNow
        };
        result.SetCorrelationId(context.CorrelationId);

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            // Validate strategy properties
            if (string.IsNullOrEmpty(strategy.Name))
            {
                result.AddError("Strategy name is required");
            }

            if (string.IsNullOrEmpty(strategy.Description))
            {
                result.AddError("Strategy description is required");
            }

            if (strategy.Actions == null || !strategy.Actions.Any())
            {
                result.AddError("Strategy must have at least one action");
            }

            result.EndTime = DateTime.UtcNow;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating strategy {StrategyName}", strategy.Name);
            return new ValidationResult
            {
                IsValid = false,
                Messages = new List<string> { $"Error validating strategy: {ex.Message}" },
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ValidationResult> ValidateActionAsync(RemediationAction action, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        var result = new ValidationResult
        {
            StartTime = DateTime.UtcNow,
            Timestamp = DateTime.UtcNow
        };
        result.SetCorrelationId(context.CorrelationId);

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            // Validate action properties
            if (string.IsNullOrEmpty(action.Name))
            {
                result.AddError("Action name is required");
            }

            if (string.IsNullOrEmpty(action.Description))
            {
                result.AddError("Action description is required");
            }

            if (string.IsNullOrEmpty(action.Action))
            {
                result.AddError("Action cannot be empty");
            }

            // Perform risk assessment
            var riskLevel = CalculateRiskLevel(action);
            var potentialIssues = GeneratePotentialIssues(action);
            var mitigationSteps = GenerateMitigationSteps(action);

            if (riskLevel == RemediationRiskLevel.High || riskLevel == RemediationRiskLevel.Critical)
            {
                result.AddWarning($"Action has {riskLevel} risk level", ValidationSeverity.Warning);
                foreach (var issue in potentialIssues)
                {
                    result.AddWarning($"Potential issue: {issue}", ValidationSeverity.Warning);
                }
            }

            result.EndTime = DateTime.UtcNow;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating action {ActionName}", action?.Name);
            return new ValidationResult
            {
                IsValid = false,
                Messages = new List<string> { $"Error validating action: {ex.Message}" },
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ValidationResult> ValidateStepAsync(RemediationStep step, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(step);
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        var result = new ValidationResult
        {
            StartTime = DateTime.UtcNow,
            Timestamp = DateTime.UtcNow
        };
        result.SetCorrelationId(context.CorrelationId);

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            // Validate step properties
            if (string.IsNullOrEmpty(step.Name))
            {
                result.AddError("Step name is required");
            }

            if (string.IsNullOrEmpty(step.Description))
            {
                result.AddError("Step description is required");
            }

            result.EndTime = DateTime.UtcNow;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating step {StepId}", step.StepId);
            return new ValidationResult
            {
                IsValid = false,
                Messages = new List<string> { $"Error validating step: {ex.Message}" },
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ValidationResult> ValidateRemediationAsync(ErrorAnalysisResult analysisResult, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(analysisResult);
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        var result = new ValidationResult
        {
            StartTime = DateTime.UtcNow,
            Timestamp = DateTime.UtcNow
        };
        result.SetCorrelationId(context.CorrelationId);

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            // Validate analysis result properties
            if (string.IsNullOrEmpty(analysisResult.AnalysisId))
            {
                result.IsValid = false;
                result.Messages.Add("Analysis ID is required");
                result.Details = new Dictionary<string, object>
                {
                    ["code"] = "InvalidAnalysisId",
                    ["severity"] = SeverityLevel.Error
                };
                return result;
            }

            if (analysisResult.ErrorContext == null)
            {
                result.IsValid = false;
                result.Messages.Add("Error context is required");
                result.Details = new Dictionary<string, object>
                {
                    ["code"] = "MissingErrorContext",
                    ["severity"] = SeverityLevel.Error
                };
                return result;
            }

            // Validate each suggested remediation strategy
            if (analysisResult.SuggestedStrategies != null && analysisResult.SuggestedStrategies.Any())
            {
                foreach (var strategy in analysisResult.SuggestedStrategies)
                {
                    var strategyResult = await ValidateStrategyAsync(strategy, context);
                    if (!strategyResult.IsValid)
                    {
                        result.IsValid = false;
                        result.Messages.Add($"Invalid strategy: {strategyResult.Messages.FirstOrDefault() ?? "Validation failed"}");
                        result.Details = strategyResult.Details;
                        result.Errors.Add(new ValidationError 
                        { 
                            ErrorId = Guid.NewGuid().ToString(),
                            Message = strategyResult.Messages.FirstOrDefault() ?? "Validation failed",
                            Severity = ValidationSeverity.Error,
                            Code = "InvalidStrategy",
                            Timestamp = DateTime.UtcNow
                        });
                    }
                }
            }

            // Check system health
            var healthStatus = await ValidateSystemHealthAsync(context);
            if (!healthStatus.IsHealthy)
            {
                result.Warnings.Add(new ValidationWarning
                {
                    WarningId = Guid.NewGuid().ToString(),
                    Message = "System health check failed",
                    Severity = ValidationSeverity.Warning,
                    Code = "UnhealthySystem",
                    Timestamp = DateTime.UtcNow
                });
            }

            result.EndTime = DateTime.UtcNow;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating remediation {ErrorId}", context.ErrorId);
            return new ValidationResult
            {
                IsValid = false,
                Messages = new List<string> { $"Error validating remediation: {ex.Message}" },
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow
            };
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationHealthStatus> ValidateSystemHealthAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            var metrics = await CollectMetricsAsync(context, timeoutCts.Token);
            var healthScore = CalculateHealthScore(metrics);

            return new RemediationHealthStatus
            {
                IsHealthy = healthScore >= _options.MinimumHealthScore,
                HealthScore = healthScore,
                Metrics = metrics,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating system health");
            throw new RemediationValidationException("Error validating system health", ex);
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(RemediationValidator));
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _globalCts.Cancel();
            _globalCts.Dispose();
            _disposed = true;
        }
    }

    private async Task<Dictionary<string, double>> CollectMetricsAsync(ErrorContext context, CancellationToken cancellationToken)
    {
        var metrics = new Dictionary<string, double>();
        try
        {
            var systemMetrics = await _metricsCollector.CollectSystemMetricsAsync(context, cancellationToken);
            foreach (var metric in systemMetrics)
            {
                metrics[metric.Key] = metric.Value;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error collecting system metrics");
        }
        return metrics;
    }

    private double CalculateHealthScore(Dictionary<string, double> metrics)
    {
        if (metrics == null || !metrics.Any())
        {
            return 0;
        }

        var totalScore = 0.0;
        var weightSum = 0.0;

        foreach (var metric in metrics)
        {
            var weight = GetMetricWeight(metric.Key);
            totalScore += metric.Value * weight;
            weightSum += weight;
        }

        return weightSum > 0 ? totalScore / weightSum : 0;
    }

    private double GetMetricWeight(string metricKey)
    {
        return metricKey switch
        {
            "cpu_usage" => 0.3,
            "memory_usage" => 0.3,
            "disk_usage" => 0.2,
            "network_usage" => 0.2,
            _ => 0.1
        };
    }

    private RemediationRiskLevel CalculateRiskLevel(RemediationAction action)
    {
        var riskFactors = new List<int>();

        // Analyze error type severity
        if (action.ErrorType?.Contains("Critical", StringComparison.OrdinalIgnoreCase) == true)
            riskFactors.Add(3);
        else if (action.ErrorType?.Contains("Error", StringComparison.OrdinalIgnoreCase) == true)
            riskFactors.Add(2);
        else if (action.ErrorType?.Contains("Warning", StringComparison.OrdinalIgnoreCase) == true)
            riskFactors.Add(1);

        // Analyze stack trace depth
        var stackTraceDepth = action.StackTrace?.Split('\n').Length ?? 0;
        if (stackTraceDepth > 10) riskFactors.Add(2);
        else if (stackTraceDepth > 5) riskFactors.Add(1);

        // Analyze context complexity
        var contextComplexity = action.Context?.Count ?? 0;
        if (contextComplexity > 10) riskFactors.Add(2);
        else if (contextComplexity > 5) riskFactors.Add(1);

        // Calculate average risk factor
        var averageRisk = riskFactors.Any() ? riskFactors.Average() : 1;

        // Map to RemediationRiskLevel
        return averageRisk switch
        {
            var r when r >= 2.5 => RemediationRiskLevel.Critical,
            var r when r >= 1.75 => RemediationRiskLevel.High,
            var r when r >= 1.25 => RemediationRiskLevel.Medium,
            _ => RemediationRiskLevel.Low
        };
    }

    private List<string> GeneratePotentialIssues(RemediationAction action)
    {
        // Implement potential issues generation logic
        return new List<string>();
    }

    private List<string> GenerateMitigationSteps(RemediationAction action)
    {
        // Implement mitigation steps generation logic
        return new List<string>();
    }

    private void AddValidationWarning(ValidationResult result, string message, SeverityLevel severity)
    {
        result.AddWarning(message, severity.ToValidationSeverity());
    }

    private void AddValidationError(ValidationResult result, string message, SeverityLevel severity)
    {
        result.AddError(message, severity.ToValidationSeverity());
    }

    private ValidationResult ValidateAction(RemediationAction action)
    {
        var result = new ValidationResult();

        // Check risk level
        var riskLevel = RiskAssessmentHelper.CalculateRiskLevel(action.Severity.ToSeverityLevel(), action.ImpactScope);
        if (riskLevel == RemediationRiskLevel.Critical)
        {
            AddValidationWarning(result, $"Action has {riskLevel} risk level", SeverityLevel.High);
        }

        // Check for potential issues
        foreach (var issue in action.PotentialIssues)
        {
            AddValidationWarning(result, $"Potential issue: {issue}", SeverityLevel.Medium);
        }

        return result;
    }

    private ValidationResult ValidateSuggestion(RemediationSuggestion suggestion)
    {
        var result = new ValidationResult();

        // Validate each action
        foreach (var action in suggestion.Actions)
        {
            var actionResult = ValidateAction(action);
            result.Merge(actionResult);
        }

        return result;
    }

    private ValidationResult ValidateImpact(RemediationImpact impact)
    {
        var result = new ValidationResult();

        if (impact.Severity.ToSeverityLevel() == SeverityLevel.Critical)
        {
            AddValidationError(result, "Critical impact requires additional review", SeverityLevel.Critical);
        }

        if (impact.AffectedComponents?.Count > 5)
        {
            AddValidationWarning(result, "High number of affected components", SeverityLevel.High);
        }

        return result;
    }
}
