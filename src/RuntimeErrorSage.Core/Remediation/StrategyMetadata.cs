using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Remediation
{
    /// <summary>
    /// Represents metadata for a remediation strategy.
    /// </summary>
    public class StrategyMetadata
    {
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
        public DateTime CreationDate { get; }

        /// <summary>
        /// Gets or sets the last modified date of the strategy.
        /// </summary>
        public DateTime LastModifiedDate { get; }

        /// <summary>
        /// Gets or sets the dependencies of the strategy.
        /// </summary>
        public Dictionary<string, string> Dependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the requirements of the strategy.
        /// </summary>
        public Dictionary<string, string> Requirements { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the strategy is deprecated.
        /// </summary>
        public bool IsDeprecated { get; }

        /// <summary>
        /// Gets or sets the reason for deprecation, if applicable.
        /// </summary>
        public string? DeprecationReason { get; set; }

        /// <summary>
        /// Gets or sets the replacement strategy, if applicable.
        /// </summary>
        public string? ReplacementStrategy { get; set; }
    }
} 






