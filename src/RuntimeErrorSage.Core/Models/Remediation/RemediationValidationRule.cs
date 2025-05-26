using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents a validation rule for remediation operations.
/// </summary>
public class RemediationValidationRule
{
    /// <summary>
    /// Gets or sets the unique identifier of the rule.
    /// </summary>
    public string RuleId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the rule.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the rule.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the validation priority.
    /// </summary>
    public ValidationPriority Priority { get; set; }

    /// <summary>
    /// Gets or sets the validation scope.
    /// </summary>
    public ValidationScope Scope { get; set; }

    /// <summary>
    /// Gets or sets whether the rule is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the rule is required.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets the error message template.
    /// </summary>
    public string ErrorMessageTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the warning message template.
    /// </summary>
    public string WarningMessageTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the validation condition.
    /// </summary>
    public string ValidationCondition { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the validation parameters.
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();

    /// <summary>
    /// Gets or sets the validation metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets when the rule was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets when the rule was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets whether the rule is active.
    /// </summary>
    public bool IsActive { get; set; }
} 