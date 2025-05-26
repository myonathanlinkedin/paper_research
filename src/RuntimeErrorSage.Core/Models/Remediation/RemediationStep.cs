using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation;

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
    /// Gets or sets the type of the step.
    /// </summary>
    public RemediationStepType Type { get; set; }

    /// <summary>
    /// Gets or sets the status of the step.
    /// </summary>
    public RemediationState Status { get; set; }

    /// <summary>
    /// Gets or sets the start time of the step.
    /// </summary>
    public DateTimeOffset? StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the step.
    /// </summary>
    public DateTimeOffset? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the duration of the step in milliseconds.
    /// </summary>
    public long? DurationMs { get; set; }

    /// <summary>
    /// Gets or sets the error message if the step failed.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the exception details if the step failed.
    /// </summary>
    public Exception Exception { get; set; }

    /// <summary>
    /// Gets or sets the action to execute for this step.
    /// </summary>
    public RemediationAction Action { get; set; }

    /// <summary>
    /// Gets or sets the validation results for this step.
    /// </summary>
    public List<RemediationValidationResult> ValidationResults { get; set; } = new();

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
    /// Gets or sets the metadata associated with the step.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents the type of a remediation step.
/// </summary>
public enum RemediationStepType
{
    /// <summary>
    /// A step that validates the current state.
    /// </summary>
    Validation,

    /// <summary>
    /// A step that executes a remediation action.
    /// </summary>
    Action,

    /// <summary>
    /// A step that verifies the remediation result.
    /// </summary>
    Verification,

    /// <summary>
    /// A step that rolls back changes if needed.
    /// </summary>
    Rollback,

    /// <summary>
    /// A step that collects metrics.
    /// </summary>
    MetricsCollection,

    /// <summary>
    /// A step that performs cleanup.
    /// </summary>
    Cleanup
} 