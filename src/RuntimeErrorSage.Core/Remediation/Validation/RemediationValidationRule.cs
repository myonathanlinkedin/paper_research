using System;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Models.Validation;
using RemediationPlan = RuntimeErrorSage.Core.Models.Remediation.RemediationPlan;

namespace RuntimeErrorSage.Core.Remediation.Validation;

/// <summary>
/// Base class for custom remediation validation rules.
/// </summary>
public abstract class RemediationValidationRule
{
    /// <summary>
    /// Gets the unique identifier of the validation rule.
    /// </summary>
    public string RuleId { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the name of the validation rule.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Gets the description of the validation rule.
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// Gets the priority of the validation rule (1-5, where 5 is highest).
    /// </summary>
    public abstract int Priority { get; }

    /// <summary>
    /// Gets whether the validation result can be cached.
    /// </summary>
    public virtual bool IsCacheable => true;

    /// <summary>
    /// Gets the cache duration for the validation result.
    /// </summary>
    public virtual TimeSpan CacheDuration => TimeSpan.FromMinutes(5);

    /// <summary>
    /// Validates the remediation plan against this rule.
    /// </summary>
    /// <param name="plan">The remediation plan to validate</param>
    /// <param name="context">The error context</param>
    /// <returns>The validation result</returns>
    public abstract Task<ValidationResult> ValidateAsync(RemediationPlan plan, ErrorContext context);

    /// <summary>
    /// Gets the cache key for this validation rule.
    /// </summary>
    /// <param name="plan">The remediation plan</param>
    /// <param name="context">The error context</param>
    /// <returns>The cache key</returns>
    public virtual string GetCacheKey(RemediationPlan plan, ErrorContext context)
    {
        return $"{RuleId}:{plan.GetHashCode()}:{context.GetHashCode()}";
    }
}

/// <summary>
/// Represents the result of a validation rule.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Gets or sets whether the validation was successful.
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// Gets or sets the validation message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any validation details.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();

    /// <summary>
    /// Gets or sets the validation timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the validation duration in milliseconds.
    /// </summary>
    public long DurationMs { get; set; }

    /// <summary>
    /// Gets or sets whether the result was retrieved from cache.
    /// </summary>
    public bool IsFromCache { get; set; }
} 