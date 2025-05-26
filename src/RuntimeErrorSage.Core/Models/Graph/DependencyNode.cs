using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents a node in the dependency graph.
    /// </summary>
    public class DependencyNode
    {
        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the node type.
        /// </summary>
        public string NodeType { get; set; }

        /// <summary>
        /// Gets or sets the node properties.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }
    }
} 