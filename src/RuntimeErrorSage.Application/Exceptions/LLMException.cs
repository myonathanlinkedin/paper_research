using System;

namespace RuntimeErrorSage.Application.Exceptions;

/// <summary>
/// Exception thrown when LLM operations fail.
/// </summary>
public class LLMException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LLMException"/> class.
    /// </summary>
    public LLMException() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LLMException"/> class with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public LLMException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LLMException"/> class with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public LLMException(string message, Exception innerException) : base(message, innerException)
    {
    }
}




