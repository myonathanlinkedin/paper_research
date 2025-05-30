using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for managing validation registrations.
    /// </summary>
    public interface IValidationRegistry
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
        /// Registers a validator for a specific validation type.
        /// </summary>
        /// <param name="validationType">The type of validation.</param>
        /// <param name="validator">The validator to register.</param>
        /// <returns>True if registration was successful, false otherwise.</returns>
        Task<bool> RegisterValidatorAsync(string validationType, IValidator validator);

        /// <summary>
        /// Unregisters a validator for a specific validation type.
        /// </summary>
        /// <param name="validationType">The type of validation.</param>
        /// <returns>True if unregistration was successful, false otherwise.</returns>
        Task<bool> UnregisterValidatorAsync(string validationType);

        /// <summary>
        /// Gets a validator for a specific validation type.
        /// </summary>
        /// <param name="validationType">The type of validation.</param>
        /// <returns>The validator for the specified type.</returns>
        Task<IValidator> GetValidatorAsync(string validationType);

        /// <summary>
        /// Checks if a validator exists for a specific validation type.
        /// </summary>
        /// <param name="validationType">The type of validation.</param>
        /// <returns>True if a validator exists, false otherwise.</returns>
        Task<bool> HasValidatorAsync(string validationType);
    }
} 
