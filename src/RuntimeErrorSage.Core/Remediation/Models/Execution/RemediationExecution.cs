using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Remediation.Models.Execution
{
    /// <summary>
    /// Represents the status of a remediation execution.
    /// </summary>
    public enum RemediationExecutionStatus
    {
        /// <summary>
        /// The execution has not started.
        /// </summary>
        NotStarted,

        /// <summary>
        /// The execution is in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// The execution completed successfully.
        /// </summary>
        Completed,

        /// <summary>
        /// The execution failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The execution timed out.
        /// </summary>
        Timeout,

        /// <summary>
        /// The execution was cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The execution failed validation.
        /// </summary>
        ValidationFailed,

        /// <summary>
        /// The execution was rolled back.
        /// </summary>
        RolledBack
    }

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
        public RemediationExecutionStatus Status { get; set; }

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
        public bool IsSuccessful => Status == RemediationExecutionStatus.Completed && string.IsNullOrEmpty(Error);

        /// <summary>
        /// Gets or sets the total duration of the remediation in seconds.
        /// </summary>
        public double? DurationSeconds => EndTime.HasValue ? (EndTime.Value - StartTime).TotalSeconds : null;

        /// <summary>
        /// Gets or sets the metrics for this remediation execution.
        /// </summary>
        public RemediationMetrics? Metrics { get; set; }

        /// <summary>
        /// Gets or sets the validation result for this remediation execution.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the rollback status if the execution was rolled back.
        /// </summary>
        public RollbackStatus? RollbackStatus { get; set; }
    }

    /// <summary>
    /// Represents the status of a rollback operation.
    /// </summary>
    public class RollbackStatus
    {
        /// <summary>
        /// Gets or sets whether the rollback was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the time when the rollback started.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time when the rollback completed.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets the duration of the rollback in seconds.
        /// </summary>
        public double? DurationSeconds => EndTime.HasValue ? (EndTime.Value - StartTime).TotalSeconds : null;

        /// <summary>
        /// Gets or sets any error message if the rollback failed.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the steps that were rolled back.
        /// </summary>
        public List<string> RolledBackSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets any steps that failed to roll back.
        /// </summary>
        public List<string> FailedRollbackSteps { get; set; } = new();
    }

    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets or sets whether the validation was successful.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the validation type.
        /// </summary>
        public string ValidationType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any additional validation details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();
    }
}
