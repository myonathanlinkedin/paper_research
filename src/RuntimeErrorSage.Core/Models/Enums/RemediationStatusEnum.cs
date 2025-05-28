namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the status of a remediation operation.
/// </summary>
public enum RemediationStatusEnum
{
    /// <summary>
    /// The remediation has been created but not yet started.
    /// </summary>
    Created = 0,

    /// <summary>
    /// The remediation is currently in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The remediation is waiting for user input or external resources.
    /// </summary>
    Waiting = 2,

    /// <summary>
    /// The remediation has completed successfully.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// The remediation has failed.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// The remediation has been cancelled.
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// The remediation is being rolled back.
    /// </summary>
    RollingBack = 6,

    /// <summary>
    /// The remediation has been rolled back successfully.
    /// </summary>
    RolledBack = 7
} 