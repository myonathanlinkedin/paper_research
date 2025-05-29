using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents the impact of a remediation operation.
    /// </summary>
    public class RemediationImpact
    {
        /// <summary>
        /// Gets or sets the severity of the impact.
        /// </summary>
        public ImpactSeverity Severity { get; }

        /// <summary>
        /// Gets or sets the scope of the impact.
        /// </summary>
        public ImpactScope Scope { get; }

        /// <summary>
        /// Gets or sets the estimated number of users affected.
        /// </summary>
        public int AffectedUsers { get; }

        /// <summary>
        /// Gets or sets the estimated recovery time.
        /// </summary>
        public TimeSpan? EstimatedRecoveryTime { get; set; }

        /// <summary>
        /// Gets or sets the description of the business impact.
        /// </summary>
        public string BusinessImpact { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the impact.
        /// </summary>
        public string Description { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the components affected by the remediation.
        /// </summary>
        public IReadOnlyCollection<AffectedComponents> AffectedComponents { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the services affected by the remediation.
        /// </summary>
        public IReadOnlyCollection<AffectedServices> AffectedServices { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets additional impact metrics.
        /// </summary>
        public Dictionary<string, object> Metrics { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets whether the impact requires approval.
        /// </summary>
        public bool RequiresApproval { get; }

        /// <summary>
        /// Gets or sets the confidence level of the impact assessment.
        /// </summary>
        public double ConfidenceLevel { get; }
    }
} 



