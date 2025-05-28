using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Metrics;

/// <summary>
/// Represents metrics collected during error handling.
/// </summary>
public class ErrorHandlingMetrics
{
    /// <summary>
    /// Gets or sets the unique identifier for these metrics.
    /// </summary>
    public string MetricsId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets when the metrics were recorded.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string ErrorType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the duration of error handling in milliseconds.
    /// </summary>
    public double DurationMs { get; set; }

    /// <summary>
    /// Gets or sets whether the error was successfully handled.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the number of remediation attempts.
    /// </summary>
    public int RemediationAttempts { get; set; }

    /// <summary>
    /// Gets or sets the memory usage during error handling in bytes.
    /// </summary>
    public long MemoryUsageBytes { get; set; }

    /// <summary>
    /// Gets or sets the CPU usage percentage during error handling.
    /// </summary>
    public double CpuUsagePercent { get; set; }

    /// <summary>
    /// Gets or sets any additional metrics collected.
    /// </summary>
    public Dictionary<string, double> AdditionalMetrics { get; set; } = new();

    /// <summary>
    /// Gets or sets any labels associated with these metrics.
    /// </summary>
    public Dictionary<string, string> Labels { get; set; } = new();
} 