using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Application.Services.Remediation
{
    /// <summary>
    /// Interface for executing remediation actions.
    /// </summary>
    public interface IRemediationActionExecutor
    {
        /// <summary>
        /// Executes a remediation action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>The result of the action execution.</returns>
        Task<RemediationResult> ExecuteActionAsync(RemediationAction action);

        /// <summary>
        /// Gets the status of a remediation action.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <returns>The action status.</returns>
        Task<RemediationResult> GetActionStatusAsync(string actionId);
    }
} 


