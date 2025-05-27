using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents the result of a graph analysis operation.
/// </summary>
public class GraphAnalysis
{
    /// <summary>
    /// Gets or sets whether the graph analysis is valid.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the error message if the analysis is invalid.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the analysis.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the health status of components in the graph.
    /// </summary>
    public Dictionary<string, ComponentHealth> ComponentHealth { get; set; } = new();

    /// <summary>
    /// Gets or sets the relationships between components.
    /// </summary>
    public List<ComponentRelationship> ComponentRelationships { get; set; } = new();

    /// <summary>
    /// Gets or sets the error propagation analysis result.
    /// </summary>
    public ErrorPropagation ErrorPropagation { get; set; }
} 