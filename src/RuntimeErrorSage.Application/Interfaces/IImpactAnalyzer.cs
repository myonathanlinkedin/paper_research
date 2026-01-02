using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Models.Analysis; // For GraphAnalysisResult, RemediationAnalysis, etc.
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;

namespace RuntimeErrorSage.Application.Interfaces;

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
    Task<List<RuntimeErrorSage.Domain.Models.Graph.ImpactAnalysisResult>> AnalyzeImpactAsync(ErrorContext context, DependencyGraph graph);

    /// <summary>
    /// Validates the impact analyzer configuration.
    /// </summary>
    /// <returns>True if the configuration is valid, false otherwise</returns>
    Task<bool> ValidateConfigurationAsync();
} 
