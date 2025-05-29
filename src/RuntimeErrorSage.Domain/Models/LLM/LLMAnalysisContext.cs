using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.LLM;

/// <summary>
/// Represents the context for an LLM analysis.
/// </summary>
public class LLMAnalysisContext
{
    /// <summary>
    /// Gets or sets the unique identifier of the context.
    /// </summary>
    public string ContextId { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error context.
    /// </summary>
    public string ErrorContext { get; }

    /// <summary>
    /// Gets or sets the code context.
    /// </summary>
    public string CodeContext { get; }

    /// <summary>
    /// Gets or sets the runtime context.
    /// </summary>
    public string RuntimeContext { get; }

    /// <summary>
    /// Gets or sets the system context.
    /// </summary>
    public string SystemContext { get; }

    /// <summary>
    /// Gets or sets the context metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 






