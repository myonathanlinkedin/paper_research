using System;

namespace RuntimeErrorSage.Examples.Models.Responses;

/// <summary>
/// Base response model for all operations
/// </summary>
public class OperationResponse
{
    /// <summary>
    /// Unique identifier for the operation
    /// </summary>
    public Guid OperationId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Status of the operation
    /// </summary>
    public OperationStatus Status { get; set; }

    /// <summary>
    /// Timestamp when the operation completed
    /// </summary>
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Duration of the operation in milliseconds
    /// </summary>
    public long DurationMs { get; set; }

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

/// <summary>
/// Status of an operation
/// </summary>
public enum OperationStatus
{
    /// <summary>
    /// Operation completed successfully
    /// </summary>
    Success,

    /// <summary>
    /// Operation failed
    /// </summary>
    Failed,

    /// <summary>
    /// Operation is in progress
    /// </summary>
    InProgress,

    /// <summary>
    /// Operation was cancelled
    /// </summary>
    Cancelled
}

/// <summary>
/// Detailed error information
/// </summary>
public class ErrorDetails
{
    /// <summary>
    /// Error code for the operation
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Human-readable error message
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Detailed error description
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Stack trace if available
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// Additional context about the error
    /// </summary>
    public Dictionary<string, object>? Context { get; set; }
} 
