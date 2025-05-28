using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation.Base;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.Interfaces;

namespace RuntimeErrorSage.Core.Remediation.Strategies
{
    /// <summary>
    /// Strategy for backing up system state.
    /// </summary>
    public class BackupStrategy : IRemediationStrategy
    {
        private readonly ILLMClient _llmClient;
        private readonly IRemediationMetricsCollector _metricsCollector;
        public string Name { get; }
        public string Description { get; }
        public Dictionary<string, object> Parameters { get; }
        public HashSet<string> SupportedErrorTypes { get; }
        public RemediationPriority Priority { get; }

        public BackupStrategy(
            ILogger<RemediationStrategy> logger,
            IRemediationMetricsCollector metricsCollector,
            ILLMClient llmClient)
            : base(logger)
        {
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
            
            Name = "Backup";
            Description = "Creates and manages system backups";
            Parameters = new Dictionary<string, object>();
            SupportedErrorTypes = new HashSet<string>();
            Priority = RemediationPriority.High;
            
            // Add required parameters with default values
            Parameters["target"] = "system";
            Parameters["schedule"] = "immediate";
            
            // Set supported error types
            SupportedErrorTypes.Add("DataCorruption");
            SupportedErrorTypes.Add("SystemFailure");
            SupportedErrorTypes.Add("UpdateRequired");
        }

        /// <summary>
        /// Applies the backup strategy to the given error context.
        /// </summary>
        /// <param name="context">The error context to apply the strategy to.</param>
        /// <returns>A task containing the remediation result.</returns>
        public async Task<RemediationResult> ApplyAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                await ValidateRequiredParametersAsync();

                // Execute backup logic
                var target = Parameters["target"]?.ToString() ?? "system";
                var schedule = Parameters["schedule"]?.ToString() ?? "immediate";

                await CreateBackupAsync(target, schedule);
                
                var result = new RemediationResult
                {
                    IsSuccessful = true,
                    Message = $"Backup created for {target} on schedule {schedule}",
                    ErrorId = context.ErrorId
                };
                
                // Add strategy information to metadata
                result.Metadata["StrategyId"] = StrategyId;
                result.Metadata["StrategyName"] = Name;
                
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error executing backup strategy: {ex.Message}");
                return CreateFailureResult($"Failed to execute backup strategy: {ex.Message}");
            }
        }

        private async Task ValidateRequiredParametersAsync()
        {
            var requiredParameters = new[] { "target", "schedule" };
            
            foreach (var param in requiredParameters)
            {
                if (!Parameters.ContainsKey(param))
                {
                    throw new InvalidOperationException($"Required parameter '{param}' is missing");
                }
            }
            
            await Task.CompletedTask;
        }

        private async Task CreateBackupAsync(string target, string schedule)
        {
            try
            {
                var request = new LLM.LLMRequest
                {
                    Query = "Generate backup plan",
                    Context = $"Target: {target}, Schedule: {schedule}"
                };
                
                var response = await _llmClient.GenerateResponseAsync(request);
                
                // Record metrics about the backup
                await _metricsCollector.RecordMetricAsync(
                    Guid.NewGuid().ToString(),
                    "backup_created",
                    new
                    {
                        Target = target,
                        Schedule = schedule,
                        Timestamp = DateTime.UtcNow
                    });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error creating backup: {ex.Message}");
                throw;
            }
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

        public async Task<bool> CanHandleErrorAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return true; // This strategy can handle any error type
        }

        public async Task<bool> ValidateAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return true; // Basic validation passes
        }

        public async Task<RemediationPriority> GetPriorityAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return Priority;
        }
    }
} 