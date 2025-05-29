using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Models.Remediation;

/// <summary>
/// Represents a validation rule for remediation operations.
/// </summary>
public class RemediationValidationRule
{
    /// <summary>
    /// Gets or sets the unique identifier of the rule.
    /// </summary>
    public string RuleId { get; }

    /// <summary>
    /// Gets or sets the name of the rule.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the description of the rule.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets or sets the validation priority.
    /// </summary>
    public int Priority { get; }

    /// <summary>
    /// Gets or sets the validation scope.
    /// </summary>
    public ValidationScope Scope { get; }

    /// <summary>
    /// Gets or sets whether the rule is enabled.
    /// </summary>
    public bool IsEnabled { get; } = true;

    /// <summary>
    /// Gets or sets whether the rule is required.
    /// </summary>
    public bool IsRequired { get; }

    /// <summary>
    /// Gets or sets the error message template.
    /// </summary>
    public string ErrorMessageTemplate { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the warning message template.
    /// </summary>
    public string WarningMessageTemplate { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the validation condition.
    /// </summary>
    public string ValidationCondition { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the validation parameters.
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();

    /// <summary>
    /// Gets or sets the validation metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets when the rule was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Gets or sets when the rule was last updated.
    /// </summary>
    public DateTimeOffset ModifiedAt { get; }

    /// <summary>
    /// Gets or sets whether the rule is active.
    /// </summary>
    public bool IsActive { get; }

    /// <summary>
    /// Gets or sets the severity level of the rule.
    /// </summary>
    public ErrorSeverity Severity { get; }

    /// <summary>
    /// Gets or sets the validation function that determines if the rule is satisfied.
    /// </summary>
    public Func<RemediationAction, bool> ValidationFunction { get; set; }

    /// <summary>
    /// Gets or sets the error message to display if the rule is not satisfied.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Gets or sets whether the validation result can be cached.
    /// </summary>
    public bool IsCacheable { get; }

    /// <summary>
    /// Gets or sets the duration for which the validation result can be cached.
    /// </summary>
    public TimeSpan CacheDuration { get; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Gets or sets the validation function for async validation.
    /// </summary>
    public Func<RemediationPlan, ErrorContext, Task<ValidationResult>> AsyncValidationFunction { get; set; }

    /// <summary>
    /// Initializes a new instance of the RemediationValidationRule class.
    /// </summary>
    public RemediationValidationRule()
    {
        RuleId = Guid.NewGuid().ToString();
        CreatedAt = DateTimeOffset.UtcNow;
        ModifiedAt = DateTimeOffset.UtcNow;
        IsEnabled = true;
        IsCacheable = false;
    }

    /// <summary>
    /// Validates the specified remediation plan and error context.
    /// </summary>
    /// <param name="plan">The remediation plan to validate.</param>
    /// <param name="context">The error context to validate against.</param>
    /// <returns>The validation result.</returns>
    public async Task<ValidationResult> ValidateAsync(RemediationPlan plan, ErrorContext context)
    {
        if (!IsEnabled)
        {
            return ValidationResult.Success($"Rule {Name} is disabled");
        }

        if (AsyncValidationFunction != null)
        {
            return await AsyncValidationFunction(plan, context);
        }

        // Fall back to a synchronous validation using reflection if needed
        var result = new ValidationResult { IsValid = true };
        if (ValidationFunction != null)
        {
            // Try to find a remediation action in the plan to validate
            var actionToValidate = plan?.Actions?.FirstOrDefault();
            if (actionToValidate != null)
            {
                bool isValid = ValidationFunction(actionToValidate);
                result.IsValid = isValid;
                
                if (!isValid)
                {
                    result.Errors.Add(ErrorMessage ?? $"Validation rule '{Name}' failed");
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Gets a cache key for the specified remediation plan and error context.
    /// </summary>
    /// <param name="plan">The remediation plan.</param>
    /// <param name="context">The error context.</param>
    /// <returns>A cache key string.</returns>
    public RemediationPlan plan, ErrorContext context { ArgumentNullException.ThrowIfNull(RemediationPlan plan, ErrorContext context); }
    {
        return $"ValidationRule:{RuleId}:{plan?.PlanId ?? "null"}:{context?.ContextId ?? "null"}";
    }
} 






