using System;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents a change in the remediation state.
    /// </summary>
    public class RemediationStateChange
    {
        /// <summary>
        /// Gets or sets the unique identifier for this state change.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the state after the change.
        /// </summary>
        public RemediationStateEnum State { get; set; }

        /// <summary>
        /// Gets or sets the state before the change.
        /// </summary>
        public RemediationStateEnum? PreviousState { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the change occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the reason for the state change.
        /// </summary>
        public string Reason { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets additional details about the state change.
        /// </summary>
        public string Details { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user who initiated the change, if applicable.
        /// </summary>
        public string ChangedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional metadata about the change.
        /// </summary>
        public System.Collections.Generic.Dictionary<string, object> Metadata { get; set; } = new System.Collections.Generic.Dictionary<string, object>();
    }
} 