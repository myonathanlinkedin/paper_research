using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Validation;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Application.Validation.Interfaces;

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
    /// Gets the severity level of the validation rule.
    /// </summary>
    SeverityLevel Severity { get; }

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
