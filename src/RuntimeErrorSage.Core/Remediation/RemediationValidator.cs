using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Health;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using System.Collections.Concurrent;
using IRemediationValidator = RuntimeErrorSage.Core.Remediation.Interfaces.IRemediationValidator;
using ValidationResult = RuntimeErrorSage.Core.Models.Validation.ValidationResult;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Graph;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Validation.Interfaces;
using RuntimeErrorSage.Core.Analysis.Interfaces;

namespace RuntimeErrorSage.Core.Remediation;

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
        IEnumerable<IValidationRule> rules,
        IEnumerable<IValidationWarning> warnings,
        IErrorContextAnalyzer errorContextAnalyzer,
        IRemediationMetricsCollector metricsCollector,
        IRemediationRegistry registry,
        ILLMClient llmClient,
        Dictionary<string, IRemediationActionValidator> supportedActions,
        EventId? validationErrorEventId = null)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _rules = rules?.ToList() ?? throw new ArgumentNullException(nameof(rules));
        _warnings = warnings?.ToList() ?? throw new ArgumentNullException(nameof(warnings));
        _globalCts = new CancellationTokenSource();
        _errorContextAnalyzer = errorContextAnalyzer ?? throw new ArgumentNullException(nameof(errorContextAnalyzer));
        _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
        _supportedActions = supportedActions ?? throw new ArgumentNullException(nameof(supportedActions));
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
            CorrelationId = context.CorrelationId,
            Timestamp = DateTime.UtcNow
        };

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
                ValidationMessage = $"Error validating plan: {ex.Message}",
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
            CorrelationId = context.CorrelationId,
            Timestamp = DateTime.UtcNow
        };

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
                ValidationMessage = $"Error validating strategy: {ex.Message}",
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
            CorrelationId = context.CorrelationId,
            Timestamp = DateTime.UtcNow
        };

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
                ValidationMessage = $"Error validating action: {ex.Message}",
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
            CorrelationId = context.CorrelationId,
            Timestamp = DateTime.UtcNow
        };

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
                ValidationMessage = $"Error validating step: {ex.Message}",
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
            CorrelationId = context.CorrelationId,
            Timestamp = DateTime.UtcNow
        };

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            // Validate analysis result properties
            if (string.IsNullOrEmpty(analysisResult.AnalysisId))
            {
                result.IsValid = false;
                result.ValidationMessage = "Analysis ID is required";
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
                result.ValidationMessage = "Error context is required";
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
                        result.ValidationMessage = $"Invalid strategy: {strategyResult.ValidationMessage}";
                        result.Details = strategyResult.Details;
                        result.Errors.Add(new ValidationError 
                        { 
                            ErrorId = Guid.NewGuid().ToString(),
                            Message = strategyResult.ValidationMessage,
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
            _logger.LogError(ex, "Error validating remediation");
            return new ValidationResult
            {
                IsValid = false,
                ValidationMessage = $"Error validating remediation: {ex.Message}",
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
}