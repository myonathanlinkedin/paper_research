namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Represents the approval status of a remediation action.
/// </summary>
public enum RemediationApprovalStatus
{
    /// <summary>
    /// Approval not required.
    /// </summary>
    NotRequired,

    /// <summary>
    /// Waiting for approval.
    /// </summary>
    Pending,

    /// <summary>
    /// Approval granted.
    /// </summary>
    Approved,

    /// <summary>
    /// Approval denied.
    /// </summary>
    Denied,

    /// <summary>
    /// Approval status is unknown.
    /// </summary>
    Unknown
} 
