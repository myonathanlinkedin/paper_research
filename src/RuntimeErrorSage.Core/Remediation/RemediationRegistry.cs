using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Graph;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.Analysis.Interfaces;

namespace RuntimeErrorSage.Application.Remediation
{
    /// <summary>
    /// Registry for managing remediation strategies and actions.
    /// </summary>
    public class RemediationRegistry : IRemediationRegistry
    {
        private readonly ILogger<RemediationRegistry> _logger;
        private readonly IErrorContextAnalyzer _errorContextAnalyzer;
        private readonly ILLMClient _llmClient;
        private readonly Dictionary<string, IRemediationStrategy> _strategies;
        private readonly Dictionary<string, Collection<string>> _errorTypeStrategies;

        public bool IsEnabled { get; } = true;
        public string Name { get; } = "RemediationRegistry";
        public string Version { get; } = "1.0.0";

        public RemediationRegistry(
            ILogger<RemediationRegistry> logger,
            IErrorContextAnalyzer errorContextAnalyzer,
            ILLMClient llmClient)
        {
            _logger = logger ?? ArgumentNullException.ThrowIfNull(nameof(logger));
            _errorContextAnalyzer = errorContextAnalyzer ?? ArgumentNullException.ThrowIfNull(nameof(errorContextAnalyzer));
            _llmClient = llmClient ?? ArgumentNullException.ThrowIfNull(nameof(llmClient));
            _strategies = new Dictionary<string, IRemediationStrategy>(StringComparer.OrdinalIgnoreCase);
            _errorTypeStrategies = new Dictionary<string, Collection<string>>(StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public async Task RegisterStrategyAsync(IRemediationStrategy strategy)
        {
            ArgumentNullException.ThrowIfNull(strategy);

            try
            {
                if (string.IsNullOrEmpty(strategy.Name))
                {
                    throw new ArgumentException("Strategy name cannot be null or empty", nameof(strategy));
                }

                if (_strategies.ContainsKey(strategy.Name))
                {
                    throw new InvalidOperationException($"Strategy with name '{strategy.Name}' is already registered");
                }

                _strategies[strategy.Name] = strategy;

                // Register strategy for its supported error types
                foreach (var errorType in strategy.SupportedErrorTypes)
                {
                    if (!_errorTypeStrategies.ContainsKey(errorType))
                    {
                        _errorTypeStrategies[errorType] = new Collection<string>();
                    }
                    _errorTypeStrategies[errorType].Add(strategy.Name);
                }

                _logger.LogInformation(
                    "Registered strategy '{StrategyName}' for error types: {ErrorTypes}",
                    strategy.Name,
                    string.Join(", ", strategy.SupportedErrorTypes));

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
                if (!_strategies.ContainsKey(strategyName))
                {
                    throw new KeyNotFoundException($"Strategy '{strategyName}' is not registered");
                }

                var strategy = _strategies[strategyName];
                _strategies.Remove(strategyName);

                // Remove strategy from error type mappings
                foreach (var errorType in strategy.SupportedErrorTypes)
                {
                    if (_errorTypeStrategies.TryGetValue(errorType, out var strategies))
                    {
                        strategies.Remove(strategyName);
                        if (!strategies.Any())
                        {
                            _errorTypeStrategies.Remove(errorType);
                        }
                    }
                }

                _logger.LogInformation("Unregistered strategy '{StrategyName}'", strategyName);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unregistering strategy {StrategyName}", strategyName);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IRemediationStrategy>> GetStrategiesAsync()
        {
            try
            {
                var strategies = _strategies.Values.ToList();
                // Order by priority - need to call GetPriorityAsync for each strategy
                // This is a temporary workaround until we have proper synchronous Priority property
                var orderedStrategies = new Collection<IRemediationStrategy>();
                foreach (var strategy in strategies)
                {
                    orderedStrategies.Add(strategy);
                }
                orderedStrategies = orderedStrategies.OrderByDescending(s => 5).ToList(); // Default priority 5

                _logger.LogDebug("Retrieved all {Count} strategies", strategies.Count);
                return strategies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all strategies");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IRemediationStrategy>> GetStrategiesForErrorAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                if (_errorTypeStrategies.TryGetValue(context.ErrorType, out var strategyNames))
                {
                    var strategies = strategyNames
                        .Select(name => _strategies[name])
                        .OrderByDescending(s => s.Priority)
                        .ToList();

                    _logger.LogDebug(
                        "Retrieved {Count} strategies for error type '{ErrorType}'",
                        strategies.Count,
                        context.ErrorType);

                    return strategies;
                }

                _logger.LogWarning("No strategies found for error type '{ErrorType}'", context.ErrorType);
                return Enumerable.Empty<IRemediationStrategy>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving strategies for error type {ErrorType}", context.ErrorType);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IRemediationStrategy> GetStrategyAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Strategy id cannot be null or empty", nameof(id));
            }

            try
            {
                if (_strategies.TryGetValue(id, out var strategy))
                {
                    _logger.LogDebug("Retrieved strategy '{StrategyId}'", id);
                    return strategy;
                }

                _logger.LogWarning("Strategy '{StrategyId}' not found", id);
                throw new KeyNotFoundException($"Strategy '{id}' is not registered");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving strategy {StrategyId}", id);
                throw;
            }
        }

        /// <inheritdoc />
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

                    return strategies;
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

                    return strategies;
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
        public async Task<bool> IsStrategyRegisteredAsync(string strategyName)
        {
            if (string.IsNullOrEmpty(strategyName))
            {
                throw new ArgumentException("Strategy name cannot be null or empty", nameof(strategyName));
            }

            try
            {
                var result = _strategies.ContainsKey(strategyName);
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

        public async Task<IEnumerable<string>> GetRegisteredErrorTypesAsync()
        {
            try
            {
                var errorTypes = _errorTypeStrategies.Keys.ToList();
                _logger.LogDebug("Retrieved {Count} registered error types", errorTypes.Count);
                return errorTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving registered error types");
                throw;
            }
        }

        public async Task<IEnumerable<IRemediationStrategy>> GetStrategiesByPriorityAsync(int minPriority)
        {
            try
            {
                var strategies = _strategies.Values
                    .Where(s => s.Priority >= minPriority)
                    .OrderByDescending(s => s.Priority)
                    .ToList();

                _logger.LogDebug("Retrieved {Count} strategies with priority >= {MinPriority}", strategies.Count, minPriority);
                return strategies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving strategies by priority");
                throw;
            }
        }

        public async Task<IEnumerable<IRemediationStrategy>> GetStrategiesByErrorTypeAsync(string errorType)
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

                    return strategies;
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

        /// <inheritdoc/>
        public async Task<IEnumerable<IRemediationStrategy>> GetAllStrategiesAsync()
        {
            try
            {
                var strategies = _strategies.Values.ToList();
                
                _logger.LogDebug("Retrieved all {Count} strategies", strategies.Count);
                return strategies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all strategies");
                throw;
            }
        }
    }
} 





