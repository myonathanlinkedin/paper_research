using System;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Models.Remediation.Factories
{
    /// <summary>
    /// Factory for creating RemediationActionResult instances.
    /// </summary>
    public class RemediationActionResultFactory : IRemediationActionResultFactory
    {
        /// <summary>
        /// Creates a new RemediationActionResult instance.
        /// </summary>
        /// <param name="name">The action name.</param>
        /// <param name="success">Whether the action was successful.</param>
        /// <param name="errorMessage">The error message if the action failed.</param>
        /// <returns>A new RemediationActionResult instance.</returns>
        public RemediationActionResult Create(string name, bool success, string errorMessage = null)
        {
            return new RemediationActionResult
            {
                ActionId = Guid.NewGuid().ToString(),
                Name = name,
                Success = success,
                ErrorMessage = errorMessage,
                Timestamp = DateTime.UtcNow
            };
        }
    }
} 