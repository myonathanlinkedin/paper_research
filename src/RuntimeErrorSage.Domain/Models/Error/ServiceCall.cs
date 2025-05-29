using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Models.Error;

/// <summary>
/// Represents a service call in the error context.
/// </summary>
public class ServiceCall
{
    /// <summary>
    /// Gets or sets the source of the service call.
    /// </summary>
    public string Source { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the target of the service call.
    /// </summary>
    public string Target { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the operation performed.
    /// </summary>
    public string Operation { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp of the service call.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets whether the service call was successful.
    /// </summary>
    public bool Success { get; }
} 






