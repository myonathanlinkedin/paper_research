using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Health;

/// <summary>
/// Represents a health prediction for a service or component.
/// </summary>
public class HealthPrediction
{
    /// <summary>
    /// Gets or sets the unique identifier of this prediction.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the timestamp when the prediction was made.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the predicted health score (0-100).
    /// </summary>
    public double PredictedHealthScore { get; set; }

    /// <summary>
    /// Gets or sets the estimated time until the service becomes unhealthy.
    /// </summary>
    public TimeSpan? TimeToUnhealthy { get; set; }

    /// <summary>
    /// Gets or sets the confidence level of the prediction (0-1).
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the trends for various health metrics.
    /// </summary>
    public Dictionary<string, double> Trends { get; set; } = new Dictionary<string, double>();

    /// <summary>
    /// Gets or sets the component ID this prediction is for.
    /// </summary>
    public string ComponentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional metadata for the prediction.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
} 

