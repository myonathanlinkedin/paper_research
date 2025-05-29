using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents risk metrics for a remediation operation.
    /// </summary>
    public class RiskMetrics
    {
        /// <summary>
        /// Gets or sets the unique identifier for these metrics.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the remediation ID these metrics are associated with.
        /// </summary>
        public string RemediationId { get; }

        /// <summary>
        /// Gets or sets the timestamp when these metrics were collected.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the overall risk level.
        /// </summary>
        public RemediationRiskLevel RiskLevel { get; }

        /// <summary>
        /// Gets or sets the risk score (0-1).
        /// </summary>
        public double RiskScore { get; }

        /// <summary>
        /// Gets or sets the probability of failure (0-1).
        /// </summary>
        public double FailureProbability { get; }

        /// <summary>
        /// Gets or sets the impact severity.
        /// </summary>
        public ImpactSeverity ImpactSeverity { get; }

        /// <summary>
        /// Gets or sets the impact scope.
        /// </summary>
        public ImpactScope ImpactScope { get; }

        /// <summary>
        /// Gets or sets the confidence level (0-1).
        /// </summary>
        public double Confidence { get; }

        /// <summary>
        /// Gets or sets the potential issues.
        /// </summary>
        public IReadOnlyCollection<PotentialIssues> PotentialIssues { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the mitigation steps.
        /// </summary>
        public IReadOnlyCollection<MitigationSteps> MitigationSteps { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the affected components.
        /// </summary>
        public IReadOnlyCollection<AffectedComponents> AffectedComponents { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the risk factors.
        /// </summary>
        public Dictionary<string, double> RiskFactors { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Gets or sets the estimated duration of the remediation.
        /// </summary>
        public TimeSpan EstimatedDuration { get; }

        /// <summary>
        /// Gets or sets additional metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
} 






