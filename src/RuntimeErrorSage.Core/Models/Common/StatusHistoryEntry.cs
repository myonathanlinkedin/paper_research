using System;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Common;

/// <summary>
/// Represents a single entry in the status history.
/// </summary>
public class StatusHistoryEntry
{
    /// <summary>
    /// Gets or sets the status at this point in time.
    /// </summary>
    public RemediationStatusEnum Status { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this status was recorded.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the message associated with this status.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any error details.
    /// </summary>
    public string ErrorDetails { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any warnings.
    /// </summary>
    public List<string> Warnings { get; set; } = new();

    /// <summary>
    /// Gets or sets the progress percentage (0-100).
    /// </summary>
    public double Progress { get; set; }

    /// <summary>
    /// Gets or sets any additional metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 
