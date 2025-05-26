using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Remediation.Strategies
{
    /// <summary>
    /// Strategy for alerting on system issues.
    /// </summary>
    public class AlertStrategy : RemediationStrategy
    {
        public AlertStrategy(
            ILogger<RemediationStrategy> logger,
            IRemediationValidator validator)
            : base(logger, validator, "Alert", "Sends alerts for system issues", 2)
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

                // Execute alert logic
                var channel = Parameters["channel"];
                var severity = Parameters["severity"];

                // Simulate alert sending
                await SendAlertAsync(channel, severity, context);
                
                return await CreateSuccessResultAsync(
                    $"Alert sent to channel '{channel}' with severity '{severity}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing alert strategy");
                return await CreateFailureResultAsync("Failed to execute alert strategy", ex);
            }
        }

        protected override IEnumerable<string> GetRequiredParameters()
        {
            return new[] { "channel", "severity" };
        }

        private async Task SendAlertAsync(string channel, string severity, ErrorContext context)
        {
            // Simulate alert sending
            await Task.Delay(100);
            _logger.LogInformation(
                "Alert sent to {Channel} with severity {Severity} for error: {ErrorType}",
                channel, severity, context.ErrorType);
        }
    }
} 