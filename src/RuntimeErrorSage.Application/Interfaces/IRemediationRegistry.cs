using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Application.Interfaces;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    /// <summary>
    /// Interface for registering and retrieving remediation strategies.
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
        /// Registers a remediation strategy.
        /// </summary>
        /// <param name="strategy">The strategy to register.</param>
        Task RegisterStrategyAsync(IRemediationStrategy strategy);

        /// <summary>
        /// Gets a strategy by its identifier.
        /// </summary>
        /// <param name="id">The strategy identifier.</param>
        /// <returns>The remediation strategy.</returns>
        Task<IRemediationStrategy> GetStrategyAsync(string id);

        /// <summary>
        /// Gets all registered strategies.
        /// </summary>
        /// <returns>The list of registered strategies.</returns>
        Task<IEnumerable<IRemediationStrategy>> GetAllStrategiesAsync();

        /// <summary>
        /// Gets the strategies applicable for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The list of applicable strategies.</returns>
        Task<IEnumerable<IRemediationStrategy>> GetStrategiesForErrorAsync(ErrorContext context);
    }
} 
