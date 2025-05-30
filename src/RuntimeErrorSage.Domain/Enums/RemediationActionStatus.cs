using System;
using RuntimeErrorSage.Domain.Models.Remediation;
namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Represents the status of a remediation action.
    /// </summary>
    public enum RemediationActionStatus
    {
        /// <summary>
        /// The action has not started yet.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The action is in progress.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// The action has completed successfully.
        /// </summary>
        Completed = 2,

        /// <summary>
        /// The action has failed.
        /// </summary>
        Failed = 3,

        /// <summary>
        /// The action has been cancelled.
        /// </summary>
        Cancelled = 4,

        /// <summary>
        /// The action is waiting for something.
        /// </summary>
        Waiting = 5,

        /// <summary>
        /// The action has been rolled back.
        /// </summary>
        RolledBack = 6
    }
} 


