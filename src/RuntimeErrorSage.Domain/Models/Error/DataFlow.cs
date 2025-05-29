using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Models.Error;

/// <summary>
/// Represents a data flow in the error context.
/// </summary>
public class ErrorDataFlow
{
    /// <summary>
    /// Gets or sets the source of the data flow.
    /// </summary>
    public string Source { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the target of the data flow.
    /// </summary>
    public string Target { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of data being flowed.
    /// </summary>
    public string DataType { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the volume of data.
    /// </summary>
    public double Volume { get; }

    /// <summary>
    /// Gets or sets the timestamp of the data flow.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;
} 





