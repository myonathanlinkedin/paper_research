using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Graph;

/// <summary>
/// Represents a dependency analysis result.
/// </summary>
public class DependencyAnalysisResult
{
    /// <summary>
    /// Gets or sets the component identifier.
    /// </summary>
    public string ComponentId { get; }

    /// <summary>
    /// Gets or sets the direct dependencies.
    /// </summary>
    public IReadOnlyCollection<DirectDependencies> DirectDependencies { get; } = new();

    /// <summary>
    /// Gets or sets the indirect dependencies.
    /// </summary>
    public IReadOnlyCollection<IndirectDependencies> IndirectDependencies { get; } = new();

    /// <summary>
    /// Gets or sets the dependency metrics.
    /// </summary>
    public Dictionary<string, double> DependencyMetrics { get; set; } = new();
} 






