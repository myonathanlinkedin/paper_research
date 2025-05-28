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
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Options;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;

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
        private readonly IRemediationStrategySelector _strategySelector;
        private readonly IRemediationValidator _validator;
        private readonly Dictionary<string, IRemediationStrategy> _registeredStrategies;

        public RemediationService(
            ILogger<RemediationService> logger,
            IRemediationPlanManager planManager,
            IRemediationExecutor executor,
            IRemediationMetricsCollector metricsCollector,
            IRemediationSuggestionManager suggestionManager,
            IRemediationActionManager actionManager,
            IRemediationStrategySelector strategySelector,
            IRemediationValidator validator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _planManager = planManager ?? throw new ArgumentNullException(nameof(planManager));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _suggestionManager = suggestionManager ?? throw new ArgumentNullException(nameof(suggestionManager));
            _actionManager = actionManager ?? throw new ArgumentNullException(nameof(actionManager));
            _strategySelector = strategySelector ?? throw new ArgumentNullException(nameof(strategySelector));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _registeredStrategies = new Dictionary<string, IRemediationStrategy>();
        }

        /// <inheritdoc/>
        public bool IsEnabled => true;

        /// <inheritdoc/>
        public string Name => "RuntimeErrorSage Remediation Service";

        /// <inheritdoc/>
        public string Version => "1.0.0";

        /// <inheritdoc />
        public void RegisterStrategy(Models.Remediation.Interfaces.IRemediationStrategy strategy)
        {
            ArgumentNullException.ThrowIfNull(strategy);

            try
            {
                _logger.LogInformation("Registering remediation strategy: {StrategyName}", strategy.Name);
                
                if (string.IsNullOrEmpty(strategy.Id))
                {
                    strategy.Id = Guid.NewGuid().ToString();
                }
                
                _registeredStrategies[strategy.Id] = strategy;
                
                _logger.LogInformation("Strategy registered successfully: {StrategyName} ({StrategyId})", 
                    strategy.Name, strategy.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register strategy {StrategyName}", strategy.Name);
                throw;
            }
        }

        /// <inheritdoc />
        public void Configure(RuntimeErrorSageOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            try
            {
                _logger.LogInformation("Configuring remediation service with options");
                
                // Apply configuration options here
                // This could include configuring strategy selectors, executors, etc.
                
                _logger.LogInformation("Remediation service configured");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to configure remediation service");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> RemediateAsync(ErrorAnalysisResult analysis, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(analysis);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Remediating error for context {ContextId}", context.Id);
                
                // Create remediation plan based on analysis
                var plan = await CreatePlanAsync(context);
                
                // Apply the remediation
                return await ApplyRemediationAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemediateAsync for context {ContextId}", context.Id);
                
                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Remediation failed: {ex.Message}",
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ApplyRemediationAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Starting remediation for error context {ErrorId}", context.Id);

                // Create remediation plan
                var plan = await CreatePlanAsync(context);
                if (plan.Status != RemediationStatusEnum.Pending)
                {
                    return new RemediationResult
                    {
                        Context = context,
                        Status = plan.Status,
                        Message = "Failed to create remediation plan",
                        Validation = new ValidationResult { IsValid = false, Messages = new List<string> { "Failed to create remediation plan" } }
                    };
                }

                // Validate plan
                var validationResult = await ValidatePlanAsync(plan);
                if (!validationResult)
                {
                    return new RemediationResult
                    {
                        Context = context,
                        Status = RemediationStatusEnum.Failed,
                        Message = "Plan validation failed",
                        Validation = new ValidationResult { IsValid = false, Messages = new List<string> { "Plan validation failed" } }
                    };
                }

                // Convert model strategy to interface strategy using adapter
                var modelStrategy = plan.Strategies.FirstOrDefault();
                if (modelStrategy == null)
                {
                    return new RemediationResult
                    {
                        Context = context,
                        Status = RemediationStatusEnum.Failed,
                        Message = "No strategy found in plan",
                        Validation = new ValidationResult { IsValid = false, Messages = new List<string> { "No strategy found in plan" } }
                    };
                }

                var strategyAdapter = new RemediationStrategyAdapter(modelStrategy);
                
                // Execute plan with adapter
                var result = await _executor.ExecuteStrategyAsync(strategyAdapter, context);
                if (result.Status != RemediationStatusEnum.Completed)
                {
                    return new RemediationResult
                    {
                        Context = context,
                        Status = RemediationStatusEnum.Failed,
                        Message = result.Message,
                        CompletedSteps = result.CompletedSteps,
                        FailedSteps = result.FailedSteps,
                        Metrics = result.Metrics,
                        Validation = new ValidationResult { IsValid = false, Messages = new List<string> { result.Message } }
                    };
                }

                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.Completed,
                    Message = "Remediation completed successfully",
                    CompletedSteps = result.CompletedSteps,
                    FailedSteps = result.FailedSteps,
                    Metrics = result.Metrics,
                    Validation = new ValidationResult { IsValid = true, Messages = new List<string> { "Remediation completed successfully" } }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying remediation for context {ErrorId}", context.Id);
                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Remediation failed: {ex.Message}",
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                var strategy = await _strategySelector.SelectStrategyAsync(context);
                if (strategy == null)
                {
                    return new RemediationPlan
                    {
                        Context = context,
                        Status = RemediationStatusEnum.Failed,
                        StatusInfo = "No suitable remediation strategy found",
                        RollbackPlan = new RollbackPlan { IsAvailable = false }
                    };
                }

                return new RemediationPlan
                {
                    Context = context,
                    Strategies = new[] { strategy }.ToList(),
                    Status = RemediationStatusEnum.Pending,
                    CreatedAt = DateTime.UtcNow,
                    StatusInfo = "Plan created successfully",
                    RollbackPlan = new RollbackPlan { IsAvailable = true }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating remediation plan for context {ErrorId}", context.Id);
                return new RemediationPlan
                {
                    Context = context,
                    Status = RemediationStatusEnum.Failed,
                    StatusInfo = $"Failed to create plan: {ex.Message}",
                    RollbackPlan = new RollbackPlan { IsAvailable = false }
                };
            }
        }

        /// <inheritdoc />
        public async Task<bool> ValidatePlanAsync(RemediationPlan plan)
        {
            ArgumentNullException.ThrowIfNull(plan);

            try
            {
                var result = await _validator.ValidatePlanAsync(plan, plan.Context);
                return result.IsValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating remediation plan");
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationMetrics> GetMetricsAsync(string remediationId)
        {
            ArgumentNullException.ThrowIfNull(remediationId);

            try
            {
                var metrics = await _metricsCollector.GetMetricsAsync(remediationId);
                return metrics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting metrics for remediation {RemediationId}", remediationId);
                return new RemediationMetrics
                {
                    ExecutionId = remediationId,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Metadata = new Dictionary<string, string>()
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationStatusEnum> GetStatusAsync(string remediationId)
        {
            ArgumentNullException.ThrowIfNull(remediationId);

            try
            {
                var status = await _executor.GetRemediationStatusAsync(remediationId);
                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting status for remediation {RemediationId}", remediationId);
                return RemediationStatusEnum.Unknown;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationSuggestion> GetRemediationSuggestionsAsync(ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                _logger.LogInformation("Getting remediation suggestions for error context {ErrorId}", errorContext.Id);
                var strategy = await _strategySelector.SelectStrategyAsync(errorContext);
                
                if (strategy == null)
                {
                    return new RemediationSuggestion
                    {
                        ErrorContext = errorContext,
                        Status = SuggestionStatus.Failed,
                        Strategies = new List<string>(),
                        Message = "No applicable strategies found"
                    };
                }

                return new RemediationSuggestion
                {
                    ErrorContext = errorContext,
                    Status = SuggestionStatus.Available,
                    Strategies = new List<string> { strategy.Name },
                    ConfidenceScore = 0.8, // TODO: Get actual confidence score
                    Message = $"Suggested strategy: {strategy.Name}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting remediation suggestions for error context {ErrorId}", errorContext.Id);
                
                return new RemediationSuggestion
                {
                    ErrorContext = errorContext,
                    Status = SuggestionStatus.Failed,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        /// <inheritdoc />
        public async Task<ValidationResult> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                if (string.IsNullOrEmpty(suggestion.StrategyName))
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Messages = new List<string> { "No strategy found in suggestion" }
                    };
                }

                // Create a mock context with the strategy name to use with SelectStrategyAsync
                var mockContext = new ErrorContext 
                { 
                    ErrorType = suggestion.StrategyName,
                    CorrelationId = errorContext.CorrelationId
                };
                
                var strategy = await _strategySelector.SelectStrategyAsync(mockContext);
                if (strategy == null)
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Messages = new List<string> { "Strategy not found" }
                    };
                }

                var strategyAdapter = new RemediationStrategyAdapter(strategy);
                var result = await _validator.ValidateStrategyAsync(strategyAdapter, errorContext);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating suggestion for error context {ErrorId}", errorContext.Id);
                return new ValidationResult
                {
                    IsValid = false,
                    Messages = new List<string> { $"Error validating suggestion: {ex.Message}" }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                _logger.LogInformation("Executing remediation suggestion for error context {ErrorId}", errorContext.Id);
                
                // Validate the suggestion first
                var validationResult = await ValidateSuggestionAsync(suggestion, errorContext);
                if (!validationResult.IsValid)
                {
                    return new RemediationResult
                    {
                        Context = errorContext,
                        Status = RemediationStatusEnum.Failed,
                        Message = "Suggestion validation failed",
                        Validation = validationResult
                    };
                }

                // Create action from suggestion
                var action = new RemediationAction
                {
                    ActionId = Guid.NewGuid().ToString(),
                    ErrorContext = errorContext,
                    Description = suggestion.Description,
                    Strategy = suggestion.Strategies.FirstOrDefault() ?? "Unknown",
                    Parameters = suggestion.Parameters,
                    CreatedAt = DateTime.UtcNow
                };

                // Execute the action
                var result = await ExecuteActionAsync(action);
                result.Context = errorContext;
                
                await _metricsCollector.TrackRemediationAsync(new RemediationMetrics
                {
                    ExecutionId = action.ActionId,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Success = result.Status == RemediationStatusEnum.Completed,
                    Error = result.Status == RemediationStatusEnum.Failed ? result.Message : null,
                    Metadata = new Dictionary<string, string>
                    {
                        { "Suggestion", suggestion.Id },
                        { "ErrorId", errorContext.Id },
                        { "Status", result.Status.ToString() }
                    }
                });
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing remediation suggestion for context {ErrorId}", errorContext.Id);
                return new RemediationResult
                {
                    Context = errorContext,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Error executing remediation suggestion: {ex.Message}",
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            return await ExecuteSuggestionAsync(suggestion, suggestion.ErrorContext);
        }

        /// <inheritdoc />
        public async Task<RemediationImpact> GetSuggestionImpactAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                if (string.IsNullOrEmpty(suggestion.StrategyName))
                {
                    return new RemediationImpact
                    {
                        Severity = ImpactSeverity.Unknown,
                        Description = "No strategy found in suggestion"
                    };
                }

                _logger.LogInformation("Getting impact for remediation suggestion for error context {ErrorId}", errorContext.Id);
                
                // Create a mock context with the strategy name to use with SelectStrategyAsync
                var mockContext = new ErrorContext 
                { 
                    ErrorType = suggestion.StrategyName,
                    CorrelationId = errorContext.CorrelationId
                };
                
                var strategy = await _strategySelector.SelectStrategyAsync(mockContext);
                if (strategy == null)
                {
                    return new RemediationImpact
                    {
                        Severity = ImpactSeverity.Unknown,
                        Description = "Strategy not found"
                    };
                }

                var strategyAdapter = new RemediationStrategyAdapter(strategy);
                var impact = await strategyAdapter.GetImpactAsync(errorContext);
                return impact ?? new RemediationImpact
                {
                    Severity = ImpactSeverity.Unknown,
                    Description = "No impact information available"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting impact for suggestion for error context {ErrorId}", errorContext.Id);
                return new RemediationImpact
                {
                    Severity = ImpactSeverity.Unknown,
                    Description = $"Error getting suggestion impact: {ex.Message}"
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ExecuteActionAsync(RemediationAction action)
        {
            ArgumentNullException.ThrowIfNull(action);

            try
            {
                _logger.LogInformation("Executing remediation action {ActionId}", action.ActionId);
                
                if (action.Context == null)
                {
                    return new RemediationResult
                    {
                        Status = RemediationStatusEnum.Failed,
                        Message = "No context provided for action execution"
                    };
                }
                
                var errorContext = action.Context;
                var result = await _executor.ExecuteActionAsync(action, errorContext);
                
                return new RemediationResult
                {
                    Context = errorContext,
                    Status = result.Status == RemediationStatus.Success ? RemediationStatusEnum.Completed : RemediationStatusEnum.Failed,
                    Message = result.ErrorMessage ?? "Action executed successfully",
                    ActionId = result.ActionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing action {ActionId}", action.ActionId);
                
                return new RemediationResult
                {
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Error executing action: {ex.Message}"
                };
            }
        }

        /// <inheritdoc />
        public async Task<ValidationResult> ValidateActionAsync(RemediationAction action)
        {
            ArgumentNullException.ThrowIfNull(action);

            try
            {
                _logger.LogInformation("Validating remediation action {ActionId}", action.ActionId);
                
                if (action.Context == null)
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Messages = new List<string> { "No context provided for action validation" }
                    };
                }
                
                return await _validator.ValidateActionAsync(action, action.Context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating action {ActionId}", action.ActionId);
                
                return new ValidationResult
                {
                    IsValid = false,
                    Messages = new List<string> { $"Error validating action: {ex.Message}" }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RollbackStatus> RollbackActionAsync(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);

            try
            {
                _logger.LogInformation("Rolling back remediation action {ActionId}", actionId);
                
                var result = await _executor.RollbackRemediationAsync(actionId);
                
                // Return the enum value based on the result
                return result.Status == RemediationStatusEnum.Completed ? RollbackStatus.Completed : RollbackStatus.Failed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back action {ActionId}", actionId);
                return RollbackStatus.Failed;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> GetActionStatusAsync(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);

            try
            {
                _logger.LogInformation("Getting status for remediation action {ActionId}", actionId);
                
                var execution = await _executor.GetExecutionHistoryAsync(actionId);
                if (execution == null)
                {
                    return new RemediationResult
                    {
                        Status = RemediationStatusEnum.Unknown,
                        Message = "No execution history found"
                    };
                }
                
                return new RemediationResult
                {
                    Status = execution.Status,
                    Message = execution.ErrorMessage ?? "Remediation execution details retrieved",
                    IsSuccessful = execution.Status == RemediationStatusEnum.Completed,
                    StartTime = execution.StartTime,
                    EndTime = execution.EndTime,
                    ActionId = actionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting status for action {ActionId}", actionId);
                
                return new RemediationResult
                {
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Error getting action status: {ex.Message}"
                };
            }
        }
    }
} 