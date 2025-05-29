using System.Collections.ObjectModel;
using System;
using RuntimeErrorSage.Application.Models.Context;

namespace RuntimeErrorSage.Application.Models.Error;

/// <summary>
/// Extension methods for ErrorContext
/// </summary>
public static class ErrorContextExtensions
{
    /// <summary>
    /// Converts an ErrorContext to a RuntimeContext
    /// </summary>
    /// <param name="errorContext">The error context to convert</param>
    /// <returns>A new RuntimeContext instance</returns>
    public static RuntimeContext ToRuntimeContext(this ErrorContext errorContext)
    {
        if (errorContext == null)
            ArgumentNullException.ThrowIfNull(nameof(errorContext));

        return new RuntimeContext
        {
            ContextId = errorContext.Id,
            ApplicationName = errorContext.ComponentName,
            Environment = errorContext.ServiceName,
            CorrelationId = errorContext.CorrelationId,
            Timestamp = errorContext.Timestamp,
            Metadata = errorContext.Metadata.ToDictionary(kv => kv.Key, kv => kv.Value)
        };
    }
} 





