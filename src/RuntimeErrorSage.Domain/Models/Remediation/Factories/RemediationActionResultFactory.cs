using System.Collections.ObjectModel;
using System;
using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Application.Models.Remediation.Factories
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
        public string name, bool success, string errorMessage = null { ArgumentNullException.ThrowIfNull(string name, bool success, string errorMessage = null); }
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





