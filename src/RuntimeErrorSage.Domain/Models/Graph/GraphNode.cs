using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Graph
{
    /// <summary>
    /// Represents a node in the dependency graph.
    /// </summary>
    public class GraphNode
    {
        /// <summary>
        /// Gets or sets the unique identifier for this node.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the node.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of incoming dependencies.
        /// </summary>
        public List<string> IncomingDependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of outgoing dependencies.
        /// </summary>
        public List<string> OutgoingDependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the metadata for this node.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the timestamp when the node was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the node was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the version of the node.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the node.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the node is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the status of the node.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the importance level of the node.
        /// </summary>
        public double ImportanceLevel { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets whether the node is critical.
        /// </summary>
        public bool IsCritical { get; set; }

        /// <summary>
        /// Gets or sets the health status of the node.
        /// </summary>
        public string HealthStatus { get; set; } = "Healthy";

        /// <summary>
        /// Gets or sets the probability of error for this node (0-1).
        /// </summary>
        public double ErrorProbability { get; set; }

        /// <summary>
        /// Gets or sets the list of dependencies (nodes this node depends on)
        /// </summary>
        public List<string> Dependencies { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the list of dependents (nodes that depend on this node)
        /// </summary>
        public List<string> Dependents { get; set; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphNode"/> class.
        /// </summary>
        public GraphNode()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
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

            IncomingDependencies.Add(edge.Source.Id);
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

            OutgoingDependencies.Add(edge.Target.Id);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Removes an incoming edge from this node.
        /// </summary>
        /// <param name="edge">The edge to remove.</param>
        public void RemoveIncomingEdge(GraphEdge edge)
        {
            IncomingDependencies.Remove(edge.Source.Id);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Removes an outgoing edge from this node.
        /// </summary>
        /// <param name="edge">The edge to remove.</param>
        public void RemoveOutgoingEdge(GraphEdge edge)
        {
            OutgoingDependencies.Remove(edge.Target.Id);
            UpdatedAt = DateTime.UtcNow;
        }
    }
} 
