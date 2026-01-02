using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Graph
{
    /// <summary>
    /// Represents a node in a dependency graph.
    /// </summary>
    public class DependencyNode : GraphNode
    {
        /// <summary>
        /// Gets or sets the node label.
        /// </summary>
        public string Label 
        { 
            get => Name; 
            set => Name = value;
        }
        
        /// <summary>
        /// Gets or sets the component ID.
        /// </summary>
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the node is an error source.
        /// </summary>
        public bool IsErrorSource { get; set; }

        /// <summary>
        /// Gets or sets the dependencies.
        /// </summary>
        public new List<DependencyNode> Dependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the dependents.
        /// </summary>
        public new List<DependencyNode> Dependents { get; set; } = new();
        
        /// <summary>
        /// Gets the key for the node (alias for Id).
        /// </summary>
        public string Key => Id;
        
        /// <summary>
        /// Gets the value for the node (alias for this instance).
        /// </summary>
        public DependencyNode Value => this;
    }
} 
