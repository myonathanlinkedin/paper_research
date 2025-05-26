using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents an edge in the dependency graph.
    /// </summary>
    public class DependencyEdge
    {
        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        public string SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the target node identifier.
        /// </summary>
        public string TargetNodeId { get; set; }

        /// <summary>
        /// Gets or sets the edge type.
        /// </summary>
        public string EdgeType { get; set; }

        /// <summary>
        /// Gets or sets the edge properties.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }
    }
} 