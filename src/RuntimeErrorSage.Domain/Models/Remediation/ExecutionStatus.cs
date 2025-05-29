using System;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents the status of an action execution.
    /// </summary>
    public enum ExecutionStatus
    {
        /// <summary>
        /// The action is waiting to be executed.
        /// </summary>
        Pending,

        /// <summary>
        /// The action is waiting for manual approval.
        /// </summary>
        WaitingForApproval,

        /// <summary>
        /// The action is currently running.
        /// </summary>
        Running,

        /// <summary>
        /// The action has completed successfully.
        /// </summary>
        Completed,

        /// <summary>
        /// The action has failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The action has been cancelled.
        /// </summary>
        Cancelled
    }
} 