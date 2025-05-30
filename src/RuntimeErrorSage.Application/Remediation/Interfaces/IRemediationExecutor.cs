using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Error;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    /// <summary>
    /// Defines the interface for executing remediation actions.
    /// </summary>
    public interface IRemediationExecutor
    {
        /// <summary>
        /// Executes a remediation action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The result of the execution.</returns>
        Task<RemediationResult> ExecuteActionAsync(RemediationAction action, ErrorContext context);

        /// <summary>
        /// Executes a remediation plan.
        /// </summary>
        /// <param name="plan">The plan to execute.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The result of the execution.</returns>
        Task<RemediationResult> ExecuteRemediationAsync(RemediationPlan plan, ErrorContext context);

        /// <summary>
        /// Rolls back a remediation action.
        /// </summary>
        /// <param name="actionId">The ID of the action to roll back.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The result of the rollback.</returns>
        Task<RemediationResult> RollbackActionAsync(string actionId, ErrorContext context);

        /// <summary>
        /// Gets the status of a remediation action.
        /// </summary>
        /// <param name="actionId">The ID of the action.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The status of the action.</returns>
        Task<RemediationResult> GetActionStatusAsync(string actionId, ErrorContext context);
    }
} 