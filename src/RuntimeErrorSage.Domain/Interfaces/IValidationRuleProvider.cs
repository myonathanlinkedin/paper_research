using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Domain.Interfaces
{
    /// <summary>
    /// Interface for validation rule providers.
    /// </summary>
    public interface IValidationRuleProvider
    {
        /// <summary>
        /// Gets all validation rules.
        /// </summary>
        /// <returns>A collection of validation rules.</returns>
        IEnumerable<ValidationRule> GetRules();

        /// <summary>
        /// Gets a validation rule by ID.
        /// </summary>
        /// <param name="ruleId">The rule ID.</param>
        /// <returns>The validation rule, or null if not found.</returns>
        ValidationRule GetRule(string ruleId);

        /// <summary>
        /// Adds a validation rule.
        /// </summary>
        /// <param name="rule">The rule to add.</param>
        void AddRule(ValidationRule rule);

        /// <summary>
        /// Removes a validation rule.
        /// </summary>
        /// <param name="ruleId">The ID of the rule to remove.</param>
        /// <returns>True if the rule was removed; otherwise, false.</returns>
        bool RemoveRule(string ruleId);
    }
} 