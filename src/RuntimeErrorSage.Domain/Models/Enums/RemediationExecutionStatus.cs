namespace RuntimeErrorSage.Model.Models.Enums;

/// <summary>
/// Defines the execution status of a remediation action.
/// </summary>
public enum RemediationExecutionStatus
{
    /// <summary>
    /// Execution status is unknown.
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Execution has not started.
    /// </summary>
    NotStarted = 1,
    
    /// <summary>
    /// Execution is in progress.
    /// </summary>
    Running = 2,
    
    /// <summary>
    /// Execution completed successfully.
    /// </summary>
    Success = 3,
    
    /// <summary>
    /// Execution failed.
    /// </summary>
    Failed = 4,
    
    /// <summary>
    /// Execution was cancelled.
    /// </summary>
    Cancelled = 5,
    
    /// <summary>
    /// Execution completed with partial success.
    /// </summary>
    Partial = 6,
    
    /// <summary>
    /// Execution is waiting for dependencies.
    /// </summary>
    Waiting = 7,
    
    /// <summary>
    /// Execution is blocked.
    /// </summary>
    Blocked = 8,
    
    /// <summary>
    /// Execution is paused.
    /// </summary>
    Paused = 9,
    
    /// <summary>
    /// Execution timed out.
    /// </summary>
    Timeout = 10,
    
    /// <summary>
    /// Execution validation failed.
    /// </summary>
    ValidationFailed = 11
} 
