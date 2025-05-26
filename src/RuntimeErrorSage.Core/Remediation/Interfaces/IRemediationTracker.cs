using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models;
using RemediationStep = RuntimeErrorSage.Core.Remediation.Models.Common.RemediationStep;
using RemediationMetrics = RuntimeErrorSage.Core.Models.Metrics.RemediationMetrics;
using ValidationResult = RuntimeErrorSage.Core.Models.Validation.ValidationResult;
using RemediationPlan = RuntimeErrorSage.Core.Models.Remediation.RemediationPlan;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
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
        Task<RemediationStatus> GetStatusAsync(string planId);

        /// <summary>
        /// Updates the status of a remediation plan.
        /// </summary>
        /// <param name="planId">The ID of the remediation plan.</param>
        /// <param name="status">The new status.</param>
        /// <param name="details">Optional details about the status change.</param>
        Task UpdateStatusAsync(string planId, RemediationStatus status, string details = null);

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
    }
} 