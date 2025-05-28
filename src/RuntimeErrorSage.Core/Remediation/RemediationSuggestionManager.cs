using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Analysis.Interfaces;

namespace RuntimeErrorSage.Core.Remediation
{
    public class RemediationSuggestionManager : IRemediationSuggestionManager
    {
        private readonly ILogger<RemediationSuggestionManager> _logger;
        private readonly IErrorContextAnalyzer _errorContextAnalyzer;
        private readonly IRemediationValidator _validator;
        private readonly IRemediationExecutor _executor;

        public RemediationSuggestionManager(
            ILogger<RemediationSuggestionManager> logger,
            IErrorContextAnalyzer errorContextAnalyzer,
            IRemediationValidator validator,
            IRemediationExecutor executor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _errorContextAnalyzer = errorContextAnalyzer ?? throw new ArgumentNullException(nameof(errorContextAnalyzer));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        public async Task<RemediationSuggestion> GetSuggestionsAsync(ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                var suggestions = await _errorContextAnalyzer.AnalyzeContextAsync(errorContext);
                return new RemediationSuggestion
                {
                    SuggestionId = Guid.NewGuid().ToString(),
                    Description = "Suggested remediation based on error analysis",
                    Actions = suggestions.SuggestedActions
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting remediation suggestions for error {ErrorType}", errorContext.ErrorType);
                return new RemediationSuggestion { Description = "Failed to generate suggestions" };
            }
        }

        public async Task<ValidationResult> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                var validationResult = await _validator.ValidatePlanAsync(new RemediationPlan { Actions = suggestion.Actions }, errorContext);
                return validationResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating suggestion for error {ErrorType}", errorContext.ErrorType);
                return new ValidationResult { IsValid = false, Message = ex.Message };
            }
        }

        public async Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                var plan = new RemediationPlan { Actions = suggestion.Actions };
                return await _executor.ExecuteRemediationAsync(plan, errorContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing suggestion for error {ErrorType}", errorContext.ErrorType);
                return new RemediationResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<RemediationImpact> GetSuggestionImpactAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                var impact = new RemediationImpact
                {
                    ActionId = suggestion.SuggestionId,
                    Severity = RemediationSeverity.Medium,
                    Scope = "Global",
                    AffectedComponents = new List<string>(),
                    EstimatedDuration = TimeSpan.FromMinutes(5)
                };
                return impact;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting suggestion impact for error {ErrorType}", errorContext.ErrorType);
                return new RemediationImpact { Severity = RemediationSeverity.High };
            }
        }
    }
} 