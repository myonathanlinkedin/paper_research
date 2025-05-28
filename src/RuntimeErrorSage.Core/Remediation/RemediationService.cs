using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Execution;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Analysis.Interfaces;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Service for managing remediation operations.
    /// </summary>
    public class RemediationService : IRemediationService
    {
        private readonly ILogger<RemediationService> _logger;
        private readonly IRemediationPlanManager _planManager;
        private readonly IRemediationExecutor _executor;
        private readonly IRemediationMetricsCollector _metricsCollector;
        private readonly IRemediationSuggestionManager _suggestionManager;
        private readonly IRemediationActionManager _actionManager;

        public RemediationService(
            ILogger<RemediationService> logger,
            IRemediationPlanManager planManager,
            IRemediationExecutor executor,
            IRemediationMetricsCollector metricsCollector,
            IRemediationSuggestionManager suggestionManager,
            IRemediationActionManager actionManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _planManager = planManager ?? throw new ArgumentNullException(nameof(planManager));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _suggestionManager = suggestionManager ?? throw new ArgumentNullException(nameof(suggestionManager));
            _actionManager = actionManager ?? throw new ArgumentNullException(nameof(actionManager));
        }

        /// <inheritdoc/>
        public bool IsEnabled => true;

        /// <inheritdoc/>
        public string Name => "RuntimeErrorSage Remediation Service";

        /// <inheritdoc/>
        public string Version => "1.0.0";

        public async Task<RemediationResult> ApplyRemediationAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                // Create and validate plan
                var plan = await _planManager.CreatePlanAsync(context);
                if (!await _planManager.ValidatePlanAsync(plan))
                {
                    return new RemediationResult
                    {
                        Success = false,
                        ErrorMessage = "Plan validation failed",
                        Status = RemediationStatusEnum.Failed
                    };
                }

                // Record initial metrics
                var metrics = await _metricsCollector.InitializeMetricsAsync(context);

                // Execute remediation
                await _metricsCollector.UpdateMetricsStatusAsync(metrics.MetricsId, RemediationStatusEnum.Executing);
                var execution = await _executor.ExecuteRemediationAsync(plan, context);

                // Record final metrics
                await _metricsCollector.FinalizeMetricsAsync(metrics.MetricsId, execution);

                return new RemediationResult
                {
                    RemediationId = execution.RemediationId,
                    Success = execution.Success,
                    ErrorMessage = execution.ErrorMessage,
                    Status = execution.Status,
                    Metrics = await _metricsCollector.GetMetricsAsync(execution.RemediationId)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying remediation for error {ErrorType}", context.ErrorType);
                return new RemediationResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Status = RemediationStatusEnum.Failed
                };
            }
        }

        public async Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
        {
            return await _planManager.CreatePlanAsync(context);
        }

        public async Task<bool> ValidatePlanAsync(RemediationPlan plan)
        {
            return await _planManager.ValidatePlanAsync(plan);
        }

        public async Task<RemediationStatusEnum> GetStatusAsync(string remediationId)
        {
            return await _executor.GetRemediationStatusAsync(remediationId);
        }

        public async Task<RemediationMetrics> GetMetricsAsync(string remediationId)
        {
            return await _metricsCollector.GetMetricsAsync(remediationId);
        }

        public async Task<RemediationSuggestion> GetRemediationSuggestionsAsync(ErrorContext errorContext)
        {
            return await _suggestionManager.GetSuggestionsAsync(errorContext);
        }

        public async Task<ValidationResult> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            return await _suggestionManager.ValidateSuggestionAsync(suggestion, errorContext);
        }

        public async Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            return await _suggestionManager.ExecuteSuggestionAsync(suggestion, errorContext);
        }

        public async Task<RemediationImpact> GetSuggestionImpactAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            return await _suggestionManager.GetSuggestionImpactAsync(suggestion, errorContext);
        }

        public async Task<RemediationResult> ExecuteActionAsync(RemediationAction action)
        {
            return await _actionManager.ExecuteActionAsync(action);
        }

        public async Task<ValidationResult> ValidateActionAsync(RemediationAction action)
        {
            return await _actionManager.ValidateActionAsync(action);
        }

        public async Task<RollbackStatus> RollbackActionAsync(string actionId)
        {
            return await _actionManager.RollbackActionAsync(actionId);
        }

        public async Task<RemediationResult> GetActionStatusAsync(string actionId)
        {
            return await _actionManager.GetActionStatusAsync(actionId);
        }
    }
} 