using System;

namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Represents the status of a rollback operation.
    /// </summary>
    public enum RollbackStatus
    {
        /// <summary>
        /// Rollback operation has not started.
        /// </summary>
        NotStarted = 0,
        
        /// <summary>
        /// Rollback operation is in progress.
        /// </summary>
        InProgress = 1,
        
        /// <summary>
        /// Rollback operation completed successfully.
        /// </summary>
        Completed = 2,
        
        /// <summary>
        /// Rollback operation failed.
        /// </summary>
        Failed = 3,
        
        /// <summary>
        /// Rollback operation was cancelled.
        /// </summary>
        Cancelled = 4,
        
        /// <summary>
        /// Rollback operation is partially completed.
        /// </summary>
        PartiallyCompleted = 5
    }
} 