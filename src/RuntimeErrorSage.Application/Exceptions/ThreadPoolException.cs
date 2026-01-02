using System;

namespace RuntimeErrorSage.Application.Exceptions;

/// <summary>
/// Exception thrown when thread pool operations fail.
/// </summary>
public class ThreadPoolException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThreadPoolException"/> class.
    /// </summary>
    public ThreadPoolException() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThreadPoolException"/> class with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ThreadPoolException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThreadPoolException"/> class with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ThreadPoolException(string message, Exception innerException) : base(message, innerException)
    {
    }
}




