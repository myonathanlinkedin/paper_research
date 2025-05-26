using System;

namespace RuntimeErrorSage.Core.Remediation.Models.Common
{
    /// <summary>
    /// Represents the status of a remediation operation.
    /// </summary>
    public enum RemediationStatus
    {
        /// <summary>
        /// The remediation has been created.
        /// </summary>
        Created,

        /// <summary>
        /// The remediation is being validated.
        /// </summary>
        Validating,

        /// <summary>
        /// The remediation has been validated.
        /// </summary>
        Validated,

        /// <summary>
        /// The remediation validation has failed.
        /// </summary>
        ValidationFailed,

        /// <summary>
        /// The remediation is being approved.
        /// </summary>
        Approving,

        /// <summary>
        /// The remediation has been approved.
        /// </summary>
        Approved,

        /// <summary>
        /// The remediation approval has failed.
        /// </summary>
        ApprovalFailed,

        /// <summary>
        /// The remediation is being scheduled.
        /// </summary>
        Scheduling,

        /// <summary>
        /// The remediation has been scheduled.
        /// </summary>
        Scheduled,

        /// <summary>
        /// The remediation scheduling has failed.
        /// </summary>
        SchedulingFailed,

        /// <summary>
        /// The remediation is being executed.
        /// </summary>
        Executing,

        /// <summary>
        /// The remediation has been executed.
        /// </summary>
        Executed,

        /// <summary>
        /// The remediation execution has failed.
        /// </summary>
        ExecutionFailed,

        /// <summary>
        /// The remediation is being rolled back.
        /// </summary>
        RollingBack,

        /// <summary>
        /// The remediation has been rolled back.
        /// </summary>
        RolledBack,

        /// <summary>
        /// The remediation rollback has failed.
        /// </summary>
        RollbackFailed,

        /// <summary>
        /// The remediation is being cancelled.
        /// </summary>
        Cancelling,

        /// <summary>
        /// The remediation has been cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The remediation cancellation has failed.
        /// </summary>
        CancellationFailed,

        /// <summary>
        /// The remediation is being paused.
        /// </summary>
        Pausing,

        /// <summary>
        /// The remediation has been paused.
        /// </summary>
        Paused,

        /// <summary>
        /// The remediation pause has failed.
        /// </summary>
        PauseFailed,

        /// <summary>
        /// The remediation is being resumed.
        /// </summary>
        Resuming,

        /// <summary>
        /// The remediation has been resumed.
        /// </summary>
        Resumed,

        /// <summary>
        /// The remediation resume has failed.
        /// </summary>
        ResumeFailed,

        /// <summary>
        /// The remediation is being completed.
        /// </summary>
        Completing,

        /// <summary>
        /// The remediation has been completed.
        /// </summary>
        Completed,

        /// <summary>
        /// The remediation completion has failed.
        /// </summary>
        CompletionFailed,

        /// <summary>
        /// The remediation is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The remediation was partially successful.
        /// </summary>
        Partial
    }
} 