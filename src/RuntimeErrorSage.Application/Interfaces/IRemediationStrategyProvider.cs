using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Interfaces;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    /// <summary>
    /// Interface for providing remediation strategies.
    /// </summary>
    public interface IRemediationStrategyProvider
    {
        /// <summary>
        /// Returns applicable remediation strategies for the given error context.
        /// </summary>
        Task<Collection<IRemediationStrategy>> GetApplicableStrategiesAsync(ErrorContext context);

        /// <summary>
        /// Gets a specific remediation strategy by name.
        /// </summary>
        /// <param name="strategyName">The name of the strategy to retrieve.</param>
        /// <returns>The remediation strategy, or null if not found.</returns>
        Task<IRemediationStrategy> GetStrategyAsync(string strategyName);
    }
} 






