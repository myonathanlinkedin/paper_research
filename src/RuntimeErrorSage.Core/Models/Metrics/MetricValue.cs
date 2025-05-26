using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Metrics;

/// <summary>
/// Represents a single metric value with metadata.
/// </summary>
public sealed class MetricValue
{
    /// <summary>
    /// Gets the timestamp when the metric was recorded.
    /// </summary>
    public required DateTime Timestamp { get; init; }

    /// <summary>
    /// Gets the numeric value of the metric.
    /// </summary>
    public required double Value { get; init; }

    /// <summary>
    /// Gets the name of the metric.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the labels associated with this metric value.
    /// </summary>
    public required IReadOnlyDictionary<string, string> Labels { get; init; } = new Dictionary<string, string>();
} 
