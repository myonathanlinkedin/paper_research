using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Execution;
using RuntimeErrorSage.Application.Models.Metrics;
using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for tracking remediation progress and status.
    /// </summary>
    public interface IRemediationTracker
    {
        /// <summary>
        /// Gets the current status of a remediation plan.
        /// </summary>
        /// <param name="planId">The ID of the remediation plan.</param>
        /// <returns>The current status of the plan.</returns>
        Task<RemediationStatusEnum> GetStatusAsync(string planId);

        /// <summary>
        /// Updates the status of a remediation plan.
        /// </summary>
        /// <param name="planId">The ID of the remediation plan.</param>
        /// <param name="status">The new status.</param>
        /// <param name="details">Optional details about the status change.</param>
        Task UpdateStatusAsync(string planId, RemediationStatusEnum status, string? details = null);

        /// <summary>
        /// Records a step in the remediation process.
        /// </summary>
        /// <param name="planId">The ID of the remediation plan.</param>
        /// <param name="step">The remediation step to record.</param>
        Task RecordStepAsync(string planId, RemediationStep step);

        /// <summary>
        /// Gets the history of steps for a remediation plan.
        /// </summary>
        /// <param name="planId">The ID of the remediation plan.</param>
        /// <returns>The list of steps in the remediation process.</returns>
        Task<IEnumerable<RemediationStep>> GetStepHistoryAsync(string planId);

        /// <summary>
        /// Gets the metrics for a remediation plan.
        /// </summary>
        /// <param name="planId">The ID of the remediation plan.</param>
        /// <returns>The remediation metrics.</returns>
        Task<RemediationMetrics> GetMetricsAsync(string planId);

        /// <summary>
        /// Records metrics for a remediation plan.
        /// </summary>
        /// <param name="planId">The ID of the remediation plan.</param>
        /// <param name="metrics">The metrics to record.</param>
        Task RecordMetricsAsync(string planId, RemediationMetrics metrics);

        /// <summary>
        /// Tracks a remediation execution.
        /// </summary>
        /// <param name="execution">The remediation execution to track.</param>
        Task TrackRemediationAsync(RemediationExecution execution);

        /// <summary>
        /// Gets the execution details for a remediation.
        /// </summary>
        /// <param name="remediationId">The ID of the remediation.</param>
        /// <returns>The remediation execution details.</returns>
        Task<RemediationExecution> GetExecutionAsync(string remediationId);

        /// <summary>
        /// Gets the execution history for all remediations.
        /// </summary>
        /// <returns>The list of remediation executions.</returns>
        Task<IEnumerable<RemediationExecution>> GetExecutionHistoryAsync();

        /// <summary>
        /// Tracks the start of a remediation action.
        /// </summary>
        /// <param name="planId">The ID of the remediation plan.</param>
        /// <param name="actionId">The ID of the action.</param>
        /// <returns>A tracking task.</returns>
        Task TrackActionStartAsync(string planId, string actionId);

        /// <summary>
        /// Tracks the completion of a remediation action.
        /// </summary>
        /// <param name="planId">The ID of the remediation plan.</param>
        /// <param name="actionId">The ID of the action.</param>
        /// <param name="success">Whether the action succeeded.</param>
        /// <param name="errorMessage">Optional error message if the action failed.</param>
        /// <returns>A tracking task.</returns>
        Task TrackActionCompletionAsync(string planId, string actionId, bool success, string? errorMessage = null);
    }
} 
