using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Options;
using System.Text.Json;
using RuntimeErrorSage.Core.Remediation.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Models.Common;
using IRemediationValidator = RuntimeErrorSage.Core.Remediation.Interfaces.IRemediationValidator;
using ValidationResult = RuntimeErrorSage.Core.Models.Validation.ValidationResult;
using RemediationValidationResult = RuntimeErrorSage.Core.Models.Remediation.RemediationValidationResult;
using RemediationHealthStatus = RuntimeErrorSage.Core.Remediation.Models.Common.RemediationHealthStatus;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RemediationPlan = RuntimeErrorSage.Core.Models.Remediation.RemediationPlan;

namespace RuntimeErrorSage.Core.Remediation;

public class RemediationValidatorOptions
{
    public bool EnableStrictValidation { get; set; } = true;
    public TimeSpan ValidationTimeout { get; set; } = TimeSpan.FromMinutes(2);
    public int MaxValidationRetries { get; set; } = 3;
    public Dictionary<string, string[]> AllowedStepTypes { get; set; } = new()
    {
        ["restart"] = new[] { "service" },
        ["clear"] = new[] { "resource" },
        ["update"] = new[] { "component", "version" },
        ["script"] = new[] { "script", "timeout" }
    };
    public Dictionary<string, string[]> AllowedStrategyTypes { get; set; } = new()
    {
        ["monitor"] = new[] { "metric", "threshold" },
        ["alert"] = new[] { "channel", "severity" },
        ["backup"] = new[] { "target", "schedule" }
    };
}

/// <summary>
/// Validates remediation plans and actions.
/// </summary>
public class RemediationValidator : IRemediationValidator
{
    private readonly ILogger<RemediationValidator> _logger;

    public RemediationValidator(ILogger<RemediationValidator> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<RemediationValidationResult> ValidatePlanAsync(RemediationPlan plan, ErrorContext context)
    {
        if (plan == null)
        {
            throw new ArgumentNullException(nameof(plan));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            var result = new RemediationValidationResult
            {
                IsValid = true,
                Status = ValidationStatus.Valid,
                Timestamp = DateTime.UtcNow
            };

            // Validate plan structure
            if (plan.Steps == null || plan.Steps.Count == 0)
            {
                result.IsValid = false;
                result.Status = ValidationStatus.Invalid;
                result.Errors.Add(new ValidationError
                {
                    Code = "PLAN_NO_STEPS",
                    Message = "Remediation plan must contain at least one step",
                    Severity = SeverityLevel.Error
                });
            }

            // Validate step dependencies
            foreach (var step in plan.Steps)
            {
                if (step.Dependencies != null)
                {
                    foreach (var depId in step.Dependencies)
                    {
                        if (!plan.Steps.Exists(s => s.StepId == depId))
                        {
                            result.IsValid = false;
                            result.Status = ValidationStatus.Invalid;
                            result.Errors.Add(new ValidationError
                            {
                                Code = "STEP_INVALID_DEPENDENCY",
                                Message = $"Step {step.StepId} depends on non-existent step {depId}",
                                Severity = SeverityLevel.Error
                            });
                        }
                    }
                }
            }

            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating remediation plan");
            throw;
        }
    }

    public async Task<RemediationValidationResult> ValidateRemediationAsync(ErrorContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            var result = new RemediationValidationResult
            {
                IsValid = true,
                Status = ValidationStatus.Valid,
                Timestamp = DateTime.UtcNow
            };

            // Validate error context
            if (string.IsNullOrEmpty(context.ErrorType))
            {
                result.IsValid = false;
                result.Status = ValidationStatus.Invalid;
                result.Errors.Add(new ValidationError
                {
                    Code = "CONTEXT_NO_ERROR_TYPE",
                    Message = "Error context must specify an error type",
                    Severity = SeverityLevel.Error
                });
            }

            // Validate error message
            if (string.IsNullOrEmpty(context.ErrorMessage))
            {
                result.IsValid = false;
                result.Status = ValidationStatus.Invalid;
                result.Errors.Add(new ValidationError
                {
                    Code = "CONTEXT_NO_ERROR_MESSAGE",
                    Message = "Error context must specify an error message",
                    Severity = SeverityLevel.Error
                });
            }

            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating remediation");
            throw;
        }
    }

    public async Task<RemediationValidationResult> ValidateStepAsync(RemediationStep step, ErrorContext context)
    {
        if (step == null)
        {
            throw new ArgumentNullException(nameof(step));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            var result = new RemediationValidationResult
            {
                IsValid = true,
                Status = ValidationStatus.Valid,
                Timestamp = DateTime.UtcNow
            };

            // Validate step ID
            if (string.IsNullOrEmpty(step.StepId))
            {
                result.IsValid = false;
                result.Status = ValidationStatus.Invalid;
                result.Errors.Add(new ValidationError
                {
                    Code = "STEP_NO_ID",
                    Message = "Step must have an ID",
                    Severity = SeverityLevel.Error
                });
            }

            // Validate step name
            if (string.IsNullOrEmpty(step.Name))
            {
                result.IsValid = false;
                result.Status = ValidationStatus.Invalid;
                result.Errors.Add(new ValidationError
                {
                    Code = "STEP_NO_NAME",
                    Message = "Step must have a name",
                    Severity = SeverityLevel.Error
                });
            }

            // Validate step action
            if (string.IsNullOrEmpty(step.Action))
            {
                result.IsValid = false;
                result.Status = ValidationStatus.Invalid;
                result.Errors.Add(new ValidationError
                {
                    Code = "STEP_NO_ACTION",
                    Message = "Step must specify an action",
                    Severity = SeverityLevel.Error
                });
            }

            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating remediation step");
            throw;
        }
    }

    public async Task<RemediationValidationResult> ValidateStrategyAsync(IRemediationStrategy strategy, ErrorContext context)
    {
        if (strategy == null)
        {
            throw new ArgumentNullException(nameof(strategy));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            var result = new RemediationValidationResult
            {
                IsValid = true,
                Status = ValidationStatus.Valid,
                Timestamp = DateTime.UtcNow
            };

            // Validate strategy name
            if (string.IsNullOrEmpty(strategy.Name))
            {
                result.IsValid = false;
                result.Status = ValidationStatus.Invalid;
                result.Errors.Add(new ValidationError
                {
                    Code = "STRATEGY_NO_NAME",
                    Message = "Strategy must have a name",
                    Severity = SeverityLevel.Error
                });
            }

            // Validate strategy description
            if (string.IsNullOrEmpty(strategy.Description))
            {
                result.IsValid = false;
                result.Status = ValidationStatus.Invalid;
                result.Errors.Add(new ValidationError
                {
                    Code = "STRATEGY_NO_DESCRIPTION",
                    Message = "Strategy must have a description",
                    Severity = SeverityLevel.Error
                });
            }

            // Validate strategy priority
            if (strategy.Priority < 1 || strategy.Priority > 5)
            {
                result.IsValid = false;
                result.Status = ValidationStatus.Invalid;
                result.Errors.Add(new ValidationError
                {
                    Code = "STRATEGY_INVALID_PRIORITY",
                    Message = "Strategy priority must be between 1 and 5",
                    Severity = SeverityLevel.Error
                });
            }

            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating remediation strategy");
            throw;
        }
    }

    public async Task<RemediationHealthStatus> ValidateSystemHealthAsync(ErrorContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            var result = new RemediationHealthStatus
            {
                IsHealthy = true,
                Timestamp = DateTime.UtcNow
            };

            // Validate system state
            if (context.SystemState != null)
            {
                foreach (var (key, value) in context.SystemState)
                {
                    if (value is bool boolValue && !boolValue)
                    {
                        result.IsHealthy = false;
                        result.Issues.Add(new HealthIssue
                        {
                            Component = key,
                            Message = $"Component {key} is not healthy",
                            Severity = SeverityLevel.Error
                        });
                    }
                }
            }

            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating system health");
            throw;
        }
    }
}

public class SystemHealthStatus
{
    public bool IsHealthy { get; set; }
    public Dictionary<string, object> Details { get; set; } = new();
}

public class HealthStatus
{
    public bool IsHealthy { get; set; }
    public Dictionary<string, object> Details { get; set; } = new();
}

public class MetricsValidation
{
    public bool IsWithinThresholds { get; set; }
    public Dictionary<string, object> Details { get; set; } = new();
}

public class RemediationValidationException : Exception
{
    public RemediationValidationException(string message, Exception inner) : base(message, inner) { }
    public RemediationValidationException(string message) : base(message) { }
} 
