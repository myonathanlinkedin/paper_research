using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents the health status of a system after remediation.
/// </summary>
public class RemediationHealthStatus
{
    /// <summary>
    /// Gets or sets whether the system is considered healthy.
    /// </summary>
    public bool IsHealthy { get; set; }

    /// <summary>
    /// Gets or sets the health score as a percentage (0-100).
    /// </summary>
    public double HealthScore { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the health status was determined.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the metrics used to determine the health status.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new Dictionary<string, double>();

    /// <summary>
    /// Gets or sets the ID of the component being evaluated.
    /// </summary>
    public string ComponentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the component being evaluated.
    /// </summary>
    public string ComponentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error message if health check failed.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Gets or sets additional details about the health status.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the recommendations for improving health.
    /// </summary>
    public List<string> Recommendations { get; set; } = new List<string>();
} 