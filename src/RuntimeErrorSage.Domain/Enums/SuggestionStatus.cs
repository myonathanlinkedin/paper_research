using System;

namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Represents the status of a remediation suggestion.
    /// </summary>
    public enum SuggestionStatus
    {
        /// <summary>
        /// The suggestion is pending.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The suggestion is available for use.
        /// </summary>
        Available = 1,

        /// <summary>
        /// The suggestion is active and being applied.
        /// </summary>
        Active = 2,

        /// <summary>
        /// The suggestion was accepted.
        /// </summary>
        Accepted = 3,

        /// <summary>
        /// The suggestion was rejected.
        /// </summary>
        Rejected = 4,

        /// <summary>
        /// The suggestion was completed.
        /// </summary>
        Completed = 5,

        /// <summary>
        /// The suggestion failed.
        /// </summary>
        Failed = 6
    }
} 
