using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Interfaces;

/// <summary>
/// Interface for remediation validation.
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
    /// <param name="plan">The remediation plan.</param>
    /// <param name="context">The error context.</param>
    /// <returns>The validation result.</returns>
    Task<RemediationValidationResult> ValidatePlanAsync(RemediationPlan plan, ErrorContext context);

    /// <summary>
    /// Validates a remediation step.
    /// </summary>
    /// <param name="step">The remediation step.</param>
    /// <param name="context">The error context.</param>
    /// <returns>The validation result.</returns>
    Task<RemediationValidationResult> ValidateStepAsync(RemediationStep step, ErrorContext context);

    /// <summary>
    /// Validates a remediation strategy.
    /// </summary>
    /// <param name="strategy">The remediation strategy.</param>
    /// <param name="context">The error context.</param>
    /// <returns>The validation result.</returns>
    Task<RemediationValidationResult> ValidateStrategyAsync(IRemediationStrategy strategy, ErrorContext context);

    /// <summary>
    /// Validates a remediation.
    /// </summary>
    /// <param name="context">The error context.</param>
    /// <returns>The validation result.</returns>
    Task<RemediationValidationResult> ValidateRemediationAsync(ErrorContext context);
} 