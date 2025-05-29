using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents metadata for a remediation strategy.
    /// </summary>
    public class StrategyMetadata
    {
        /// <summary>
        /// Gets or sets the unique identifier for this strategy metadata.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the strategy.
        /// </summary>
        public string Name { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the version of the strategy.
        /// </summary>
        public string Version { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the strategy.
        /// </summary>
        public string Description { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the author of the strategy.
        /// </summary>
        public string Author { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the creation date of the strategy.
        /// </summary>
        public DateTime CreatedDate { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last modified date of the strategy.
        /// </summary>
        public DateTime LastModifiedDate { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the list of error types this strategy can handle.
        /// </summary>
        public IReadOnlyCollection<SupportedErrorTypes> SupportedErrorTypes { get; } = new();

        /// <summary>
        /// Gets or sets the list of dependencies required by this strategy.
        /// </summary>
        public IReadOnlyCollection<Dependencies> Dependencies { get; } = new();

        /// <summary>
        /// Gets or sets the configuration parameters for this strategy.
        /// </summary>
        public Dictionary<string, object> Configuration { get; set; } = new();

        /// <summary>
        /// Gets or sets whether this strategy is enabled.
        /// </summary>
        public bool IsEnabled { get; } = true;

        /// <summary>
        /// Gets or sets the priority level of this strategy.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Gets or sets tags associated with this strategy.
        /// </summary>
        public IReadOnlyCollection<Tags> Tags { get; } = new();
    }
} 






