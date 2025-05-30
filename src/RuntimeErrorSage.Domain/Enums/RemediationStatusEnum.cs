namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the status of a remediation action or plan.
/// </summary>
public enum RemediationStatusEnum
{
    /// <summary>
    /// Status is unknown.
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Remediation has not started.
    /// </summary>
    NotStarted = 1,
    
    /// <summary>
    /// Remediation is in progress.
    /// </summary>
    InProgress = 2,
    
    /// <summary>
    /// Remediation completed successfully.
    /// </summary>
    Success = 3,
    
    /// <summary>
    /// Remediation is complete (alias for Success to maintain compatibility).
    /// </summary>
    Completed = 3,
    
    /// <summary>
    /// Remediation failed.
    /// </summary>
    Failed = 4,
    
    /// <summary>
    /// Remediation was cancelled.
    /// </summary>
    Cancelled = 5,
    
    /// <summary>
    /// Remediation completed with partial success.
    /// </summary>
    Partial = 6,
    
    /// <summary>
    /// Remediation is waiting for dependencies.
    /// </summary>
    Waiting = 7,
    
    /// <summary>
    /// Remediation is blocked.
    /// </summary>
    Blocked = 8,
    
    /// <summary>
    /// Remediation is paused.
    /// </summary>
    Paused = 9,
    
    /// <summary>
    /// Remediation timed out.
    /// </summary>
    Timeout = 10,
    
    /// <summary>
    /// Remediation validation failed.
    /// </summary>
    ValidationFailed = 11,
    
    /// <summary>
    /// Remediation is pending.
    /// </summary>
    Pending = 12,
    
    /// <summary>
    /// Remediation has been rolled back.
    /// </summary>
    RolledBack = 13,
    
    /// <summary>
    /// Remediation rollback failed.
    /// </summary>
    RollbackFailed = 14
} 
