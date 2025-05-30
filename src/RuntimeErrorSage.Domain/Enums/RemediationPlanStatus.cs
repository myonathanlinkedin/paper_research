namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the status values for remediation plans.
/// </summary>
public enum RemediationPlanStatus
{
    /// <summary>
    /// No status specified.
    /// </summary>
    None = 0,

    /// <summary>
    /// Plan is pending execution.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Plan is in progress.
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// Plan has completed successfully.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Plan has failed.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// Plan has been cancelled.
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// Plan is waiting for additional input.
    /// </summary>
    Waiting = 6,

    /// <summary>
    /// Plan has timed out.
    /// </summary>
    TimedOut = 7,

    /// <summary>
    /// Plan needs validation.
    /// </summary>
    NeedsValidation = 8,

    /// <summary>
    /// Plan has been validated.
    /// </summary>
    Validated = 9,

    /// <summary>
    /// Plan has been rolled back.
    /// </summary>
    RolledBack = 10,

    /// <summary>
    /// Plan rollback has failed.
    /// </summary>
    RollbackFailed = 11
} 
