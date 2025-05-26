using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents a dependency graph for analyzing component relationships and error propagation.
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Gets or sets the graph identifier.
        /// </summary>
        public string GraphId { get; set; }

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
        /// Gets or sets the graph metrics.
        /// </summary>
        public Dictionary<string, double> Metrics { get; set; } = new();

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(DependencyNode node)
        {
            if (!Nodes.Any(n => n.Id == node.Id))
            {
                Nodes.Add(node);
            }
        }

        /// <summary>
        /// Adds an edge to the graph.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public void AddEdge(DependencyEdge edge)
        {
            if (!Edges.Any(e => e.SourceId == edge.SourceId && e.TargetId == edge.TargetId))
            {
                Edges.Add(edge);
            }
        }

        /// <summary>
        /// Updates the graph metrics.
        /// </summary>
        public void UpdateMetrics()
        {
            Metrics["complexity"] = CalculateComplexity();
            Metrics["reliability"] = CalculateReliability();
            Metrics["connectivity"] = CalculateConnectivity();
        }

        private double CalculateComplexity()
        {
            return Edges.Count / (double)Math.Max(1, Nodes.Count);
        }

        private double CalculateReliability()
        {
            if (Nodes.Count == 0) return 1.0;
            return Nodes.Average(n => n.Reliability);
        }

        private double CalculateConnectivity()
        {
            if (Nodes.Count <= 1) return 1.0;
            return Edges.Count / (double)(Nodes.Count * (Nodes.Count - 1));
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
}