using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the state of a remediation operation.
/// </summary>
public enum RemediationState
{
    /// <summary>
    /// Remediation has not started.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// Remediation is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Remediation has completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Remediation has failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Remediation has been cancelled.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Remediation has been rolled back.
    /// </summary>
    RolledBack = 5
} 






