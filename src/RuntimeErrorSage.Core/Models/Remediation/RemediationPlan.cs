using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents a plan for remediating an error.
    /// </summary>
    public class RemediationPlan
    {
        /// <summary>
        /// Gets or sets the unique identifier of the plan.
        /// </summary>
        public string PlanId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error context that this plan addresses.
        /// </summary>
        public ErrorContext ErrorContext { get; set; }

        /// <summary>
        /// Gets or sets the name of the plan.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the plan.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the priority of the plan.
        /// </summary>
        public RemediationActionPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the impact scope of the plan.
        /// </summary>
        public RemediationActionImpactScope ImpactScope { get; set; }

        /// <summary>
        /// Gets or sets the severity level of the plan.
        /// </summary>
        public RemediationActionSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the steps in the remediation plan.
        /// </summary>
        public List<RemediationStep> Steps { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation rules for the plan.
        /// </summary>
        public List<RemediationActionValidationRule> ValidationRules { get; set; } = new();

        /// <summary>
        /// Gets or sets the rollback plan if this plan fails.
        /// </summary>
        public RemediationPlan RollbackPlan { get; set; }

        /// <summary>
        /// Gets or sets the timeout for the plan in seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 300;

        /// <summary>
        /// Gets or sets the maximum number of retries for the plan.
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// Gets or sets the delay between retries in seconds.
        /// </summary>
        public int RetryDelaySeconds { get; set; } = 30;

        /// <summary>
        /// Gets or sets whether the plan requires confirmation.
        /// </summary>
        public bool RequiresConfirmation { get; set; }

        /// <summary>
        /// Gets or sets the confirmation message.
        /// </summary>
        public string ConfirmationMessage { get; set; }

        /// <summary>
        /// Gets or sets the status of the plan.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the start time of the plan.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the plan.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the plan in milliseconds.
        /// </summary>
        public double DurationMs => (StartTime.HasValue && EndTime.HasValue) ? (EndTime.Value - StartTime.Value).TotalMilliseconds : 0;

        /// <summary>
        /// Gets or sets the error message if the plan failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the exception that occurred during plan execution.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the validation results for the plan.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the plan was rolled back.
        /// </summary>
        public bool WasRolledBack { get; set; }

        /// <summary>
        /// Gets or sets the impact of the plan.
        /// </summary>
        public RemediationImpact Impact { get; set; }

        /// <summary>
        /// Gets or sets additional metadata about the plan.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Represents a step in a remediation plan.
    /// </summary>
    public class RemediationStep
    {
        /// <summary>
        /// Gets or sets the unique identifier of the step.
        /// </summary>
        public string StepId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the plan identifier.
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets the name of the step.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the step.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the order of the step in the plan.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the remediation action to execute.
        /// </summary>
        public RemediationAction Action { get; set; }

        /// <summary>
        /// Gets or sets the status of the step.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the start time of the step.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the step.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the step in milliseconds.
        /// </summary>
        public double DurationMs => (StartTime.HasValue && EndTime.HasValue) ? (EndTime.Value - StartTime.Value).TotalMilliseconds : 0;

        /// <summary>
        /// Gets or sets the error message if the step failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the exception that occurred during step execution.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the validation results for the step.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the number of retries attempted.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets whether the step was rolled back.
        /// </summary>
        public bool WasRolledBack { get; set; }

        /// <summary>
        /// Gets or sets the rollback step if this step fails.
        /// </summary>
        public RemediationStep RollbackStep { get; set; }

        /// <summary>
        /// Gets or sets the dependencies for this step.
        /// </summary>
        public List<string> Dependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the impact of the step.
        /// </summary>
        public RemediationImpact Impact { get; set; }

        /// <summary>
        /// Gets or sets additional metadata about the step.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 