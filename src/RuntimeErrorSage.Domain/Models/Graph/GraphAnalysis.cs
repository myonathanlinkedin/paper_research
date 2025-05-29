using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;

namespace RuntimeErrorSage.Application.Models.Graph;

/// <summary>
/// Represents the analysis of a component graph for remediation purposes.
/// </summary>
public class GraphAnalysis
{
    /// <summary>
    /// Gets or sets whether the analysis is valid.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Gets or sets the error message if analysis failed.
    /// </summary>
    public string ErrorMessage { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the component health scores (component id -> health score).
    /// </summary>
    public Dictionary<string, double> ComponentHealth { get; set; } = new Dictionary<string, double>();

    /// <summary>
    /// Gets or sets the component relationships.
    /// </summary>
    public IReadOnlyCollection<ComponentRelationships> ComponentRelationships { get; } = new Collection<ComponentRelationship>();

    /// <summary>
    /// Gets or sets the error propagation analysis.
    /// </summary>
    public ErrorPropagation ErrorPropagation { get; }

    /// <summary>
    /// Gets or sets the timestamp of the analysis.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the correlation ID.
    /// </summary>
    public string CorrelationId { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the metrics associated with the analysis.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new Dictionary<string, double>();
} 






