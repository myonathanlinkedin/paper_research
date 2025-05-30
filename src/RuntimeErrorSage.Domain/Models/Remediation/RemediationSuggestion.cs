using System;
using RuntimeErrorSage.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
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
        /// Gets or sets the list of remediation actions.
        /// </summary>
        public List<RemediationAction> Actions { get; set; } = new List<RemediationAction>();

        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public ErrorContext ErrorContext { get; set; }

        /// <summary>
        /// Gets or sets the status of the suggestion.
        /// </summary>
        public SuggestionStatus Status { get; set; } = SuggestionStatus.Pending;

        /// <summary>
        /// Gets or sets the list of strategies.
        /// </summary>
        public List<string> Strategies { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the confidence score.
        /// </summary>
        public double ConfidenceScore { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confidence level of this suggestion (0-1).
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the severity of the suggested action.
        /// </summary>
        public RemediationSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the scope of the suggested action.
        /// </summary>
        public ImpactScope Scope { get; set; }

        /// <summary>
        /// Gets or sets the estimated duration of the suggested action.
        /// </summary>
        public TimeSpan EstimatedDuration { get; set; }

        /// <summary>
        /// Gets or sets the prerequisites for this suggestion.
        /// </summary>
        public List<string> Prerequisites { get; set; } = new();

        /// <summary>
        /// Gets or sets additional metadata about the suggestion.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Creates a suggestion from a strategy.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        /// <param name="confidenceLevel">The confidence level.</param>
        /// <returns>The remediation suggestion.</returns>
        public static RemediationSuggestion FromStrategy(IRemediationStrategy strategy, double confidenceLevel)
        {
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy));
            }

            return new RemediationSuggestion
            {
                StrategyName = strategy.Name,
                Description = strategy.Description,
                ConfidenceLevel = confidenceLevel,
                Parameters = new Dictionary<string, object>()
            };
        }
    }
} 
