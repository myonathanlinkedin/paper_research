using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents risk metrics for a remediation operation.
    /// </summary>
    public class RiskMetrics
    {
        /// <summary>
        /// Gets or sets the unique identifier for these metrics.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the remediation ID these metrics are associated with.
        /// </summary>
        public string RemediationId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when these metrics were collected.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the overall risk level.
        /// </summary>
        public RemediationRiskLevel RiskLevel { get; set; }

        /// <summary>
        /// Gets or sets the risk score (0-1).
        /// </summary>
        public double RiskScore { get; set; }

        /// <summary>
        /// Gets or sets the overall risk score (0-1).
        /// </summary>
        public double OverallRiskScore { get; set; }

        /// <summary>
        /// Gets or sets the confidence score (0-1).
        /// </summary>
        public double ConfidenceScore { get; set; }

        /// <summary>
        /// Gets or sets the potential impact score (0-1).
        /// </summary>
        public double PotentialImpactScore { get; set; }

        /// <summary>
        /// Gets or sets the complexity score (0-1).
        /// </summary>
        public double ComplexityScore { get; set; }

        /// <summary>
        /// Gets or sets the time sensitivity score (0-1).
        /// </summary>
        public double TimeSensitivityScore { get; set; }

        /// <summary>
        /// Gets or sets the strategy ID associated with these metrics.
        /// </summary>
        public string StrategyId { get; set; }

        /// <summary>
        /// Gets or sets the probability of failure (0-1).
        /// </summary>
        public double FailureProbability { get; set; }

        /// <summary>
        /// Gets or sets the impact severity.
        /// </summary>
        public ImpactSeverity ImpactSeverity { get; set; }

        /// <summary>
        /// Gets or sets the impact scope.
        /// </summary>
        public ImpactScope ImpactScope { get; set; }

        /// <summary>
        /// Gets or sets the confidence level (0-1).
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the potential issues.
        /// </summary>
        public List<string> PotentialIssues { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the mitigation steps.
        /// </summary>
        public List<string> MitigationSteps { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the affected components.
        /// </summary>
        public List<string> AffectedComponents { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the risk factors.
        /// </summary>
        public Dictionary<string, double> RiskFactors { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Gets or sets the estimated duration of the remediation.
        /// </summary>
        public TimeSpan EstimatedDuration { get; set; }

        /// <summary>
        /// Gets or sets additional metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
} 
