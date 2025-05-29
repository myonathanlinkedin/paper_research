using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Graph;
using RuntimeErrorSage.Model.Models.Analysis;

namespace RuntimeErrorSage.Model.Services.Interfaces;

/// <summary>
/// Service for analyzing the impact of errors on system components.
/// </summary>
public interface IImpactAnalyzer
{
    /// <summary>
    /// Analyzes the impact of an error on system components.
    /// </summary>
    /// <param name="context">The error context</param>
    /// <param name="graph">The dependency graph</param>
    /// <returns>A list of impact analysis results</returns>
    Task<List<RuntimeErrorSage.Model.Models.Graph.ImpactAnalysisResult>> AnalyzeImpactAsync(ErrorContext context, DependencyGraph graph);

    /// <summary>
    /// Validates the impact analyzer configuration.
    /// </summary>
    /// <returns>True if the configuration is valid, false otherwise</returns>
    Task<bool> ValidateConfigurationAsync();
} 
