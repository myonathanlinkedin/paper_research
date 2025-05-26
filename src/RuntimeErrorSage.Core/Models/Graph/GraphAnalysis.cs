using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents the result of a graph analysis.
    /// </summary>
    public class GraphAnalysis
    {
        /// <summary>
        /// Gets or sets whether the analysis is valid.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the error message if analysis failed.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the component health scores.
        /// </summary>
        public Dictionary<string, double> ComponentHealth { get; set; } = new();

        /// <summary>
        /// Gets or sets the component relationships.
        /// </summary>
        public Dictionary<string, HashSet<string>> ComponentRelationships { get; set; } = new();

        /// <summary>
        /// Gets or sets the error propagation paths.
        /// </summary>
        public List<List<string>> ErrorPropagationPaths { get; set; } = new();

        /// <summary>
        /// Gets or sets the impact metrics.
        /// </summary>
        public Dictionary<string, double> ImpactMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets additional metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 