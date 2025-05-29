using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents a suggested remediation action for an error.
    /// </summary>
    public class RemediationSuggestion
    {
        /// <summary>
        /// Gets or sets the unique identifier of the suggestion.
        /// </summary>
        public string SuggestionId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the title of the suggestion.
        /// </summary>
        public string Title { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the suggestion.
        /// </summary>
        public string Description { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the strategy name.
        /// </summary>
        public string StrategyName { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the priority of the suggestion.
        /// </summary>
        public RemediationPriority Priority { get; } = RemediationPriority.Medium;

        /// <summary>
        /// Gets or sets the confidence level of the suggestion (0-1).
        /// </summary>
        public double ConfidenceLevel { get; }

        /// <summary>
        /// Gets or sets the required parameters for the remediation.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the expected outcome of the remediation.
        /// </summary>
        public string ExpectedOutcome { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the potential risks of the remediation.
        /// </summary>
        public IReadOnlyCollection<PotentialRisks> PotentialRisks { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the timestamp of the suggestion.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of remediation actions.
        /// </summary>
        public IReadOnlyCollection<Actions> Actions { get; } = new Collection<RemediationAction>();

        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public ErrorContext ErrorContext { get; }

        /// <summary>
        /// Gets or sets the status of the suggestion.
        /// </summary>
        public SuggestionStatus Status { get; } = SuggestionStatus.Pending;

        /// <summary>
        /// Gets or sets the list of strategies.
        /// </summary>
        public IReadOnlyCollection<Strategies> Strategies { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the confidence score.
        /// </summary>
        public double ConfidenceScore { get; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; } = string.Empty;

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
                ArgumentNullException.ThrowIfNull(nameof(strategy));
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






