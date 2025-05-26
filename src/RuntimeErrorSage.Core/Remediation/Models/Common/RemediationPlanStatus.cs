namespace RuntimeErrorSage.Core.Remediation.Models.Common;

/// <summary>
/// Defines the status of a remediation plan.
/// </summary>
public enum RemediationPlanStatus
{
    /// <summary>
    /// The plan has been created.
    /// </summary>
    Created,

    /// <summary>
    /// The plan is being validated.
    /// </summary>
    Validating,

    /// <summary>
    /// The plan has been validated.
    /// </summary>
    Validated,

    /// <summary>
    /// The plan validation has failed.
    /// </summary>
    ValidationFailed,

    /// <summary>
    /// The plan is being approved.
    /// </summary>
    Approving,

    /// <summary>
    /// The plan has been approved.
    /// </summary>
    Approved,

    /// <summary>
    /// The plan approval has failed.
    /// </summary>
    ApprovalFailed,

    /// <summary>
    /// The plan is being scheduled.
    /// </summary>
    Scheduling,

    /// <summary>
    /// The plan has been scheduled.
    /// </summary>
    Scheduled,

    /// <summary>
    /// The plan scheduling has failed.
    /// </summary>
    SchedulingFailed,

    /// <summary>
    /// The plan is being executed.
    /// </summary>
    Executing,

    /// <summary>
    /// The plan has been executed.
    /// </summary>
    Executed,

    /// <summary>
    /// The plan execution has failed.
    /// </summary>
    ExecutionFailed,

    /// <summary>
    /// The plan is being rolled back.
    /// </summary>
    RollingBack,

    /// <summary>
    /// The plan has been rolled back.
    /// </summary>
    RolledBack,

    /// <summary>
    /// The plan rollback has failed.
    /// </summary>
    RollbackFailed,

    /// <summary>
    /// The plan is being cancelled.
    /// </summary>
    Cancelling,

    /// <summary>
    /// The plan has been cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// The plan cancellation has failed.
    /// </summary>
    CancellationFailed,

    /// <summary>
    /// The plan is being paused.
    /// </summary>
    Pausing,

    /// <summary>
    /// The plan has been paused.
    /// </summary>
    Paused,

    /// <summary>
    /// The plan pause has failed.
    /// </summary>
    PauseFailed,

    /// <summary>
    /// The plan is being resumed.
    /// </summary>
    Resuming,

    /// <summary>
    /// The plan has been resumed.
    /// </summary>
    Resumed,

    /// <summary>
    /// The plan resume has failed.
    /// </summary>
    ResumeFailed,

    /// <summary>
    /// The plan is being completed.
    /// </summary>
    Completing,

    /// <summary>
    /// The plan has been completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The plan completion has failed.
    /// </summary>
    CompletionFailed,

    /// <summary>
    /// The plan is unknown.
    /// </summary>
    Unknown
} 