using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Domain.Models.Validation
{
    /// <summary>
    /// Interface for validating remediation operations.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validates a remediation context.
        /// </summary>
        /// <param name="context">The remediation context to validate.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateAsync(RemediationContext context);

        /// <summary>
        /// Gets whether the validator is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the name of the validator.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the version of the validator.
        /// </summary>
        string Version { get; }
    }
} 