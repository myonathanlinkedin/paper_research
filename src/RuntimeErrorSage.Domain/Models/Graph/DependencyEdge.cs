using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Graph
{
    /// <summary>
    /// Represents an edge in a dependency graph.
    /// </summary>
    public class DependencyEdge
    {
        /// <summary>
        /// Gets or sets the unique identifier of the edge.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the source node ID.
        /// </summary>
        public string SourceId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target node ID.
        /// </summary>
        public string TargetId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        public GraphNode Source { get; set; }

        /// <summary>
        /// Gets or sets the target node.
        /// </summary>
        public GraphNode Target { get; set; }

        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        public RelationshipType RelationshipType { get; set; }

        /// <summary>
        /// Gets or sets the edge weight (0-1).
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the edge label.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the edge is directed.
        /// </summary>
        public bool IsDirected { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the edge is critical.
        /// </summary>
        public bool IsCritical { get; set; }

        /// <summary>
        /// Gets or sets the edge metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyEdge"/> class.
        /// </summary>
        public DependencyEdge()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyEdge"/> class.
        /// </summary>
        /// <param name="sourceNode">The source node.</param>
        /// <param name="targetNode">The target node.</param>
        /// <param name="label">The edge label.</param>
        public DependencyEdge(GraphNode sourceNode, GraphNode targetNode, string label = "depends_on")
        {
            Source = sourceNode ?? throw new ArgumentNullException(nameof(sourceNode));
            Target = targetNode ?? throw new ArgumentNullException(nameof(targetNode));
            SourceId = sourceNode.Id;
            TargetId = targetNode.Id;
            Label = label;
        }
    }
} 
