using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents the impact of a remediation action.
    /// </summary>
    public class RemediationImpact
    {
        /// <summary>
        /// Gets or sets the impact level.
        /// </summary>
        public ImpactLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the impact description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the affected components.
        /// </summary>
        public List<string> AffectedComponents { get; set; } = new();

        /// <summary>
        /// Gets or sets the estimated duration.
        /// </summary>
        public TimeSpan EstimatedDuration { get; set; }

        /// <summary>
        /// Gets or sets the risk level.
        /// </summary>
        public RiskLevel RiskLevel { get; set; }

        /// <summary>
        /// Gets or sets the confidence level (0-1).
        /// </summary>
        public double ConfidenceLevel { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 
