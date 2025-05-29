using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Remediation;
namespace RuntimeErrorSage.Application.Models.Enums
{
    /// <summary>
    /// Defines the possible status values for a remediation action.
    /// </summary>
    public enum RemediationActionStatus
    {
        /// <summary>
        /// The status is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The action is queued and waiting to start.
        /// </summary>
        Queued = 1,

        /// <summary>
        /// The action is running.
        /// </summary>
        Running = 2,

        /// <summary>
        /// The action has completed successfully.
        /// </summary>
        Completed = 3,

        /// <summary>
        /// The action has failed.
        /// </summary>
        Failed = 4,

        /// <summary>
        /// The action was cancelled.
        /// </summary>
        Cancelled = 5,

        /// <summary>
        /// The action is pending validation.
        /// </summary>
        PendingValidation = 6,

        /// <summary>
        /// The action was skipped.
        /// </summary>
        Skipped = 7
    }
} 


