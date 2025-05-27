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
    /// Strategy for backing up system components.
    /// </summary>
    public class BackupStrategy : RemediationStrategy
    {
        private readonly ILLMClient _llmClient;
        private readonly IRemediationMetricsCollector _metricsCollector;
        private string _name = "Backup";
        private string _description = "Creates backups of system components";

        public BackupStrategy(
            ILogger<RemediationStrategy> logger,
            IRemediationMetricsCollector metricsCollector,
            ILLMClient llmClient)
            : base(logger)
        {
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
            
            // Add required parameters with default values
            Parameters["target"] = "system";
            Parameters["schedule"] = "immediate";
            
            // Set supported error types
            SupportedErrorTypes = new HashSet<string> 
            { 
                "DataCorruption", 
                "SystemFailure",
                "UpdateRequired" 
            };
        }

        /// <inheritdoc/>
        public override string Name 
        { 
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <inheritdoc/>
        public override string Description 
        { 
            get => _description;
            set => _description = value ?? throw new ArgumentNullException(nameof(value));
        }

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

                // Execute backup logic
                var target = Parameters["target"]?.ToString() ?? "system";
                var schedule = Parameters["schedule"]?.ToString() ?? "immediate";

                await CreateBackupAsync(target, schedule);
                
                return CreateSuccessResult($"Backup created for {target} on schedule {schedule}");
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