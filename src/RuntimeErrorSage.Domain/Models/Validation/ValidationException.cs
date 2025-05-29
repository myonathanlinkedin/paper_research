using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RuntimeErrorSage.Model.Models.Validation;

/// <summary>
/// Exception thrown when validation fails.
/// </summary>
public sealed class ValidationException : Exception
{
    /// <summary>
    /// Gets the collection of validation errors.
    /// </summary>
    public IReadOnlyCollection<ValidationError> Errors { get; }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with a message and validation errors.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">The collection of validation errors.</param>
    public ValidationException(string message, IEnumerable<ValidationError> errors)
        : base(message)
    {
        Errors = new ReadOnlyCollection<ValidationError>(errors.ToList());
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with a message, validation errors, and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">The collection of validation errors.</param>
    /// <param name="innerException">The inner exception.</param>
    public ValidationException(string message, IEnumerable<ValidationError> errors, Exception innerException)
        : base(message, innerException)
    {
        Errors = new ReadOnlyCollection<ValidationError>(errors.ToList());
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException class.
    /// </summary>
    public ValidationException()
        : base()
    {
        Errors = Array.Empty<ValidationError>();
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ValidationException(string message)
        : base(message)
    {
        Errors = Array.Empty<ValidationError>();
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        Errors = Array.Empty<ValidationError>();
    }
} 
