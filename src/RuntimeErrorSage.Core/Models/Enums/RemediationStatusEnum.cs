using System;

namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the status of a remediation operation.
/// </summary>
public enum RemediationStatusEnum
{
    /// <summary>
    /// The remediation has not started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// The remediation is pending execution.
    /// </summary>
    Pending,

    /// <summary>
    /// The remediation is in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// The remediation has been completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// The remediation has failed.
    /// </summary>
    Failed,

    /// <summary>
    /// The remediation has been rolled back.
    /// </summary>
    RolledBack,

    /// <summary>
    /// The remediation has been cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// The remediation is pending validation.
    /// </summary>
    PendingValidation,

    /// <summary>
    /// The remediation is waiting for approval.
    /// </summary>
    WaitingForApproval,

    /// <summary>
    /// The remediation has been rejected.
    /// </summary>
    Rejected,

    /// <summary>
    /// The remediation has been paused.
    /// </summary>
    Paused
} 