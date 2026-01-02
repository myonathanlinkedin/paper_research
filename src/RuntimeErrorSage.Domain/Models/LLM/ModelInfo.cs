using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.LLM;

/// <summary>
/// Represents information about a language model.
/// </summary>
public class ModelInfo
{
    /// <summary>
    /// Gets or sets the unique identifier of the model.
    /// </summary>
    public string ModelId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the model.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the version of the model.
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the provider of the model (e.g., OpenAI, Anthropic).
    /// </summary>
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the model's capabilities.
    /// </summary>
    public List<string> Capabilities { get; set; } = new();

    /// <summary>
    /// Gets or sets the model's limitations.
    /// </summary>
    public List<string> Limitations { get; set; } = new();

    /// <summary>
    /// Gets or sets the maximum context length the model can handle.
    /// </summary>
    public int MaxContextLength { get; set; }

    /// <summary>
    /// Gets or sets the model's parameters count.
    /// </summary>
    public long ParameterCount { get; set; }

    /// <summary>
    /// Gets or sets when the model was last updated.
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Gets or sets the model's configuration settings.
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();

    /// <summary>
    /// Gets or sets the model's performance metrics.
    /// </summary>
    public Dictionary<string, double> PerformanceMetrics { get; set; } = new();

    /// <summary>
    /// Gets or sets whether the model is currently available.
    /// </summary>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Gets or sets the model's license information.
    /// </summary>
    public string License { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any additional metadata about the model.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the unique identifier of the model (alias for ModelId).
    /// </summary>
    public string Id
    {
        get => ModelId;
        set => ModelId = value;
    }

    /// <summary>
    /// Gets or sets the status of the model.
    /// </summary>
    public ModelStatus Status { get; set; } = ModelStatus.Available;
} 
