using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.Interfaces;

namespace RuntimeErrorSage.Application.Remediation.Strategies
{
    /// <summary>
    /// Strategy for alerting about errors.
    /// </summary>
    public class AlertStrategy : IRemediationStrategy
    {
        private readonly ILogger<AlertStrategy> _logger;
        private readonly ILLMClient _llmClient;
        private readonly IRemediationMetricsCollector _metricsCollector;
        
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Alert";
        public RemediationPriority Priority { get; set; } = RemediationPriority.High;
        public string Description { get; set; } = "Sends alerts for error conditions";
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
        public ISet<string> SupportedErrorTypes { get; } = new HashSet<string> 
        { 
            "CriticalFailure", 
            "ServiceUnavailable", 
            "SecurityViolation" 
        };

        public AlertStrategy(
            ILogger<AlertStrategy> logger,
            IRemediationMetricsCollector metricsCollector,
            ILLMClient llmClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
            
            // Add required parameters with default values
            Parameters["channel"] = "default";
            Parameters["severity"] = "medium";
        }

        public async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                await ValidateConfigurationAsync();

                // Execute alert logic
                var channel = Parameters["channel"]?.ToString() ?? "default";
                var severity = Parameters["severity"]?.ToString() ?? "medium";

                await SendAlertAsync(channel, severity, context);
                
                var result = new RemediationResult
                {
                    IsSuccessful = true,
                    Message = $"Alert sent to {channel} with severity {severity}",
                    ErrorId = context.ErrorId
                };
                
                // Add strategy information to metadata
                result.Metadata["StrategyId"] = Id;
                result.Metadata["StrategyName"] = Name;
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing alert strategy: {ex.Message}");
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

        private async Task SendAlertAsync(string channel, string severity, ErrorContext errorContext)
        {
            try
            {
                var request = new LLM.LLMRequest
                {
                    Query = "Generate alert for error",
                    Context = $"Error: {errorContext.ErrorType}, Component: {errorContext.ComponentId}, Severity: {severity}"
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
                        ErrorType = errorContext.ErrorType,
                        Timestamp = DateTime.UtcNow
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending alert: {ex.Message}");
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
            result.Metadata["StrategyId"] = Id;
            result.Metadata["StrategyName"] = Name;
            
            return result;
        }

        public async Task<bool> CanApplyAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return SupportedErrorTypes.Contains(context.ErrorType) || SupportedErrorTypes.Count == 0;
        }

        public async Task<RemediationImpact> GetImpactAsync(ErrorContext context)
        {
            return new RemediationImpact
            {
                Severity = ImpactSeverity.Low,
                Scope = ImpactScope.Service,
                Description = "Alert notification only - no system changes",
                AffectedUsers = 0,
                RequiresApproval = false,
                ConfidenceLevel = 0.95
            };
        }

        public async Task<RiskAssessmentModel> GetRiskAsync(ErrorContext context)
        {
            return new RiskAssessmentModel
            {
                RiskLevel = RemediationRiskLevel.Low,
                Description = "Low risk - alert notification only",
                PotentialIssues = new List<string> { "Alert might not reach intended recipients" },
                MitigationStrategies = new List<string> { "Verify alert channel configuration" },
                ConfidenceLevel = 0.95,
                IsRollbackable = false
            };
        }

        public async Task<bool> ValidateConfigurationAsync()
        {
            try
            {
                await ValidateRequiredParametersAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Configuration validation failed: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<ValidationResult> ValidateAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var result = new ValidationResult
            {
                IsValid = true,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                if (!await ValidateConfigurationAsync())
                {
                    result.IsValid = false;
                    result.Errors.Add("Strategy configuration is invalid");
                    result.Severity = ValidationSeverity.Error;
                    return result;
                }

                if (!await CanApplyAsync(context))
                {
                    result.IsValid = false;
                    result.Errors.Add($"Error type '{context.ErrorType}' is not supported by this strategy");
                    result.Severity = ValidationSeverity.Error;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add($"Validation failed: {ex.Message}");
                result.Severity = ValidationSeverity.Error;
                _logger.LogError(ex, "Error validating alert strategy for context {ErrorId}", context.ErrorId);
                return result;
            }
        }

        private RemediationImpact CreateDefaultImpact()
        {
            return new RemediationImpact
            {
                Severity = SeverityLevel.Low.ToImpactSeverity(),
                Scope = RemediationActionImpactScope.Local,
                AffectedComponents = new List<string>(),
                EstimatedDuration = TimeSpan.FromMinutes(5)
            };
        }

        private void AddValidationResult(ValidationResult result, string message, SeverityLevel severity)
        {
            result.AddError(message, severity.ToValidationSeverity());
        }

        private void AddValidationWarning(ValidationResult result, string message, SeverityLevel severity)
        {
            result.AddWarning(message, severity.ToValidationSeverity());
        }
    }
} 
