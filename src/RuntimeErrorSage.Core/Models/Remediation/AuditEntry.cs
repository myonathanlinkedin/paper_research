using System;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents an audit entry for a remediation action.
    /// </summary>
    public class AuditEntry
    {
        /// <summary>
        /// Gets or sets the ID of the action.
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the type of event.
        /// </summary>
        public AuditEventType EventType { get; set; }

        /// <summary>
        /// Gets or sets additional details about the event.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who performed the action.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the event.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
} 