using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Represents the result of a validation operation.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Gets or sets whether the validation was successful.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the validation message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the validation status.
    /// </summary>
    public ValidationStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the validation priority.
    /// </summary>
    public ValidationPriority Priority { get; set; }

    /// <summary>
    /// Gets or sets the validation impact.
    /// </summary>
    public ValidationImpact Impact { get; set; }

    /// <summary>
    /// Gets or sets the validation scope.
    /// </summary>
    public ValidationScope Scope { get; set; }

    /// <summary>
    /// Gets or sets the validation category.
    /// </summary>
    public ValidationCategory Category { get; set; }

    /// <summary>
    /// Gets or sets the validation type.
    /// </summary>
    public ValidationType Type { get; set; }

    /// <summary>
    /// Gets or sets the validation mode.
    /// </summary>
    public ValidationMode Mode { get; set; }

    /// <summary>
    /// Gets or sets the validation phase.
    /// </summary>
    public ValidationPhase Phase { get; set; }

    /// <summary>
    /// Gets or sets the validation stage.
    /// </summary>
    public ValidationStage Stage { get; set; }

    /// <summary>
    /// Gets or sets the validation level.
    /// </summary>
    public ValidationLevel Level { get; set; }

    /// <summary>
    /// Gets or sets additional validation details.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();
} 