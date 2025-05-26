using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Graph.Enums;

namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents the result of an impact analysis on a dependency graph.
/// </summary>
public class ImpactAnalysisResult
{
    /// <summary>
    /// Gets or sets the analysis identifier.
    /// </summary>
    public string AnalysisId { get; set; }

    /// <summary>
    /// Gets or sets the component identifier.
    /// </summary>
    public string ComponentId { get; set; }

    /// <summary>
    /// Gets or sets the impact severity.
    /// </summary>
    public ImpactSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the impact scope.
    /// </summary>
    public ImpactScope Scope { get; set; }

    /// <summary>
    /// Gets or sets the impact metrics.
    /// </summary>
    public Dictionary<string, double> ImpactMetrics { get; set; } = new();

    /// <summary>
    /// Gets or sets the affected nodes.
    /// </summary>
    public List<string> AffectedNodes { get; set; } = new();

    /// <summary>
    /// Gets or sets the impact description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the confidence score.
    /// </summary>
    public double ConfidenceScore { get; set; }

    /// <summary>
    /// Gets or sets the analysis timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the analysis result is valid.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the metadata associated with the analysis result.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Adds a new impact metric to the analysis result.
    /// </summary>
    /// <param name="name">The name of the metric.</param>
    /// <param name="value">The value of the metric.</param>
    public void AddImpactMetric(string name, double value)
    {
        ImpactMetrics[name] = value;
    }

    /// <summary>
    /// Adds a new affected node to the analysis result.
    /// </summary>
    /// <param name="nodeId">The ID of the affected node.</param>
    public void AddAffectedNode(string nodeId)
    {
        if (!AffectedNodes.Contains(nodeId))
        {
            AffectedNodes.Add(nodeId);
        }
    }

    /// <summary>
    /// Adds new metadata to the analysis result.
    /// </summary>
    /// <param name="key">The key of the metadata.</param>
    /// <param name="value">The value of the metadata.</param>
    public void AddMetadata(string key, object value)
    {
        Metadata[key] = value;
    }
} 