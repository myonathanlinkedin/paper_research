using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Models.Analysis
{
    /// <summary>
    /// Represents the result of an impact analysis operation.
    /// </summary>
    public class ImpactAnalysisResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the analysis.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the context identifier.
        /// </summary>
        public string ContextId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error identifier.
        /// </summary>
        public string ErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component type.
        /// </summary>
        public GraphNodeType ComponentType { get; set; } = GraphNodeType.Unknown;

        /// <summary>
        /// Gets or sets whether the analysis is valid.
        /// </summary>
        public bool IsValid { get; set; } = true;

        /// <summary>
        /// Gets or sets the error message if the analysis is invalid.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of affected components.
        /// </summary>
        public List<string> AffectedComponents { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the direct dependencies of the component.
        /// </summary>
        public List<DependencyNode> DirectDependencies { get; set; } = new List<DependencyNode>();

        /// <summary>
        /// Gets or sets the indirect dependencies of the component.
        /// </summary>
        public List<DependencyNode> IndirectDependencies { get; set; } = new List<DependencyNode>();

        /// <summary>
        /// Gets or sets the blast radius (number of affected components).
        /// </summary>
        public int BlastRadius { get; set; }

        /// <summary>
        /// Gets or sets the severity of the impact.
        /// </summary>
        public ImpactSeverity Severity { get; set; } = ImpactSeverity.Low;

        /// <summary>
        /// Gets or sets the scope of the impact.
        /// </summary>
        public ImpactScope Scope { get; set; } = ImpactScope.Local;

        /// <summary>
        /// Gets or sets the confidence level of the analysis (0-1).
        /// </summary>
        public double Confidence { get; set; } = 0.0;

        /// <summary>
        /// Gets or sets the impact score (0-1).
        /// </summary>
        public double ImpactScore { get; set; } = 0.0;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the additional context.
        /// </summary>
        public Dictionary<string, object> AdditionalContext { get; set; } = new Dictionary<string, object>();
    }
} 
