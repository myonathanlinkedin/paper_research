using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Models.Graph;

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
    public List<RelatedError> RelatedErrors { get; set; } = new();

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
    /// Gets or sets the analysis metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents a dependency graph used for analysis.
/// </summary>
public class DependencyGraph
{
    /// <summary>
    /// Gets or sets the unique identifier of the graph.
    /// </summary>
    public string GraphId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the nodes in the graph.
    /// </summary>
    public List<GraphNode> Nodes { get; set; } = new();

    /// <summary>
    /// Gets or sets the edges in the graph.
    /// </summary>
    public List<GraphEdge> Edges { get; set; } = new();

    /// <summary>
    /// Gets or sets the graph metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents a node in the dependency graph.
/// </summary>
public class GraphNode
{
    /// <summary>
    /// Gets or sets the unique identifier of the node.
    /// </summary>
    public string NodeId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the node.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the node.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the node metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents an edge in the dependency graph.
/// </summary>
public class GraphEdge
{
    /// <summary>
    /// Gets or sets the unique identifier of the edge.
    /// </summary>
    public string EdgeId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the source node identifier.
    /// </summary>
    public string SourceNodeId { get; set; }

    /// <summary>
    /// Gets or sets the target node identifier.
    /// </summary>
    public string TargetNodeId { get; set; }

    /// <summary>
    /// Gets or sets the type of relationship.
    /// </summary>
    public RelationshipType Type { get; set; }

    /// <summary>
    /// Gets or sets the edge metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents the result of an impact analysis.
/// </summary>
public class ImpactAnalysisResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the impact analysis.
    /// </summary>
    public string ImpactId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the scope of the impact.
    /// </summary>
    public ImpactScope Scope { get; set; }

    /// <summary>
    /// Gets or sets the severity of the impact.
    /// </summary>
    public ImpactSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the affected components.
    /// </summary>
    public List<string> AffectedComponents { get; set; } = new();

    /// <summary>
    /// Gets or sets the impact metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents a related error found during analysis.
/// </summary>
public class RelatedError
{
    /// <summary>
    /// Gets or sets the unique identifier of the related error.
    /// </summary>
    public string ErrorId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public SeverityLevel Severity { get; set; }

    /// <summary>
    /// Gets or sets the component where the error occurred.
    /// </summary>
    public string Component { get; set; }

    /// <summary>
    /// Gets or sets the error metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Defines the types of relationships between graph nodes.
/// </summary>
public enum RelationshipType
{
    /// <summary>
    /// Direct dependency relationship.
    /// </summary>
    DirectDependency,

    /// <summary>
    /// Indirect dependency relationship.
    /// </summary>
    IndirectDependency,

    /// <summary>
    /// Reference relationship.
    /// </summary>
    Reference,

    /// <summary>
    /// Inheritance relationship.
    /// </summary>
    Inheritance,

    /// <summary>
    /// Implementation relationship.
    /// </summary>
    Implementation,

    /// <summary>
    /// Association relationship.
    /// </summary>
    Association,

    /// <summary>
    /// Composition relationship.
    /// </summary>
    Composition,

    /// <summary>
    /// Aggregation relationship.
    /// </summary>
    Aggregation,

    /// <summary>
    /// Unknown relationship type.
    /// </summary>
    Unknown
} 