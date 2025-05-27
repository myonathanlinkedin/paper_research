namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the status of an execution.
/// </summary>
public enum ExecutionStatus
{
    /// <summary>
    /// No status.
    /// </summary>
    None,

    /// <summary>
    /// Execution is pending.
    /// </summary>
    Pending,

    /// <summary>
    /// Execution is in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Execution is completed.
    /// </summary>
    Completed,

    /// <summary>
    /// Execution has failed.
    /// </summary>
    Failed,

    /// <summary>
    /// Execution is cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// Execution is timed out.
    /// </summary>
    TimedOut,

    /// <summary>
    /// Execution is blocked.
    /// </summary>
    Blocked,

    /// <summary>
    /// Execution is skipped.
    /// </summary>
    Skipped
} 