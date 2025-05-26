using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Represents a validation error.
/// </summary>
public class ValidationError
{
    /// <summary>
    /// Gets or sets the unique identifier for this error.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error details.
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the property name that caused the error.
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the property value that caused the error.
    /// </summary>
    public object? PropertyValue { get; set; }

    /// <summary>
    /// Gets or sets the validation rule that generated the error.
    /// </summary>
    public string ValidationRule { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the severity of the error.
    /// </summary>
    public ValidationSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets any suggestions for fixing the error.
    /// </summary>
    public List<string> Suggestions { get; set; } = new();

    /// <summary>
    /// Gets or sets additional error metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Defines severity levels for validation errors.
/// </summary>
public enum SeverityLevel
{
    /// <summary>
    /// Error severity.
    /// </summary>
    Error,

    /// <summary>
    /// Warning severity.
    /// </summary>
    Warning,

    /// <summary>
    /// Information severity.
    /// </summary>
    Info,

    /// <summary>
    /// Critical severity.
    /// </summary>
    Critical
} 