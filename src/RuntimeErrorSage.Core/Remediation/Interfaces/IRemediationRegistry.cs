using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for managing remediation strategies.
    /// </summary>
    public interface IRemediationRegistry
    {
        /// <summary>
        /// Gets whether the registry is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the registry name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the registry version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets a remediation strategy by name.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The remediation strategy.</returns>
        Task<IRemediationStrategy> GetStrategyAsync(string strategyName);

        /// <summary>
        /// Gets all remediation strategies for a specific error type.
        /// </summary>
        /// <param name="errorType">The error type.</param>
        /// <returns>A collection of remediation strategies.</returns>
        Task<IEnumerable<IRemediationStrategy>> GetStrategiesForErrorTypeAsync(string errorType);

        /// <summary>
        /// Gets all remediation strategies for a specific error type synchronously.
        /// </summary>
        /// <param name="errorType">The error type.</param>
        /// <returns>A collection of remediation strategies.</returns>
        IEnumerable<IRemediationStrategy> GetStrategiesForErrorType(string errorType);

        /// <summary>
        /// Registers a remediation strategy.
        /// </summary>
        /// <param name="strategy">The strategy to register.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RegisterStrategyAsync(IRemediationStrategy strategy);

        /// <summary>
        /// Unregisters a remediation strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy to unregister.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UnregisterStrategyAsync(string strategyName);

        /// <summary>
        /// Gets all registered strategies.
        /// </summary>
        /// <returns>The list of registered strategies.</returns>
        Task<IEnumerable<IRemediationStrategy>> GetStrategiesAsync();

        /// <summary>
        /// Gets strategies for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The list of applicable strategies.</returns>
        Task<IEnumerable<IRemediationStrategy>> GetStrategiesForErrorAsync(ErrorContext context);

        /// <summary>
        /// Gets whether a strategy is registered.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>True if the strategy is registered, false otherwise.</returns>
        Task<bool> IsStrategyRegisteredAsync(string strategyName);
    }
} 