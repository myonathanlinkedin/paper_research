namespace RuntimeErrorSage.Model.Models.Enums;

/// <summary>
/// Represents the status of a remediation operation result
/// </summary>
public enum RemediationResultStatus
{
    /// <summary>
    /// Remediation was successful
    /// </summary>
    Success = 0,

    /// <summary>
    /// Remediation failed
    /// </summary>
    Failure = 1,

    /// <summary>
    /// Remediation was cancelled
    /// </summary>
    Cancelled = 2,

    /// <summary>
    /// Remediation is in progress
    /// </summary>
    InProgress = 3,

    /// <summary>
    /// Remediation is waiting for approval
    /// </summary>
    WaitingForApproval = 4,

    /// <summary>
    /// Remediation is being validated
    /// </summary>
    Validating = 5,

    /// <summary>
    /// Remediation is being rolled back
    /// </summary>
    RollingBack = 6,

    /// <summary>
    /// Remediation has been rolled back
    /// </summary>
    RolledBack = 7,

    /// <summary>
    /// Remediation status is unknown
    /// </summary>
    Unknown = 8
} 
