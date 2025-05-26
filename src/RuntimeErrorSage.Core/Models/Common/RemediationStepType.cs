namespace RuntimeErrorSage.Core.Models.Common;

/// <summary>
/// Defines the type of a remediation step.
/// </summary>
public enum RemediationStepType
{
    /// <summary>
    /// An action step that performs a specific operation.
    /// </summary>
    Action = 0,

    /// <summary>
    /// A validation step that checks conditions.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// A rollback step that reverts changes.
    /// </summary>
    Rollback = 2,

    /// <summary>
    /// A notification step that sends alerts.
    /// </summary>
    Notification = 3,

    /// <summary>
    /// A monitoring step that observes system state.
    /// </summary>
    Monitoring = 4,

    /// <summary>
    /// A cleanup step that performs post-remediation tasks.
    /// </summary>
    Cleanup = 5
} 