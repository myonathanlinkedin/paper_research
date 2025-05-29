using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Examples.Models.Responses.Enums;

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






