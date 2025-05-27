using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for collecting remediation metrics.
    /// </summary>
    public interface IRemediationMetricsCollector
    {
        /// <summary>
        /// Gets the execution history for a remediation.
        /// </summary>
        /// <param name="remediationId">The remediation ID.</param>
        /// <returns>The list of execution history entries.</returns>
        Task<IEnumerable<RemediationExecution>> GetExecutionHistoryAsync(string remediationId);
    }
} 