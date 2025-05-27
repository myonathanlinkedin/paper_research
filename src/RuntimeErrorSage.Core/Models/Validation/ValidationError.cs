using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Represents a validation error.
/// </summary>
public class ValidationError
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;

    /// <summary>
    /// Gets or sets the error source.
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error details.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp when the error was generated.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the exception that caused the error.
    /// </summary>
    public Exception Exception { get; set; }

    /// <summary>
    /// Gets or sets the property that caused the error.
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value that caused the error.
    /// </summary>
    public object PropertyValue { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    public ValidationError()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ValidationError(string message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    public ValidationError(string code, string message)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="propertyName">The property name that caused the error.</param>
    public ValidationError(string code, string message, string propertyName)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
    }
}