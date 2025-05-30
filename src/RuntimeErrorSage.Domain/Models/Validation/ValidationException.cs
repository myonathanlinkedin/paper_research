using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RuntimeErrorSage.Domain.Models.Validation;

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
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    public ValidationException()
        : base("Validation failed.")
    {
        Errors = Array.Empty<ValidationError>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <exception cref="ArgumentNullException">Thrown when message is null.</exception>
    public ValidationException(string message)
        : base(message ?? throw new ArgumentNullException(nameof(message)))
    {
        Errors = Array.Empty<ValidationError>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    /// <exception cref="ArgumentNullException">Thrown when message is null.</exception>
    public ValidationException(string message, Exception innerException)
        : base(message ?? throw new ArgumentNullException(nameof(message)), innerException)
    {
        Errors = Array.Empty<ValidationError>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class with a message and validation errors.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">The collection of validation errors.</param>
    /// <exception cref="ArgumentNullException">Thrown when message or errors is null.</exception>
    public ValidationException(string message, IEnumerable<ValidationError> errors)
        : base(message ?? throw new ArgumentNullException(nameof(message)))
    {
        ArgumentNullException.ThrowIfNull(errors);
        Errors = new ReadOnlyCollection<ValidationError>(errors.ToList());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class with a message, validation errors, and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">The collection of validation errors.</param>
    /// <param name="innerException">The inner exception.</param>
    /// <exception cref="ArgumentNullException">Thrown when message or errors is null.</exception>
    public ValidationException(string message, IEnumerable<ValidationError> errors, Exception innerException)
        : base(message ?? throw new ArgumentNullException(nameof(message)), innerException)
    {
        ArgumentNullException.ThrowIfNull(errors);
        Errors = new ReadOnlyCollection<ValidationError>(errors.ToList());
    }

    /// <summary>
    /// Creates a validation exception with a single error.
    /// </summary>
    /// <param name="error">The validation error.</param>
    /// <returns>A new validation exception.</returns>
    /// <exception cref="ArgumentNullException">Thrown when error is null.</exception>
    public static ValidationException FromError(ValidationError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new ValidationException(error.Message, new[] { error });
    }

    /// <summary>
    /// Creates a validation exception with multiple errors.
    /// </summary>
    /// <param name="errors">The validation errors.</param>
    /// <returns>A new validation exception.</returns>
    /// <exception cref="ArgumentNullException">Thrown when errors is null.</exception>
    public static ValidationException FromErrors(IEnumerable<ValidationError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);
        var errorList = errors.ToList();
        if (!errorList.Any())
        {
            throw new ArgumentException("At least one error must be provided.", nameof(errors));
        }

        var message = string.Join(Environment.NewLine, errorList.Select(e => e.Message));
        return new ValidationException(message, errorList);
    }
} 
