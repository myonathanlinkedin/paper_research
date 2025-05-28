using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents a validation rule for remediation operations.
/// </summary>
public class RemediationValidationRule
{
    /// <summary>
    /// Gets or sets the unique identifier of the rule.
    /// </summary>
    public string RuleId { get; set; }

    /// <summary>
    /// Gets or sets the name of the rule.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the rule.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the validation priority.
    /// </summary>
    public int Priority { get; set; }

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
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets when the rule was last updated.
    /// </summary>
    public DateTimeOffset ModifiedAt { get; set; }

    /// <summary>
    /// Gets or sets whether the rule is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the severity level of the rule.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the validation function that determines if the rule is satisfied.
    /// </summary>
    public Func<RemediationAction, bool> ValidationFunction { get; set; }

    /// <summary>
    /// Gets or sets the error message to display if the rule is not satisfied.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Initializes a new instance of the RemediationValidationRule class.
    /// </summary>
    public RemediationValidationRule()
    {
        RuleId = Guid.NewGuid().ToString();
        CreatedAt = DateTimeOffset.UtcNow;
        ModifiedAt = DateTimeOffset.UtcNow;
        IsEnabled = true;
    }
} 