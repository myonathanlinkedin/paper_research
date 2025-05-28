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
        /// Gets or sets the nodes in the graph.
        /// </summary>
        public List<DependencyNode> Nodes { get; set; } = new List<DependencyNode>();

        /// <summary>
        /// Gets or sets the edges in the graph.
        /// </summary>
        public List<DependencyEdge> Edges { get; set; } = new List<DependencyEdge>();

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
    }
}