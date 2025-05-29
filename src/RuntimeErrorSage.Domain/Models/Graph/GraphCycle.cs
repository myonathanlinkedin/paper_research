using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Models.Graph;

/// <summary>
/// Represents a cycle in a dependency graph.
/// </summary>
public class GraphCycle
{
    /// <summary>
    /// Gets or sets the unique identifier of the cycle.
    /// </summary>
    public string CycleId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the nodes in the cycle.
    /// </summary>
    public List<GraphNode> Nodes { get; set; } = new();

    /// <summary>
    /// Gets or sets the edges in the cycle.
    /// </summary>
    public List<GraphEdge> Edges { get; set; } = new();

    /// <summary>
    /// Gets or sets the length of the cycle.
    /// </summary>
    public int Length => Nodes.Count;

    /// <summary>
    /// Gets or sets the description of the cycle.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the cycle was identified.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the severity of the cycle.
    /// </summary>
    public int Severity { get; set; }

    /// <summary>
    /// Gets or sets whether the cycle is critical.
    /// </summary>
    public bool IsCritical { get; set; }

    /// <summary>
    /// Gets or sets the weight of the cycle.
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Gets or sets the metadata of the cycle.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the risk level of the cycle.
    /// </summary>
    public double RiskLevel { get; set; }
} 
