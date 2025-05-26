using System.Threading.Tasks;
using CodeSage.Core.Remediation.Models.Execution;

namespace CodeSage.Core.Interfaces
{
    /// <summary>
    /// Defines the interface for tracking remediation executions.
    /// </summary>
    public interface IRemediationTracker
    {
        /// <summary>
        /// Tracks a remediation execution.
        /// </summary>
        /// <param name="execution">The execution to track</param>
        Task TrackRemediationAsync(RemediationExecution execution);

        /// <summary>
        /// Gets a remediation execution by ID.
        /// </summary>
        /// <param name="executionId">The execution ID</param>
        /// <returns>The execution, or null if not found</returns>
        Task<RemediationExecution?> GetRemediationAsync(string remediationId);
    }
} 