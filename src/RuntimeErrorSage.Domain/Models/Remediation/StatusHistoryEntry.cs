using System;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents a status history entry.
    /// </summary>
    public class StatusHistoryEntry
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public RemediationState State { get; set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets when the status was recorded.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
} 
