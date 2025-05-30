using System;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Domain.Interfaces
{
    /// <summary>
    /// Interface for validation state checking.
    /// </summary>
    public interface IValidationStateChecker
    {
        /// <summary>
        /// Checks if the validation state is valid.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>True if valid; otherwise, false.</returns>
        Task<bool> IsValidAsync(ValidationContext context);

        /// <summary>
        /// Gets the validation result.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> GetValidationResultAsync(ValidationContext context);
    }
} 