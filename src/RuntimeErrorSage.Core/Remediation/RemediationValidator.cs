using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Health;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using System.Collections.Concurrent;
using CoreIRemediationValidator = RuntimeErrorSage.Core.Interfaces.IRemediationValidator;
using RemediationIRemediationValidator = RuntimeErrorSage.Core.Remediation.Interfaces.IRemediationValidator;
using ValidationResult = RuntimeErrorSage.Core.Models.Validation.RemediationValidationResult;

namespace RuntimeErrorSage.Core.Remediation;

public class RemediationValidator : CoreIRemediationValidator, RemediationIRemediationValidator, IDisposable
{
    private readonly ILogger<RemediationValidator> _logger;
    private readonly IReadOnlyCollection<IValidationRule> _rules;
    private readonly IReadOnlyCollection<IValidationWarning> _warnings;
    private readonly RemediationValidatorOptions _options;
    private readonly CancellationTokenSource _globalCts;
    private bool _disposed;

    public bool IsEnabled => !_disposed && _options.IsEnabled;
    public string Name => "RuntimeErrorSage Remediation Validator";
    public string Version => "1.0.0";

    private static readonly Action<ILogger, string, Exception?> LogValidationError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(ValidateRemediationAsync)),
            "Validation error: {Message}");

    public RemediationValidator(
        ILogger<RemediationValidator> logger,
        IOptions<RemediationValidatorOptions> options,
        IEnumerable<IValidationRule> rules,
        IEnumerable<IValidationWarning> warnings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _rules = rules?.ToList() ?? throw new ArgumentNullException(nameof(rules));
        _warnings = warnings?.ToList() ?? throw new ArgumentNullException(nameof(warnings));
        _globalCts = new CancellationTokenSource();
    }

    public async Task<RemediationValidationResult> ValidateRemediationAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        var result = new RemediationValidationResult
        {
            IsValid = true,
            Timestamp = DateTime.UtcNow
        };

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            foreach (var rule in _rules)
            {
                var ruleResult = await rule.ValidateAsync(context, timeoutCts.Token).ConfigureAwait(false);
                if (!ruleResult.IsValid)
                {
                    result.IsValid = false;
                    result.Errors.Add(new ValidationError
                    {
                        Code = ruleResult.Code,
                        Message = ruleResult.Message,
                        Severity = ruleResult.Severity,
                        PropertyName = ruleResult.PropertyName,
                        Timestamp = DateTime.UtcNow
                    });
                }
            }

            foreach (var warning in _warnings)
            {
                var warningResult = await warning.ValidateAsync(context, timeoutCts.Token).ConfigureAwait(false);
                if (!warningResult.IsValid)
                {
                    result.Warnings.Add(new ValidationWarning
                    {
                        Code = warningResult.Code,
                        Message = warningResult.Message,
                        Severity = warningResult.Severity,
                        WarningId = Guid.NewGuid().ToString(),
                        Timestamp = DateTime.UtcNow
                    });
                }
            }
        }
        catch (OperationCanceledException) when (!_globalCts.Token.IsCancellationRequested)
        {
            result.IsValid = false;
            result.Errors.Add(new ValidationError
            {
                Code = "ValidationTimeout",
                Message = "Validation timed out",
                Severity = SeverityLevel.Error,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            LogValidationError(_logger, ex.Message, ex);
            result.IsValid = false;
            result.Errors.Add(new ValidationError
            {
                Code = "ValidationError",
                Message = ex.Message,
                Severity = SeverityLevel.Error,
                Timestamp = DateTime.UtcNow
            });
        }

        return result;
    }

    public async Task<RemediationValidationResult> ValidatePlanAsync(RemediationPlan plan, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        var result = new RemediationValidationResult
        {
            IsValid = true,
            Timestamp = DateTime.UtcNow
        };

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            // Validate plan properties
            if (string.IsNullOrEmpty(plan.PlanId))
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Code = "InvalidPlanId",
                    Message = "Plan ID is required",
                    Severity = SeverityLevel.Error,
                    Timestamp = DateTime.UtcNow
                });
            }

            if (plan.Steps == null || !plan.Steps.Any())
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Code = "NoSteps",
                    Message = "Plan must contain at least one step",
                    Severity = SeverityLevel.Error,
                    Timestamp = DateTime.UtcNow
                });
            }

            // Validate each step
            if (plan.Steps != null)
            {
                foreach (var step in plan.Steps)
                {
                    var stepResult = await ValidateStepAsync(step, context).ConfigureAwait(false);
                    if (!stepResult.IsValid)
                    {
                        result.IsValid = false;
                        result.Errors.AddRange(stepResult.Errors);
                    }
                    result.Warnings.AddRange(stepResult.Warnings);
                }
            }

            // Validate step dependencies
            if (plan.Steps != null)
            {
                var stepIds = new HashSet<string>();
                foreach (var step in plan.Steps)
                {
                    if (!stepIds.Add(step.StepId))
                    {
                        result.IsValid = false;
                        result.Errors.Add(new ValidationError
                        {
                            Code = "DuplicateStepId",
                            Message = $"Duplicate step ID found: {step.StepId}",
                            Severity = SeverityLevel.Error,
                            Timestamp = DateTime.UtcNow
                        });
                    }
                }
            }
        }
        catch (OperationCanceledException) when (!_globalCts.Token.IsCancellationRequested)
        {
            result.IsValid = false;
            result.Errors.Add(new ValidationError
            {
                Code = "ValidationTimeout",
                Message = "Plan validation timed out",
                Severity = SeverityLevel.Error,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            LogValidationError(_logger, ex.Message, ex);
            result.IsValid = false;
            result.Errors.Add(new ValidationError
            {
                Code = "PlanValidationError",
                Message = ex.Message,
                Severity = SeverityLevel.Error,
                Timestamp = DateTime.UtcNow
            });
        }

        return result;
    }

    public async Task<RemediationValidationResult> ValidateStepAsync(RemediationStep step, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(step);
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        var result = new RemediationValidationResult
        {
            IsValid = true,
            Timestamp = DateTime.UtcNow
        };

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            if (string.IsNullOrEmpty(step.StepId))
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Code = "InvalidStepId",
                    Message = "Step ID is required",
                    Severity = SeverityLevel.Error,
                    Timestamp = DateTime.UtcNow
                });
            }

            if (step.Action == null)
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Code = "NoAction",
                    Message = "Step must have an action",
                    Severity = SeverityLevel.Error,
                    Timestamp = DateTime.UtcNow
                });
            }
            else if (_options.EnableStrictValidation)
            {
                if (!_options.AllowedStepTypes.TryGetValue(step.Action.Type.ToLowerInvariant(), out var allowedParams))
                {
                    result.IsValid = false;
                    result.Errors.Add(new ValidationError
                    {
                        Code = "InvalidActionType",
                        Message = $"Action type '{step.Action.Type}' is not allowed",
                        Severity = SeverityLevel.Error,
                        Timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    // Validate action parameters
                    foreach (var param in step.Action.Parameters)
                    {
                        if (!allowedParams.Contains(param.Key.ToLowerInvariant()))
                        {
                            result.Warnings.Add(new ValidationWarning
                            {
                                Code = "UnknownParameter",
                                Message = $"Unknown parameter '{param.Key}' for action type '{step.Action.Type}'",
                                Severity = SeverityLevel.Warning,
                                Timestamp = DateTime.UtcNow
                            });
                        }
                    }
                }
            }
        }
        catch (OperationCanceledException) when (!_globalCts.Token.IsCancellationRequested)
        {
            result.IsValid = false;
            result.Errors.Add(new ValidationError
            {
                Code = "ValidationTimeout",
                Message = "Step validation timed out",
                Severity = SeverityLevel.Error,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            LogValidationError(_logger, ex.Message, ex);
            result.IsValid = false;
            result.Errors.Add(new ValidationError
            {
                Code = "StepValidationError",
                Message = ex.Message,
                Severity = SeverityLevel.Error,
                Timestamp = DateTime.UtcNow
            });
        }

        return result;
    }

    public async Task<RemediationValidationResult> ValidateStrategyAsync(IRemediationStrategy strategy, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(strategy);
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        var result = new RemediationValidationResult
        {
            IsValid = true,
            Timestamp = DateTime.UtcNow
        };

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            // Validate strategy properties
            if (string.IsNullOrEmpty(strategy.Name))
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Code = "InvalidStrategyName",
                    Message = "Strategy name is required",
                    Severity = SeverityLevel.Error,
                    Timestamp = DateTime.UtcNow
                });
            }

            if (string.IsNullOrEmpty(strategy.Description))
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Code = "InvalidStrategyDescription",
                    Message = "Strategy description is required",
                    Severity = SeverityLevel.Error,
                    Timestamp = DateTime.UtcNow
                });
            }

            if (strategy.Priority < 1 || strategy.Priority > 5)
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Code = "InvalidStrategyPriority",
                    Message = "Strategy priority must be between 1 and 5",
                    Severity = SeverityLevel.Error,
                    Timestamp = DateTime.UtcNow
                });
            }

            // Validate strategy parameters
            if (strategy.Parameters == null)
            {
                result.Warnings.Add(new ValidationWarning
                {
                    Code = "NoParameters",
                    Message = "Strategy has no parameters defined",
                    Severity = SeverityLevel.Warning,
                    Timestamp = DateTime.UtcNow
                });
            }
            else if (!_options.AllowedStrategyTypes.TryGetValue(strategy.Name.ToLowerInvariant(), out var allowedParams))
            {
                result.Warnings.Add(new ValidationWarning
                {
                    Code = "UnknownStrategyType",
                    Message = $"Strategy type '{strategy.Name}' is not in the allowed list",
                    Severity = SeverityLevel.Warning,
                    Timestamp = DateTime.UtcNow
                });
            }
            else
            {
                foreach (var param in allowedParams)
                {
                    if (!strategy.Parameters.ContainsKey(param))
                    {
                        result.Warnings.Add(new ValidationWarning
                        {
                            Code = "MissingParameter",
                            Message = $"Required parameter '{param}' is missing",
                            Severity = SeverityLevel.Warning,
                            Timestamp = DateTime.UtcNow
                        });
                    }
                }
            }
        }
        catch (OperationCanceledException) when (!_globalCts.Token.IsCancellationRequested)
        {
            result.IsValid = false;
            result.Errors.Add(new ValidationError
            {
                Code = "ValidationTimeout",
                Message = "Strategy validation timed out",
                Severity = SeverityLevel.Error,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            LogValidationError(_logger, ex.Message, ex);
            result.IsValid = false;
            result.Errors.Add(new ValidationError
            {
                Code = "ValidationError",
                Message = ex.Message,
                Severity = SeverityLevel.Error,
                Timestamp = DateTime.UtcNow
            });
        }

        return result;
    }

    public async Task<HealthStatusInfo> ValidateSystemHealthAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        var healthStatus = new HealthStatusInfo
        {
            IsHealthy = true,
            StatusMessage = "System health check completed successfully",
            LastCheckTime = DateTime.UtcNow
        };

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_globalCts.Token);
            timeoutCts.CancelAfter(_options.ValidationTimeout);

            // Validate system metrics
            var metricsValidation = await ValidateMetricsAsync(context, timeoutCts.Token).ConfigureAwait(false);
            healthStatus.IsHealthy &= metricsValidation.IsHealthy;
            healthStatus.Metrics = metricsValidation.Metrics;

            if (!metricsValidation.IsHealthy)
            {
                healthStatus.Warnings.Add("System metrics exceeded thresholds");
            }

            // Add system details
            healthStatus.Details["os_version"] = Environment.OSVersion.ToString();
            healthStatus.Details["machine_name"] = Environment.MachineName;
            healthStatus.Details["processor_count"] = Environment.ProcessorCount;
        }
        catch (OperationCanceledException) when (!_globalCts.Token.IsCancellationRequested)
        {
            healthStatus.IsHealthy = false;
            healthStatus.Errors.Add("Health check timed out");
        }
        catch (Exception ex)
        {
            LogValidationError(_logger, ex.Message, ex);
            healthStatus.IsHealthy = false;
            healthStatus.Errors.Add($"Health check error: {ex.Message}");
        }

        return healthStatus;
    }

    private async Task<MetricsValidationResult> ValidateMetricsAsync(ErrorContext context, CancellationToken cancellationToken)
    {
        var result = new MetricsValidationResult
        {
            IsHealthy = true,
            HealthScore = 1.0,
            Metrics = new Dictionary<string, double>()
        };

        try
        {
            var metrics = await CollectMetricsAsync(context, cancellationToken).ConfigureAwait(false);
            foreach (var (key, value) in metrics)
            {
                var threshold = _options.DefaultMetricThreshold;
                result.Metrics[key] = value;

                if (value > threshold)
                {
                    result.IsHealthy = false;
                    result.HealthScore *= 0.8; // Reduce health score by 20% for each threshold exceeded
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating metrics");
            result.IsHealthy = false;
            result.HealthScore = 0.0;
        }

        return result;
    }

    private async Task<Dictionary<string, double>> CollectMetricsAsync(ErrorContext context, CancellationToken cancellationToken)
    {
        // This is a placeholder implementation. In a real system, you would collect actual metrics.
        var metrics = new Dictionary<string, double>
        {
            ["cpu_usage"] = Random.Shared.NextDouble() * 100,
            ["memory_usage"] = Random.Shared.NextDouble() * 100,
            ["disk_usage"] = Random.Shared.NextDouble() * 100
        };

        return metrics;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _globalCts.Cancel();
                _globalCts.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(RemediationValidator));
        }
    }

    Task<Models.Remediation.RemediationValidationResult> CoreIRemediationValidator.ValidatePlanAsync(RemediationPlan plan, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        return ValidatePlanAsync(plan, context);
    }

    Task<Models.Remediation.RemediationValidationResult> CoreIRemediationValidator.ValidateRemediationAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        return ValidateRemediationAsync(context);
    }

    Task<Models.Remediation.RemediationValidationResult> CoreIRemediationValidator.ValidateStepAsync(RemediationStep remediationStep, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(remediationStep);
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        return ValidateStepAsync(remediationStep, context);
    }

    Task<RemediationHealthStatus> RemediationIRemediationValidator.ValidateSystemHealthAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        return ValidateSystemHealthAsync(context).ContinueWith(t => new RemediationHealthStatus
        {
            IsHealthy = t.Result.IsHealthy,
            HealthScore = t.Result.HealthScore,
            Timestamp = t.Result.Timestamp,
            Details = t.Result.Details,
            Issues = t.Result.Issues,
            Metrics = t.Result.Metrics
        });
    }
}

public class MetricsValidationResult
{
    public required bool IsHealthy { get; set; }
    public required double HealthScore { get; set; }
    public required Dictionary<string, double> Metrics { get; set; } = new();
}