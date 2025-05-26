using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Interfaces;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents a plan for remediating an error.
    /// </summary>
    public class RemediationPlan
    {
        /// <summary>
        /// Gets or sets the unique identifier for this plan.
        /// </summary>
        public string PlanId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error analysis.
        /// </summary>
        public RemediationAnalysis Analysis { get; set; }

        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public required ErrorContext Context { get; set; }

        /// <summary>
        /// Gets or sets the list of remediation strategies.
        /// </summary>
        public List<IRemediationStrategy> Strategies { get; set; } = new();

        /// <summary>
        /// Gets or sets when the plan was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the plan status.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the rollback plan.
        /// </summary>
        public required RollbackPlan RollbackPlan { get; set; }

        /// <summary>
        /// Gets or sets the validation rules.
        /// </summary>
        public List<ValidationRule> ValidationRules { get; set; } = new();

        /// <summary>
        /// Gets or sets the execution steps.
        /// </summary>
        public List<RemediationStep> Steps { get; set; } = new();

        /// <summary>
        /// Gets or sets the plan metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the name of the remediation strategy.
        /// </summary>
        public string StrategyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the plan.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the estimated duration in seconds.
        /// </summary>
        public double EstimatedDuration { get; set; }

        /// <summary>
        /// Gets or sets the risk assessment for this plan.
        /// </summary>
        public RiskAssessment RiskAssessment { get; set; } = new();

        /// <summary>
        /// Gets or sets the scope of the remediation plan.
        /// </summary>
        public RemediationPlanScope Scope { get; set; }

        /// <summary>
        /// Gets or sets when the plan was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets whether this plan requires manual intervention.
        /// </summary>
        public bool RequiresManualIntervention { get; set; }

        /// <summary>
        /// Gets or sets the prerequisites for this plan.
        /// </summary>
        public List<string> Prerequisites { get; set; } = new();

        /// <summary>
        /// Gets or sets additional configuration parameters.
        /// </summary>
        public Dictionary<string, object> Configuration { get; set; } = new();

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
        public required string StatusInfo { get; set; }

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
        /// Gets or sets the update time of the plan.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Represents a rollback plan.
    /// </summary>
    public class RollbackPlan
    {
        /// <summary>
        /// Gets or sets the rollback steps.
        /// </summary>
        public List<RollbackStep> Steps { get; set; } = new();

        /// <summary>
        /// Gets or sets whether rollback is available.
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the rollback order.
        /// </summary>
        public RollbackOrder Order { get; set; }
    }

    /// <summary>
    /// Specifies the rollback order.
    /// </summary>
    public enum RollbackOrder
    {
        /// <summary>
        /// Roll back in the same order as execution.
        /// </summary>
        Forward,

        /// <summary>
        /// Roll back in reverse order of execution.
        /// </summary>
        Reverse
    }
} 