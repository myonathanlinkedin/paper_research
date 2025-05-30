using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for validation state checking.
    /// </summary>
    public interface IValidationStateChecker
    {
        /// <summary>
        /// Checks if the remediation action is valid.
        /// </summary>
        /// <param name="action">The remediation action to check.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> CheckValidationStateAsync(IRemediationAction action);

        /// <summary>
        /// Gets the name of the checker.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets whether the checker is enabled.
        /// </summary>
        bool IsEnabled { get; }
    }
} 