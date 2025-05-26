using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents a runtime error.
/// </summary>
public class Error
{
    /// <summary>
    /// Gets the error type.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the error source.
    /// </summary>
    public string Source { get; }

    /// <summary>
    /// Gets the stack trace.
    /// </summary>
    public string StackTrace { get; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="type">The error type.</param>
    /// <param name="message">The error message.</param>
    /// <param name="source">The error source.</param>
    /// <param name="stackTrace">The stack trace.</param>
    /// <param name="metadata">The metadata.</param>
    public Error(
        string type,
        string message,
        string source,
        string stackTrace,
        IDictionary<string, string> metadata = null)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Source = source ?? throw new ArgumentNullException(nameof(source));
        StackTrace = stackTrace ?? throw new ArgumentNullException(nameof(stackTrace));
        Metadata = metadata != null
            ? new Dictionary<string, string>(metadata)
            : new Dictionary<string, string>();
    }

    /// <summary>
    /// Validates the error.
    /// </summary>
    /// <returns>True if the error is valid; otherwise, false.</returns>
    public bool Validate()
    {
        if (string.IsNullOrEmpty(Type))
            return false;

        if (string.IsNullOrEmpty(Message))
            return false;

        if (string.IsNullOrEmpty(Source))
            return false;

        if (string.IsNullOrEmpty(StackTrace))
            return false;

        return true;
    }
} 