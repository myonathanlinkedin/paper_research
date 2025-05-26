namespace RuntimeErrorSage.Core.Models.Common;

/// <summary>
/// Represents the status of a remediation operation.
/// </summary>
public enum RemediationStatus
{
    /// <summary>
    /// Unknown status.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Remediation plan created.
    /// </summary>
    Created = 1,

    /// <summary>
    /// Analyzing the error.
    /// </summary>
    Analyzing = 2,

    /// <summary>
    /// Executing remediation.
    /// </summary>
    Executing = 3,

    /// <summary>
    /// Remediation completed successfully.
    /// </summary>
    Succeeded = 4,

    /// <summary>
    /// Remediation failed.
    /// </summary>
    Failed = 5,

    /// <summary>
    /// Remediation cancelled.
    /// </summary>
    Cancelled = 6,

    /// <summary>
    /// Rolling back changes.
    /// </summary>
    RollingBack = 7,

    /// <summary>
    /// Rollback completed.
    /// </summary>
    RolledBack = 8,

    /// <summary>
    /// Validating remediation.
    /// </summary>
    Validating = 9,

    /// <summary>
    /// Waiting for external action.
    /// </summary>
    WaitingForAction = 10
} 