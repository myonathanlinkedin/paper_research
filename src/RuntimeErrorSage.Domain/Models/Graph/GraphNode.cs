using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Graph
{
    /// <summary>
    /// Represents a node in a dependency graph.
    /// </summary>
    public class GraphNode
    {
        /// <summary>
        /// Gets or sets the unique identifier for this node.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the type of the node.
        /// </summary>
        public GraphNodeType Type { get; } = GraphNodeType.Unknown;

        /// <summary>
        /// Gets or sets the status of the node.
        /// </summary>
        public string Status { get; } = "Active";

        /// <summary>
        /// Gets or sets the description of the node.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets or sets the properties of the node.
        /// </summary>
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the importance level of the node.
        /// </summary>
        public double ImportanceLevel { get; } = 1.0;

        /// <summary>
        /// Gets or sets whether the node is critical.
        /// </summary>
        public bool IsCritical { get; }

        /// <summary>
        /// Gets or sets the health status of the node.
        /// </summary>
        public string HealthStatus { get; } = "Healthy";

        /// <summary>
        /// Gets or sets the creation time of the node.
        /// </summary>
        public DateTime CreationTime { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets metadata associated with the node.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the incoming dependencies.
        /// </summary>
        public IReadOnlyCollection<IncomingEdges> IncomingEdges { get; }

        /// <summary>
        /// Gets or sets the outgoing dependencies.
        /// </summary>
        public IReadOnlyCollection<OutgoingEdges> OutgoingEdges { get; }

        /// <summary>
        /// Gets or sets the timestamp when the node was created.
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Gets or sets the timestamp when the node was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; }

        /// <summary>
        /// Gets or sets the probability of error for this node (0-1).
        /// </summary>
        public double ErrorProbability { get; }

        /// <summary>
        /// Gets or sets the list of dependencies (nodes this node depends on)
        /// </summary>
        public IReadOnlyCollection<Dependencies> Dependencies { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the list of dependents (nodes that depend on this node)
        /// </summary>
        public IReadOnlyCollection<Dependents> Dependents { get; } = new Collection<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphNode"/> class.
        /// </summary>
        public GraphNode()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IncomingEdges = new Collection<GraphEdge>();
            OutgoingEdges = new Collection<GraphEdge>();
        }

        /// <summary>
        /// Adds an incoming edge to this node.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public GraphEdge edge { ArgumentNullException.ThrowIfNull(GraphEdge edge); }
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
        public GraphEdge edge { ArgumentNullException.ThrowIfNull(GraphEdge edge); }
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
        public GraphEdge edge { ArgumentNullException.ThrowIfNull(GraphEdge edge); }
        {
            IncomingEdges.Remove(edge);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Removes an outgoing edge from this node.
        /// </summary>
        /// <param name="edge">The edge to remove.</param>
        public GraphEdge edge { ArgumentNullException.ThrowIfNull(GraphEdge edge); }
        {
            OutgoingEdges.Remove(edge);
            UpdatedAt = DateTime.UtcNow;
        }
    }
} 






