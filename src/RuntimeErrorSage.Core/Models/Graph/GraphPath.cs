using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents a path in a dependency graph.
/// </summary>
public class GraphPath
{
    /// <summary>
    /// Gets or sets the unique identifier of the path.
    /// </summary>
    public string PathId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the source node.
    /// </summary>
    public GraphNode Source { get; set; }

    /// <summary>
    /// Gets or sets the target node.
    /// </summary>
    public GraphNode Target { get; set; }

    /// <summary>
    /// Gets or sets the nodes in the path.
    /// </summary>
    public List<GraphNode> Nodes { get; set; } = new();

    /// <summary>
    /// Gets or sets the edges in the path.
    /// </summary>
    public List<GraphEdge> Edges { get; set; } = new();

    /// <summary>
    /// Gets or sets the length of the path.
    /// </summary>
    public int Length => Edges.Count;

    /// <summary>
    /// Gets or sets the weight of the path.
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Gets or sets the description of the path.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this path is critical.
    /// </summary>
    public bool IsCritical { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the path was identified.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the metadata of the path.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 