using System;

namespace RuntimeErrorSage.Domain.Models.Graph
{
    /// <summary>
    /// Represents a dependency between components in the system.
    /// </summary>
    public class ComponentDependency
    {
        /// <summary>
        /// Gets or sets the unique identifier for this dependency.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the source component ID.
        /// </summary>
        public string SourceComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target component ID.
        /// </summary>
        public string TargetComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of dependency.
        /// </summary>
        public string DependencyType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the strength of the dependency.
        /// </summary>
        public double Strength { get; set; }

        /// <summary>
        /// Gets or sets whether the dependency is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the direction of the dependency.
        /// </summary>
        public string Direction { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the dependency was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the dependency was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the version of the dependency.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the dependency.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
} 
