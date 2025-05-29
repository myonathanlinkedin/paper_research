using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Examples.Models.Responses;

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






