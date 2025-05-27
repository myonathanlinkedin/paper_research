using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Base;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Remediation.Strategies
{
    /// <summary>
    /// Strategy for monitoring system components.
    /// </summary>
    public class MonitorStrategy : RemediationStrategy
    {
        private readonly IRemediationMetricsCollector _metricsCollector;
        private readonly ILLMClient _llmClient;

        public MonitorStrategy(
            ILogger<RemediationStrategy> logger,
            IRemediationMetricsCollector metricsCollector,
            ILLMClient llmClient)
            : base(logger)
        {
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
            
            // Add required parameters with default values
            Parameters["cpu_threshold"] = 80.0;
            Parameters["memory_threshold"] = 85.0;
            Parameters["disk_threshold"] = 90.0;
            
            // Set supported error types
            SupportedErrorTypes = new List<string> 
            { 
                "ResourceExhaustion", 
                "PerformanceDegradation",
                "SystemOverload" 
            };
        }

        /// <inheritdoc/>
        public override string Name => "Monitor";

        /// <inheritdoc/>
        public override string Description => "Monitors system components for health and performance";

        /// <inheritdoc/>
        public override async Task<RemediationResult> ApplyAsync(ErrorContext context)
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
                Logger.LogError(ex, $"Error executing monitor strategy: {ex.Message}");
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
                Logger.LogError(ex, $"Error collecting metrics: {ex.Message}");
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
                Logger.LogError(ex, $"Error checking metric health: {ex.Message}");
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
    }
} 