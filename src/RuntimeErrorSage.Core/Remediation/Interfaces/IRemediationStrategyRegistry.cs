using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Models.Common;

namespace RuntimeErrorSage.Core.Remediation.Interfaces;

/// <summary>
/// Defines the interface for managing remediation strategies.
/// </summary>
public interface IRemediationStrategyRegistry
{
    /// <summary>
    /// Registers a remediation strategy with its metadata.
    /// </summary>
    /// <param name="strategy">The strategy to register</param>
    /// <param name="metadata">The strategy metadata</param>
    void RegisterStrategy(IRemediationStrategy strategy, StrategyMetadata metadata);

    /// <summary>
    /// Unregisters a remediation strategy.
    /// </summary>
    /// <param name="strategyName">The name of the strategy</param>
    /// <param name="version">The version of the strategy</param>
    void UnregisterStrategy(string strategyName, string version);

    /// <summary>
    /// Gets all strategies that can handle the given error analysis result.
    /// </summary>
    /// <param name="analysis">The error analysis result</param>
    /// <returns>An ordered collection of applicable strategies</returns>
    IEnumerable<IRemediationStrategy> GetStrategiesForError(ErrorAnalysisResult analysis);

    /// <summary>
    /// Gets a specific strategy by name and version.
    /// </summary>
    /// <param name="strategyName">The name of the strategy</param>
    /// <param name="version">The version of the strategy</param>
    /// <returns>The strategy if found, null otherwise</returns>
    IRemediationStrategy? GetStrategy(string strategyName, string version);

    /// <summary>
    /// Gets all versions of a strategy.
    /// </summary>
    /// <param name="strategyName">The name of the strategy</param>
    /// <returns>An ordered collection of version strings</returns>
    IEnumerable<string> GetStrategyVersions(string strategyName);

    /// <summary>
    /// Gets the latest version of a strategy.
    /// </summary>
    /// <param name="strategyName">The name of the strategy</param>
    /// <returns>The latest version string</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no versions are found</exception>
    string GetLatestVersion(string strategyName);

    /// <summary>
    /// Gets the metadata for a specific strategy version.
    /// </summary>
    /// <param name="strategyName">The name of the strategy</param>
    /// <param name="version">The version of the strategy</param>
    /// <returns>The strategy metadata</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no metadata is found</exception>
    StrategyMetadata GetStrategyMetadata(string strategyName, string version);
} 