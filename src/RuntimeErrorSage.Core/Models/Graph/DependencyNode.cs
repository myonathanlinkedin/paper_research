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
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        public string ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the node type.
        /// </summary>
        public string NodeType { get; set; }

        /// <summary>
        /// Gets or sets the node properties.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new();

        /// <summary>
        /// Gets or sets the node reliability score (0.0 to 1.0).
        /// </summary>
        public double Reliability { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the error probability (0.0 to 1.0).
        /// </summary>
        public double ErrorProbability { get; set; } = 0.0;
    }
} 