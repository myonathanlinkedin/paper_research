using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents an edge in a dependency graph.
    /// </summary>
    public class GraphEdge
    {
        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        public GraphNode Source { get; set; } = null!;

        /// <summary>
        /// Gets or sets the target node.
        /// </summary>
        public GraphNode Target { get; set; } = null!;

        /// <summary>
        /// Gets or sets the edge type.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the edge weight.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the edge metadata.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the edge label.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the edge color.
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the edge style.
        /// </summary>
        public string Style { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the edge thickness.
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// Gets or sets whether the edge is directed.
        /// </summary>
        public bool IsDirected { get; set; }

        /// <summary>
        /// Gets or sets whether the edge is bidirectional.
        /// </summary>
        public bool IsBidirectional { get; set; }

        /// <summary>
        /// Gets or sets the creation time of the edge.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update time of the edge.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphEdge"/> class.
        /// </summary>
        public GraphEdge()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphEdge"/> class.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="target">The target node.</param>
        /// <param name="type">The edge type.</param>
        public GraphEdge(GraphNode source, GraphNode target, string type)
        {
            Source = source;
            Target = target;
            Type = type;
        }

        /// <summary>
        /// Removes this edge from its source and target nodes.
        /// </summary>
        public void Remove()
        {
            Source.RemoveOutgoingEdge(this);
            Target.RemoveIncomingEdge(this);
        }

        /// <summary>
        /// Gets the source ID of the edge.
        /// </summary>
        public string SourceId => Source?.Id;

        /// <summary>
        /// Gets the target ID of the edge.
        /// </summary>
        public string TargetId => Target?.Id;
    }
} 
