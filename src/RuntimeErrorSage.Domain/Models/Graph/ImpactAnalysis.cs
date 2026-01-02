using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Graph;

/// <summary>
/// Represents the impact analysis of a graph.
/// </summary>
public class ImpactAnalysis
{
    /// <summary>
    /// Gets or sets the affected services.
    /// </summary>
    public List<string> AffectedServices { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the critical path.
    /// </summary>
    public List<string> CriticalPath { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the severity.
    /// </summary>
    public string Severity { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether there is a circular dependency.
    /// </summary>
    public bool HasCircularDependency { get; set; }

    /// <summary>
    /// Gets or sets the failure chain.
    /// </summary>
    public List<string> FailureChain { get; set; } = new List<string>();
}




