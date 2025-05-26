using System;
using System.Collections.Generic;
using ValidationResult = RuntimeErrorSage.Core.Models.Validation.ValidationResult;
using RemediationStep = RuntimeErrorSage.Core.Remediation.Models.Common.RemediationStep;

namespace RuntimeErrorSage.Core.Remediation.Models.Common
{
    /// <summary>
    /// Represents a step in a remediation plan.
    /// </summary>
    public class RemediationStep
    {
        /// <summary>
        /// Gets or sets the unique identifier for the step.
        /// </summary>
        public string StepId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the plan identifier.
        /// </summary>
        public string PlanId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the step name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the step description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the step type.
        /// </summary>
        public string StepType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the step order in the plan.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets whether the step is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of retries allowed.
        /// </summary>
        public int MaxRetries { get; set; }

        /// <summary>
        /// Gets or sets the timeout in seconds.
        /// </summary>
        public double TimeoutSeconds { get; set; }

        /// <summary>
        /// Gets or sets the execution status.
        /// </summary>
        public StepExecutionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the start time of execution.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of execution.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets the duration of execution in seconds.
        /// </summary>
        public double? DurationSeconds => EndTime.HasValue && StartTime.HasValue
            ? (EndTime.Value - StartTime.Value).TotalSeconds
            : null;

        /// <summary>
        /// Gets or sets the number of retries attempted.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets whether the step was successful.
        /// </summary>
        public bool IsSuccessful => Status == StepExecutionStatus.Completed;

        /// <summary>
        /// Gets or sets any error message if the step failed.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the input parameters for the step.
        /// </summary>
        public Dictionary<string, object> InputParameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the output results from the step.
        /// </summary>
        public Dictionary<string, object> OutputResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation results.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the rollback status.
        /// </summary>
        public StepRollbackStatus? RollbackStatus { get; set; }

        /// <summary>
        /// Gets or sets any additional metadata about the step.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets whether the step is valid.
        /// </summary>
        public bool IsValid => ValidationResults.Count > 0 && ValidationResults.TrueForAll(r => r.IsValid);

        /// <summary>
        /// Gets whether the step is executing.
        /// </summary>
        public bool IsExecuting => Status == StepExecutionStatus.InProgress;

        /// <summary>
        /// Gets whether the step has completed execution.
        /// </summary>
        public bool IsCompleted => Status == StepExecutionStatus.Completed;

        /// <summary>
        /// Gets whether the step has failed execution.
        /// </summary>
        public bool HasFailed => Status == StepExecutionStatus.Failed;

        /// <summary>
        /// Gets whether the step has timed out.
        /// </summary>
        public bool HasTimedOut => Status == StepExecutionStatus.Timeout;

        /// <summary>
        /// Gets whether the step has been rolled back.
        /// </summary>
        public bool HasBeenRolledBack => RollbackStatus != null;
    }

    /// <summary>
    /// Represents the execution status of a remediation step.
    /// </summary>
    public enum StepExecutionStatus
    {
        /// <summary>
        /// The step has not started execution.
        /// </summary>
        NotStarted,

        /// <summary>
        /// The step is in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// The step has completed execution.
        /// </summary>
        Completed,

        /// <summary>
        /// The step has failed execution.
        /// </summary>
        Failed,

        /// <summary>
        /// The step has timed out.
        /// </summary>
        Timeout,

        /// <summary>
        /// The step has been skipped.
        /// </summary>
        Skipped,

        /// <summary>
        /// The step is being retried.
        /// </summary>
        Retrying,

        /// <summary>
        /// The step is being rolled back.
        /// </summary>
        RollingBack,

        /// <summary>
        /// The step has been rolled back.
        /// </summary>
        RolledBack,

        /// <summary>
        /// The step has been cancelled.
        /// </summary>
        Cancelled
    }

    /// <summary>
    /// Represents the rollback status of a remediation step.
    /// </summary>
    public class StepRollbackStatus
    {
        /// <summary>
        /// Gets or sets whether the rollback was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the start time of the rollback.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the rollback.
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
        /// Gets or sets the rollback actions performed.
        /// </summary>
        public List<string> RollbackActions { get; set; } = new();

        /// <summary>
        /// Gets or sets any failed rollback actions.
        /// </summary>
        public List<string> FailedActions { get; set; } = new();
    }
} 