using System;
using System.Collections.Generic;

namespace CodeSage.Core.Remediation.Models.Execution
{
    /// <summary>
    /// Represents the execution status and details of a remediation action.
    /// </summary>
    public class RemediationExecution
    {
        /// <summary>
        /// Gets or sets the unique identifier of the remediation execution.
        /// </summary>
        public string ExecutionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation ID of the error context.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the remediation was started.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the remediation was completed.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the current status of the remediation execution.
        /// </summary>
        public Models.RemediationStatus Status { get; set; } = Models.RemediationStatus.Pending;

        /// <summary>
        /// Gets or sets the list of actions that were executed.
        /// </summary>
        public List<RemediationActionExecution> ExecutedActions { get; set; } = new();

        /// <summary>
        /// Gets or sets any error that occurred during remediation.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets additional execution metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the remediation was successful.
        /// </summary>
        public bool IsSuccessful => Status == Models.RemediationStatus.Completed && string.IsNullOrEmpty(Error);

        /// <summary>
        /// Gets or sets the total duration of the remediation in seconds.
        /// </summary>
        public double? DurationSeconds => EndTime.HasValue ? (EndTime.Value - StartTime).TotalSeconds : null;

        /// <summary>
        /// Gets or sets the metrics for this remediation execution.
        /// </summary>
        public Metrics.RemediationMetrics Metrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation result for this remediation execution.
        /// </summary>
        public Validation.RemediationValidationResult Validation { get; set; } = new();
    }
}