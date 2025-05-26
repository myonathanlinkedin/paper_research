namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents the status of a remediation step.
/// </summary>
public enum RemediationStepStatus
{
    /// <summary>
    /// The step has not started.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// The step is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The step has completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// The step has failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// The step has been cancelled.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// The step is waiting for external input or conditions.
    /// </summary>
    Waiting = 5,

    /// <summary>
    /// The step has been skipped.
    /// </summary>
    Skipped = 6,

    /// <summary>
    /// The step has timed out.
    /// </summary>
    TimedOut = 7,

    /// <summary>
    /// The step has been rolled back.
    /// </summary>
    RolledBack = 8
} 