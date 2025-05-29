using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Common;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Validation;

/// <summary>
/// Represents a validation error.
/// </summary>
public class ValidationError
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string Code { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public ValidationSeverity Severity { get; } = ValidationSeverity.Error;

    /// <summary>
    /// Gets or sets the error source.
    /// </summary>
    public string Source { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the error details.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp when the error was generated.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the exception that caused the error.
    /// </summary>
    public Exception Exception { get; }

    /// <summary>
    /// Gets or sets the property that caused the error.
    /// </summary>
    public string PropertyName { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the value that caused the error.
    /// </summary>
    public object PropertyValue { get; }

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
        ArgumentNullException.ThrowIfNull(message);
        Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    public ValidationError(string code, string message)
    {
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(message);
        Code = code;
        Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="propertyName">The property name that caused the error.</param>
    public ValidationError(string code, string message, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(propertyName);
        Code = code;
        Message = message;
        PropertyName = propertyName;
    }
}






