using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Models.Common;
using RuntimeErrorSage.Core.Remediation.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Context;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Implements the remediation action system as specified in the research paper.
    /// </summary>
    public class RemediationActionSystem : IRemediationActionSystem
    {
        private readonly ILLMIntegration _llmIntegration;
        private readonly IRemediationExecutor _executor;
        private readonly IRemediationValidator _validator;
        private readonly IContextProvider _contextProvider;

        public RemediationActionSystem(
            ILLMIntegration llmIntegration,
            IRemediationExecutor executor,
            IRemediationValidator validator,
            IContextProvider contextProvider)
        {
            _llmIntegration = llmIntegration ?? throw new ArgumentNullException(nameof(llmIntegration));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _contextProvider = contextProvider ?? throw new ArgumentNullException(nameof(contextProvider));
        }

        /// <summary>
        /// Generates and validates remediation actions for a runtime error.
        /// </summary>
        public async Task<RemediationPlan> GenerateRemediationPlanAsync(RuntimeError error, ErrorContext context)
        {
            // Analyze the error using LLM
            var analysis = await _llmIntegration.AnalyzeErrorAsync(error, context);

            // Generate remediation suggestions
            var suggestions = await _llmIntegration.GenerateRemediationSuggestionsAsync(error, analysis);

            // Validate and rank suggestions
            var validatedSuggestions = new List<ValidatedRemediationSuggestion>();
            foreach (var suggestion in suggestions)
            {
                var validationResult = await _llmIntegration.ValidateRemediationAsync(suggestion, context);
                validatedSuggestions.Add(new ValidatedRemediationSuggestion
                {
                    Suggestion = suggestion,
                    ValidationResult = validationResult,
                    Score = CalculateRemediationScore(suggestion, validationResult)
                });
            }

            // Sort suggestions by score
            validatedSuggestions.Sort((a, b) => b.Score.CompareTo(a.Score));

            return new RemediationPlan
            {
                ErrorAnalysis = analysis,
                ValidatedSuggestions = validatedSuggestions,
                Context = context,
                GeneratedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Executes a remediation plan.
        /// </summary>
        public async Task<RemediationResult> ExecuteRemediationPlanAsync(RemediationPlan plan)
        {
            var result = new RemediationResult
            {
                PlanId = plan.Id,
                StartTime = DateTime.UtcNow,
                Status = RemediationStatus.InProgress,
                Actions = new List<RemediationAction>()
            };

            try
            {
                // Execute each suggestion in order of score
                foreach (var validatedSuggestion in plan.ValidatedSuggestions)
                {
                    var action = await ExecuteRemediationActionAsync(validatedSuggestion, plan.Context);
                    result.Actions.Add(action);

                    if (!action.Success)
                    {
                        result.Status = RemediationStatus.PartiallySucceeded;
                        break;
                    }
                }

                if (result.Status != RemediationStatus.PartiallySucceeded)
                {
                    result.Status = RemediationStatus.Succeeded;
                }
            }
            catch (Exception ex)
            {
                result.Status = RemediationStatus.Failed;
                result.ErrorMessage = ex.Message;
            }

            result.EndTime = DateTime.UtcNow;
            return result;
        }

        private async Task<RemediationAction> ExecuteRemediationActionAsync(ValidatedRemediationSuggestion suggestion, ErrorContext context)
        {
            var action = new RemediationAction
            {
                SuggestionId = suggestion.Suggestion.Id,
                StartTime = DateTime.UtcNow
            };

            try
            {
                // Validate the action before execution
                var validationResult = await _validator.ValidateActionAsync(suggestion.Suggestion, context);
                if (!validationResult.IsValid)
                {
                    action.Success = false;
                    action.ErrorMessage = validationResult.ErrorMessage;
                    return action;
                }

                // Execute the remediation action
                await _executor.ExecuteActionAsync(suggestion.Suggestion, context);
                action.Success = true;

                // Update context after successful execution
                await _contextProvider.UpdateContextAsync(context.Id, new RuntimeUpdate
                {
                    Type = "RemediationAction",
                    Data = new Dictionary<string, object>
                    {
                        { "actionId", action.Id },
                        { "suggestionId", suggestion.Suggestion.Id }
                    }
                });
            }
            catch (Exception ex)
            {
                action.Success = false;
                action.ErrorMessage = ex.Message;
            }

            action.EndTime = DateTime.UtcNow;
            return action;
        }

        private double CalculateRemediationScore(RemediationSuggestion suggestion, RemediationValidationResult validation)
        {
            // Implement scoring logic based on:
            // 1. Effectiveness score from validation
            // 2. Implementation complexity
            // 3. Risk assessment
            // 4. Expected success probability
            return validation.EffectivenessScore * 0.4 +
                   (1.0 - suggestion.ComplexityScore) * 0.3 +
                   (1.0 - validation.RiskScore) * 0.2 +
                   validation.SuccessProbability * 0.1;
        }
    }
} 