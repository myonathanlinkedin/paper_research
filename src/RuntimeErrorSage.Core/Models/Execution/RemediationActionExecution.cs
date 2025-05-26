using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Execution
{
    /// <summary>
    /// Represents the execution status of a remediation action.
    /// </summary>
    public class RemediationActionExecution
    {
        /// <summary>
        /// Gets or sets the unique identifier of the action execution.
        /// </summary>
        public string ExecutionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the action that was executed.
        /// </summary>
        public string ActionId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string ActionName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the action was started.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the action was completed.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the current status of the action execution.
        /// </summary>
        public RemediationActionStatus Status { get; set; } = RemediationActionStatus.Pending;

        /// <summary>
        /// Gets or sets any error that occurred during action execution.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the result of the action execution.
        /// </summary>
        public Dictionary<string, object> Result { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the action execution was successful.
        /// </summary>
        public bool IsSuccessful => Status == RemediationActionStatus.Completed && string.IsNullOrEmpty(Error);

        /// <summary>
        /// Gets or sets the duration of the action execution in milliseconds.
        /// </summary>
        public double DurationMs => EndTime.HasValue ? (EndTime.Value - StartTime).TotalMilliseconds : 0;

        /// <summary>
        /// Gets or sets the validation results for this action execution.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the metrics collected during this action execution.
        /// </summary>
        public Dictionary<string, object> Metrics { get; set; } = new();
    }
} 
