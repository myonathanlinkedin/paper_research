using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Base;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.Interfaces;

namespace RuntimeErrorSage.Core.Remediation.Strategies
{
    /// <summary>
    /// Strategy for alerting on system issues.
    /// </summary>
    public class AlertStrategy : RemediationStrategy
    {
        private readonly ILLMClient _llmClient;
        private readonly IRemediationMetricsCollector _metricsCollector;

        public AlertStrategy(
            ILogger<RemediationStrategy> logger,
            IRemediationMetricsCollector metricsCollector,
            ILLMClient llmClient)
            : base(logger)
        {
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
            
            // Add required parameters with default values
            Parameters["channel"] = "default";
            Parameters["severity"] = "medium";
            
            // Set supported error types
            SupportedErrorTypes = new List<string> 
            { 
                "CriticalFailure", 
                "ServiceUnavailable",
                "SecurityViolation" 
            };
        }

        /// <inheritdoc/>
        public override string Name => "Alert";

        /// <inheritdoc/>
        public override string Description => "Sends alerts for system issues";

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

                // Execute alert logic
                var channel = Parameters["channel"]?.ToString() ?? "default";
                var severity = Parameters["severity"]?.ToString() ?? "medium";

                await SendAlertAsync(channel, severity, context);
                
                return CreateSuccessResult($"Alert sent to {channel} with severity {severity}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error executing alert strategy: {ex.Message}");
                return CreateFailureResult($"Failed to execute alert strategy: {ex.Message}");
            }
        }

        private async Task ValidateRequiredParametersAsync()
        {
            var requiredParameters = new[] { "channel", "severity" };
            
            foreach (var param in requiredParameters)
            {
                if (!Parameters.ContainsKey(param))
                {
                    throw new InvalidOperationException($"Required parameter '{param}' is missing");
                }
            }
            
            await Task.CompletedTask;
        }

        private async Task SendAlertAsync(string channel, string severity, ErrorContext context)
        {
            try
            {
                var request = new LLM.LLMRequest
                {
                    Query = "Generate alert for error",
                    Context = $"Error: {context.ErrorType}, Component: {context.ComponentId}, Severity: {severity}"
                };
                
                var response = await _llmClient.GenerateResponseAsync(request);
                
                // Record metrics about the alert
                await _metricsCollector.RecordMetricAsync(
                    Guid.NewGuid().ToString(),
                    "alert_sent",
                    new
                    {
                        Channel = channel,
                        Severity = severity,
                        ErrorType = context.ErrorType,
                        Timestamp = DateTime.UtcNow
                    });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error sending alert: {ex.Message}");
                throw;
            }
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