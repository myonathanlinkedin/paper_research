using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Remediation.Strategies
{
    /// <summary>
    /// Strategy for monitoring system components.
    /// </summary>
    public class MonitorStrategy : RemediationStrategy
    {
        public MonitorStrategy(
            ILogger<RemediationStrategy> logger,
            IRemediationValidator validator)
            : base(logger, validator, "Monitor", "Monitors system components for health and performance", 1)
        {
        }

        public override async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                await ValidateParametersAsync();

                // Validate strategy before execution
                var validationResult = await ValidateAsync(context);
                if (!validationResult.IsValid)
                {
                    return await CreateFailureResultAsync(
                        $"Strategy validation failed: {string.Join(", ", validationResult.Errors)}");
                }

                // Execute monitoring logic
                var metric = Parameters["metric"];
                var threshold = double.Parse(Parameters["threshold"]);

                // Simulate monitoring check
                var isHealthy = await CheckMetricHealthAsync(metric, threshold);
                
                return await CreateSuccessResultAsync(
                    $"Monitoring check completed for metric '{metric}'. Health status: {(isHealthy ? "Healthy" : "Unhealthy")}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing monitor strategy");
                return await CreateFailureResultAsync("Failed to execute monitor strategy", ex);
            }
        }

        protected override IEnumerable<string> GetRequiredParameters()
        {
            return new[] { "metric", "threshold" };
        }

        private async Task<bool> CheckMetricHealthAsync(string metric, double threshold)
        {
            // Simulate metric check
            await Task.Delay(100);
            return new Random().NextDouble() < 0.9; // 90% chance of being healthy
        }
    }
} 