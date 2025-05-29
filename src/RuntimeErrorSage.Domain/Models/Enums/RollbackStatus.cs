using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Defines the status of a rollback operation.
/// </summary>
public enum RollbackStatus
{
    /// <summary>
    /// No rollback has been attempted.
    /// </summary>
    NotAttempted = 0,
    
    /// <summary>
    /// Rollback is in progress.
    /// </summary>
    InProgress = 1,
    
    /// <summary>
    /// Rollback completed successfully.
    /// </summary>
    Completed = 2,
    
    /// <summary>
    /// Rollback failed.
    /// </summary>
    Failed = 3,
    
    /// <summary>
    /// Rollback was partially completed.
    /// </summary>
    PartiallyCompleted = 4,
    
    /// <summary>
    /// Rollback is not supported for this operation.
    /// </summary>
    NotSupported = 5
} 






