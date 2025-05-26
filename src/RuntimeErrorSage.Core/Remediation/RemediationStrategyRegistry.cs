using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Remediation.Models.Common;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Implements a registry pattern for managing remediation strategies.
/// </summary>
public class RemediationStrategyRegistry : IRemediationStrategyRegistry
{
    private readonly ILogger<RemediationStrategyRegistry> _logger;
    private readonly ConcurrentDictionary<string, IRemediationStrategy> _strategies;
    private readonly ConcurrentDictionary<string, StrategyMetadata> _strategyMetadata;
    private readonly ConcurrentDictionary<string, List<string>> _strategyVersions;

    public RemediationStrategyRegistry(ILogger<RemediationStrategyRegistry> logger)
    {
        _logger = logger;
        _strategies = new ConcurrentDictionary<string, IRemediationStrategy>();
        _strategyMetadata = new ConcurrentDictionary<string, StrategyMetadata>();
        _strategyVersions = new ConcurrentDictionary<string, List<string>>();
    }

    public void RegisterStrategy(IRemediationStrategy strategy, StrategyMetadata metadata)
    {
        if (strategy == null)
            throw new ArgumentNullException(nameof(strategy));
        if (metadata == null)
            throw new ArgumentNullException(nameof(metadata));

        var strategyKey = GetStrategyKey(strategy.Name, metadata.Version);
        
        if (_strategies.TryAdd(strategyKey, strategy))
        {
            _strategyMetadata[strategyKey] = metadata;
            _strategyVersions.AddOrUpdate(
                strategy.Name,
                new List<string> { metadata.Version },
                (_, versions) =>
                {
                    if (!versions.Contains(metadata.Version))
                        versions.Add(metadata.Version);
                    return versions;
                });

            _logger.LogInformation(
                "Registered remediation strategy {Strategy} version {Version}",
                strategy.Name,
                metadata.Version);
        }
        else
        {
            _logger.LogWarning(
                "Strategy {Strategy} version {Version} already registered",
                strategy.Name,
                metadata.Version);
        }
    }

    public void UnregisterStrategy(string strategyName, string version)
    {
        var strategyKey = GetStrategyKey(strategyName, version);
        
        if (_strategies.TryRemove(strategyKey, out _))
        {
            _strategyMetadata.TryRemove(strategyKey, out _);
            _strategyVersions.AddOrUpdate(
                strategyName,
                new List<string>(),
                (_, versions) =>
                {
                    versions.Remove(version);
                    return versions;
                });

            _logger.LogInformation(
                "Unregistered remediation strategy {Strategy} version {Version}",
                strategyName,
                version);
        }
    }

    public IEnumerable<IRemediationStrategy> GetStrategiesForError(ErrorAnalysisResult analysis)
    {
        return _strategies.Values
            .Where(s => s.CanHandle(analysis))
            .OrderByDescending(s => s.Priority)
            .ThenBy(s => _strategyMetadata[GetStrategyKey(s.Name, GetLatestVersion(s.Name))].CreationDate);
    }

    public IRemediationStrategy? GetStrategy(string strategyName, string version)
    {
        var strategyKey = GetStrategyKey(strategyName, version);
        return _strategies.TryGetValue(strategyKey, out var strategy) ? strategy : null;
    }

    public IEnumerable<string> GetStrategyVersions(string strategyName)
    {
        return _strategyVersions.TryGetValue(strategyName, out var versions)
            ? versions.OrderByDescending(v => v)
            : Enumerable.Empty<string>();
    }

    public string GetLatestVersion(string strategyName)
    {
        return _strategyVersions.TryGetValue(strategyName, out var versions) && versions.Any()
            ? versions.OrderByDescending(v => v).First()
            : throw new KeyNotFoundException($"No versions found for strategy {strategyName}");
    }

    public StrategyMetadata GetStrategyMetadata(string strategyName, string version)
    {
        var strategyKey = GetStrategyKey(strategyName, version);
        return _strategyMetadata.TryGetValue(strategyKey, out var metadata)
            ? metadata
            : throw new KeyNotFoundException($"No metadata found for strategy {strategyName} version {version}");
    }

    private static string GetStrategyKey(string strategyName, string version) => $"{strategyName}:{version}";
}

public class StrategyMetadata
{
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public Dictionary<string, string> Dependencies { get; set; } = new();
    public Dictionary<string, string> Requirements { get; set; } = new();
    public bool IsDeprecated { get; set; }
    public string? DeprecationReason { get; set; }
    public string? ReplacementStrategy { get; set; }
} 