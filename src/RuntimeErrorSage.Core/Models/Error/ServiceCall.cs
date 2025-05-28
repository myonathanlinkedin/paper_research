using System;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents a service call in the error context.
/// </summary>
public class ServiceCall
{
    /// <summary>
    /// Gets or sets the source of the service call.
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target of the service call.
    /// </summary>
    public string Target { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the operation performed.
    /// </summary>
    public string Operation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp of the service call.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets whether the service call was successful.
    /// </summary>
    public bool Success { get; set; }
} 
