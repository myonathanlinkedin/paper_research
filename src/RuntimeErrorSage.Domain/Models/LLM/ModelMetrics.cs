using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.LLM;

/// <summary>
/// Represents metrics for a model execution.
/// </summary>
public class ModelMetrics
{
    /// <summary>
    /// Gets or sets the timestamp of the metrics.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the name of the model.
    /// </summary>
    public string ModelName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the version of the model.
    /// </summary>
    public string ModelVersion { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the response time in milliseconds.
    /// </summary>
    public double ResponseTime { get; set; }
    
    /// <summary>
    /// Gets or sets the token count.
    /// </summary>
    public int TokenCount { get; set; }
    
    /// <summary>
    /// Gets or sets additional properties.
    /// </summary>
    public Dictionary<string, object> Properties { get; set; } = new();

    /// <summary>
    /// Adds a property to the metrics.
    /// </summary>
    /// <param name="key">The property key.</param>
    /// <param name="value">The property value.</param>
    public void AddProperty(string key, object value)
    {
        Properties[key] = value;
    }
} 
