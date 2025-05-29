using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Represents the status of a remediation suggestion.
    /// </summary>
    public enum SuggestionStatus
    {
        /// <summary>
        /// The suggestion is pending review.
        /// </summary>
        Pending,

        /// <summary>
        /// The suggestion has been approved.
        /// </summary>
        Approved,

        /// <summary>
        /// The suggestion has been rejected.
        /// </summary>
        Rejected,

        /// <summary>
        /// The suggestion is being implemented.
        /// </summary>
        InProgress,

        /// <summary>
        /// The suggestion has been completed.
        /// </summary>
        Completed,

        /// <summary>
        /// The suggestion has been cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The suggestion has failed.
        /// </summary>
        Failed
    }
} 






