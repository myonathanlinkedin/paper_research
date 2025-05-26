using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Models.Common;
using RemediationPlan = RuntimeErrorSage.Core.Models.Remediation.RemediationPlan;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Defines the interface for validating remediation plans and results.
    /// </summary>
    public interface IRemediationValidator
    {
        /// <summary>
        /// Validates a remediation plan before execution.
        /// </summary>
        /// <param name="plan">The remediation plan to validate</param>
        /// <param name="context">The error context</param>
        /// <returns>The validation result</returns>
        Task<RemediationValidationResult> ValidatePlanAsync(RemediationPlan plan, ErrorContext context);

        /// <summary>
        /// Validates the results of a remediation execution.
        /// </summary>
        /// <param name="context">The error context</param>
        /// <returns>The validation result</returns>
        Task<RemediationValidationResult> ValidateRemediationAsync(ErrorContext context);

        /// <summary>
        /// Validates a specific remediation step.
        /// </summary>
        /// <param name="step">The remediation step to validate</param>
        /// <param name="context">The error context</param>
        /// <returns>The validation result</returns>
        Task<RemediationValidationResult> ValidateStepAsync(RemediationStep step, ErrorContext context);

        /// <summary>
        /// Validates a remediation strategy.
        /// </summary>
        /// <param name="strategy">The remediation strategy to validate</param>
        /// <param name="context">The error context</param>
        /// <returns>The validation result</returns>
        Task<RemediationValidationResult> ValidateStrategyAsync(IRemediationStrategy strategy, ErrorContext context);

        /// <summary>
        /// Validates the system health after remediation.
        /// </summary>
        /// <param name="context">The error context</param>
        /// <returns>The health validation result</returns>
        Task<RemediationHealthStatus> ValidateSystemHealthAsync(ErrorContext context);
    }
} 
