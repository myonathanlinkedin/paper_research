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
                    Validation = new ValidationResult { IsValid = false, Message = "Failed to create remediation plan" }
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
                    Validation = new ValidationResult { IsValid = false, Message = "Plan validation failed" }
                };
            }

            // Execute plan
            var result = await _executor.ExecuteStrategyAsync(plan.Strategies[0], context);
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
                    Validation = new ValidationResult { IsValid = false, Message = result.Message }
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
                Validation = new ValidationResult { IsValid = true, Message = "Remediation completed successfully" }
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
                Validation = new ValidationResult { IsValid = false, Message = ex.Message }
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
                RemediationId = remediationId,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow,
                Status = RemediationStatusEnum.Failed,
                Values = new Dictionary<string, double>()
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
            return RemediationStatusEnum.NotStarted;
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
                    Status = RemediationStatusEnum.Failed,
                    Message = "No suitable remediation strategy found"
                };
            }

            return new RemediationSuggestion
            {
                ErrorContext = errorContext,
                Strategy = strategy,
                Status = RemediationStatusEnum.Pending,
                Message = "Remediation strategy found"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting remediation suggestions for context {ErrorId}", errorContext.Id);
            return new RemediationSuggestion
            {
                ErrorContext = errorContext,
                Status = RemediationStatusEnum.Failed,
                Message = $"Error getting suggestions: {ex.Message}"
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
            _logger.LogInformation("Validating remediation suggestion for error context {ErrorId}", errorContext.Id);
            var strategy = suggestion.Strategy;
            if (strategy == null)
            {
                return new ValidationResult { IsValid = false, Message = "No strategy specified in suggestion" };
            }

            var result = await _validator.ValidateStrategyAsync(strategy, errorContext);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating remediation suggestion for context {ErrorId}", errorContext.Id);
            return new ValidationResult { IsValid = false, Message = ex.Message };
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
            var strategy = suggestion.Strategy;
            if (strategy == null)
            {
                return new RemediationResult
                {
                    IsSuccessful = false,
                    Message = "No strategy specified in suggestion",
                    Status = RemediationStatusEnum.Failed
                };
            }

            return await _executor.ExecuteStrategyAsync(strategy, errorContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing remediation suggestion for context {ErrorId}", errorContext.Id);
            return new RemediationResult
            {
                IsSuccessful = false,
                Message = ex.Message,
                Status = RemediationStatusEnum.Failed
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
            _logger.LogInformation("Getting impact assessment for suggestion on error context {ErrorId}", errorContext.Id);
            var strategy = suggestion.Strategy;
            if (strategy == null)
            {
                return new RemediationImpact
                {
                    Severity = ImpactSeverity.Unknown,
                    Message = "No strategy specified in suggestion"
                };
            }

            var action = new RemediationAction
            {
                Strategy = strategy,
                Context = errorContext
            };

            return await _executor.GetActionImpactAsync(action, errorContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting suggestion impact for context {ErrorId}", errorContext.Id);
            return new RemediationImpact
            {
                Severity = ImpactSeverity.Unknown,
                Message = $"Error assessing impact: {ex.Message}"
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
            var validationResult = await ValidateActionAsync(action);
            if (!validationResult.IsValid)
            {
                return new RemediationResult
                {
                    IsSuccessful = false,
                    Message = validationResult.Message,
                    Status = RemediationStatusEnum.Failed
                };
            }

            var result = await _executor.ExecuteActionAsync(action, action.Context);
            return new RemediationResult
            {
                IsSuccessful = result.IsSuccessful,
                Message = result.Message,
                Status = result.Status,
                ActionId = result.ActionId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing remediation action {ActionId}", action.ActionId);
            return new RemediationResult
            {
                IsSuccessful = false,
                Message = ex.Message,
                Status = RemediationStatusEnum.Failed,
                ActionId = action.ActionId
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
            return await _validator.ValidateActionAsync(action, action.Context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating remediation action {ActionId}", action.ActionId);
            return new ValidationResult { IsValid = false, Message = ex.Message };
        }
    }

    /// <inheritdoc />
    public async Task<RollbackStatus> RollbackActionAsync(string actionId)
    {
        ArgumentNullException.ThrowIfNull(actionId);

        try
        {
            _logger.LogInformation("Rolling back remediation action {ActionId}", actionId);
            var result = await _executor.RollbackAsync(new RemediationResult { ActionId = actionId });
            return result.IsSuccessful ? RollbackStatus.Completed : RollbackStatus.Failed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rolling back remediation action {ActionId}", actionId);
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
            var status = await _executor.GetRemediationStatusAsync(actionId);
            return new RemediationResult
            {
                ActionId = actionId,
                Status = status,
                IsSuccessful = status == RemediationStatusEnum.Completed
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting status for remediation action {ActionId}", actionId);
            return new RemediationResult
            {
                ActionId = actionId,
                Status = RemediationStatusEnum.Failed,
                IsSuccessful = false,
                Message = ex.Message
            };
        }
    }
} 