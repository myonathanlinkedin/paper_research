using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents a node in the dependency graph.
    /// </summary>
    public class GraphNode
    {
        /// <summary>
        /// Gets or sets the unique identifier of the node.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the node.
        /// </summary>
        public GraphNodeType Type { get; set; }

        /// <summary>
        /// Gets or sets the severity of impact on this node.
        /// </summary>
        public ImpactSeverity ImpactSeverity { get; set; }

        /// <summary>
        /// Gets or sets the scope of impact on this node.
        /// </summary>
        public ImpactScope ImpactScope { get; set; }

        /// <summary>
        /// Gets or sets the metadata associated with this node.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// Gets or sets the incoming dependencies.
        /// </summary>
        public List<GraphEdge> IncomingEdges { get; set; }

        /// <summary>
        /// Gets or sets the outgoing dependencies.
        /// </summary>
        public List<GraphEdge> OutgoingEdges { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the node was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the node was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the probability of error for this node (0-1).
        /// </summary>
        public double ErrorProbability { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphNode"/> class.
        /// </summary>
        public GraphNode()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Metadata = new Dictionary<string, object>();
            IncomingEdges = new List<GraphEdge>();
            OutgoingEdges = new List<GraphEdge>();
        }

        /// <summary>
        /// Adds an incoming edge to this node.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public void AddIncomingEdge(GraphEdge edge)
        {
            if (edge.Target.Id != Id)
            {
                throw new ArgumentException("Edge target must be this node.");
            }

            IncomingEdges.Add(edge);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Adds an outgoing edge from this node.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public void AddOutgoingEdge(GraphEdge edge)
        {
            if (edge.Source.Id != Id)
            {
                throw new ArgumentException("Edge source must be this node.");
            }

            OutgoingEdges.Add(edge);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Removes an incoming edge from this node.
        /// </summary>
        /// <param name="edge">The edge to remove.</param>
        public void RemoveIncomingEdge(GraphEdge edge)
        {
            IncomingEdges.Remove(edge);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Removes an outgoing edge from this node.
        /// </summary>
        /// <param name="edge">The edge to remove.</param>
        public void RemoveOutgoingEdge(GraphEdge edge)
        {
            OutgoingEdges.Remove(edge);
            UpdatedAt = DateTime.UtcNow;
        }
    }
} 