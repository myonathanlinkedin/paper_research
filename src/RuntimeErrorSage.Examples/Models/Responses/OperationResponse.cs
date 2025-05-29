using System.Collections.ObjectModel;
using System;
using RuntimeErrorSage.Examples.Models.Responses.Enums;

namespace RuntimeErrorSage.Examples.Models.Responses;

/// <summary>
/// Base response model for all operations
/// </summary>
public class OperationResponse
{
    /// <summary>
    /// Unique identifier for the operation
    /// </summary>
    public Guid OperationId { get; } = Guid.NewGuid();

    /// <summary>
    /// Status of the operation
    /// </summary>
    public OperationStatus Status { get; }

    /// <summary>
    /// Timestamp when the operation completed
    /// </summary>
    public DateTime CompletedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// Duration of the operation in milliseconds
    /// </summary>
    public long DurationMs { get; }

    /// <summary>
    /// Optional error details if the operation failed
    /// </summary>
    public ErrorDetails? Error { get; set; }

    /// <summary>
    /// Creates a new instance of OperationResponse
    /// </summary>
    public OperationResponse()
    {
        Status = OperationStatus.InProgress;
    }

    /// <summary>
    /// Creates a new instance of OperationResponse with error details
    /// </summary>
    /// <param name="error">The error details</param>
    public OperationResponse(ErrorDetails error)
    {
        Status = OperationStatus.Failed;
        Error = error;
    }
}







