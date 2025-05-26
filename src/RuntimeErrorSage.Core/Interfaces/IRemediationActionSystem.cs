using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Defines the interface for the remediation action system.
    /// </summary>
    public interface IRemediationActionSystem
    {
        /// <summary>
        /// Executes a remediation action for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <param name="action">The remediation action to execute.</param>
        /// <returns>The result of the remediation action.</returns>
        Task<RemediationResult> ExecuteRemediationAsync(ErrorContext context, RemediationAction action);
    }
} 