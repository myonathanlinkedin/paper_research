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
    /// Gets or sets the validation timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

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

    /// <summary>
    /// Gets or sets the validation errors.
    /// </summary>
    public List<ValidationError> Errors { get; set; } = new();

    /// <summary>
    /// Gets or sets the validation warnings.
    /// </summary>
    public List<ValidationWarning> Warnings { get; set; } = new();

    /// <summary>
    /// Gets or sets the validation suggestions.
    /// </summary>
    public List<ValidationSuggestion> Suggestions { get; set; } = new();
}

/// <summary>
/// Represents a validation detail.
/// </summary>
public class ValidationDetail
{
    /// <summary>
    /// Gets or sets the detail message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the detail code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the detail source.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the detail metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents a validation error.
/// </summary>
public class ValidationError
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the error source.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the error severity level.
    /// </summary>
    public SeverityLevel Severity { get; set; }

    /// <summary>
    /// Gets or sets the name of the property that caused the error.
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the error metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents a validation warning.
/// </summary>
public class ValidationWarning
{
    /// <summary>
    /// Gets or sets the warning message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the warning code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the warning source.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the warning severity level.
    /// </summary>
    public SeverityLevel Severity { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the warning.
    /// </summary>
    public string WarningId { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the warning occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the warning metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents a validation suggestion.
/// </summary>
public class ValidationSuggestion
{
    /// <summary>
    /// Gets or sets the suggestion message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the suggestion code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the suggestion source.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the suggestion metadata.
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