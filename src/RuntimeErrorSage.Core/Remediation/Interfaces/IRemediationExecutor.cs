using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Models.Execution;
using RuntimeErrorSage.Core.Remediation.Models.Validation;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Defines the interface for executing remediation actions.
    /// </summary>
    public interface IRemediationExecutor
    {
        /// <summary>
        /// Executes a remediation action based on the provided analysis and context.
        /// </summary>
        /// <param name="analysis">The error analysis result</param>
        /// <param name="context">The error context</param>
        /// <returns>The remediation execution result</returns>
        Task<RemediationExecution> ExecuteRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context);

        /// <summary>
        /// Gets the status of a remediation execution.
        /// </summary>
        /// <param name="remediationId">The unique identifier of the remediation execution</param>
        /// <returns>The remediation execution status or null if not found</returns>
        Task<RemediationExecution?> GetRemediationStatusAsync(string remediationId);

        /// <summary>
        /// Cancels an ongoing remediation execution.
        /// </summary>
        /// <param name="remediationId">The unique identifier of the remediation execution</param>
        /// <returns>True if the cancellation was successful, false otherwise</returns>
        Task<bool> CancelRemediationAsync(string remediationId);

        /// <summary>
        /// Validates a remediation action before execution.
        /// </summary>
        /// <param name="analysis">The error analysis result</param>
        /// <param name="context">The error context</param>
        /// <returns>The validation result</returns>
        Task<RemediationValidationResult> ValidateRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context);

        /// <summary>
        /// Gets the execution history for a specific remediation.
        /// </summary>
        /// <param name="remediationId">The remediation ID</param>
        /// <returns>The execution history</returns>
        Task<List<RemediationExecution>> GetExecutionHistoryAsync(string remediationId);

        /// <summary>
        /// Gets the current execution metrics.
        /// </summary>
        /// <param name="remediationId">The remediation ID</param>
        /// <returns>The execution metrics</returns>
        Task<RemediationMetrics> GetExecutionMetricsAsync(string remediationId);
    }
} 
