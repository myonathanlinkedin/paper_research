using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Examples.Models;

/// <summary>
/// Base class for all request models
/// </summary>
public abstract class BaseRequest
{
    /// <summary>
    /// Unique identifier for the request
    /// </summary>
    public Guid RequestId { get; } = Guid.NewGuid();

    /// <summary>
    /// Timestamp when the request was created
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Optional correlation ID for distributed tracing
    /// </summary>
    public string? CorrelationId { get; set; }

    /// <summary>
    /// Optional user context for the request
    /// </summary>
    public string? UserContext { get; set; }
} 





