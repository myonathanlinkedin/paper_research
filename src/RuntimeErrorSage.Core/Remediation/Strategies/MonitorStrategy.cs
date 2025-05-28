using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Remediation.Strategies
{
    /// <summary>
    /// Strategy for monitoring system health.
    /// </summary>
    public class MonitorStrategy : IRemediationStrategy
    {
        private readonly ILogger<MonitorStrategy> _logger;
        private readonly IRemediationMetricsCollector _metricsCollector;
        private readonly ILLMClient _llmClient;
        
        /// <summary>
        /// Gets or sets the strategy name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the strategy description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the strategy parameters.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets the error types this strategy can handle.
        /// </summary>
        public ISet<string> SupportedErrorTypes { get; }

        /// <summary>
        /// Gets the strategy id.
        /// </summary>
        public string StrategyId { get; } = Guid.NewGuid().ToString();

        public MonitorStrategy(
            ILogger<MonitorStrategy> logger,
            IRemediationMetricsCollector metricsCollector,
            ILLMClient llmClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
            
            Name = "Monitor";
            Description = "Monitors system health and performance";
            Parameters = new Dictionary<string, object>();
            SupportedErrorTypes = new HashSet<string>();
            
            // Add required parameters with default values
            Parameters["cpu_threshold"] = 80.0;
            Parameters["memory_threshold"] = 85.0;
            Parameters["disk_threshold"] = 90.0;
            
            // Set supported error types
            SupportedErrorTypes.Add("ResourceExhaustion");
            SupportedErrorTypes.Add("PerformanceDegradation");
            SupportedErrorTypes.Add("SystemOverload");
        }

        /// <summary>
        /// Executes the remediation strategy.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation result.</returns>
        public async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                await ValidateRequiredParametersAsync();

                var metrics = await CollectMetricsAsync(context);
                var healthChecks = new List<bool>();

                foreach (var metric in metrics)
                {
                    if (Parameters.TryGetValue(metric.Key, out var thresholdObj) && 
                        thresholdObj is double threshold)
                    {
                        var isHealthy = await CheckMetricHealthAsync(metric.Key, threshold);
                        healthChecks.Add(isHealthy);
                    }
                }

                if (healthChecks.Any() && healthChecks.All(h => h))
                {
                    return CreateSuccessResult("All metrics are within healthy thresholds");
                }

                return CreateFailureResult("One or more metrics are outside healthy thresholds");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing monitor strategy: {ex.Message}");
                return CreateFailureResult($"Error executing monitor strategy: {ex.Message}");
            }
        }

        private async Task<Dictionary<string, double>> CollectMetricsAsync(ErrorContext context)
        {
            var metrics = new Dictionary<string, double>();
            
            try
            {
                var collectedMetrics = await _metricsCollector.CollectMetricsAsync(context);
                
                foreach (var kvp in collectedMetrics)
                {
                    if (kvp.Value is double doubleValue)
                    {
                        metrics[kvp.Key] = doubleValue;
                    }
                    else if (double.TryParse(kvp.Value?.ToString(), out var parsedValue))
                    {
                        metrics[kvp.Key] = parsedValue;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error collecting metrics: {ex.Message}");
            }
            
            return metrics;
        }

        private async Task ValidateRequiredParametersAsync()
        {
            var requiredParameters = new[] { "cpu_threshold", "memory_threshold", "disk_threshold" };
            
            foreach (var param in requiredParameters)
            {
                if (!Parameters.ContainsKey(param))
                {
                    throw new InvalidOperationException($"Required parameter '{param}' is missing");
                }
            }
            
            await Task.CompletedTask;
        }

        private async Task<bool> CheckMetricHealthAsync(string metric, double threshold)
        {
            if (metric == null)
            {
                return true;
            }
            
            try
            {
                if (double.TryParse(metric, out var metricValue))
                {
                    return metricValue <= threshold;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking metric health: {ex.Message}");
            }
            
            return true;
        }

        private RemediationResult CreateSuccessResult(string message)
        {
            var result = new RemediationResult
            {
                IsSuccessful = true,
                Message = message,
                Timestamp = DateTime.UtcNow
            };
            
            // Add strategy information to metadata
            result.Metadata["StrategyId"] = StrategyId;
            result.Metadata["StrategyName"] = Name;
            
            return result;
        }

        private RemediationResult CreateFailureResult(string message)
        {
            var result = new RemediationResult
            {
                IsSuccessful = false,
                ErrorMessage = message,
                Timestamp = DateTime.UtcNow
            };
            
            // Add strategy information to metadata
            result.Metadata["StrategyId"] = StrategyId;
            result.Metadata["StrategyName"] = Name;
            
            return result;
        }

        /// <summary>
        /// Validates if this strategy can handle the given error.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>True if the strategy can handle the error; otherwise, false.</returns>
        public async Task<bool> CanHandleErrorAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return SupportedErrorTypes.Contains(context.ErrorType);
        }

        /// <summary>
        /// Validates the strategy for a given error context.
        /// </summary>
        /// <param name="errorContext">The error context to validate against.</param>
        /// <returns>The validation result.</returns>
        public async Task<ValidationResult> ValidateAsync(ErrorContext errorContext)
        {
            if (errorContext == null)
            {
                throw new ArgumentNullException(nameof(errorContext));
            }

            try
            {
                await ValidateRequiredParametersAsync();
                return ValidationResult.Success("Monitor strategy validation successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Monitor strategy validation failed: {Message}", ex.Message);
                return ValidationResult.Failure($"Monitor strategy validation failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the priority of this strategy.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        /// <returns>The remediation priority.</returns>
        public async Task<RemediationPriority> GetPriorityAsync(ErrorContext errorContext)
        {
            if (errorContext == null)
            {
                throw new ArgumentNullException(nameof(errorContext));
            }

            await Task.CompletedTask;
            return RemediationPriority.Medium;
        }
    }
} 