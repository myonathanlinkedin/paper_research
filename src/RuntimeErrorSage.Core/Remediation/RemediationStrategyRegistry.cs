using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Implements a registry pattern for managing remediation strategies.
/// </summary>
public class RemediationStrategyRegistry : IRemediationStrategyRegistry, IRemediationRegistry
{
    private readonly ILogger<RemediationStrategyRegistry> _logger;
    private readonly ConcurrentDictionary<string, IRemediationStrategy> _strategies;
    private readonly ConcurrentDictionary<string, Domain.Models.Remediation.StrategyMetadata> _strategyMetadata;
    private readonly ConcurrentDictionary<string, List<string>> _strategyVersions;
    private readonly ConcurrentDictionary<string, List<string>> _errorTypeStrategies;
    private readonly Func<List<string>> _stringListFactory;
    private readonly Func<List<IRemediationStrategy>> _strategyListFactory;

    public bool IsEnabled { get; } = true;
    public string Name { get; } = "RemediationStrategyRegistry";
    public string Version { get; } = "1.0.0";

    public RemediationStrategyRegistry(
        ILogger<RemediationStrategyRegistry> logger,
        ConcurrentDictionary<string, IRemediationStrategy>? strategies = null,
        ConcurrentDictionary<string, Domain.Models.Remediation.StrategyMetadata>? strategyMetadata = null,
        ConcurrentDictionary<string, List<string>>? strategyVersions = null,
        ConcurrentDictionary<string, List<string>>? errorTypeStrategies = null,
        Func<List<string>>? stringListFactory = null,
        Func<List<IRemediationStrategy>>? strategyListFactory = null)
    {
        _logger = logger;
        _strategies = strategies ?? new ConcurrentDictionary<string, IRemediationStrategy>();
        _strategyMetadata = strategyMetadata ?? new ConcurrentDictionary<string, Domain.Models.Remediation.StrategyMetadata>();
        _strategyVersions = strategyVersions ?? new ConcurrentDictionary<string, List<string>>();
        _errorTypeStrategies = errorTypeStrategies ?? new ConcurrentDictionary<string, List<string>>();
        _stringListFactory = stringListFactory ?? (() => new List<string>());
        _strategyListFactory = strategyListFactory ?? (() => new List<IRemediationStrategy>());
    }

    public void RegisterStrategy(IRemediationStrategy strategy, Domain.Models.Remediation.StrategyMetadata metadata)
    {
        if (strategy == null)
            throw new ArgumentNullException(nameof(strategy));
        if (metadata == null)
            throw new ArgumentNullException(nameof(metadata));

        var key = GetStrategyKey(metadata.Name, metadata.Version);
        _strategies.TryAdd(key, strategy);
        _strategyMetadata.TryAdd(key, metadata);

        // Add version to strategy versions
        _strategyVersions.AddOrUpdate(
            metadata.Name,
            _stringListFactory(),
            (_, versions) =>
            {
                if (!versions.Contains(metadata.Version))
                {
                    versions.Add(metadata.Version);
                }
                return versions;
            });

        // Add strategy to error type mappings
        foreach (var errorType in strategy.SupportedErrorTypes)
        {
            _errorTypeStrategies.AddOrUpdate(
                errorType,
                _stringListFactory(),
                (_, strategies) =>
                {
                    if (!strategies.Contains(metadata.Name))
                    {
                        strategies.Add(metadata.Name);
                    }
                    return strategies;
                });
        }

        _logger.LogInformation(
            "Registered remediation strategy: {StrategyName} v{Version}",
            metadata.Name,
            metadata.Version);
    }

    public void UnregisterStrategy(string strategyName, string version)
    {
        var strategyKey = GetStrategyKey(strategyName, version);
        
        if (_strategies.TryRemove(strategyKey, out var strategy))
        {
            _strategyMetadata.TryRemove(strategyKey, out _);
            _strategyVersions.AddOrUpdate(
                strategyName,
                _stringListFactory(),
                (_, versions) =>
                {
                    versions.Remove(version);
                    return versions;
                });

            // Remove strategy from error type mappings
            foreach (var errorType in strategy.SupportedErrorTypes)
            {
                _errorTypeStrategies.AddOrUpdate(
                    errorType,
                    _stringListFactory(),
                    (_, strategies) =>
                    {
                        strategies.Remove(strategyName);
                        return strategies;
                    });
            }

            _logger.LogInformation(
                "Unregistered remediation strategy {Strategy} version {Version}",
                strategyName,
                version);
        }
    }

    public IRemediationStrategy? GetStrategy(string strategyName, string version)
    {
        var strategyKey = GetStrategyKey(strategyName, version);
        _strategies.TryGetValue(strategyKey, out var strategy);
        return strategy;
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

    public Domain.Models.Remediation.StrategyMetadata GetStrategyMetadata(string strategyName, string version)
    {
        var key = GetStrategyKey(strategyName, version);
        if (!_strategyMetadata.TryGetValue(key, out var metadata))
        {
            throw new KeyNotFoundException($"No metadata found for strategy {strategyName} version {version}");
        }
        return metadata;
    }

    /// <inheritdoc />
    public async Task<IRemediationStrategy> GetStrategyAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentException("Strategy ID cannot be null or empty", nameof(id));

        try
        {
            var strategy = _strategies.Values.FirstOrDefault(s => s.Id == id);
            if (strategy != null)
            {
                _logger.LogDebug("Retrieved strategy with ID '{StrategyId}'", id);
                return strategy;
            }

            _logger.LogWarning("Strategy with ID '{StrategyId}' not found", id);
            throw new KeyNotFoundException($"Strategy with ID '{id}' is not registered");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving strategy with ID {StrategyId}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IRemediationStrategy>> GetStrategiesForErrorTypeAsync(string errorType)
    {
        if (string.IsNullOrEmpty(errorType))
            throw new ArgumentException("Error type cannot be null or empty", nameof(errorType));

        try
        {
            if (_errorTypeStrategies.TryGetValue(errorType, out var strategyNames))
            {
                var strategies = _strategyListFactory();
                
                foreach (var name in strategyNames)
                {
                    try
                    {
                        var latestVersion = GetLatestVersion(name);
                        var strategyKey = GetStrategyKey(name, latestVersion);
                        
                        if (_strategies.TryGetValue(strategyKey, out var strategy))
                        {
                            strategies.Add(strategy);
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        // Skip if we can't find the latest version
                        _logger.LogWarning("Could not find latest version for strategy {StrategyName}", name);
                    }
                }
                
                var orderedStrategies = strategies
                    .OrderByDescending(s => s.Priority)
                    .ToList();

                _logger.LogDebug(
                    "Retrieved {Count} strategies for error type '{ErrorType}'",
                    orderedStrategies.Count,
                    errorType);

                return orderedStrategies;
            }

            _logger.LogWarning("No strategies found for error type '{ErrorType}'", errorType);
            return Enumerable.Empty<IRemediationStrategy>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving strategies for error type {ErrorType}", errorType);
            throw;
        }
    }

    /// <inheritdoc />
    public IEnumerable<IRemediationStrategy> GetStrategiesForErrorType(string errorType)
    {
        if (string.IsNullOrEmpty(errorType))
            throw new ArgumentException("Error type cannot be null or empty", nameof(errorType));

        try
        {
            if (_errorTypeStrategies.TryGetValue(errorType, out var strategyNames))
            {
                var strategies = _strategyListFactory();
                
                foreach (var name in strategyNames)
                {
                    try
                    {
                        var latestVersion = GetLatestVersion(name);
                        var strategyKey = GetStrategyKey(name, latestVersion);
                        
                        if (_strategies.TryGetValue(strategyKey, out var strategy))
                        {
                            strategies.Add(strategy);
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        // Skip if we can't find the latest version
                        _logger.LogWarning("Could not find latest version for strategy {StrategyName}", name);
                    }
                }
                
                return strategies
                    .OrderByDescending(s => s.Priority)
                    .ToList();
            }

            _logger.LogWarning("No strategies found for error type '{ErrorType}'", errorType);
            return Enumerable.Empty<IRemediationStrategy>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving strategies for error type {ErrorType}", errorType);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task RegisterStrategyAsync(IRemediationStrategy strategy)
    {
        ArgumentNullException.ThrowIfNull(strategy);

        try
        {
            var metadata = new Domain.Models.Remediation.StrategyMetadata
            {
                Name = strategy.Name,
                Version = "1.0.0",
                Description = strategy.Description,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                SupportedErrorTypes = strategy.SupportedErrorTypes.ToList(),
                IsEnabled = true,
                Priority = (int)strategy.Priority
            };

            RegisterStrategy(strategy, metadata);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering strategy {StrategyName}", strategy.Name);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UnregisterStrategyAsync(string strategyName)
    {
        if (string.IsNullOrEmpty(strategyName))
        {
            throw new ArgumentException("Strategy name cannot be null or empty", nameof(strategyName));
        }

        try
        {
            var versions = GetStrategyVersions(strategyName).ToList();
            foreach (var version in versions)
            {
                UnregisterStrategy(strategyName, version);
            }
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unregistering strategy {StrategyName}", strategyName);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IRemediationStrategy>> GetStrategiesForErrorAsync(ErrorContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        var applicableStrategies = new List<IRemediationStrategy>();
        foreach (var strategy in _strategies.Values)
        {
            if (await strategy.CanHandleAsync(context))
            {
                applicableStrategies.Add(strategy);
            }
        }

        return applicableStrategies
            .OrderByDescending(s => s.Priority)
            .ThenBy(s => _strategyMetadata[GetStrategyKey(s.Name, GetLatestVersion(s.Name))].CreatedDate);
    }

    /// <inheritdoc />
    public async Task<bool> IsStrategyRegisteredAsync(string strategyName)
    {
        if (string.IsNullOrEmpty(strategyName))
        {
            throw new ArgumentException("Strategy name cannot be null or empty", nameof(strategyName));
        }

        try
        {
            var result = _strategies.Keys.Any(k => k.StartsWith($"{strategyName}:"));
            _logger.LogDebug(
                "Strategy '{StrategyName}' is {Status}",
                strategyName,
                result ? "registered" : "not registered");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if strategy {StrategyName} is registered", strategyName);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<IRemediationStrategy>> GetAllStrategiesAsync()
    {
        try
        {
            // Get the latest version of each strategy
            var latestStrategies = _strategyVersions.Keys
                .Select(strategyName => {
                    var latestVersion = GetLatestVersion(strategyName);
                    var key = GetStrategyKey(strategyName, latestVersion);
                    return _strategies.TryGetValue(key, out var strategy) ? strategy : null;
                })
                .Where(s => s != null)
                .ToList();

            _logger.LogDebug("Retrieved {Count} strategies", latestStrategies.Count);
            return latestStrategies;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all strategies");
            throw;
        }
    }

    private static string GetStrategyKey(string strategyName, string version) => $"{strategyName}:{version}";
} 
