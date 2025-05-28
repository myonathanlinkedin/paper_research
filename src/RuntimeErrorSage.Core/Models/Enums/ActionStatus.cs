namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the status of a remediation action.
/// </summary>
public enum ActionStatus
{
    /// <summary>
    /// The action status is unknown.
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// The action is pending execution.
    /// </summary>
    Pending = 1,
    
    /// <summary>
    /// The action is in progress.
    /// </summary>
    InProgress = 2,
    
    /// <summary>
    /// The action is completed successfully.
    /// </summary>
    Completed = 3,
    
    /// <summary>
    /// The action has failed.
    /// </summary>
    Failed = 4,
    
    /// <summary>
    /// The action was skipped.
    /// </summary>
    Skipped = 5,
    
    /// <summary>
    /// The action was canceled.
    /// </summary>
    Canceled = 6,
    
    /// <summary>
    /// The action is waiting for a dependency to complete.
    /// </summary>
    Waiting = 7,
    
    /// <summary>
    /// The action was rolled back.
    /// </summary>
    RolledBack = 8,
    
    /// <summary>
    /// The action is being rolled back.
    /// </summary>
    RollingBack = 9
} 