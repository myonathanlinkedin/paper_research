using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Enums;
using RemediationActionExecution = RuntimeErrorSage.Domain.Models.Execution.RemediationActionExecution;

namespace RuntimeErrorSage.Core.Remediation.Mapping
{
    /// <summary>
    /// Implementation of IRemediationModelMapper that converts between remediation model types.
    /// </summary>
    public class RemediationModelMapper : IRemediationModelMapper
    {
        private readonly IValidationRuleProvider _validationRuleProvider;

        public RemediationModelMapper(IValidationRuleProvider validationRuleProvider)
        {
            _validationRuleProvider = validationRuleProvider ?? throw new ArgumentNullException(nameof(validationRuleProvider));
        }

        /// <inheritdoc/>
        public RemediationAction ToAction(
            RemediationSuggestion suggestion, 
            ErrorContext context, 
            IRemediationStrategyMapper strategyMapper)
        {
            if (suggestion == null)
                throw new ArgumentNullException(nameof(suggestion));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (strategyMapper == null)
                throw new ArgumentNullException(nameof(strategyMapper));

            var action = new RemediationAction(_validationRuleProvider)
            {
                ActionId = Guid.NewGuid().ToString(),
                ErrorContext = context,
                Description = suggestion.Description,
                Parameters = suggestion.Parameters ?? new Dictionary<string, object>(),
                CreatedAt = DateTime.UtcNow
            };

            // Convert strategy if available
            if (suggestion.Strategies?.Any() == true)
            {
                // Assuming strategies is List<string>, we need to get the actual strategy
                // This is a simplified version - actual implementation may need strategy lookup
                action.Strategy = null; // Will be set by caller if needed
            }

            return action;
        }

        /// <inheritdoc/>
        public RemediationResult ToResult(RemediationActionExecution execution, ErrorContext context)
        {
            if (execution == null)
                throw new ArgumentNullException(nameof(execution));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            // Convert execution status string to result status enum
            // Execution.RemediationActionExecution uses string Status, so we need to parse it
            var status = ParseStatusString(execution.Status);

            return new RemediationResult
            {
                Context = context,
                Status = status,
                Message = execution.ErrorMessage ?? "Execution completed",
                ErrorMessage = execution.ErrorMessage ?? string.Empty,
                StartTime = execution.StartTime,
                EndTime = execution.EndTime ?? DateTime.UtcNow
            };
        }

        /// <inheritdoc/>
        public RemediationPlan ToPlan(
            string planId, 
            string name, 
            List<RemediationAction> actions, 
            ErrorContext context)
        {
            if (string.IsNullOrWhiteSpace(planId))
                throw new ArgumentException("Plan ID cannot be null or empty", nameof(planId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Plan name cannot be null or empty", nameof(name));
            if (actions == null)
                throw new ArgumentNullException(nameof(actions));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var plan = new RemediationPlan(
                name: name,
                description: $"Remediation plan for error {context.ErrorId}",
                actions: actions,
                parameters: new Dictionary<string, object>
                {
                    { "ErrorId", context.ErrorId },
                    { "ErrorType", context.ErrorType },
                    { "CorrelationId", context.CorrelationId }
                },
                estimatedDuration: CalculateEstimatedDuration(actions)
            );
            
            // Set PlanId separately since it's not in constructor
            plan.PlanId = planId;
            plan.Metadata = new Dictionary<string, object>
            {
                { "ErrorId", context.ErrorId },
                { "ErrorType", context.ErrorType },
                { "CorrelationId", context.CorrelationId }
            };
            
            return plan;
        }

        private TimeSpan CalculateEstimatedDuration(List<RemediationAction> actions)
        {
            if (actions == null || !actions.Any())
                return TimeSpan.Zero;

            // Sum up estimated durations, defaulting to 1 minute per action if not specified
            // EstimatedDuration is in _core, so we need to access it differently or use default
            var totalMinutes = actions.Sum(a => 
            {
                // Try to get EstimatedDuration from _core if available, otherwise default to 1 minute
                // Since EstimatedDuration is in RemediationActionCore, we'll use a default
                return 1.0; // Default to 1 minute per action
            });

            return TimeSpan.FromMinutes(totalMinutes);
        }

        private RemediationStatusEnum ParseStatusString(string statusString)
        {
            if (string.IsNullOrWhiteSpace(statusString))
                return RemediationStatusEnum.Failed;

            // Try to parse as enum first
            if (Enum.TryParse<RemediationStatusEnum>(statusString, true, out var status))
                return status;

            // Fallback to string comparison
            return statusString.ToLowerInvariant() switch
            {
                "success" or "completed" => RemediationStatusEnum.Success,
                "failed" or "failure" => RemediationStatusEnum.Failed,
                "inprogress" or "in progress" or "running" => RemediationStatusEnum.InProgress,
                _ => RemediationStatusEnum.Failed
            };
        }
    }
}

