using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;
using System;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Represents a validation rule for remediation actions.
/// </summary>
public class ValidationRule
{
    /// <summary>
    /// Gets or sets the name of the rule.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the rule.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the validation function.
    /// </summary>
    public Func<IRemediationAction, Task<ValidationResult>> ValidateAsync { get; set; }

    /// <summary>
    /// Gets or sets the severity of the rule.
    /// </summary>
    public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;

    /// <summary>
    /// Gets or sets whether the rule is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;
} 

