using System;
using System.Collections.Generic;
using System.Linq;

namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents a dependency graph.
/// </summary>
public class DependencyGraph
{
    /// <summary>
    /// Gets or sets the nodes in the graph.
    /// </summary>
    public List<DependencyNode> Nodes { get; set; } = new();

    /// <summary>
    /// Gets or sets the edges in the graph.
    /// </summary>
    public List<DependencyEdge> Edges { get; set; } = new();

    /// <summary>
    /// Gets or sets the graph metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Adds a node to the graph.
    /// </summary>
    /// <param name="node">The node to add.</param>
    public void AddNode(DependencyNode node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        if (Nodes.Any(n => n.Id == node.Id))
        {
            throw new InvalidOperationException($"Node with ID {node.Id} already exists in the graph.");
        }

        Nodes.Add(node);
    }

    /// <summary>
    /// Adds an edge to the graph.
    /// </summary>
    /// <param name="edge">The edge to add.</param>
    public void AddEdge(DependencyEdge edge)
    {
        if (edge == null)
        {
            throw new ArgumentNullException(nameof(edge));
        }

        if (!Nodes.Any(n => n.Id == edge.SourceId))
        {
            throw new InvalidOperationException($"Source node with ID {edge.SourceId} does not exist in the graph.");
        }

        if (!Nodes.Any(n => n.Id == edge.TargetId))
        {
            throw new InvalidOperationException($"Target node with ID {edge.TargetId} does not exist in the graph.");
        }

        if (Edges.Any(e => e.Id == edge.Id))
        {
            throw new InvalidOperationException($"Edge with ID {edge.Id} already exists in the graph.");
        }

        Edges.Add(edge);
    }

    /// <summary>
    /// Gets the neighbors of a node.
    /// </summary>
    /// <param name="nodeId">The ID of the node.</param>
    /// <returns>The list of neighboring nodes.</returns>
    public List<DependencyNode> GetNeighbors(string nodeId)
    {
        var neighbors = new List<DependencyNode>();
        var edges = Edges.Where(e => e.SourceId == nodeId || e.TargetId == nodeId);

        foreach (var edge in edges)
        {
            var neighborId = edge.SourceId == nodeId ? edge.TargetId : edge.SourceId;
            var neighbor = Nodes.FirstOrDefault(n => n.Id == neighborId);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    /// <summary>
    /// Gets the outgoing edges from a node.
    /// </summary>
    /// <param name="nodeId">The ID of the node.</param>
    /// <returns>The list of outgoing edges.</returns>
    public List<DependencyEdge> GetOutgoingEdges(string nodeId)
    {
        return Edges.Where(e => e.SourceId == nodeId).ToList();
    }

    /// <summary>
    /// Gets the incoming edges to a node.
    /// </summary>
    /// <param name="nodeId">The ID of the node.</param>
    /// <returns>The list of incoming edges.</returns>
    public List<DependencyEdge> GetIncomingEdges(string nodeId)
    {
        return Edges.Where(e => e.TargetId == nodeId).ToList();
    }

    /// <summary>
    /// Gets the path between two nodes.
    /// </summary>
    /// <param name="sourceId">The ID of the source node.</param>
    /// <param name="targetId">The ID of the target node.</param>
    /// <returns>The list of nodes in the path, or null if no path exists.</returns>
    public List<DependencyNode> GetPath(string sourceId, string targetId)
    {
        var visited = new HashSet<string>();
        var path = new List<DependencyNode>();
        var found = FindPath(sourceId, targetId, visited, path);
        return found ? path : null;
    }

    private bool FindPath(string currentId, string targetId, HashSet<string> visited, List<DependencyNode> path)
    {
        if (currentId == targetId)
        {
            var node = Nodes.FirstOrDefault(n => n.Id == currentId);
            if (node != null)
            {
                path.Add(node);
            }
            return true;
        }

        visited.Add(currentId);
        var currentNode = Nodes.FirstOrDefault(n => n.Id == currentId);
        if (currentNode != null)
        {
            path.Add(currentNode);
        }

        foreach (var edge in GetOutgoingEdges(currentId))
        {
            if (!visited.Contains(edge.TargetId))
            {
                if (FindPath(edge.TargetId, targetId, visited, path))
                {
                    return true;
                }
            }
        }

        path.RemoveAt(path.Count - 1);
        return false;
    }
}

/// <summary>
/// Represents a node in the dependency graph.
/// </summary>
public class DependencyNode
{
    /// <summary>
    /// Gets or sets the unique identifier for this node.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the node name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the node type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the node properties.
    /// </summary>
    public Dictionary<string, object> Properties { get; set; } = new();

    /// <summary>
    /// Gets or sets the node metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents an edge in the dependency graph.
/// </summary>
public class DependencyEdge
{
    /// <summary>
    /// Gets or sets the unique identifier for this edge.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the source node ID.
    /// </summary>
    public string SourceId { get; set; }

    /// <summary>
    /// Gets or sets the target node ID.
    /// </summary>
    public string TargetId { get; set; }

    /// <summary>
    /// Gets or sets the edge type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the edge weight.
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Gets or sets whether the edge is directed.
    /// </summary>
    public bool IsDirected { get; set; }

    /// <summary>
    /// Gets or sets the edge properties.
    /// </summary>
    public Dictionary<string, object> Properties { get; set; } = new();

    /// <summary>
    /// Gets or sets the edge metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

public class ImpactAnalysisResult
{
    public string AnalysisId { get; set; }
    public DateTime Timestamp { get; set; }
    public List<ImpactedComponent> ImpactedComponents { get; set; } = new();
    public Dictionary<string, double> Metrics { get; set; } = new();
}

public class ImpactedComponent
{
    public string ComponentId { get; set; }
    public string Type { get; set; }
    public double ImpactScore { get; set; }
    public List<string> Dependencies { get; set; } = new();
    public Dictionary<string, object> Properties { get; set; } = new();
}