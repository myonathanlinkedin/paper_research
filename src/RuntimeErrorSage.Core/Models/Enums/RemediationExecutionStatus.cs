namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the status of a remediation execution operation.
/// </summary>
public enum RemediationExecutionStatus
{
    /// <summary>
    /// The execution has not started yet.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// The execution is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The execution has completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// The execution has failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// The execution has been cancelled.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// The execution is waiting for approval.
    /// </summary>
    WaitingForApproval = 5,

    /// <summary>
    /// The execution is being rolled back.
    /// </summary>
    RollingBack = 6,

    /// <summary>
    /// The execution has been rolled back successfully.
    /// </summary>
    RolledBack = 7,

    /// <summary>
    /// The execution has been partially completed.
    /// </summary>
    PartiallyCompleted = 8,

    /// <summary>
    /// The execution has timed out.
    /// </summary>
    TimedOut = 9
} 