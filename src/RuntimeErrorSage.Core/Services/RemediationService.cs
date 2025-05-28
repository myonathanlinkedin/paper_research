using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Interfaces;
using System.Linq;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Remediation;

namespace RuntimeErrorSage.Core.Services;

/// <summary>
/// Service for managing remediation operations.
/// </summary>
public class RemediationService : IRemediationService
{
    private readonly ILogger<RemediationService> _logger;
    private readonly IRemediationStrategySelector _strategySelector;
    private readonly IRemediationExecutor _executor;
    private readonly IRemediationValidator _validator;
    private readonly IRemediationMetricsCollector _metricsCollector;

    public RemediationService(
        ILogger<RemediationService> logger,
        IRemediationStrategySelector strategySelector,
        IRemediationExecutor executor,
        IRemediationValidator validator,
        IRemediationMetricsCollector metricsCollector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _strategySelector = strategySelector ?? throw new ArgumentNullException(nameof(strategySelector));
        _executor = executor ?? throw new ArgumentNullException(nameof(executor));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
    }

    /// <inheritdoc />
    public bool IsEnabled => true;

    /// <inheritdoc />
    public string Name => "RuntimeErrorSage Remediation Service";

    /// <inheritdoc />
    public string Version => "1.0.0";

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
                    SuggestionId = Guid.NewGuid().ToString(),
                    Title = "No suitable remediation found",
                    Description = "No suitable remediation strategy could be found for this error.",
                    ConfidenceLevel = 0,
                    CorrelationId = errorContext.CorrelationId
                };
            }

            // Get priority using the async method
            var priorityTask = strategy.GetPriorityAsync(errorContext);
            var priority = await priorityTask;

            return new RemediationSuggestion
            {
                SuggestionId = Guid.NewGuid().ToString(),
                Title = strategy.Name,
                Description = strategy.Description,
                StrategyName = strategy.Name,
                Priority = priority, // Use the result from GetPriorityAsync
                ConfidenceLevel = 0.8,
                Parameters = strategy.Parameters,
                CorrelationId = errorContext.CorrelationId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting remediation suggestions for error context {ErrorId}", errorContext.Id);
            return new RemediationSuggestion
            {
                SuggestionId = Guid.NewGuid().ToString(),
                Title = "Error",
                Description = $"Error getting remediation suggestions: {ex.Message}",
                CorrelationId = errorContext.CorrelationId
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
            if (string.IsNullOrEmpty(suggestion.StrategyName))
            {
                return new RemediationResult
                {
                    Context = errorContext,
                    Status = RemediationStatusEnum.Failed,
                    Message = "No strategy found in suggestion"
                };
            }

            _logger.LogInformation("Executing remediation suggestion for error context {ErrorId}", errorContext.Id);
            
            // Create a mock context with the strategy name to use with SelectStrategyAsync
            var mockContext = new ErrorContext 
            { 
                ErrorType = suggestion.StrategyName,
                CorrelationId = errorContext.CorrelationId
            };
            
            var strategy = await _strategySelector.SelectStrategyAsync(mockContext);
            if (strategy == null)
            {
                return new RemediationResult
                {
                    Context = errorContext,
                    Status = RemediationStatusEnum.Failed,
                    Message = "Strategy not found"
                };
            }

            var strategyAdapter = new RemediationStrategyAdapter(strategy);
            return await _executor.ExecuteStrategyAsync(strategyAdapter, errorContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing suggestion for error context {ErrorId}", errorContext.Id);
            return new RemediationResult
            {
                Context = errorContext,
                Status = RemediationStatusEnum.Failed,
                Message = $"Error executing suggestion: {ex.Message}"
            };
        }
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
            
            if (string.IsNullOrEmpty(action.StrategyName))
            {
                return new RemediationResult
                {
                    Context = action.Context,
                    Status = RemediationStatusEnum.Failed,
                    Message = "No strategy name found in action"
                };
            }
            
            // Get the error context
            var errorContext = action.Context;
            if (errorContext == null)
            {
                return new RemediationResult
                {
                    Context = errorContext,
                    Status = RemediationStatusEnum.Failed,
                    Message = "No error context found in action"
                };
            }
            
            var result = await _executor.ExecuteActionAsync(action, errorContext);
            
            return new RemediationResult
            {
                Context = errorContext,
                Status = result.IsSuccessful ? RemediationStatusEnum.Completed : RemediationStatusEnum.Failed,
                Message = result.ErrorMessage ?? "Action executed",
                ActionId = result.ActionId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing action {ActionId}", action.ActionId);
            return new RemediationResult
            {
                Context = action.Context,
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
                    Messages = new List<string> { "No error context found in action" }
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
            return result.IsSuccessful ? RollbackStatus.Completed : RollbackStatus.Failed;
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
            
            // Create a new RemediationResult since RemediationExecution doesn't have a Result property
            return new RemediationResult
            {
                Status = execution.Status,
                Message = execution.ErrorMessage ?? "Remediation execution details retrieved",
                IsSuccessful = execution.IsSuccessful,
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