using System;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Common;

/// <summary>
/// Represents a single entry in a status history.
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
    /// Gets or sets the timestamp when this status was recorded.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
} 