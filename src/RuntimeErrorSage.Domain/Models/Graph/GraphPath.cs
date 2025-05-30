using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Graph;

/// <summary>
/// Represents a path between nodes in a dependency graph.
/// </summary>
public class GraphPath
{
    /// <summary>
    /// Gets or sets the unique identifier for this path.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the nodes in the path.
    /// </summary>
    public List<GraphNode> Nodes { get; set; } = new List<GraphNode>();

    /// <summary>
    /// Gets or sets the ID of the source node.
    /// </summary>
    public string SourceNodeId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the target node.
    /// </summary>
    public string TargetNodeId { get; set; }

    /// <summary>
    /// Gets or sets whether the path is complete (connects source to target).
    /// </summary>
    public bool IsComplete { get; set; }

    /// <summary>
    /// Gets the length of the path (number of nodes - 1).
    /// </summary>
    public int Length => Nodes.Count > 0 ? Nodes.Count - 1 : 0;

    /// <summary>
    /// Gets or sets the creation time of the path.
    /// </summary>
    public DateTime CreationTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the path weight (sum of edge weights).
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Gets or sets metadata associated with the path.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
} 
