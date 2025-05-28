using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents a node in a dependency graph.
    /// </summary>
    public class DependencyNode
    {
        /// <summary>
        /// Gets or sets the unique identifier of the node.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the node label.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component ID.
        /// </summary>
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the node type.
        /// </summary>
        public string NodeType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the health score of the node (0-1).
        /// </summary>
        public double HealthScore { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the node metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets whether the node is critical.
        /// </summary>
        public bool IsCritical { get; set; }

        /// <summary>
        /// Gets or sets whether the node is an error source.
        /// </summary>
        public bool IsErrorSource { get; set; }

        /// <summary>
        /// Gets or sets the dependencies.
        /// </summary>
        public List<DependencyNode> Dependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the dependents.
        /// </summary>
        public List<DependencyNode> Dependents { get; set; } = new();
    }
} 