using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.LLM;
using RuntimeErrorSage.Application.Models.Context;

namespace RuntimeErrorSage.Application.Models.Graph;

/// <summary>
/// Represents the result of a graph analysis operation.
/// </summary>
public class GraphAnalysisResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the analysis.
    /// </summary>
    public string AnalysisId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the dependency graph used for analysis.
    /// </summary>
    public DependencyGraph DependencyGraph { get; set; }

    /// <summary>
    /// Gets or sets the impact analysis results.
    /// </summary>
    public List<ImpactAnalysisResult> ImpactResults { get; set; } = new();

    /// <summary>
    /// Gets or sets the related errors found during analysis.
    /// </summary>
    public List<RuntimeErrorSage.Application.Models.Error.RelatedError> RelatedErrors { get; set; } = new();

    /// <summary>
    /// Gets or sets the analysis start time.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the analysis end time.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the analysis duration in milliseconds.
    /// </summary>
    public double DurationMs => EndTime.HasValue ? (EndTime.Value - StartTime).TotalMilliseconds : 0;

    /// <summary>
    /// Gets or sets the analysis status.
    /// </summary>
    public AnalysisStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the analysis error message if any.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets whether the analysis is valid.
    /// </summary>
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// Gets or sets the analysis metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the correlation ID.
    /// </summary>
    public string CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the analysis.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the analysis metrics.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of nodes in the graph.
    /// </summary>
    public List<DependencyNode> Nodes { get; set; } = new List<DependencyNode>();

    /// <summary>
    /// Gets or sets the list of edges in the graph.
    /// </summary>
    public List<DependencyEdge> Edges { get; set; } = new List<DependencyEdge>();

    /// <summary>
    /// Gets or sets the root node.
    /// </summary>
    public DependencyNode RootNode { get; set; }

    /// <summary>
    /// Gets or sets the error context.
    /// </summary>
    public ErrorContext Context { get; set; }

    /// <summary>
    /// Gets or sets the component ID.
    /// </summary>
    public string ComponentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the component name.
    /// </summary>
    public string ComponentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional context.
    /// </summary>
    public Dictionary<string, object> AdditionalContext { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the analysis insights.
    /// </summary>
    public GraphInsights Insights { get; set; }
}
