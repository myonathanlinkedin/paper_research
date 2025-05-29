using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Models.Graph;

/// <summary>
/// Represents a dependency analysis result.
/// </summary>
public class DependencyAnalysisResult
{
    /// <summary>
    /// Gets or sets the component identifier.
    /// </summary>
    public string ComponentId { get; set; }

    /// <summary>
    /// Gets or sets the direct dependencies.
    /// </summary>
    public List<DependencyInfo> DirectDependencies { get; set; } = new();

    /// <summary>
    /// Gets or sets the indirect dependencies.
    /// </summary>
    public List<DependencyInfo> IndirectDependencies { get; set; } = new();

    /// <summary>
    /// Gets or sets the dependency metrics.
    /// </summary>
    public Dictionary<string, double> DependencyMetrics { get; set; } = new();
} 
