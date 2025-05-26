namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the status of a remediation execution.
/// </summary>
public enum RemediationExecutionStatus
{
    /// <summary>
    /// The remediation has not started.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// The remediation is currently running.
    /// </summary>
    Running = 1,

    /// <summary>
    /// The remediation has completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// The remediation has failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// The remediation has been cancelled.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// The remediation is waiting for external input or conditions.
    /// </summary>
    Waiting = 5,

    /// <summary>
    /// The remediation has been partially completed.
    /// </summary>
    PartiallyCompleted = 6
} 