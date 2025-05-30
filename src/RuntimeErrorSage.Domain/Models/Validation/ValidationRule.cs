using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Interfaces;

namespace RuntimeErrorSage.Domain.Models.Validation;

/// <summary>
/// Represents a validation rule for remediation actions.
/// </summary>
public class ValidationRule
{
    /// <summary>
    /// Gets the name of the rule.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the description of the rule.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the validation function.
    /// </summary>
    public Func<IRemediationAction, Task<ValidationResult>> ValidateAsync { get; }

    /// <summary>
    /// Gets the severity of the rule.
    /// </summary>
    public ValidationSeverity Severity { get; }

    /// <summary>
    /// Gets whether the rule is enabled.
    /// </summary>
    public bool IsEnabled { get; private set; } = true;

    /// <summary>
    /// Gets the rule ID.
    /// </summary>
    public string RuleId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationRule"/> class.
    /// </summary>
    /// <param name="name">The name of the rule.</param>
    /// <param name="description">The description of the rule.</param>
    /// <param name="validateAsync">The validation function.</param>
    /// <param name="severity">The severity of the rule.</param>
    /// <exception cref="ArgumentNullException">Thrown when name, description, or validateAsync is null.</exception>
    public ValidationRule(
        string name,
        string description,
        Func<IRemediationAction, Task<ValidationResult>> validateAsync,
        ValidationSeverity severity = ValidationSeverity.Error)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(validateAsync);

        Name = name;
        Description = description;
        ValidateAsync = validateAsync;
        Severity = severity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationRule"/> class.
    /// </summary>
    /// <param name="ruleId">The rule ID.</param>
    /// <exception cref="ArgumentNullException">Thrown when ruleId is null.</exception>
    public ValidationRule(string ruleId)
    {
        ArgumentNullException.ThrowIfNull(ruleId);
        RuleId = ruleId;
        Name = ruleId;
        Description = string.Empty;
        ValidateAsync = _ => Task.FromResult(new ValidationResult(new ValidationContext(), false, ValidationSeverity.Error, ValidationStatus.Failed, new MetricsValidation(), string.Empty));
        Severity = ValidationSeverity.Error;
    }

    /// <summary>
    /// Enables the rule.
    /// </summary>
    public void Enable()
    {
        IsEnabled = true;
    }

    /// <summary>
    /// Disables the rule.
    /// </summary>
    public void Disable()
    {
        IsEnabled = false;
    }

    /// <summary>
    /// Validates a remediation action using this rule.
    /// </summary>
    /// <param name="action">The action to validate.</param>
    /// <returns>The validation result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when action is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the rule is disabled.</exception>
    public async Task<ValidationResult> ValidateActionAsync(IRemediationAction action)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (!IsEnabled)
        {
            throw new InvalidOperationException($"Validation rule '{Name}' is disabled.");
        }

        try
        {
            return await ValidateAsync(action);
        }
        catch (Exception ex)
        {
            return new ValidationResult(new ValidationContext(), false, ValidationSeverity.Error, ValidationStatus.Failed, new MetricsValidation(), $"Error validating rule '{Name}': {ex.Message}");
        }
    }

    protected ValidationResult CreateValidationResult(ValidationContext context, bool isValid, ValidationSeverity severity, ValidationStatus status, MetricsValidation metrics, string message)
    {
        var result = new ValidationResult(context, isValid, severity, status, metrics, message);
        if (!isValid)
        {
            result.AddError(message);
        }
        return result;
    }
} 

