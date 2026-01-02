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
        /// Gets or sets the severity of the impact.
        /// </summary>
        public RemediationActionSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the scope of the impact.
        /// </summary>
        public RemediationActionImpactScope Scope { get; set; }

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
        /// Gets or sets the estimated recovery time.
        /// </summary>
        public TimeSpan EstimatedRecoveryTime { get; set; }

        /// <summary>
        /// Gets or sets the estimated downtime.
        /// </summary>
        public TimeSpan EstimatedDowntime { get; set; }

        /// <summary>
        /// Gets or sets the affected users.
        /// </summary>
        public List<string> AffectedUsers { get; set; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether the impact requires approval.
        /// </summary>
        public bool RequiresApproval { get; set; }

        /// <summary>
        /// Gets or sets the risk level.
        /// </summary>
        public RiskLevel RiskLevel { get; set; }

        /// <summary>
        /// Gets or sets the confidence level (0-1).
        /// </summary>
        public double ConfidenceLevel { get; set; }

        /// <summary>
        /// Gets or sets the confidence for impact assessment.
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the timestamp when this impact was assessed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID for tracking.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the duration of the remediation impact.
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
} 
