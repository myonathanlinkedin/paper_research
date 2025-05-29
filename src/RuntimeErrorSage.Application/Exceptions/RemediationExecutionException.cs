using System;

namespace RuntimeErrorSage.Application.Remediation.Exceptions;

/// <summary>
/// Exception thrown when a remediation execution fails.
/// </summary>
public class RemediationExecutionException : Exception
{
    /// <summary>
    /// Gets the remediation ID associated with this exception.
    /// </summary>
    public string RemediationId { get; }

    /// <summary>
    /// Gets the step ID where the failure occurred, if any.
    /// </summary>
    public string? StepId { get; }

    /// <summary>
    /// Gets the error code associated with this exception.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Initializes a new instance of the RemediationExecutionException class.
    /// </summary>
    public RemediationExecutionException() : base() { }

    /// <summary>
    /// Initializes a new instance of the RemediationExecutionException class with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public RemediationExecutionException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the RemediationExecutionException class with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public RemediationExecutionException(string message, Exception innerException) 
        : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the RemediationExecutionException class with detailed information.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="remediationId">The remediation ID.</param>
    /// <param name="stepId">The step ID where the failure occurred.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="innerException">The inner exception.</param>
    public RemediationExecutionException(
        string message,
        string remediationId,
        string? stepId,
        string errorCode,
        Exception? innerException = null)
        : base(message, innerException)
    {
        RemediationId = remediationId;
        StepId = stepId;
        ErrorCode = errorCode;
    }
} 
