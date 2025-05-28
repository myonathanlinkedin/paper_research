using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Graph;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Dependency
{
    /// <summary>
    /// Represents a potential source of an error in a dependency graph.
    /// </summary>
    public class PotentialErrorSource
    {
        /// <summary>
        /// Gets or sets the unique identifier for this potential error source.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the node in the dependency graph that is the potential source of the error.
        /// </summary>
        public GraphNode Node { get; set; }

        /// <summary>
        /// Gets or sets the confidence level that this is the actual error source.
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the impact severity of this error source.
        /// </summary>
        public ImpactSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the impact scope of this error source.
        /// </summary>
        public ImpactScope Scope { get; set; }

        /// <summary>
        /// Gets or sets the reasons why this node is considered a potential error source.
        /// </summary>
        public List<string> Reasons { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets additional evidence supporting this node as a potential error source.
        /// </summary>
        public Dictionary<string, string> Evidence { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the error type if known.
        /// </summary>
        public string ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the error message if known.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the error was detected.
        /// </summary>
        public DateTime DetectionTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the estimated time when the error occurred.
        /// </summary>
        public DateTime? OccurrenceTime { get; set; }

        /// <summary>
        /// Gets or sets the potentially affected downstream nodes.
        /// </summary>
        public List<GraphNode> AffectedNodes { get; set; } = new List<GraphNode>();

        /// <summary>
        /// Gets or sets the potential remediation actions.
        /// </summary>
        public List<string> PotentialRemediations { get; set; } = new List<string>();
    }
} 