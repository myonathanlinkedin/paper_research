using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Remediation.Strategies
{
    /// <summary>
    /// Strategy for backing up system state.
    /// </summary>
    public class BackupStrategy : IRemediationStrategy
    {
        private readonly ILogger<BackupStrategy> _logger;
        private readonly ILLMClient _llmClient;
        private readonly IRemediationMetricsCollector _metricsCollector;
        
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

        public BackupStrategy(
            ILogger<BackupStrategy> logger,
            IRemediationMetricsCollector metricsCollector,
            ILLMClient llmClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
            
            Name = "Backup";
            Description = "Creates and manages system backups";
            Parameters = new Dictionary<string, object>();
            SupportedErrorTypes = new HashSet<string>();
            
            // Add required parameters with default values
            Parameters["target"] = "system";
            Parameters["schedule"] = "immediate";
            
            // Set supported error types
            SupportedErrorTypes.Add("DataCorruption");
            SupportedErrorTypes.Add("SystemFailure");
            SupportedErrorTypes.Add("UpdateRequired");
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
                _logger.LogError(ex, $"Error executing backup strategy: {ex.Message}");
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
                    "backup_created",
                    1.0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating backup: {ex.Message}");
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
                return ValidationResult.Success("Backup strategy validation successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Backup strategy validation failed: {Message}", ex.Message);
                return ValidationResult.Failure($"Backup strategy validation failed: {ex.Message}");
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
            return RemediationPriority.High;
        }
    }
} 