using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.LLM;

/// <summary>
/// Represents the context for an LLM analysis.
/// </summary>
public class LLMAnalysisContext
{
    /// <summary>
    /// Gets or sets the unique identifier of the context.
    /// </summary>
    public string ContextId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error context.
    /// </summary>
    public string ErrorContext { get; set; }

    /// <summary>
    /// Gets or sets the code context.
    /// </summary>
    public string CodeContext { get; set; }

    /// <summary>
    /// Gets or sets the runtime context.
    /// </summary>
    public string RuntimeContext { get; set; }

    /// <summary>
    /// Gets or sets the system context.
    /// </summary>
    public string SystemContext { get; set; }

    /// <summary>
    /// Gets or sets the context metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 