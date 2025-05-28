namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the status of a remediation operation.
/// </summary>
public enum RemediationStatusEnum
{
    /// <summary>
    /// The remediation status is unknown.
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// The remediation is pending execution.
    /// </summary>
    Pending = 1,
    
    /// <summary>
    /// The remediation is currently in progress.
    /// </summary>
    InProgress = 2,
    
    /// <summary>
    /// The remediation completed successfully.
    /// </summary>
    Completed = 3,
    
    /// <summary>
    /// The remediation failed.
    /// </summary>
    Failed = 4,
    
    /// <summary>
    /// The remediation was canceled.
    /// </summary>
    Canceled = 5,
    
    /// <summary>
    /// The remediation was partially completed.
    /// </summary>
    PartiallyCompleted = 6,
    
    /// <summary>
    /// The remediation was rolled back.
    /// </summary>
    RolledBack = 7
} 