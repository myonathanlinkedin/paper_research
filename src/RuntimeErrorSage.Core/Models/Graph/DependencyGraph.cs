using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents a dependency graph of components and their relationships.
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Gets or sets the unique identifier of the graph.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the nodes in the graph, keyed by node ID.
        /// </summary>
        public Dictionary<string, GraphNode> Nodes { get; set; } = new Dictionary<string, GraphNode>();

        /// <summary>
        /// Gets or sets the dependency nodes in the graph.
        /// </summary>
        public List<DependencyNode> DependencyNodes { get; set; } = new List<DependencyNode>();

        /// <summary>
        /// Gets or sets the edges in the graph.
        /// </summary>
        public List<GraphEdge> Edges { get; set; } = new List<GraphEdge>();

        /// <summary>
        /// Gets or sets the dependency edges in the graph.
        /// </summary>
        public List<DependencyEdge> DependencyEdges { get; set; } = new List<DependencyEdge>();

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the graph creation.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the metadata associated with the graph.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
        
        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(GraphNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            Nodes[node.Id] = node;
        }

        /// <summary>
        /// Adds an edge to the graph.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public void AddEdge(GraphEdge edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            Edges.Add(edge);
            edge.Source.AddOutgoingEdge(edge);
            edge.Target.AddIncomingEdge(edge);
        }

        /// <summary>
        /// Removes a node from the graph.
        /// </summary>
        /// <param name="nodeId">The ID of the node to remove.</param>
        /// <returns>True if the node was removed; otherwise, false.</returns>
        public bool RemoveNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
            {
                throw new ArgumentException("Node ID cannot be null or empty.", nameof(nodeId));
            }

            if (!Nodes.TryGetValue(nodeId, out var node))
            {
                return false;
            }

            // Remove associated edges
            var edgesToRemove = new List<GraphEdge>();
            edgesToRemove.AddRange(node.IncomingEdges);
            edgesToRemove.AddRange(node.OutgoingEdges);

            foreach (var edge in edgesToRemove)
            {
                Edges.Remove(edge);
            }

            return Nodes.Remove(nodeId);
        }
    }
}