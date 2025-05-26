using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.Graph.Enums;

namespace RuntimeErrorSage.Core.Interfaces;

/// <summary>
/// Interface for graph-based analysis of runtime errors.
/// </summary>
public interface IGraphAnalyzer
{
    /// <summary>
    /// Analyzes the error context and builds a dependency graph.
    /// </summary>
    Task<DependencyGraph> BuildDependencyGraphAsync(ErrorContext context);

    /// <summary>
    /// Analyzes the impact of an error through the dependency graph.
    /// </summary>
    Task<ImpactAnalysisResult> AnalyzeImpactAsync(ErrorContext context);

    /// <summary>
    /// Calculates the shortest path between two nodes in the graph.
    /// </summary>
    Task<List<string>> CalculateShortestPathAsync(string sourceId, string targetId);

    /// <summary>
    /// Updates the graph metrics based on the current state.
    /// </summary>
    Task<Dictionary<string, double>> UpdateMetricsAsync(DependencyGraph graph);
}

/// <summary>
/// Specifies the impact severity.
/// </summary>
public enum ImpactSeverity
{
    Low,
    Medium,
    High,
    Critical
}

/// <summary>
/// Specifies the impact scope.
/// </summary>
public enum ImpactScope
{
    Local,
    Component,
    Service,
    System
}

/// <summary>
/// Specifies the dependency type.
/// </summary>
public enum DependencyType
{
    Runtime,
    Compile,
    Development,
    Test
} 