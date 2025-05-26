using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Remediation.Models.Common;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Registry for managing remediation strategies and actions.
    /// </summary>
    public class RemediationRegistry : IRemediationRegistry
    {
        private readonly ILogger<RemediationRegistry> _logger;
        private readonly IErrorContextAnalyzer _errorContextAnalyzer;
        private readonly IQwenLLMClient _llmClient;
        private readonly Dictionary<string, IRemediationStrategy> _strategies;
        private readonly Dictionary<string, List<string>> _errorTypeStrategies;

        public RemediationRegistry(
            ILogger<RemediationRegistry> logger,
            IErrorContextAnalyzer errorContextAnalyzer,
            IQwenLLMClient llmClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _errorContextAnalyzer = errorContextAnalyzer ?? throw new ArgumentNullException(nameof(errorContextAnalyzer));
            _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
            _strategies = new Dictionary<string, IRemediationStrategy>(StringComparer.OrdinalIgnoreCase);
            _errorTypeStrategies = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        }

        public async Task RegisterStrategyAsync(IRemediationStrategy strategy, IEnumerable<string> errorTypes)
        {
            ArgumentNullException.ThrowIfNull(strategy);
            ArgumentNullException.ThrowIfNull(errorTypes);

            try
            {
                var errorTypeList = errorTypes.ToList();
                if (!errorTypeList.Any())
                {
                    throw new ArgumentException("At least one error type must be specified", nameof(errorTypes));
                }

                if (string.IsNullOrEmpty(strategy.Name))
                {
                    throw new ArgumentException("Strategy name cannot be null or empty", nameof(strategy));
                }

                if (_strategies.ContainsKey(strategy.Name))
                {
                    throw new InvalidOperationException($"Strategy with name '{strategy.Name}' is already registered");
                }

                _strategies[strategy.Name] = strategy;

                foreach (var errorType in errorTypeList)
                {
                    if (!_errorTypeStrategies.ContainsKey(errorType))
                    {
                        _errorTypeStrategies[errorType] = new List<string>();
                    }
                    _errorTypeStrategies[errorType].Add(strategy.Name);
                }

                _logger.LogInformation(
                    "Registered strategy '{StrategyName}' for error types: {ErrorTypes}",
                    strategy.Name,
                    string.Join(", ", errorTypeList));

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering strategy {StrategyName}", strategy.Name);
                throw;
            }
        }

        public async Task<IRemediationStrategy?> GetStrategyAsync(string strategyName)
        {
            if (string.IsNullOrEmpty(strategyName))
            {
                throw new ArgumentException("Strategy name cannot be null or empty", nameof(strategyName));
            }

            try
            {
                if (_strategies.TryGetValue(strategyName, out var strategy))
                {
                    _logger.LogDebug("Retrieved strategy '{StrategyName}'", strategyName);
                    return await Task.FromResult(strategy);
                }

                _logger.LogWarning("Strategy '{StrategyName}' not found", strategyName);
                return await Task.FromResult<IRemediationStrategy?>(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving strategy {StrategyName}", strategyName);
                throw;
            }
        }

        public async Task<IEnumerable<IRemediationStrategy>> GetStrategiesForErrorTypeAsync(string errorType)
        {
            if (string.IsNullOrEmpty(errorType))
            {
                throw new ArgumentException("Error type cannot be null or empty", nameof(errorType));
            }

            try
            {
                if (_errorTypeStrategies.TryGetValue(errorType, out var strategyNames))
                {
                    var strategies = strategyNames
                        .Select(name => _strategies[name])
                        .OrderByDescending(s => s.Priority)
                        .ToList();

                    _logger.LogDebug(
                        "Retrieved {Count} strategies for error type '{ErrorType}'",
                        strategies.Count,
                        errorType);

                    return await Task.FromResult(strategies);
                }

                _logger.LogWarning("No strategies found for error type '{ErrorType}'", errorType);
                return await Task.FromResult(Enumerable.Empty<IRemediationStrategy>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving strategies for error type {ErrorType}", errorType);
                throw;
            }
        }

        public async Task<IEnumerable<IRemediationStrategy>> GetAllStrategiesAsync()
        {
            try
            {
                var strategies = _strategies.Values
                    .OrderByDescending(s => s.Priority)
                    .ToList();

                _logger.LogDebug("Retrieved all {Count} strategies", strategies.Count);
                return await Task.FromResult(strategies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all strategies");
                throw;
            }
        }

        public async Task<bool> UnregisterStrategyAsync(string strategyName)
        {
            if (string.IsNullOrEmpty(strategyName))
            {
                throw new ArgumentException("Strategy name cannot be null or empty", nameof(strategyName));
            }

            try
            {
                if (!_strategies.ContainsKey(strategyName))
                {
                    _logger.LogWarning("Strategy '{StrategyName}' not found for unregistration", strategyName);
                    return await Task.FromResult(false);
                }

                // Remove strategy from error type mappings
                foreach (var errorType in _errorTypeStrategies.Keys.ToList())
                {
                    _errorTypeStrategies[errorType].Remove(strategyName);
                    if (!_errorTypeStrategies[errorType].Any())
                    {
                        _errorTypeStrategies.Remove(errorType);
                    }
                }

                // Remove strategy
                _strategies.Remove(strategyName);

                _logger.LogInformation("Unregistered strategy '{StrategyName}'", strategyName);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unregistering strategy {StrategyName}", strategyName);
                throw;
            }
        }

        public async Task<bool> HasStrategyAsync(string strategyName)
        {
            if (string.IsNullOrEmpty(strategyName))
            {
                throw new ArgumentException("Strategy name cannot be null or empty", nameof(strategyName));
            }

            try
            {
                var hasStrategy = _strategies.ContainsKey(strategyName);
                _logger.LogDebug(
                    "Strategy '{StrategyName}' {Status}",
                    strategyName,
                    hasStrategy ? "exists" : "does not exist");
                return await Task.FromResult(hasStrategy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking strategy existence {StrategyName}", strategyName);
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetRegisteredErrorTypesAsync()
        {
            try
            {
                var errorTypes = _errorTypeStrategies.Keys.ToList();
                _logger.LogDebug("Retrieved {Count} registered error types", errorTypes.Count);
                return await Task.FromResult(errorTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving registered error types");
                throw;
            }
        }
    }
} 