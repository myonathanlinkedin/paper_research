using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Model.Models.Health;

/// <summary>
/// Represents a health prediction for a system component.
/// </summary>
public class HealthPrediction
{
    /// <summary>
    /// Gets or sets the unique identifier for this prediction.
    /// </summary>
    public string PredictionId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the component ID.
    /// </summary>
    public string ComponentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the predicted health status.
    /// </summary>
    public HealthStatusEnum PredictedStatus { get; set; }

    /// <summary>
    /// Gets or sets the confidence score (0-1).
    /// </summary>
    public double ConfidenceScore { get; set; }

    /// <summary>
    /// Gets or sets the prediction timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the prediction window start.
    /// </summary>
    public DateTime WindowStart { get; set; }

    /// <summary>
    /// Gets or sets the prediction window end.
    /// </summary>
    public DateTime WindowEnd { get; set; }

    /// <summary>
    /// Gets or sets the prediction factors.
    /// </summary>
    public List<string> Factors { get; set; } = new();

    /// <summary>
    /// Gets or sets the prediction metrics.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new();

    /// <summary>
    /// Gets or sets whether the prediction is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the prediction metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 

