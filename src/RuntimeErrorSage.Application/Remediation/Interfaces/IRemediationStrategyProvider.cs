using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Interfaces;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    /// <summary>
    /// Interface for providing remediation strategies based on error context analysis.
    /// </summary>
    public interface IRemediationStrategyProvider
    {
        /// <summary>
        /// Gets whether the provider is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the provider name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the provider version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets strategies for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>A list of applicable remediation strategies.</returns>
        Task<List<IRemediationStrategy>> GetStrategiesAsync(ErrorContext context);

        /// <summary>
        /// Gets a strategy by its identifier.
        /// </summary>
        /// <param name="strategyId">The strategy identifier.</param>
        /// <returns>The remediation strategy if found, null otherwise.</returns>
        Task<IRemediationStrategy> GetStrategyByIdAsync(string strategyId);

        /// <summary>
        /// Gets all registered strategies.
        /// </summary>
        /// <returns>A list of all registered remediation strategies.</returns>
        Task<List<IRemediationStrategy>> GetAllStrategiesAsync();

        /// <summary>
        /// Creates a remediation plan for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The created remediation plan.</returns>
        Task<RemediationPlan> CreatePlanAsync(ErrorContext context);

        /// <summary>
        /// Gets the best strategy for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The best matching remediation strategy.</returns>
        Task<IRemediationStrategy> GetBestStrategyAsync(ErrorContext context);
    }
} 