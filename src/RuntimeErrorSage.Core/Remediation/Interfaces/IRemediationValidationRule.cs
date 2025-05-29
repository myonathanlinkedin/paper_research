using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Model.Remediation.Interfaces
{
    /// <summary>
    /// Interface for defining remediation validation rules.
    /// </summary>
    public interface IRemediationValidationRule
    {
        /// <summary>
        /// Gets the unique identifier for this validation rule.
        /// </summary>
        string RuleId { get; }

        /// <summary>
        /// Gets the name of the validation rule.
        /// </summary>
        string RuleName { get; }

        /// <summary>
        /// Gets the description of the validation rule.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Validates the error context against this rule.
        /// </summary>
        /// <param name="errorContext">The error context to validate.</param>
        /// <returns>True if the validation passes, false otherwise.</returns>
        bool Validate(ErrorContext errorContext);

        /// <summary>
        /// Gets the severity level of this validation rule.
        /// </summary>
        ValidationSeverity Severity { get; }
    }
} 
