using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents a recommendation for a remediation strategy.
    /// </summary>
    public class StrategyRecommendation
    {
        /// <summary>
        /// Gets or sets the unique identifier of the recommendation.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the strategy.
        /// </summary>
        public string StrategyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the priority of the strategy (lower is higher priority).
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the confidence level (0.0 to 1.0).
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the reasoning behind the recommendation.
        /// </summary>
        public string Reasoning { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the recommendation was created.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the ID of the recommended strategy.
        /// </summary>
        public string StrategyId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confidence score (0.0 to 1.0).
        /// </summary>
        public double ConfidenceScore { get; set; }

        /// <summary>
        /// Gets or sets the reason for the recommendation.
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional reasons supporting this recommendation.
        /// </summary>
        public List<string> SupportingReasons { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the source of this recommendation.
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expected impact of applying this strategy.
        /// </summary>
        public RemediationImpact ExpectedImpact { get; set; }

        /// <summary>
        /// Gets or sets the expected success probability (0.0 to 1.0).
        /// </summary>
        public double ExpectedSuccessProbability { get; set; }

        /// <summary>
        /// Gets or sets the expected time to implement this strategy.
        /// </summary>
        public TimeSpan ExpectedImplementationTime { get; set; }

        /// <summary>
        /// Gets or sets the complexity of this strategy.
        /// </summary>
        public RemediationComplexity Complexity { get; set; }

        /// <summary>
        /// Gets or sets the prerequisites for this strategy.
        /// </summary>
        public List<string> Prerequisites { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the potential risks of applying this strategy.
        /// </summary>
        public List<string> PotentialRisks { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the correlation ID for tracing.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;
    }
} 
