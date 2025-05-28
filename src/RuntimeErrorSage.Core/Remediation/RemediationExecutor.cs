using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Analysis.Interfaces;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Execution;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Utilities;
using IRemediationStrategy = RuntimeErrorSage.Core.Models.Remediation.Interfaces.IRemediationStrategy;
using IRemediationValidator = RuntimeErrorSage.Core.Remediation.Interfaces.IRemediationValidator;
using RiskAssessment = RuntimeErrorSage.Core.Models.Remediation.RiskAssessment;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Executes remediation strategies for error contexts.
/// </summary>
public class RemediationExecutor : IRemediationExecutor
{
    private readonly ILogger<RemediationExecutor> _logger;
    private readonly IErrorContextAnalyzer _errorContextAnalyzer;
    private readonly IRemediationValidator _validator;
    private readonly IRemediationTracker _tracker;
    private readonly IRemediationMetricsCollector _metricsCollector;
    private readonly IRemediationStrategyRegistry _registry;
    private readonly ILLMClient _llmClient;
    private readonly Dictionary<string, EventId> _eventIds;
    private readonly IRemediationRiskAssessment _riskAssessment;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemediationExecutor"/> class.
    /// </summary>
    public RemediationExecutor(
        ILogger<RemediationExecutor> logger,
        IErrorContextAnalyzer errorContextAnalyzer,
        IRemediationRegistry registry,
        IRemediationValidator validator,
        IRemediationTracker tracker,
        IRemediationMetricsCollector metricsCollector,
        ILLMClient llmClient,
        IRemediationStrategyRegistry strategyRegistry,
        IRemediationRiskAssessment riskAssessment,
        Dictionary<string, EventId>? eventIds = null)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _errorContextAnalyzer = errorContextAnalyzer ?? throw new ArgumentNullException(nameof(errorContextAnalyzer));
        _registry = strategyRegistry ?? throw new ArgumentNullException(nameof(strategyRegistry));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
        _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
        _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
        _riskAssessment = riskAssessment ?? throw new ArgumentNullException(nameof(riskAssessment));
        _eventIds = eventIds ?? new Dictionary<string, EventId>
        {
            { nameof(ExecuteStrategyAsync), new EventId(1, nameof(ExecuteStrategyAsync)) },
            { nameof(ValidateRemediationAsync), new EventId(2, nameof(ValidateRemediationAsync)) },
            { nameof(CancelRemediationAsync), new EventId(3, nameof(CancelRemediationAsync)) },
            { nameof(ExecuteStrategiesForErrorTypeAsync), new EventId(4, nameof(ExecuteStrategiesForErrorTypeAsync)) },
            { nameof(ExecuteRemediationAsync), new EventId(5, nameof(ExecuteRemediationAsync)) },
            { nameof(GetRemediationStatusAsync), new EventId(6, nameof(GetRemediationStatusAsync)) },
            { nameof(GetExecutionHistoryAsync), new EventId(7, nameof(GetExecutionHistoryAsync)) },
            { nameof(GetExecutionMetricsAsync), new EventId(8, nameof(GetExecutionMetricsAsync)) }
        };
    }

    /// <inheritdoc/>
    public bool IsEnabled => true;

    /// <inheritdoc/>
    public string Name => "RemediationExecutor";

    /// <inheritdoc/>
    public string Version => "1.0.0";

    private void LogError(Action<ILogger, string, Exception?> logAction, string param, Exception ex)
    {
        logAction(_logger, param, ex);
    }

    /// <inheritdoc/>
    public async Task<RemediationResult> ExecuteStrategyAsync(IRemediationStrategy strategy, ErrorContext context)
    {
        if (strategy == null)
            throw new ArgumentNullException(nameof(strategy));
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        try
        {
            _logger.LogInformation(_eventIds[nameof(ExecuteStrategyAsync)], 
                "Executing strategy {StrategyName} for error {ErrorId}", strategy.Name, context.ErrorId);

            var result = new RemediationResult
            {
                StartTime = DateTime.UtcNow,
                ExecutionId = Guid.NewGuid().ToString(),
                CorrelationId = context.CorrelationId,
                ErrorId = context.ErrorId,
                ErrorType = context.ErrorType,
                Context = context
            };

            // Create execution metrics
            var metrics = new RemediationMetrics
            {
                ExecutionId = result.ExecutionId,
                StartResourceUsage = new ResourceUsage()
            };

            // Validate strategy
            var validationResult = await _validator.ValidateStrategyAsync(strategy, context);
            if (!validationResult.IsValid)
            {
                result.Success = false;
                result.ErrorMessage = validationResult.ValidationMessage;
                result.Status = RemediationStatusEnum.ValidationFailed;
                result.EndTime = DateTime.UtcNow;
                
                await _metricsCollector.RecordExecutionAsync(result.ExecutionId, result);
                return result;
            }

            // Execute strategy actions
            foreach (var action in strategy.Actions)
            {
                try
                {
                    var actionResult = await action.ExecuteAsync();
                    result.Actions.Add(action);

                    if (!actionResult.IsSuccessful)
                    {
                        result.Success = false;
                        result.ErrorMessage = $"Action {action.Name} failed: {actionResult.ErrorMessage}";
                        result.Status = RemediationStatusEnum.Failed;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(_eventIds[nameof(ExecuteStrategyAsync)], ex, 
                        "Error executing action {ActionName}", action.Name);
                    result.Success = false;
                    result.ErrorMessage = $"Action {action.Name} failed: {ex.Message}";
                    result.Status = RemediationStatusEnum.Failed;
                    break;
                }
            }

            // Update metrics
            metrics.EndResourceUsage = new ResourceUsage();
            metrics.DurationMs = (DateTime.UtcNow - result.StartTime).TotalMilliseconds;
            await _metricsCollector.RecordRemediationMetricsAsync(metrics);

            // Set final status if not already set
            if (result.Status == RemediationStatusEnum.NotStarted)
            {
                result.Status = RemediationStatusEnum.Completed;
                result.Success = true;
            }

            result.EndTime = DateTime.UtcNow;
            await _metricsCollector.RecordExecutionAsync(result.ExecutionId, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(ExecuteStrategyAsync)], ex, 
                "Error executing strategy {StrategyName}", strategy.Name);
            
            var result = new RemediationResult
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow,
                ExecutionId = Guid.NewGuid().ToString(),
                CorrelationId = context.CorrelationId,
                ErrorId = context.ErrorId,
                ErrorType = context.ErrorType,
                Success = false,
                ErrorMessage = $"Unhandled exception: {ex.Message}",
                Status = RemediationStatusEnum.Failed,
                Context = context
            };
            
            await _metricsCollector.RecordExecutionAsync(result.ExecutionId, result);
            return result;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationResult> ExecutePlanAsync(RemediationPlan plan)
    {
        ArgumentNullException.ThrowIfNull(plan);

        try
        {
            _logger.LogInformation(_eventIds[nameof(ExecuteRemediationAsync)], 
                "Executing remediation plan {PlanId}", plan.PlanId);

            var result = new RemediationResult
            {
                StartTime = DateTime.UtcNow,
                PlanId = plan.PlanId,
                PlanName = plan.Name
            };

            foreach (var action in plan.Actions)
            {
                try
                {
                    var actionResult = await action.ExecuteAsync(plan.Analysis.Context);
                    result.Actions.Add(actionResult);

                    if (!actionResult.IsSuccessful)
                    {
                        result.Success = false;
                        result.ErrorMessage = $"Action {action.Name} failed: {actionResult.ErrorMessage}";
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(_eventIds[nameof(ExecuteRemediationAsync)], ex, 
                        "Error executing action {ActionName}", action.Name);
                    result.Success = false;
                    result.ErrorMessage = $"Action {action.Name} failed: {ex.Message}";
                    break;
                }
            }

            result.EndTime = DateTime.UtcNow;
            result.Success = result.Actions.All(a => a.IsSuccessful);
            result.Status = result.Success ? RemediationStatusEnum.Completed : RemediationStatusEnum.Failed;

            await _metricsCollector.RecordExecutionAsync(result.ExecutionId, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(ExecuteRemediationAsync)], ex, 
                "Error executing plan {PlanId}", plan.PlanId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationResult> RollbackAsync(RemediationResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        try
        {
            _logger.LogInformation(_eventIds[nameof(ExecuteRemediationAsync)], 
                "Rolling back remediation {ExecutionId}", result.ExecutionId);

            var rollbackResult = new RemediationResult
            {
                StartTime = DateTime.UtcNow,
                ExecutionId = Guid.NewGuid().ToString(),
                CorrelationId = result.CorrelationId,
                IsRollback = true,
                OriginalExecutionId = result.ExecutionId
            };

            // Rollback actions in reverse order
            foreach (var action in result.Actions.AsEnumerable().Reverse())
            {
                if (action.RollbackAction != null)
                {
                    try
                    {
                        var actionResult = await action.RollbackAction.ExecuteAsync(result.Context);
                        rollbackResult.Actions.Add(actionResult);

                        if (!actionResult.IsSuccessful)
                        {
                            rollbackResult.Success = false;
                            rollbackResult.ErrorMessage = $"Rollback action {action.RollbackAction.Name} failed: {actionResult.ErrorMessage}";
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(_eventIds[nameof(ExecuteRemediationAsync)], ex, 
                            "Error executing rollback action {ActionName}", action.RollbackAction.Name);
                        rollbackResult.Success = false;
                        rollbackResult.ErrorMessage = $"Rollback action {action.RollbackAction.Name} failed: {ex.Message}";
                        break;
                    }
                }
            }

            rollbackResult.EndTime = DateTime.UtcNow;
            rollbackResult.Success = rollbackResult.Actions.All(a => a.IsSuccessful);
            rollbackResult.Status = rollbackResult.Success ? RemediationStatusEnum.Completed : RemediationStatusEnum.Failed;

            await _metricsCollector.RecordExecutionAsync(rollbackResult.ExecutionId, rollbackResult);

            return rollbackResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(ExecuteRemediationAsync)], ex, 
                "Error rolling back remediation {ExecutionId}", result.ExecutionId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationStatusEnum> GetRemediationStatusAsync(string remediationId)
    {
        if (string.IsNullOrEmpty(remediationId))
            throw new ArgumentException("Remediation ID cannot be null or empty.", nameof(remediationId));

        // Implementation will be added
        return await Task.FromResult(RemediationStatusEnum.NotStarted);
    }

    /// <inheritdoc/>
    public async Task<RemediationExecution> GetExecutionHistoryAsync(string remediationId)
    {
        try
        {
            return await _metricsCollector.GetExecutionHistoryAsync(remediationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(GetExecutionHistoryAsync)], ex, 
                "Error getting execution history {RemediationId}", remediationId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> CancelRemediationAsync(string remediationId)
    {
        try
        {
            _logger.LogInformation(_eventIds[nameof(CancelRemediationAsync)], 
                "Cancelling remediation {RemediationId}", remediationId);
            // Implementation for cancellation logic
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(CancelRemediationAsync)], ex, 
                "Error cancelling remediation {RemediationId}", remediationId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationExecution> ExecuteRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context)
    {
        try
        {
            _logger.LogInformation(_eventIds[nameof(ExecuteRemediationAsync)], 
                "Executing remediation for error type {ErrorType}", context.ErrorType);

            var plan = await CreatePlanAsync(analysis, context);
            var result = await ExecutePlanAsync(plan);

            return new RemediationExecution
            {
                ExecutionId = result.ExecutionId,
                StartTime = result.StartTime,
                EndTime = result.EndTime,
                Status = result.Success ? RemediationStatusEnum.Completed : RemediationStatusEnum.Failed,
                ErrorMessage = result.ErrorMessage,
                Actions = result.Actions
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(ExecuteRemediationAsync)], ex, 
                "Error executing remediation");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationValidationResult> ValidateRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context)
    {
        try
        {
            var plan = await CreatePlanAsync(analysis, context);
            var validationResult = await _validator.ValidatePlanAsync(plan, context);

            return new RemediationValidationResult
            {
                IsValid = validationResult.IsValid,
                ValidationResults = new List<ValidationResult> { validationResult }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(ValidateRemediationAsync)], ex, 
                "Error validating remediation");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationMetrics> GetExecutionMetricsAsync(string remediationId)
    {
        try
        {
            var metrics = await _metricsCollector.GetMetricsAsync(remediationId);
            return new RemediationMetrics
            {
                ExecutionId = remediationId,
                Metrics = metrics
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(GetExecutionMetricsAsync)], ex, 
                "Error getting execution metrics {RemediationId}", remediationId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationAction> ExecuteActionAsync(RemediationAction action, ErrorContext context)
    {
        try
        {
            _logger.LogInformation(_eventIds[nameof(ExecuteStrategyAsync)], 
                "Executing action {ActionName}", action.Name);

            action.StartTime = DateTime.UtcNow;
            action.Status = RemediationStatus.InProgress;

            // Validate action
            var validationResult = await _validator.ValidateActionAsync(action, context);
            if (!validationResult.IsValid)
            {
                action.Status = RemediationStatus.Failed;
                action.Error = "Action validation failed";
                action.IsSuccessful = false;
                action.EndTime = DateTime.UtcNow;
                return action;
            }

            // Execute action
            try
            {
                // Implementation for action execution
                await Task.Delay(100); // Placeholder for actual execution

                action.Status = RemediationStatus.Completed;
                action.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                action.Status = RemediationStatus.Failed;
                action.Error = ex.Message;
                action.IsSuccessful = false;
            }

            action.EndTime = DateTime.UtcNow;
            return action;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(ExecuteStrategyAsync)], ex, 
                "Error executing action {ActionName}", action.Name);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<ValidationResult> ValidateActionAsync(RemediationAction action, ErrorContext context)
    {
        try
        {
            return await _validator.ValidateActionAsync(action, context);
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(ValidateRemediationAsync)], ex, 
                "Error validating action {ActionName}", action.Name);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationImpact> GetActionImpactAsync(RemediationAction action, ErrorContext context)
    {
        try
        {
            // Implementation for impact assessment
            return new RemediationImpact
            {
                ActionId = action.ActionId,
                Severity = action.Impact,
                Scope = action.ImpactScope,
                AffectedComponents = new List<string>(),
                EstimatedDuration = TimeSpan.FromMinutes(5)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(ExecuteStrategyAsync)], ex, 
                "Error getting action impact {ActionName}", action.Name);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RiskAssessment> GetActionRiskAsync(RemediationAction action, ErrorContext context)
    {
        try
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            // Use RiskAssessmentHelper for consistent risk logic
            var remediationRiskLevel = RiskAssessmentHelper.CalculateRiskLevel(action.Impact, action.ImpactScope);
            var riskLevel = remediationRiskLevel switch
            {
                RemediationRiskLevel.Low => RiskLevel.Low,
                RemediationRiskLevel.Medium => RiskLevel.Medium,
                RemediationRiskLevel.High => RiskLevel.High,
                RemediationRiskLevel.Critical => RiskLevel.Critical,
                _ => RiskLevel.Medium,
            };

            var assessment = new RiskAssessment
            {
                CorrelationId = action.ActionId,
                RiskLevel = riskLevel,
                PotentialIssues = RiskAssessmentHelper.GeneratePotentialIssues(remediationRiskLevel),
                MitigationSteps = RiskAssessmentHelper.GenerateMitigationSteps(remediationRiskLevel),
                Confidence = RiskAssessmentHelper.CalculateConfidence(action),
                ImpactScope = action.ImpactScope,
                Timestamp = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow,
                EstimatedDuration = TimeSpan.FromSeconds(action.TimeoutSeconds),
                AffectedComponents = new List<string>(),
                RiskFactors = new List<RiskFactor>(),
                Metadata = new Dictionary<string, object>()
            };

            // Add affected components if available
            if (!string.IsNullOrEmpty(action.ComponentId))
                assessment.AffectedComponents.Add(action.ComponentId);
            if (action.Context?.AffectedComponents != null)
            {
                foreach (var component in action.Context.AffectedComponents)
                {
                    if (!string.IsNullOrEmpty(component.Name))
                        assessment.AffectedComponents.Add(component.Name);
                }
            }

            // Add risk factors (reuse logic from RiskAssessmentService if needed)
            // ... (optional: add more detailed risk factors here) ...

            return assessment;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(ExecuteStrategyAsync)], ex, 
                "Error getting action risk {ActionName}", action?.Name);
            throw;
        }
    }

    private async Task<RemediationPlan> CreatePlanAsync(ErrorAnalysisResult analysis, ErrorContext context)
    {
        var plan = new RemediationPlan
        {
            PlanId = Guid.NewGuid().ToString(),
            Name = $"Remediation for {context.ErrorType}",
            Description = "Automatically generated remediation plan",
            Analysis = analysis,
            CreatedAt = DateTime.UtcNow,
            Status = new RemediationStatus { State = RemediationState.NotStarted }
        };

        // Add suggested actions as steps
        foreach (var action in analysis.SuggestedActions)
        {
            var step = new RemediationStep
            {
                StepId = Guid.NewGuid().ToString(),
                Name = action.Description,
                Description = action.Description,
                Type = RemediationStepType.Execution,
                Status = new RemediationStatus { State = RemediationState.NotStarted },
                Action = new RemediationAction
                {
                    ActionId = Guid.NewGuid().ToString(),
                    Name = action.ActionType,
                    Description = action.Description,
                    Type = action.ActionType,
                    Status = new RemediationStatus { State = RemediationState.NotStarted }
                }
            };

            // Add parameters if any
            if (action.Parameters != null)
            {
                foreach (var param in action.Parameters)
                {
                    step.Parameters[param.Key] = param.Value;
                }
            }

            plan.Steps.Add(step);
        }

        return plan;
    }

    public async Task<RemediationResult> ExecuteStrategiesForErrorTypeAsync(
        string errorType,
        ErrorContext context,
        Dictionary<string, Dictionary<string, string>>? strategyParameters = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(errorType);
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            var strategies = await _registry.GetStrategiesForErrorTypeAsync(errorType).ConfigureAwait(false);
            if (!strategies.Any())
            {
                var result = new RemediationResult
                {
                    IsSuccessful = false,
                    Message = $"No strategies found for error type '{errorType}'",
                    Timestamp = DateTime.UtcNow
                };
                return result;
            }

            var contextValidation = await _validator.ValidateRemediationAsync(context.AnalysisResult, context).ConfigureAwait(false);
            if (!contextValidation.IsValid)
            {
                var result = new RemediationResult
                {
                    IsSuccessful = false,
                    Message = $"Context validation failed: {string.Join(", ", contextValidation.Errors)}",
                    Timestamp = DateTime.UtcNow
                };
                return result;
            }

            var results = new List<RemediationResult>();
            var remediationId = Guid.NewGuid().ToString();

            foreach (var strategy in strategies)
            {
                try
                {
                    if (strategyParameters?.TryGetValue(strategy.Name, out var parameters) == true)
                    {
                        foreach (var (key, value) in parameters)
                        {
                            strategy.Parameters[key] = value;
                        }
                    }

                    var strategyValidation = await strategy.ValidateAsync(context).ConfigureAwait(false);
                    if (!strategyValidation.IsValid)
                    {
                        _logger.LogWarning(_eventIds[nameof(ExecuteStrategiesForErrorTypeAsync)], 
                            "Strategy {StrategyName} validation failed: {Errors}", 
                            strategy.Name, string.Join(", ", strategyValidation.Errors));
                        continue;
                    }

                    await _tracker.UpdateStatusAsync(remediationId, RemediationStatusEnum.InProgress).ConfigureAwait(false);

                    var startTime = DateTime.UtcNow;
                    var result = await strategy.ExecuteAsync(context).ConfigureAwait(false);
                    var endTime = DateTime.UtcNow;

                    var metrics = new RemediationMetrics
                    {
                        MetricsId = Guid.NewGuid().ToString(),
                        RemediationId = remediationId,
                        StartTime = startTime,
                        EndTime = endTime,
                        StepsExecuted = 1,
                        SuccessfulSteps = result.IsSuccessful ? 1 : 0,
                        FailedSteps = result.IsSuccessful ? 0 : 1,
                        Timestamp = DateTime.UtcNow
                    };

                    metrics.AddValue("duration_ms", (endTime - startTime).TotalMilliseconds);
                    metrics.AddValue("success", result.IsSuccessful ? 1 : 0);
                    metrics.AddLabel("strategy", strategy.Name);
                    metrics.AddLabel("error_type", errorType);
                    metrics.AddLabel("status", result.IsSuccessful ? "success" : "failure");

                    await _metricsCollector.RecordRemediationMetricsAsync(metrics).ConfigureAwait(false);
                    results.Add(result);

                    if (result.IsSuccessful)
                    {
                        await _tracker.UpdateStatusAsync(
                            remediationId,
                            RemediationStatusEnum.Completed,
                            $"Strategy '{strategy.Name}' succeeded: {result.Message}").ConfigureAwait(false);
                        break;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(_eventIds[nameof(ExecuteStrategiesForErrorTypeAsync)], ex,
                        "Error executing strategies for error type {ErrorType}", errorType);
                    var result = new RemediationResult
                    {
                        IsSuccessful = false,
                        Message = "Strategy execution failed due to invalid operation",
                        Error = ex.Message,
                        Timestamp = DateTime.UtcNow
                    };
                    results.Add(result);
                }
                catch (ValidationException ex)
                {
                    _logger.LogError(_eventIds[nameof(ExecuteStrategiesForErrorTypeAsync)], ex,
                        "Error executing strategies for error type {ErrorType}", errorType);
                    var result = new RemediationResult
                    {
                        IsSuccessful = false,
                        Message = "Strategy validation failed",
                        Error = ex.Message,
                        Timestamp = DateTime.UtcNow
                    };
                    results.Add(result);
                }
                catch (Exception ex) when (ex is not InvalidOperationException && ex is not ValidationException)
                {
                    _logger.LogError(_eventIds[nameof(ExecuteStrategiesForErrorTypeAsync)], ex,
                        "Error executing strategies for error type {ErrorType}", errorType);
                    var result = new RemediationResult
                    {
                        IsSuccessful = false,
                        Message = "Unexpected error during strategy execution",
                        Error = ex.Message,
                        Timestamp = DateTime.UtcNow
                    };
                    results.Add(result);
                }
            }

            var overallSuccess = results.Any(r => r.IsSuccessful);
            var lastResult = results.LastOrDefault();

            await _tracker.UpdateStatusAsync(
                remediationId,
                overallSuccess ? RemediationStatusEnum.Completed : RemediationStatusEnum.Failed,
                overallSuccess
                    ? $"Remediation succeeded with strategy '{lastResult?.Message}'"
                    : "All strategies failed").ConfigureAwait(false);

            var finalResult = new RemediationResult
            {
                IsSuccessful = overallSuccess,
                Message = overallSuccess
                    ? $"Remediation succeeded with strategy '{lastResult?.Message}'"
                    : "All strategies failed",
                Timestamp = DateTime.UtcNow
            };
            return finalResult;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(_eventIds[nameof(ExecuteStrategiesForErrorTypeAsync)], ex,
                "Error executing strategies for error type {ErrorType}", errorType);
            var result = new RemediationResult
            {
                IsSuccessful = false,
                Message = "Strategy execution failed due to invalid operation",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };
            return result;
        }
        catch (ValidationException ex)
        {
            _logger.LogError(_eventIds[nameof(ExecuteStrategiesForErrorTypeAsync)], ex,
                "Error executing strategies for error type {ErrorType}", errorType);
            var result = new RemediationResult
            {
                IsSuccessful = false,
                Message = "Strategy validation failed",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };
            return result;
        }
        catch (Exception ex) when (ex is not InvalidOperationException && ex is not ValidationException)
        {
            _logger.LogError(_eventIds[nameof(ExecuteStrategiesForErrorTypeAsync)], ex,
                "Error executing strategies for error type {ErrorType}", errorType);
            var result = new RemediationResult
            {
                IsSuccessful = false,
                Message = "Unexpected error during strategy execution",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };
            return result;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationResult> RollbackRemediationAsync(string remediationId)
    {
        ArgumentNullException.ThrowIfNull(remediationId);

        try
        {
            _logger.LogInformation(_eventIds[nameof(ExecuteRemediationAsync)], 
                "Rolling back remediation {RemediationId}", remediationId);

            var execution = await GetExecutionHistoryAsync(remediationId);
            if (execution == null)
            {
                throw new InvalidOperationException($"No execution found for remediation {remediationId}");
            }

            var result = await RollbackAsync(execution.Result);
            await _metricsCollector.RecordRollbackAsync(remediationId, result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(ExecuteRemediationAsync)], ex, 
                "Error rolling back remediation {RemediationId}", remediationId);
            throw;
        }
    }

    public async Task<RiskAssessment> CreateRiskAssessment(RemediationAction action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var assessment = new RiskAssessment
        {
            CorrelationId = action.ActionId,
            StartTime = DateTime.UtcNow
        };

        try
        {
            // Calculate risk level
            var remediationRiskLevel = RiskAssessmentHelper.CalculateRiskLevel(action.Impact, action.ImpactScope);
            assessment.RiskLevel = remediationRiskLevel;

            // Generate potential issues
            assessment.PotentialIssues = RiskAssessmentHelper.GeneratePotentialIssues(remediationRiskLevel);

            // Generate mitigation steps
            assessment.MitigationSteps = RiskAssessmentHelper.GenerateMitigationSteps(remediationRiskLevel);

            // Set confidence based on available information
            assessment.Confidence = CalculateConfidence(action);

            // Add metadata
            assessment.Metadata = new Dictionary<string, object>
            {
                ["ErrorType"] = action.ErrorType,
                ["StackTrace"] = action.StackTrace,
                ["ContextRiskLevel"] = action.Context?.RiskLevel ?? RemediationRiskLevel.Medium
            };

            // Set affected components
            assessment.AffectedComponents = GetAffectedComponents(action);

            // Set estimated duration
            assessment.EstimatedDuration = EstimateDuration(action);

            // Set status
            assessment.Status = AnalysisStatus.Completed;
        }
        catch (Exception ex)
        {
            assessment.Status = AnalysisStatus.Failed;
            assessment.Notes = $"Risk assessment failed: {ex.Message}";
            assessment.Warnings.Add($"Error during assessment: {ex.Message}");
        }
        finally
        {
            assessment.EndTime = DateTime.UtcNow;
        }

        return assessment;
    }

    private double CalculateConfidence(RemediationAction action)
    {
        var confidenceFactors = new List<double>();

        // Factor 1: Error type clarity
        if (!string.IsNullOrEmpty(action.ErrorType))
        {
            confidenceFactors.Add(0.8);
        }

        // Factor 2: Stack trace availability
        if (!string.IsNullOrEmpty(action.StackTrace))
        {
            confidenceFactors.Add(0.9);
        }

        // Factor 3: Context completeness
        if (action.Context?.Count > 0)
        {
            confidenceFactors.Add(0.7);
        }

        // Factor 4: Impact scope clarity
        if (action.ImpactScope != RemediationActionImpactScope.None)
        {
            confidenceFactors.Add(0.6);
        }

        // Calculate average confidence
        return confidenceFactors.Any() ? confidenceFactors.Average() * 100 : 50.0;
    }

    private List<string> GetAffectedComponents(RemediationAction action)
    {
        var components = new HashSet<string>();

        // Add components from context
        if (action.Context?.TryGetValue("component", out var component) == true)
        {
            components.Add(component.ToString());
        }

        // Add components from stack trace
        if (!string.IsNullOrEmpty(action.StackTrace))
        {
            var stackLines = action.StackTrace.Split('\n');
            foreach (var line in stackLines)
            {
                if (line.Contains("at "))
                {
                    var parts = line.Split(new[] { "at " }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1)
                    {
                        var methodInfo = parts[1].Split('(')[0];
                        components.Add(methodInfo);
                    }
                }
            }
        }

        return components.ToList();
    }

    private TimeSpan EstimateDuration(RemediationAction action)
    {
        // Base duration
        var baseDuration = TimeSpan.FromMinutes(5);

        // Adjust based on risk level
        var riskMultiplier = action.RiskLevel switch
        {
            RemediationRiskLevel.Critical => 4.0,
            RemediationRiskLevel.High => 3.0,
            RemediationRiskLevel.Medium => 2.0,
            RemediationRiskLevel.Low => 1.5,
            _ => 1.0
        };

        // Adjust based on context complexity
        var contextMultiplier = action.Context?.Count > 10 ? 2.0 : 1.0;

        return TimeSpan.FromTicks((long)(baseDuration.Ticks * riskMultiplier * contextMultiplier));
    }
} 
