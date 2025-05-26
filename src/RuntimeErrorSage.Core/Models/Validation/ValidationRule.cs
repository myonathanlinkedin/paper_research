using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Represents a validation rule for remediation operations.
/// </summary>
public class ValidationRule
{
    /// <summary>
    /// Gets or sets the unique identifier for this rule.
    /// </summary>
    public string RuleId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the rule name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the rule description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the rule is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the rule priority (lower is higher priority).
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Gets or sets the validation expression.
    /// </summary>
    public string Expression { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error message template.
    /// </summary>
    public string ErrorMessageTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the rule is required.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets when the rule was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets when the rule was last updated.
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the rule metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the validation parameters.
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();

    /// <summary>
    /// Gets or sets the validation type.
    /// </summary>
    public string ValidationType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the validation scope.
    /// </summary>
    public string ValidationScope { get; set; } = string.Empty;
} 