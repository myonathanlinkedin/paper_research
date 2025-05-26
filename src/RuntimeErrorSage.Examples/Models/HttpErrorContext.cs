namespace RuntimeErrorSage.Examples.Models;

/// <summary>
/// Context information for HTTP-related errors
/// </summary>
public class HttpErrorContext
{
    /// <summary>
    /// The HTTP method used
    /// </summary>
    public string? Method { get; set; }

    /// <summary>
    /// The URL that failed
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// The HTTP status code
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// The response content
    /// </summary>
    public string? ResponseContent { get; set; }

    /// <summary>
    /// Additional error context
    /// </summary>
    public Dictionary<string, object>? Context { get; set; }
} 