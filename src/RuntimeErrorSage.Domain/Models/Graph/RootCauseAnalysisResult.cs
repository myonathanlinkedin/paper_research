using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Graph;

/// <summary>
/// Represents the result of a root cause analysis.
/// </summary>
public class RootCauseAnalysisResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the analysis.
    /// </summary>
    public string AnalysisId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error identifier.
    /// </summary>
    public string ErrorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error node identifier.
    /// </summary>
    public string ErrorNodeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the root cause node.
    /// </summary>
    public GraphNode RootCauseNode { get; set; }

    /// <summary>
    /// Gets or sets the path from the root cause to the error.
    /// </summary>
    public GraphPath PathToError { get; set; }

    /// <summary>
    /// Gets or sets the path from the root cause to the error (alias for PathToError for compatibility).
    /// </summary>
    public GraphPath RootCausePath 
    { 
        get => PathToError;
        set => PathToError = value;
    }

    /// <summary>
    /// Gets or sets the confidence level of the analysis (0.0 to 1.0).
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the status of the analysis.
    /// </summary>
    public AnalysisStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the analysis was performed.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the description of the root cause.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the alternative root causes.
    /// </summary>
    public List<GraphNode> AlternativeRootCauses { get; set; } = new();

    /// <summary>
    /// Gets or sets the contributing factors.
    /// </summary>
    public List<GraphNode> ContributingFactors { get; set; } = new();

    /// <summary>
    /// Gets or sets potential causes for the error.
    /// </summary>
    public List<GraphNode> PotentialCauses { get; set; } = new();

    /// <summary>
    /// Gets or sets the most likely root cause ID.
    /// </summary>
    public string MostLikelyRootCauseId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the root cause probability.
    /// </summary>
    public double RootCauseProbability { get; set; }
} 
