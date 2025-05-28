using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents a suggested remediation action for an error.
    /// </summary>
    public class RemediationSuggestion
    {
        /// <summary>
        /// Gets or sets the unique identifier of the suggestion.
        /// </summary>
        public string SuggestionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the title of the suggestion.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the suggestion.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the strategy name.
        /// </summary>
        public string StrategyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the priority of the suggestion.
        /// </summary>
        public RemediationPriority Priority { get; set; } = RemediationPriority.Medium;

        /// <summary>
        /// Gets or sets the confidence level of the suggestion (0-1).
        /// </summary>
        public double ConfidenceLevel { get; set; }

        /// <summary>
        /// Gets or sets the required parameters for the remediation.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the expected outcome of the remediation.
        /// </summary>
        public string ExpectedOutcome { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the potential risks of the remediation.
        /// </summary>
        public List<string> PotentialRisks { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the timestamp of the suggestion.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Creates a suggestion from a strategy.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        /// <param name="confidenceLevel">The confidence level.</param>
        /// <returns>The remediation suggestion.</returns>
        public static RemediationSuggestion FromStrategy(Models.Remediation.Interfaces.IRemediationStrategy strategy, double confidenceLevel)
        {
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy));
            }

            return new RemediationSuggestion
            {
                StrategyName = strategy.Name,
                Priority = strategy.Priority,
                Description = strategy.Description,
                Parameters = new Dictionary<string, object>(strategy.Parameters),
                ConfidenceLevel = confidenceLevel
            };
        }
    }
} 