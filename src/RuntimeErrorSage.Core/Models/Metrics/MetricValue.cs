using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Metrics;

/// <summary>
/// Represents a metric value with associated metadata.
/// </summary>
public class MetricValue
{
    /// <summary>
    /// Gets or sets the metric name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the metric value.
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the metric was recorded.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the metric labels.
    /// </summary>
    public required Dictionary<string, string> Labels { get; set; }
} 
