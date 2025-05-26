using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Health;
using RemediationPlan = RuntimeErrorSage.Core.Models.Remediation.RemediationPlan;
using ValidationResult = RuntimeErrorSage.Core.Models.Validation.RemediationValidationResult;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Defines the interface for validating remediation plans and results.
    /// </summary>
    public interface IRemediationValidator : IDisposable
    {
        /// <summary>
        /// Validates a remediation plan before execution.
        /// </summary>
        /// <param name="plan">The remediation plan to validate</param>
        /// <param name="context">The error context</param>
        /// <returns>The validation result</returns>
        Task<ValidationResult> ValidatePlanAsync(RemediationPlan plan, ErrorContext context);

        /// <summary>
        /// Validates the results of a remediation execution.
        /// </summary>
        /// <param name="context">The error context</param>
        /// <returns>The validation result</returns>
        Task<ValidationResult> ValidateRemediationAsync(ErrorContext context);

        /// <summary>
        /// Validates a specific remediation step.
        /// </summary>
        /// <param name="step">The remediation step to validate</param>
        /// <param name="context">The error context</param>
        /// <returns>The validation result</returns>
        Task<ValidationResult> ValidateStepAsync(RemediationStep step, ErrorContext context);

        /// <summary>
        /// Validates a remediation strategy.
        /// </summary>
        /// <param name="strategy">The remediation strategy to validate</param>
        /// <param name="context">The error context</param>
        /// <returns>The validation result</returns>
        Task<ValidationResult> ValidateStrategyAsync(IRemediationStrategy strategy, ErrorContext context);

        /// <summary>
        /// Validates the system health after remediation.
        /// </summary>
        /// <param name="context">The error context</param>
        /// <returns>The health validation result</returns>
        Task<RemediationHealthStatus> ValidateSystemHealthAsync(ErrorContext context);
    }
} 
