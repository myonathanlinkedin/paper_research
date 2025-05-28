using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents the analysis of a component graph for remediation purposes.
/// </summary>
public class GraphAnalysis
{
    /// <summary>
    /// Gets or sets whether the analysis is valid.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the error message if analysis failed.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the component health scores (component id -> health score).
    /// </summary>
    public Dictionary<string, double> ComponentHealth { get; set; } = new Dictionary<string, double>();

    /// <summary>
    /// Gets or sets the component relationships.
    /// </summary>
    public List<ComponentRelationship> ComponentRelationships { get; set; } = new List<ComponentRelationship>();

    /// <summary>
    /// Gets or sets the error propagation analysis.
    /// </summary>
    public ErrorPropagation ErrorPropagation { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the analysis.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the correlation ID.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the metrics associated with the analysis.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new Dictionary<string, double>();
} 
