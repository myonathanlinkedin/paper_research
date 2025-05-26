namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the status of a remediation action.
/// </summary>
public enum ActionStatus
{
    /// <summary>
    /// Action is pending execution.
    /// </summary>
    Pending,

    /// <summary>
    /// Action is currently in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Action has completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// Action has failed.
    /// </summary>
    Failed,

    /// <summary>
    /// Action has been cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// Action has timed out.
    /// </summary>
    TimedOut,

    /// <summary>
    /// Action status is unknown.
    /// </summary>
    Unknown
} 