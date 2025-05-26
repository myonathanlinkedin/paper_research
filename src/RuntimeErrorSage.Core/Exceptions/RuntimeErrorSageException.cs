using System;
using System.Runtime.Serialization;

namespace RuntimeErrorSage.Core.Exceptions;

/// <summary>
/// Represents errors that occur during RuntimeErrorSage operations.
/// </summary>
[Serializable]
public class RuntimeErrorSageException : Exception
{
    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Gets or sets additional error details.
    /// </summary>
    public object Details { get; }

    /// <summary>
    /// Initializes a new instance of the RuntimeErrorSageException class.
    /// </summary>
    public RuntimeErrorSageException() : base() { }

    /// <summary>
    /// Initializes a new instance of the RuntimeErrorSageException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public RuntimeErrorSageException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the RuntimeErrorSageException class with a specified error message and error code.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="errorCode">The error code.</param>
    public RuntimeErrorSageException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the RuntimeErrorSageException class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public RuntimeErrorSageException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the RuntimeErrorSageException class with a specified error message, error code, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public RuntimeErrorSageException(string message, string errorCode, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the RuntimeErrorSageException class with a specified error message, error code, and additional details.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="details">Additional error details.</param>
    public RuntimeErrorSageException(string message, string errorCode, object details) : base(message)
    {
        ErrorCode = errorCode;
        Details = details;
    }

    /// <summary>
    /// Initializes a new instance of the RuntimeErrorSageException class with serialized data.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
    protected RuntimeErrorSageException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        ErrorCode = info.GetString(nameof(ErrorCode));
        Details = info.GetValue(nameof(Details), typeof(object));
    }

    /// <summary>
    /// When overridden in a derived class, sets the SerializationInfo with information about the exception.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ErrorCode), ErrorCode);
        info.AddValue(nameof(Details), Details);
    }
}

public class RuntimeErrorSageValidationException : RuntimeErrorSageException
{
    public RuntimeErrorSageValidationException(string message) : base(message)
    {
    }

    public RuntimeErrorSageValidationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    public RuntimeErrorSageValidationException()
    {
    }
}

public class RuntimeErrorSageRemediationException : RuntimeErrorSageException
{
    public RuntimeErrorSageRemediationException(string message) : base(message)
    {
    }

    public RuntimeErrorSageRemediationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    public RuntimeErrorSageRemediationException()
    {
    }
}

public class RuntimeErrorSageLLMException : RuntimeErrorSageException
{
    public RuntimeErrorSageLLMException(string message) : base(message)
    {
    }

    public RuntimeErrorSageLLMException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    public RuntimeErrorSageLLMException()
    {
    }
}

public class RuntimeErrorSageGraphAnalysisException : RuntimeErrorSageException
{
    public RuntimeErrorSageGraphAnalysisException(string message) : base(message)
    {
    }

    public RuntimeErrorSageGraphAnalysisException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    public RuntimeErrorSageGraphAnalysisException()
    {
    }
} 
