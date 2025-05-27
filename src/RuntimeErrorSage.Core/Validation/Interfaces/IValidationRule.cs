using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Validation;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.Validation.Interfaces;

/// <summary>
/// Represents a validation rule that can be applied to validate operations.
/// </summary>
public interface IValidationRule
{
    /// <summary>
    /// Gets the unique identifier of the rule.
    /// </summary>
    string RuleId { get; }

    /// <summary>
    /// Gets the name of the rule.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the description of the rule.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the severity level of the rule.
    /// </summary>
    ValidationSeverity Severity { get; }

    /// <summary>
    /// Gets whether the rule is enabled.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Validates the specified context asynchronously.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <returns>The validation result.</returns>
    Task<ValidationResult> ValidateAsync(ValidationContext context);
} 