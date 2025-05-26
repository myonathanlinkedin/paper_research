using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents the result of a remediation validation operation.
/// </summary>
public class RemediationValidationResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the validation result.
    /// </summary>
    public string ValidationId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets whether the validation was successful.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the validation message.
    /// </summary>
    public string Message { get; set; }

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
    /// Gets or sets the validation start time.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the validation end time.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the validation duration in milliseconds.
    /// </summary>
    public double DurationMs => EndTime.HasValue ? (EndTime.Value - StartTime).TotalMilliseconds : 0;

    /// <summary>
    /// Gets or sets the validation error message if any.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the validation metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the validation details.
    /// </summary>
    public List<ValidationDetail> Details { get; set; } = new();
}

/// <summary>
/// Represents a detail of a validation operation.
/// </summary>
public class ValidationDetail
{
    /// <summary>
    /// Gets or sets the unique identifier of the detail.
    /// </summary>
    public string DetailId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the detail message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the detail status.
    /// </summary>
    public ValidationStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the detail category.
    /// </summary>
    public ValidationCategory Category { get; set; }

    /// <summary>
    /// Gets or sets the detail type.
    /// </summary>
    public ValidationType Type { get; set; }

    /// <summary>
    /// Gets or sets the detail metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Defines the priority levels for validation.
/// </summary>
public enum ValidationPriority
{
    /// <summary>
    /// Critical priority.
    /// </summary>
    Critical = 1,

    /// <summary>
    /// High priority.
    /// </summary>
    High = 2,

    /// <summary>
    /// Medium priority.
    /// </summary>
    Medium = 3,

    /// <summary>
    /// Low priority.
    /// </summary>
    Low = 4,

    /// <summary>
    /// No priority.
    /// </summary>
    None = 5
}

/// <summary>
/// Defines the impact levels for validation.
/// </summary>
public enum ValidationImpact
{
    /// <summary>
    /// Critical impact.
    /// </summary>
    Critical,

    /// <summary>
    /// High impact.
    /// </summary>
    High,

    /// <summary>
    /// Medium impact.
    /// </summary>
    Medium,

    /// <summary>
    /// Low impact.
    /// </summary>
    Low,

    /// <summary>
    /// No impact.
    /// </summary>
    None
}

/// <summary>
/// Defines the scope levels for validation.
/// </summary>
public enum ValidationScope
{
    /// <summary>
    /// Global scope.
    /// </summary>
    Global,

    /// <summary>
    /// Service scope.
    /// </summary>
    Service,

    /// <summary>
    /// Component scope.
    /// </summary>
    Component,

    /// <summary>
    /// Operation scope.
    /// </summary>
    Operation,

    /// <summary>
    /// Local scope.
    /// </summary>
    Local
}

/// <summary>
/// Defines the types of validation.
/// </summary>
public enum ValidationType
{
    /// <summary>
    /// Syntax validation.
    /// </summary>
    Syntax,

    /// <summary>
    /// Semantic validation.
    /// </summary>
    Semantic,

    /// <summary>
    /// Runtime validation.
    /// </summary>
    Runtime,

    /// <summary>
    /// Security validation.
    /// </summary>
    Security,

    /// <summary>
    /// Performance validation.
    /// </summary>
    Performance,

    /// <summary>
    /// Resource validation.
    /// </summary>
    Resource,

    /// <summary>
    /// Dependency validation.
    /// </summary>
    Dependency,

    /// <summary>
    /// Configuration validation.
    /// </summary>
    Configuration,

    /// <summary>
    /// Custom validation.
    /// </summary>
    Custom
} 