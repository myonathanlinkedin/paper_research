using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Metrics;

/// <summary>
/// Represents a metric value with metadata.
/// </summary>
public class MetricValue
{
    /// <summary>
    /// Gets or sets the metric name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the metric value.
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the metric labels.
    /// </summary>
    public Dictionary<string, string> Labels { get; set; } = new();

    /// <summary>
    /// Gets or sets the unit of the metric.
    /// </summary>
    public string Unit { get; set; }

    /// <summary>
    /// Gets or sets the source of the metric.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the tags associated with the metric.
    /// </summary>
    public Dictionary<string, string> Tags { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricValue"/> class.
    /// </summary>
    public MetricValue()
    {
        Tags = new Dictionary<string, string>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricValue"/> class.
    /// </summary>
    /// <param name="name">The name of the metric.</param>
    /// <param name="value">The value of the metric.</param>
    /// <param name="unit">The unit of the metric.</param>
    /// <param name="source">The source of the metric.</param>
    public MetricValue(string name, object value, string unit = null, string source = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Metric name cannot be null or empty.", nameof(name));
        }

        Name = name;
        Value = value;
        Unit = unit;
        Source = source;
        Timestamp = DateTime.UtcNow;
        Tags = new Dictionary<string, string>();
    }

    /// <summary>
    /// Adds a tag to the metric.
    /// </summary>
    /// <param name="key">The key of the tag.</param>
    /// <param name="value">The value of the tag.</param>
    public void AddTag(string key, string value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Tag key cannot be null or empty.", nameof(key));
        }

        Tags[key] = value;
    }

    /// <summary>
    /// Gets the string representation of the metric value.
    /// </summary>
    /// <returns>The string representation of the metric value.</returns>
    public override string ToString()
    {
        var unitStr = string.IsNullOrEmpty(Unit) ? string.Empty : $" {Unit}";
        return $"{Name}: {Value}{unitStr}";
    }
} 
