using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Validation.Models;

namespace RuntimeErrorSage.Core.Validation.Interfaces
{
    /// <summary>
    /// Defines the interface for managing validation rules.
    /// </summary>
    public interface IValidationRegistry
    {
        /// <summary>
        /// Registers a validation rule.
        /// </summary>
        /// <param name="rule">The rule to register</param>
        void RegisterRule(ValidationRule rule);

        /// <summary>
        /// Unregisters a validation rule.
        /// </summary>
        /// <param name="ruleId">The ID of the rule to unregister</param>
        void UnregisterRule(string ruleId);

        /// <summary>
        /// Validates an error context against all registered rules.
        /// </summary>
        /// <param name="context">The error context to validate</param>
        /// <returns>The validation result</returns>
        Task<ValidationResult> ValidateAsync(ErrorContext context);

        /// <summary>
        /// Gets all registered validation rules.
        /// </summary>
        /// <returns>An ordered collection of validation rules</returns>
        IEnumerable<ValidationRule> GetRules();

        /// <summary>
        /// Gets a specific validation rule by ID.
        /// </summary>
        /// <param name="ruleId">The ID of the rule</param>
        /// <returns>The validation rule if found, null otherwise</returns>
        ValidationRule? GetRule(string ruleId);

        /// <summary>
        /// Clears the validation result cache.
        /// </summary>
        void ClearCache();
    }
} 