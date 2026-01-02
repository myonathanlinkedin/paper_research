using System;

namespace RuntimeErrorSage.Application.Exceptions;

/// <summary>
/// Exception thrown when there is an error in LM Studio operations.
/// </summary>
public class LMStudioException : Exception
{
    /// <summary>
    /// Gets or sets the operation that caused the error.
    /// </summary>
    public string Operation { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string ErrorCode { get; } = string.Empty;

    /// <summary>
    /// Gets or sets additional details about the error.
    /// </summary>
    public string Details { get; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="LMStudioException"/> class.
    /// </summary>
    public LMStudioException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LMStudioException"/> class with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public LMStudioException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LMStudioException"/> class with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public LMStudioException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LMStudioException"/> class with a message, operation, and error code.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="operation">The operation that caused the error.</param>
    /// <param name="errorCode">The error code.</param>
    public LMStudioException(string message, string operation, string errorCode) : base(message)
    {
        Operation = operation;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LMStudioException"/> class with a message, operation, error code, and details.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="operation">The operation that caused the error.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="details">Additional details about the error.</param>
    public LMStudioException(string message, string operation, string errorCode, string details) : base(message)
    {
        Operation = operation;
        ErrorCode = errorCode;
        Details = details;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LMStudioException"/> class with a message, operation, error code, details, and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="operation">The operation that caused the error.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="details">Additional details about the error.</param>
    /// <param name="innerException">The inner exception.</param>
    public LMStudioException(string message, string operation, string errorCode, string details, Exception innerException) : base(message, innerException)
    {
        Operation = operation;
        ErrorCode = errorCode;
        Details = details;
    }
} 

