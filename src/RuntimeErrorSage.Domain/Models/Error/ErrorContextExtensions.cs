using System;
using RuntimeErrorSage.Domain.Models.Context;

namespace RuntimeErrorSage.Domain.Models.Error;

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
            throw new ArgumentNullException(nameof(errorContext));

        var context = new RuntimeContext
        {
            Id = errorContext.ContextId,
            ComponentId = errorContext.ComponentId,
            ComponentName = errorContext.ComponentName,
            ErrorMessage = errorContext.Message,
            StackTrace = errorContext.StackTrace,
            CorrelationId = errorContext.CorrelationId
        };

        // Add additional metadata from error context
        foreach (var item in errorContext.Metadata)
        {
            context.Metadata[item.Key] = item.Value;
        }

        return context;
    }
} 
