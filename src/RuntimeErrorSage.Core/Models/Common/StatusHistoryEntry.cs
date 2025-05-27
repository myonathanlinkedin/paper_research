using System;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Common;

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