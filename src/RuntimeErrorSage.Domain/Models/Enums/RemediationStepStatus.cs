namespace RuntimeErrorSage.Model.Models.Enums;

/// <summary>
/// Defines the status of a remediation step.
/// </summary>
public enum RemediationStepStatus
{
    /// <summary>
    /// The step has not started yet.
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
    /// The step is waiting for approval.
    /// </summary>
    WaitingForApproval = 5,

    /// <summary>
    /// The step has been skipped.
    /// </summary>
    Skipped = 6,

    /// <summary>
    /// The step has timed out.
    /// </summary>
    TimedOut = 7
} 
