using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Common
{
    /// <summary>
    /// Represents detailed status information for a remediation operation.
    /// </summary>
    public class RemediationStatusInfo
    {
        /// <summary>
        /// Gets or sets the current status.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets when the status was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the progress percentage (0-100).
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// Gets or sets any error details.
        /// </summary>
        public string ErrorDetails { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any warnings.
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Gets or sets the status history.
        /// </summary>
        public List<StatusHistoryEntry> History { get; set; } = new();

        /// <summary>
        /// Gets or sets any additional status metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Represents a status history entry.
    /// </summary>
    public class StatusHistoryEntry
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public RemediationStatus Status { get; set; }

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