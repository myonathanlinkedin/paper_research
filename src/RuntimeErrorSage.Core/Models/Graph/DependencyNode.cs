using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents a node in a dependency analysis.
    /// </summary>
    public class DependencyNode
    {
        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        public string ComponentId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component type.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component version.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the dependencies.
        /// </summary>
        public List<DependencyNode> Dependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the dependents.
        /// </summary>
        public List<DependencyNode> Dependents { get; set; } = new();

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 