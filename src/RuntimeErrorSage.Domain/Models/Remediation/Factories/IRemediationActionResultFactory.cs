using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Application.Models.Remediation.Factories
{
    /// <summary>
    /// Interface for creating RemediationActionResult instances.
    /// </summary>
    public interface IRemediationActionResultFactory
    {
        /// <summary>
        /// Creates a new RemediationActionResult instance.
        /// </summary>
        /// <param name="name">The action name.</param>
        /// <param name="success">Whether the action was successful.</param>
        /// <param name="errorMessage">The error message if the action failed.</param>
        /// <returns>A new RemediationActionResult instance.</returns>
        RemediationActionResult Create(string name, bool success, string errorMessage = null);
    }
} 