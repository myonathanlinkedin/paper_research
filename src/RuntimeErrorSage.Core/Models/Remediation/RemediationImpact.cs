using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the impact assessment of a remediation operation.
    /// </summary>
    public class RemediationImpact
    {
        /// <summary>
        /// Gets or sets the unique identifier for this impact assessment.
        /// </summary>
        public string ImpactId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the impact level (0-1).
        /// </summary>
        public double ImpactLevel { get; set; }

        /// <summary>
        /// Gets or sets the confidence level (0-1).
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the impact description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the affected components.
        /// </summary>
        public List<string> AffectedComponents { get; set; } = new();

        /// <summary>
        /// Gets or sets the affected services.
        /// </summary>
        public List<string> AffectedServices { get; set; } = new();

        /// <summary>
        /// Gets or sets when the assessment was performed.
        /// </summary>
        public DateTime AssessedAt { get; set; }

        /// <summary>
        /// Gets or sets the assessment metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the severity level.
        /// </summary>
        public ErrorSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the estimated downtime in seconds.
        /// </summary>
        public double EstimatedDowntimeSeconds { get; set; }

        /// <summary>
        /// Gets or sets whether service restart is required.
        /// </summary>
        public bool RequiresServiceRestart { get; set; }

        /// <summary>
        /// Gets or sets whether data migration is required.
        /// </summary>
        public bool RequiresDataMigration { get; set; }

        /// <summary>
        /// Gets or sets whether configuration changes are required.
        /// </summary>
        public bool RequiresConfigurationChanges { get; set; }
    }
} 