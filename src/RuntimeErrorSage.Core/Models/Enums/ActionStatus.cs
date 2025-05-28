namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the status of a remediation action.
/// </summary>
public enum ActionStatus
{
    /// <summary>
    /// The action has been created but not yet started.
    /// </summary>
    Created = 0,

    /// <summary>
    /// The action is pending execution.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// The action is currently in progress.
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// The action has completed successfully.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// The action has failed.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// The action has been cancelled.
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// The action is waiting for a dependency to complete.
    /// </summary>
    Waiting = 6,

    /// <summary>
    /// The action is being rolled back.
    /// </summary>
    RollingBack = 7,

    /// <summary>
    /// The action has been rolled back successfully.
    /// </summary>
    RolledBack = 8
} 