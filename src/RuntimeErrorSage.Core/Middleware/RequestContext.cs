using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Middleware;

/// <summary>
/// Represents the context of an HTTP request.
/// </summary>
public class RequestContext
{
    /// <summary>
    /// Gets or sets the request path.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the request method.
    /// </summary>
    public string Method { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the query string.
    /// </summary>
    public string QueryString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the request headers.
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();

    /// <summary>
    /// Gets or sets the request timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; }
} 
