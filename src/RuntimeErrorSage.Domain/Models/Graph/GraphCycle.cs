using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Graph;

/// <summary>
/// Represents a cycle in a dependency graph.
/// </summary>
public class GraphCycle
{
    /// <summary>
    /// Gets or sets the nodes involved in the cycle.
    /// </summary>
    public List<GraphNode> Nodes { get; set; } = new List<GraphNode>();

    /// <summary>
    /// Gets or sets the edges that form the cycle.
    /// </summary>
    public List<GraphEdge> Edges { get; set; } = new List<GraphEdge>();

    /// <summary>
    /// Gets the length of the cycle (number of nodes involved).
    /// </summary>
    public int CycleLength => Nodes.Count;

    /// <summary>
    /// Gets or sets the timestamp when the cycle was detected.
    /// </summary>
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the impact score of the cycle.
    /// </summary>
    public double ImpactScore { get; set; }

    /// <summary>
    /// Gets or sets additional metadata about the cycle.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
} 
