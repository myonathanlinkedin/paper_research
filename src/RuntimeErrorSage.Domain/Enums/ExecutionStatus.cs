using System;

namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Represents the status of an execution.
    /// </summary>
    public enum ExecutionStatus
    {
        /// <summary>
        /// Execution has not started.
        /// </summary>
        NotStarted = 0,

        /// <summary>
        /// Execution is pending.
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Execution is running.
        /// </summary>
        Running = 2,

        /// <summary>
        /// Execution is waiting for approval.
        /// </summary>
        WaitingForApproval = 3,

        /// <summary>
        /// Execution is completed.
        /// </summary>
        Completed = 4,

        /// <summary>
        /// Execution has failed.
        /// </summary>
        Failed = 5,

        /// <summary>
        /// Execution is cancelled.
        /// </summary>
        Cancelled = 6,

        /// <summary>
        /// Execution has timed out.
        /// </summary>
        TimedOut = 7,

        /// <summary>
        /// Execution is rolling back.
        /// </summary>
        RollingBack = 8,

        /// <summary>
        /// Execution has been rolled back.
        /// </summary>
        RolledBack = 9,

        /// <summary>
        /// Execution rollback has failed.
        /// </summary>
        RollbackFailed = 10,

        /// <summary>
        /// Execution status is unknown.
        /// </summary>
        Unknown = 99
    }
} 
