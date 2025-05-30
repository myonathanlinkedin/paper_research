using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RuntimeErrorSage.Domain.Models.Common;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Validation;

/// <summary>
/// Represents a validation error.
/// </summary>
public sealed class ValidationError
{
    private readonly Dictionary<string, object> _details;

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error severity.
    /// </summary>
    public ValidationSeverity Severity { get; }

    /// <summary>
    /// Gets the error source.
    /// </summary>
    public string Source { get; }

    /// <summary>
    /// Gets the error details.
    /// </summary>
    public IReadOnlyDictionary<string, object> Details => new ReadOnlyDictionary<string, object>(_details);

    /// <summary>
    /// Gets the timestamp when the error was generated.
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Gets the exception that caused the error.
    /// </summary>
    public Exception Exception { get; }

    /// <summary>
    /// Gets the property that caused the error.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the value that caused the error.
    /// </summary>
    public object PropertyValue { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="severity">The error severity.</param>
    /// <param name="source">The error source.</param>
    /// <param name="exception">The exception that caused the error.</param>
    /// <param name="propertyName">The property name that caused the error.</param>
    /// <param name="propertyValue">The value that caused the error.</param>
    /// <param name="details">Optional error details dictionary.</param>
    /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
    public ValidationError(
        string code,
        string message,
        ValidationSeverity severity,
        string source,
        Exception exception,
        string propertyName,
        object propertyValue,
        Dictionary<string, object> details = null)
    {
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(propertyName);

        Code = code;
        Message = message;
        Severity = severity;
        Source = source;
        Exception = exception;
        PropertyName = propertyName;
        PropertyValue = propertyValue;
        Timestamp = DateTime.UtcNow;
        _details = details ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// Creates a new error with additional details.
    /// </summary>
    /// <param name="key">The detail key.</param>
    /// <param name="value">The detail value.</param>
    /// <returns>A new error with the added details.</returns>
    /// <exception cref="ArgumentException">Thrown when key is null or whitespace.</exception>
    public ValidationError WithDetail(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Detail key cannot be null or whitespace", nameof(key));
        }

        var newDetails = new Dictionary<string, object>(_details) { [key] = value };
        return new ValidationError(
            Code,
            Message,
            Severity,
            Source,
            Exception,
            PropertyName,
            PropertyValue,
            newDetails);
    }

    /// <summary>
    /// Gets a detail value from the error.
    /// </summary>
    /// <param name="key">The detail key.</param>
    /// <returns>The detail value, or null if not found.</returns>
    /// <exception cref="ArgumentException">Thrown when key is null or whitespace.</exception>
    public object GetDetail(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Detail key cannot be null or whitespace", nameof(key));
        }

        return _details.TryGetValue(key, out var value) ? value : null;
    }
}
