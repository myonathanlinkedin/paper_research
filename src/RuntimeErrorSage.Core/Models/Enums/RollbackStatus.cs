namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the status of a rollback operation.
/// </summary>
public enum RollbackStatus
{
    /// <summary>
    /// The rollback has not started yet.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// The rollback is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The rollback has completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// The rollback has failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// The rollback has been cancelled.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// The rollback is waiting for approval.
    /// </summary>
    WaitingForApproval = 5,

    /// <summary>
    /// The rollback has been partially completed.
    /// </summary>
    PartiallyCompleted = 6,

    /// <summary>
    /// The rollback has timed out.
    /// </summary>
    TimedOut = 7,
    
    /// <summary>
    /// The rollback is not applicable.
    /// </summary>
    NotApplicable = 8
} 