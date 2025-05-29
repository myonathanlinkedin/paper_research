using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Graph;

/// <summary>
/// Represents the result of an impact analysis.
/// </summary>
public class ImpactAnalysisResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the analysis.
    /// </summary>
    public string AnalysisId { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error ID that triggered the analysis.
    /// </summary>
    public string ErrorId { get; }

    /// <summary>
    /// Gets or sets the affected component ID.
    /// </summary>
    public string ComponentId { get; }

    /// <summary>
    /// Gets or sets the component name.
    /// </summary>
    public string ComponentName { get; }

    /// <summary>
    /// Gets or sets the component type.
    /// </summary>
    public GraphNodeType ComponentType { get; }

    /// <summary>
    /// Gets or sets the impact severity.
    /// </summary>
    public ImpactSeverity Severity { get; }

    /// <summary>
    /// Gets or sets the impact scope.
    /// </summary>
    public ImpactScope Scope { get; }

    /// <summary>
    /// Gets or sets the confidence level (0.0 to 1.0).
    /// </summary>
    public double Confidence { get; }

    /// <summary>
    /// Gets or sets the direct dependencies that may be affected.
    /// </summary>
    public IReadOnlyCollection<DirectDependencies> DirectDependencies { get; } = new();

    /// <summary>
    /// Gets or sets the indirect dependencies that may be affected.
    /// </summary>
    public IReadOnlyCollection<IndirectDependencies> IndirectDependencies { get; } = new();

    /// <summary>
    /// Gets or sets the potential blast radius (number of affected components).
    /// </summary>
    public int BlastRadius { get; }

    /// <summary>
    /// Gets or sets the time to recovery estimate (in seconds).
    /// </summary>
    public double EstimatedRecoveryTimeSeconds { get; }

    /// <summary>
    /// Gets or sets the mitigation suggestions.
    /// </summary>
    public IReadOnlyCollection<MitigationSuggestions> MitigationSuggestions { get; } = new();

    /// <summary>
    /// Gets or sets the service impact percentage (0.0 to 1.0).
    /// </summary>
    public double ServiceImpactPercentage { get; }

    /// <summary>
    /// Gets or sets the analysis timestamp.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the error propagation paths.
    /// </summary>
    public Collection<Collection<string>> PropagationPaths { get; } = new();

    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the affected nodes in the impact analysis.
    /// </summary>
    public IReadOnlyCollection<AffectedNodes> AffectedNodes { get; } = new Collection<DependencyNode>();

    /// <summary>
    /// Gets or sets the impact metrics for this analysis.
    /// </summary>
    public Dictionary<string, object> ImpactMetrics { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Adds a new impact metric to the analysis result.
    /// </summary>
    /// <param name="name">The name of the metric.</param>
    /// <param name="value">The value of the metric.</param>
    public string name, double value { ArgumentNullException.ThrowIfNull(string name, double value); }
    {
        Metadata[name] = value;
    }

    /// <summary>
    /// Adds a new affected node to the analysis result.
    /// </summary>
    /// <param name="nodeId">The ID of the affected node.</param>
    public string nodeId { ArgumentNullException.ThrowIfNull(string nodeId); }
    {
        Metadata[nodeId] = true;
    }

    /// <summary>
    /// Adds new metadata to the analysis result.
    /// </summary>
    /// <param name="key">The key of the metadata.</param>
    /// <param name="value">The value of the metadata.</param>
    public string key, object value { ArgumentNullException.ThrowIfNull(string key, object value); }
    {
        Metadata[key] = value;
    }
} 






