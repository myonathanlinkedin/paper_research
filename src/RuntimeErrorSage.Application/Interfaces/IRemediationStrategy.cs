using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for remediation strategies.
    /// </summary>
    public interface IRemediationStrategy
    {
        /// <summary>
        /// Gets the unique identifier for this strategy.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the name of the strategy.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the strategy.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the version of the strategy.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets whether the strategy is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the priority of the strategy.
        /// </summary>
        RemediationPriority Priority { get; }

        /// <summary>
        /// Gets the strategy parameters.
        /// </summary>
        Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets the supported error types.
        /// </summary>
        ISet<string> SupportedErrorTypes { get; }

        /// <summary>
        /// Gets the list of remediation actions.
        /// </summary>
        List<RemediationAction> Actions { get; }

        /// <summary>
        /// Gets the creation timestamp.
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Gets whether the strategy can handle the given error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>True if the strategy can handle the error; otherwise, false.</returns>
        Task<bool> CanHandleAsync(ErrorContext context);

        /// <summary>
        /// Gets the confidence level for handling the given error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The confidence level between 0 and 1.</returns>
        Task<double> GetConfidenceAsync(ErrorContext context);

        /// <summary>
        /// Gets remediation suggestions for the given error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation suggestions.</returns>
        Task<List<RemediationSuggestion>> GetSuggestionsAsync(ErrorContext context);

        /// <summary>
        /// Creates a remediation plan for the given error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation plan.</returns>
        Task<RemediationPlan> CreatePlanAsync(ErrorContext context);

        /// <summary>
        /// Executes the strategy for the given error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> ExecuteAsync(ErrorContext context);
    }
} 