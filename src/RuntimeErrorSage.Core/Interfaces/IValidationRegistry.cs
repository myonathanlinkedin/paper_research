using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for managing validators.
    /// </summary>
    public interface IValidationRegistry
    {
        /// <summary>
        /// Validates a context using a specific validator.
        /// </summary>
        /// <param name="validationId">The validation identifier.</param>
        /// <param name="context">The context to validate.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateAsync(string validationId, object context);

        /// <summary>
        /// Registers a validator for a validation type.
        /// </summary>
        /// <param name="validationType">The validation type.</param>
        /// <param name="validator">The validator to register.</param>
        /// <returns>True if registration was successful, false otherwise.</returns>
        Task<bool> RegisterValidatorAsync(string validationType, IValidator validator);

        /// <summary>
        /// Unregisters a validator for a validation type.
        /// </summary>
        /// <param name="validationType">The validation type.</param>
        /// <returns>True if unregistration was successful, false otherwise.</returns>
        Task<bool> UnregisterValidatorAsync(string validationType);

        /// <summary>
        /// Gets a validator for a validation type.
        /// </summary>
        /// <param name="validationType">The validation type.</param>
        /// <returns>The validator if found, null otherwise.</returns>
        Task<IValidator> GetValidatorAsync(string validationType);
    }
} 