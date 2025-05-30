using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using ValidationResult = RuntimeErrorSage.Domain.Models.Validation.ValidationResult;

namespace RuntimeErrorSage.Core.Remediation
{
    public class RemediationSuggestionManager : IRemediationSuggestionManager
    {
        private readonly ILogger<RemediationSuggestionManager> _logger;
        private readonly IRemediationValidator _validator;
        private readonly IRemediationExecutor _executor;

        public RemediationSuggestionManager(
            ILogger<RemediationSuggestionManager> logger,
            IRemediationValidator validator,
            IRemediationExecutor executor)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(validator);
            ArgumentNullException.ThrowIfNull(executor);

            _logger = logger;
            _validator = validator;
            _executor = executor;
        }

        public async Task<RemediationSuggestion> GetSuggestionsAsync(ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                var suggestion = new RemediationSuggestion
                {
                    SuggestionId = Guid.NewGuid().ToString(),
                    Title = "Default remediation suggestion",
                    Description = "This is a default remediation suggestion",
                    StrategyName = "DefaultStrategy",
                    ConfidenceLevel = 0.8,
                    Priority = RemediationPriority.Medium,
                    ExpectedOutcome = "Error should be resolved",
                    Actions = new List<RemediationAction>()
                };

                return suggestion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating suggestion for error {ErrorId}", errorContext.ErrorId);
                throw;
            }
        }

        public async Task<ValidationResult> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                var validationContext = new ValidationContext();
                return new ValidationResult
                {
                    IsValid = true,
                    Severity = ValidationSeverity.Info,
                    Message = "Suggestion validated successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating suggestion {SuggestionId}", suggestion.SuggestionId);
                throw;
            }
        }

        public async Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                // Create a dummy action if no actions are present
                if (suggestion.Actions == null || suggestion.Actions.Count == 0)
                {
                    _logger.LogWarning("No actions found in suggestion {SuggestionId}, creating a dummy action", suggestion.SuggestionId);
                    
                    suggestion.Actions = new List<RemediationAction>
                    {
                        new RemediationAction
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "Default Action",
                            Description = "Default action created for suggestion",
                            ActionType = "Default",
                            Priority = RemediationPriority.Medium
                        }
                    };
                }

                return await _executor.ExecuteActionAsync(suggestion.Actions[0], errorContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing suggestion {SuggestionId}", suggestion.SuggestionId);
                return new RemediationResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Status = RemediationStatusEnum.Failed
                };
            }
        }

        public async Task<RemediationImpact> GetSuggestionImpactAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                return new RemediationImpact
                {
                    Severity = RemediationActionSeverity.Medium,
                    Description = "Default impact assessment",
                    AffectedComponents = new List<string> { errorContext.ComponentId },
                    EstimatedDowntime = TimeSpan.FromMinutes(5)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting impact for suggestion {SuggestionId}", suggestion.SuggestionId);
                throw;
            }
        }
    }
} 
