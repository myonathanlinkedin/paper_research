using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    /// <summary>
    /// Interface for selecting remediation strategies.
    /// </summary>
    public interface IRemediationStrategySelector
    {
        /// <summary>
        /// Gets whether the selector is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the selector name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the selector version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Selects a remediation strategy for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The selected remediation strategy.</returns>
        Task<IRemediationStrategy> SelectStrategyAsync(ErrorContext context);

        /// <summary>
        /// Gets all available remediation strategies.
        /// </summary>
        /// <returns>The available remediation strategies.</returns>
        Task<Collection<IRemediationStrategy>> GetAvailableStrategiesAsync();

        /// <summary>
        /// Gets the confidence score for a strategy.
        /// </summary>
        /// <param name="strategy">The remediation strategy.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The confidence score between 0 and 1.</returns>
        Task<double> GetStrategyConfidenceAsync(IRemediationStrategy strategy, ErrorContext context);

        /// <summary>
        /// Gets the strategy ranking for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The ranked strategies with their scores.</returns>
        Task<Dictionary<IRemediationStrategy, double>> GetStrategyRankingAsync(ErrorContext context);
    }
} 






