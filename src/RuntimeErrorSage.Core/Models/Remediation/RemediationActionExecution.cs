using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the execution of a single remediation action.
    /// </summary>
    public class RemediationActionExecution
    {
        private readonly Dictionary<string, object> _metadata = new();

        /// <summary>
        /// Gets or sets the unique identifier of the action execution.
        /// </summary>
        public string ExecutionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the ID of the action that was executed.
        /// </summary>
        public string ActionId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string ActionName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        public string ActionType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the execution.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the action started.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the action ended.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the action execution.
        /// </summary>
        public TimeSpan? Duration => EndTime.HasValue ? EndTime.Value - StartTime : null;

        /// <summary>
        /// Gets or sets any error that occurred during execution.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the result of the action execution.
        /// </summary>
        public Dictionary<string, object> Result { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the action execution was successful.
        /// </summary>
        public bool IsSuccessful => Status == RemediationStatusEnum.Completed && string.IsNullOrEmpty(Error);

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

        /// <summary>
        /// Gets or sets the parameters for this action execution.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets any warnings that occurred during action execution.
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Gets or sets the severity of this action execution.
        /// </summary>
        public RemediationActionSeverity Severity { get; set; } = RemediationActionSeverity.Normal;

        /// <summary>
        /// Gets or sets any additional metadata.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata => _metadata;

        /// <summary>
        /// Adds metadata to the action execution.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        public void AddMetadata(string key, object value)
        {
            _metadata[key] = value;
        }

        /// <summary>
        /// Completes the action execution.
        /// </summary>
        /// <param name="success">Whether the action was successful.</param>
        /// <param name="result">The result of the action.</param>
        /// <param name="error">Any error that occurred.</param>
        public void Complete(bool success, Dictionary<string, object> result = null, string? error = null)
        {
            EndTime = DateTime.UtcNow;
            Status = success ? RemediationStatusEnum.Completed : RemediationStatusEnum.Failed;
            Result = result ?? new Dictionary<string, object>();
            Error = error;
        }
    }
} 