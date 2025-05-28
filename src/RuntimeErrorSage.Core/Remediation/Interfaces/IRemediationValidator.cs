using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Remediation.Interfaces;

/// <summary>
/// Interface for validating remediation operations.
/// </summary>
public interface IRemediationValidator
{
    /// <summary>
    /// Gets whether the validator is enabled.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Gets the validator name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the validator version.
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Validates a remediation plan.
    /// </summary>
    /// <param name="plan">The remediation plan to validate.</param>
    /// <param name="context">The error context.</param>
    /// <returns>The validation result.</returns>
    Task<Models.Validation.ValidationResult> ValidatePlanAsync(RemediationPlan plan, ErrorContext context);

    /// <summary>
    /// Validates a remediation strategy.
    /// </summary>
    /// <param name="strategy">The remediation strategy to validate.</param>
    /// <param name="context">The error context.</param>
    /// <returns>The validation result.</returns>
    Task<Models.Validation.ValidationResult> ValidateStrategyAsync(IRemediationStrategy strategy, ErrorContext context);

    /// <summary>
    /// Validates a remediation action.
    /// </summary>
    /// <param name="action">The remediation action to validate.</param>
    /// <param name="context">The error context.</param>
    /// <returns>The validation result.</returns>
    Task<Models.Validation.ValidationResult> ValidateActionAsync(RemediationAction action, ErrorContext context);

    /// <summary>
    /// Validates a remediation step.
    /// </summary>
    /// <param name="step">The remediation step to validate.</param>
    /// <param name="context">The error context.</param>
    /// <returns>The validation result.</returns>
    Task<Models.Validation.ValidationResult> ValidateStepAsync(RemediationStep step, ErrorContext context);

    /// <summary>
    /// Validates a remediation operation.
    /// </summary>
    /// <param name="analysisResult">The error analysis result.</param>
    /// <param name="context">The error context.</param>
    /// <returns>The validation result.</returns>
    Task<Models.Validation.ValidationResult> ValidateRemediationAsync(ErrorAnalysisResult analysisResult, ErrorContext context);

    /// <summary>
    /// Validates the system health after remediation.
    /// </summary>
    /// <param name="context">The error context</param>
    /// <returns>The health validation result</returns>
    Task<RemediationHealthStatus> ValidateSystemHealthAsync(ErrorContext context);
} 