using System;

namespace RuntimeErrorSage.Application.Exceptions;

/// <summary>
/// Exception thrown when validation operations timeout.
/// </summary>
public class ValidationTimeoutException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationTimeoutException"/> class.
    /// </summary>
    public ValidationTimeoutException() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationTimeoutException"/> class with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ValidationTimeoutException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationTimeoutException"/> class with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ValidationTimeoutException(string message, Exception innerException) : base(message, innerException)
    {
    }
}




