using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Model.Models.Validation;

namespace RuntimeErrorSage.Model.Interfaces
{
    /// <summary>
    /// Interface for validators.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Gets the unique identifier for this validator.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the name of this validator.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of this validator.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the type of validation performed.
        /// </summary>
        ValidationType Type { get; }

        /// <summary>
        /// Gets the scope of validation.
        /// </summary>
        ValidationScope Scope { get; }

        /// <summary>
        /// Gets the level of validation.
        /// </summary>
        ValidationLevel Level { get; }

        /// <summary>
        /// Gets the category of validation.
        /// </summary>
        ValidationCategory Category { get; }

        /// <summary>
        /// Gets whether this validator is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Validates a context.
        /// </summary>
        /// <param name="context">The context to validate.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateAsync(object context);

        /// <summary>
        /// Determines whether this validator can validate the given context.
        /// </summary>
        /// <param name="context">The context to validate.</param>
        /// <returns>True if this validator can validate the context, false otherwise.</returns>
        bool CanValidate(object context);
    }
} 
