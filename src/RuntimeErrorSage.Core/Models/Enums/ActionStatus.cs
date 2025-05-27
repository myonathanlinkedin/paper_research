namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the status of an action.
/// </summary>
public enum ActionStatus
{
    /// <summary>
    /// The action has not started.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// The action is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The action has completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// The action has failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// The action has been cancelled.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// The action is waiting for approval.
    /// </summary>
    WaitingForApproval = 5,

    /// <summary>
    /// The action has been skipped.
    /// </summary>
    Skipped = 6,

    /// <summary>
    /// The action has timed out.
    /// </summary>
    TimedOut = 7,

    /// <summary>
    /// The action status is unknown.
    /// </summary>
    Unknown = 8
} 