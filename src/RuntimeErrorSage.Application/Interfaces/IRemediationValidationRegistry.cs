using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;
using System.ComponentModel.DataAnnotations;
using RemediationPlan = RuntimeErrorSage.Application.Models.Remediation.RemediationPlan;

namespace RuntimeErrorSage.Application.Remediation.Interfaces;

/// <summary>
/// Defines the interface for managing remediation validation rules.
/// </summary>
public interface IRemediationValidationRegistry
{
    /// <summary>
    /// Registers a validation rule.
    /// </summary>
    /// <param name="rule">The rule to register</param>
    void RegisterRule(RemediationValidationRule rule);

    /// <summary>
    /// Unregisters a validation rule.
    /// </summary>
    /// <param name="ruleId">The ID of the rule to unregister</param>
    void UnregisterRule(string ruleId);

    /// <summary>
    /// Validates a remediation plan against all registered rules.
    /// </summary>
    /// <param name="plan">The remediation plan to validate</param>
    /// <param name="context">The error context</param>
    /// <returns>The aggregated validation result</returns>
    Task<ValidationResult> ValidateAsync(RemediationPlan plan, ErrorContext context);

    /// <summary>
    /// Gets all registered validation rules.
    /// </summary>
    /// <returns>An ordered collection of validation rules</returns>
    IEnumerable<RemediationValidationRule> GetRules();

    /// <summary>
    /// Gets a specific validation rule by ID.
    /// </summary>
    /// <param name="ruleId">The ID of the rule</param>
    /// <returns>The validation rule if found, null otherwise</returns>
    RemediationValidationRule? GetRule(string ruleId);

    /// <summary>
    /// Clears the validation result cache.
    /// </summary>
    void ClearCache();
} 





