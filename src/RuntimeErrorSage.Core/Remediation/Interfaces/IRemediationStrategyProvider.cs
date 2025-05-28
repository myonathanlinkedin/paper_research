using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Interfaces;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Interface for providing remediation strategies.
    /// </summary>
    public interface IRemediationStrategyProvider
    {
        /// <summary>
        /// Returns applicable remediation strategies for the given error context.
        /// </summary>
        Task<List<IRemediationStrategy>> GetApplicableStrategiesAsync(ErrorContext context);

        /// <summary>
        /// Gets a specific remediation strategy by name.
        /// </summary>
        /// <param name="strategyName">The name of the strategy to retrieve.</param>
        /// <returns>The remediation strategy, or null if not found.</returns>
        Task<IRemediationStrategy> GetStrategyAsync(string strategyName);
    }
} 
