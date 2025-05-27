using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Common
{
    /// <summary>
    /// Represents detailed status information for a remediation operation.
    /// </summary>
    public class RemediationStatusInfo
    {
        /// <summary>
        /// Gets the Running status constant.
        /// </summary>
        public static readonly RemediationStatusEnum Running = RemediationStatusEnum.InProgress;

        /// <summary>
        /// Gets the Completed status constant.
        /// </summary>
        public static readonly RemediationStatusEnum Completed = RemediationStatusEnum.Completed;

        /// <summary>
        /// Gets the Failed status constant.
        /// </summary>
        public static readonly RemediationStatusEnum Failed = RemediationStatusEnum.Failed;

        /// <summary>
        /// Gets or sets the current status.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

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
        public RemediationStatusEnum Status { get; set; }

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