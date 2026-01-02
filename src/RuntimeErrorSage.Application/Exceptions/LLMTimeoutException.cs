using System;

namespace RuntimeErrorSage.Application.Exceptions;

/// <summary>
/// Exception thrown when LLM operations timeout.
/// </summary>
public class LLMTimeoutException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LLMTimeoutException"/> class.
    /// </summary>
    public LLMTimeoutException() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LLMTimeoutException"/> class with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public LLMTimeoutException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LLMTimeoutException"/> class with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public LLMTimeoutException(string message, Exception innerException) : base(message, innerException)
    {
    }
}




