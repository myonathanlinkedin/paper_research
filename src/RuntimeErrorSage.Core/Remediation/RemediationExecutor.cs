using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Execution;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using IRemediationValidator = RuntimeErrorSage.Core.Remediation.Interfaces.IRemediationValidator;
using RemediationStatus = RuntimeErrorSage.Core.Models.Remediation.RemediationStatus;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Executes remediation strategies for error contexts.
/// </summary>
public class RemediationExecutor : IRemediationExecutor
{
    private static readonly Action<ILogger, string, Exception?> LogExecutionError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(ExecuteStrategyAsync)),
            "Error executing strategy {StrategyName}");

    private static readonly Action<ILogger, string, Exception?> LogValidationError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2, nameof(ValidateRemediationAsync)),
            "Error validating remediation {RemediationId}");

    private static readonly Action<ILogger, string, Exception?> LogExecutionCancelled =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(3, nameof(CancelRemediationAsync)),
            "Remediation {RemediationId} cancelled");

    private static readonly Action<ILogger, string, string, Exception?> LogStrategyValidationFailed =
        LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            new EventId(4, nameof(ExecuteStrategiesForErrorTypeAsync)),
            "Strategy {StrategyName} validation failed: {Errors}");

    private static readonly Action<ILogger, string, Exception?> LogStrategyExecutionError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(5, nameof(ExecuteStrategiesForErrorTypeAsync)),
            "Error executing strategies for error type {ErrorType}");

    private static readonly Action<ILogger, string, Exception?> LogRemediationExecutionError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(6, nameof(ExecuteRemediationAsync)),
            "Error during remediation execution {CorrelationId}");

    private static readonly Action<ILogger, string, Exception?> LogStatusNotImplemented =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(7, nameof(GetRemediationStatusAsync)),
            "GetRemediationStatusAsync not implemented for {RemediationId}");

    private static readonly Action<ILogger, string, Exception?> LogHistoryNotImplemented =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(8, nameof(GetExecutionHistoryAsync)),
            "GetExecutionHistoryAsync not implemented for {RemediationId}");

    private static readonly Action<ILogger, string, Exception?> LogMetricsNotImplemented =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(9, nameof(GetExecutionMetricsAsync)),
            "GetExecutionMetricsAsync not implemented for {RemediationId}");

    private readonly ILogger<RemediationExecutor> _logger;
    private readonly IRemediationRegistry _registry;
    private readonly IRemediationValidator _validator;
    private readonly IRemediationTracker _tracker;
    private readonly IRemediationMetricsCollector _metricsCollector;

    public RemediationExecutor(
        ILogger<RemediationExecutor> logger,
        IRemediationRegistry registry,
        IRemediationValidator validator,
        IRemediationTracker tracker,
        IRemediationMetricsCollector metricsCollector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
        _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
    }

    private void LogError(Action<ILogger, string, Exception?> logAction, string param, Exception ex)
    {
        logAction(_logger, param, ex);
    }

    public async Task<RemediationResult> ExecuteStrategyAsync(
        string strategyName,
        ErrorContext context,
        Dictionary<string, string>? parameters = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(strategyName);
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            var strategy = await _registry.GetStrategyAsync(strategyName).ConfigureAwait(false);
            if (strategy == null)
            {
                return new RemediationResult
                {
                    Success = false,
                    Message = $"Strategy '{strategyName}' not found",
                    Timestamp = DateTime.UtcNow
                };
            }

            // Set parameters if provided
            if (parameters != null)
            {
                foreach (var (key, value) in parameters)
                {
                    strategy.Parameters[key] = value;
                }
            }

            // Validate context
            var contextValidation = await _validator.ValidateRemediationAsync(context).ConfigureAwait(false);
            if (!contextValidation.IsValid)
            {
                return new RemediationResult
                {
                    Success = false,
                    Message = $"Context validation failed: {string.Join(", ", contextValidation.Errors)}",
                    Timestamp = DateTime.UtcNow
                };
            }

            // Validate strategy
            var strategyValidation = await strategy.ValidateAsync(context).ConfigureAwait(false);
            if (!strategyValidation.IsValid)
            {
                return new RemediationResult
                {
                    Success = false,
                    Message = $"Strategy validation failed: {string.Join(", ", strategyValidation.Errors)}",
                    Timestamp = DateTime.UtcNow
                };
            }

            // Create remediation ID
            var remediationId = Guid.NewGuid().ToString();

            // Update status
            await _tracker.UpdateStatusAsync(remediationId, RemediationStatus.Running).ConfigureAwait(false);

            // Execute strategy
            var startTime = DateTime.UtcNow;
            var result = await strategy.ExecuteAsync(context).ConfigureAwait(false);
            var endTime = DateTime.UtcNow;

            // Record metrics
            var metrics = new RemediationMetrics
            {
                MetricsId = Guid.NewGuid().ToString(),
                RemediationId = remediationId,
                StartTime = startTime,
                EndTime = endTime,
                StepsExecuted = 1,
                SuccessfulSteps = result.Success ? 1 : 0,
                FailedSteps = result.Success ? 0 : 1,
                Timestamp = DateTime.UtcNow
            };

            metrics.AddValue("duration_ms", (endTime - startTime).TotalMilliseconds);
            metrics.AddValue("success", result.Success ? 1 : 0);
            metrics.AddLabel("strategy", strategyName);
            metrics.AddLabel("error_type", context.ErrorType);
            metrics.AddLabel("status", result.Success ? "success" : "failure");

            await _metricsCollector.RecordRemediationMetricsAsync(metrics).ConfigureAwait(false);

            // Update status
            await _tracker.UpdateStatusAsync(
                remediationId,
                result.Success ? RemediationStatus.Completed : RemediationStatus.Failed,
                result.Message).ConfigureAwait(false);

            return result;
        }
        catch (InvalidOperationException ex)
        {
            LogError(LogExecutionError, strategyName, ex);
            return new RemediationResult
            {
                Success = false,
                Message = "Strategy execution failed due to invalid operation",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (ValidationException ex)
        {
            LogError(LogExecutionError, strategyName, ex);
            return new RemediationResult
            {
                Success = false,
                Message = "Strategy validation failed",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex) when (ex is not InvalidOperationException && ex is not ValidationException)
        {
            LogError(LogExecutionError, strategyName, ex);
            return new RemediationResult
            {
                Success = false,
                Message = "Unexpected error during strategy execution",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };
        }
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
                return new RemediationResult
                {
                    Success = false,
                    Message = $"No strategies found for error type '{errorType}'",
                    Timestamp = DateTime.UtcNow
                };
            }

            var contextValidation = await _validator.ValidateRemediationAsync(context).ConfigureAwait(false);
            if (!contextValidation.IsValid)
            {
                return new RemediationResult
                {
                    Success = false,
                    Message = $"Context validation failed: {string.Join(", ", contextValidation.Errors)}",
                    Timestamp = DateTime.UtcNow
                };
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
                        LogStrategyValidationFailed(_logger, strategy.Name, string.Join(", ", strategyValidation.Errors), null);
                        continue;
                    }

                    await _tracker.UpdateStatusAsync(remediationId, RemediationStatus.Running).ConfigureAwait(false);

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
                        SuccessfulSteps = result.Success ? 1 : 0,
                        FailedSteps = result.Success ? 0 : 1,
                        Timestamp = DateTime.UtcNow
                    };

                    metrics.AddValue("duration_ms", (endTime - startTime).TotalMilliseconds);
                    metrics.AddValue("success", result.Success ? 1 : 0);
                    metrics.AddLabel("strategy", strategy.Name);
                    metrics.AddLabel("error_type", errorType);
                    metrics.AddLabel("status", result.Success ? "success" : "failure");

                    await _metricsCollector.RecordRemediationMetricsAsync(metrics).ConfigureAwait(false);
                    results.Add(result);

                    if (result.Success)
                    {
                        await _tracker.UpdateStatusAsync(
                            remediationId,
                            RemediationStatus.Completed,
                            $"Strategy '{strategy.Name}' succeeded: {result.Message}").ConfigureAwait(false);
                        break;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    LogStrategyExecutionError(_logger, errorType, ex);
                    results.Add(new RemediationResult
                    {
                        Success = false,
                        Message = "Strategy execution failed due to invalid operation",
                        Error = ex.Message,
                        Timestamp = DateTime.UtcNow
                    });
                }
                catch (ValidationException ex)
                {
                    LogStrategyExecutionError(_logger, errorType, ex);
                    results.Add(new RemediationResult
                    {
                        Success = false,
                        Message = "Strategy validation failed",
                        Error = ex.Message,
                        Timestamp = DateTime.UtcNow
                    });
                }
                catch (Exception ex) when (ex is not InvalidOperationException && ex is not ValidationException)
                {
                    LogStrategyExecutionError(_logger, errorType, ex);
                    results.Add(new RemediationResult
                    {
                        Success = false,
                        Message = "Unexpected error during strategy execution",
                        Error = ex.Message,
                        Timestamp = DateTime.UtcNow
                    });
                }
            }

            var overallSuccess = results.Any(r => r.Success);
            var lastResult = results.LastOrDefault();

            await _tracker.UpdateStatusAsync(
                remediationId,
                overallSuccess ? RemediationStatus.Completed : RemediationStatus.Failed,
                overallSuccess
                    ? $"Remediation succeeded with strategy '{lastResult?.Message}'"
                    : "All strategies failed").ConfigureAwait(false);

            return new RemediationResult
            {
                Success = overallSuccess,
                Message = overallSuccess
                    ? $"Remediation succeeded with strategy '{lastResult?.Message}'"
                    : "All strategies failed",
                Timestamp = DateTime.UtcNow
            };
        }
        catch (InvalidOperationException ex)
        {
            LogStrategyExecutionError(_logger, errorType, ex);
            return new RemediationResult
            {
                Success = false,
                Message = "Strategy execution failed due to invalid operation",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (ValidationException ex)
        {
            LogStrategyExecutionError(_logger, errorType, ex);
            return new RemediationResult
            {
                Success = false,
                Message = "Strategy validation failed",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex) when (ex is not InvalidOperationException && ex is not ValidationException)
        {
            LogStrategyExecutionError(_logger, errorType, ex);
            return new RemediationResult
            {
                Success = false,
                Message = "Unexpected error during strategy execution",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };
        }
    }

    public async Task<RemediationExecution> ExecuteRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context)
    {
        var execution = new RemediationExecution
        {
            CorrelationId = analysis.CorrelationId,
            StartTime = DateTime.UtcNow,
            Status = RemediationExecutionStatus.Running
        };

        await _tracker.TrackRemediationAsync(execution).ConfigureAwait(false);

        try
        {
            // Validate the remediation plan (derived from analysis)
            var plan = CreateRemediationPlan(analysis); // Create a plan from the analysis result
            var validationResult = await _validator.ValidatePlanAsync(plan, context).ConfigureAwait(false);

            if (!validationResult.IsValid)
            {
                execution.Status = RemediationExecutionStatus.Failed;
                execution.Error = $"Remediation plan validation failed: {validationResult.Message}";
                execution.Validation = validationResult;
                await _tracker.TrackRemediationAsync(execution).ConfigureAwait(false);
                return execution;
            }

            execution.Validation = validationResult;

            // Find applicable strategies and execute them
            var applicableStrategies = _registry.GetStrategiesForErrorType(analysis.ErrorType).OrderByDescending(s => s.Priority);

            foreach (var strategy in applicableStrategies)
            {
                try
                {
                    // Validate the strategy for the current context
                    var strategyValidation = await _validator.ValidateStrategyAsync(strategy, context).ConfigureAwait(false);
                    if (!strategyValidation.IsValid)
                    {
                        _logger.LogWarning("Strategy {Strategy} validation failed: {Message}", strategy.Name, strategyValidation.Message);
                        continue; // Skip this strategy if validation fails
                    }

                    _logger.LogInformation("Executing remediation strategy: {Strategy}", strategy.Name);
                    var strategyExecution = await strategy.ExecuteAsync(context).ConfigureAwait(false);
                    execution.AddExecutedAction(strategyExecution);

                    if (strategyExecution.IsSuccessful)
                    {
                        execution.Status = RemediationExecutionStatus.Completed;
                        _logger.LogInformation("Remediation strategy {Strategy} completed successfully.", strategy.Name);
                        break;
                    }
                    else
                    {
                        _logger.LogError("Remediation strategy {Strategy} failed: {Error}", strategy.Name, strategyExecution.Error);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, "Invalid operation during remediation strategy execution: {Strategy}", strategy.Name);
                    execution.AddExecutedAction(new RemediationActionExecution
                    {
                        ActionName = $"{strategy.Name} Execution",
                        Status = RemediationActionStatus.Failed,
                        Error = ex.Message,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow
                    });
                }
                catch (ValidationException ex)
                {
                    _logger.LogError(ex, "Validation error during remediation strategy execution: {Strategy}", strategy.Name);
                    execution.AddExecutedAction(new RemediationActionExecution
                    {
                        ActionName = $"{strategy.Name} Execution",
                        Status = RemediationActionStatus.Failed,
                        Error = ex.Message,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow
                    });
                }
                catch (Exception ex) when (ex is not InvalidOperationException && ex is not ValidationException)
                {
                    _logger.LogError(ex, "Unexpected error during remediation strategy execution: {Strategy}", strategy.Name);
                    execution.AddExecutedAction(new RemediationActionExecution
                    {
                        ActionName = $"{strategy.Name} Execution",
                        Status = RemediationActionStatus.Failed,
                        Error = ex.Message,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow
                    });
                }
            }

            // Final status based on executed actions
            if (execution.Status != RemediationExecutionStatus.Completed)
            {
                if (execution.ExecutedActions.Any(a => a.Status == RemediationActionStatus.Completed))
                {
                    execution.Status = RemediationExecutionStatus.Partial;
                }
                else
                {
                    execution.Status = RemediationExecutionStatus.Failed;
                    execution.Error = execution.ExecutedActions[^1]?.Error ?? "No successful remediation strategy found.";
                }
            }

            execution.EndTime = DateTime.UtcNow;
            var metrics = await _metricsCollector.CollectMetricsAsync(context).ConfigureAwait(false);
            execution.Metrics = new RemediationMetrics
            {
                MetricsId = Guid.NewGuid().ToString(),
                RemediationId = execution.CorrelationId,
                StartTime = execution.StartTime,
                EndTime = execution.EndTime,
                Timestamp = DateTime.UtcNow
            };

            foreach (var (key, value) in metrics)
            {
                if (value is IConvertible convertible)
                {
                    execution.Metrics.AddValue(key, Convert.ToDouble(convertible, System.Globalization.CultureInfo.InvariantCulture));
                }
                else
                {
                    execution.Metrics.AddLabel(key, value?.ToString() ?? string.Empty);
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            LogRemediationExecutionError(_logger, execution.CorrelationId, ex);
            execution.Status = RemediationExecutionStatus.Failed;
            execution.Error = $"Invalid operation during remediation: {ex.Message}";
            execution.EndTime = DateTime.UtcNow;
        }
        catch (ValidationException ex)
        {
            LogRemediationExecutionError(_logger, execution.CorrelationId, ex);
            execution.Status = RemediationExecutionStatus.Failed;
            execution.Error = $"Validation error during remediation: {ex.Message}";
            execution.EndTime = DateTime.UtcNow;
        }
        catch (Exception ex) when (ex is not InvalidOperationException && ex is not ValidationException)
        {
            LogRemediationExecutionError(_logger, execution.CorrelationId, ex);
            execution.Status = RemediationExecutionStatus.Failed;
            execution.Error = $"Unexpected error during remediation: {ex.Message}";
            execution.EndTime = DateTime.UtcNow;
        }
        finally
        {
            await _tracker.TrackRemediationAsync(execution).ConfigureAwait(false);
        }

        return execution;
    }

    public Task<RemediationExecution?> GetRemediationStatusAsync(string remediationId)
    {
        LogStatusNotImplemented(_logger, remediationId, null);
        return Task.FromResult<RemediationExecution?>(null);
    }

    public Task<bool> CancelRemediationAsync(string remediationId)
    {
        // Implement cancellation logic
        LogExecutionCancelled(_logger, remediationId, null);
        return Task.FromResult(false);
    }

    public async Task<RemediationValidationResult> ValidateRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(analysis);
        ArgumentNullException.ThrowIfNull(context);

        var plan = CreateRemediationPlan(analysis);
        return await _validator.ValidatePlanAsync(plan, context);
    }

    public Task<List<RemediationExecution>> GetExecutionHistoryAsync(string remediationId)
    {
        LogHistoryNotImplemented(_logger, remediationId, null);
        return Task.FromResult(new List<RemediationExecution>());
    }

    public Task<RemediationMetrics> GetExecutionMetricsAsync(string remediationId)
    {
        LogMetricsNotImplemented(_logger, remediationId, null);
        return Task.FromResult(new RemediationMetrics
        {
            MetricsId = Guid.NewGuid().ToString(),
            RemediationId = remediationId,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow,
            Timestamp = DateTime.UtcNow
        });
    }

    private static RemediationPlan CreateRemediationPlan(ErrorAnalysisResult analysisResult)
    {
        // Create a rollback plan first
        var rollbackPlan = new RemediationPlan
        {
            PlanId = Guid.NewGuid().ToString(),
            Name = $"Rollback Plan for {analysisResult.CorrelationId}",
            Description = "Automatic rollback plan for remediation actions",
            Status = new RemediationStatus { State = RemediationState.NotStarted }, // Updated to use 'State'
            RollbackPlan = null // Rollback plan doesn't need its own rollback
        };

        // Create the main plan
        var plan = new RemediationPlan
        {
            PlanId = Guid.NewGuid().ToString(),
            Name = $"Remediation Plan for {analysisResult.CorrelationId}",
            Description = $"Remediation plan for error analysis {analysisResult.CorrelationId}",
            Context = analysisResult.RootCause,
            Status = new RemediationStatus { State = RemediationState.Pending }, // Updated to use 'State'
            RollbackPlan = rollbackPlan
        };

        // Fix for CS0117: 'RemediationState' does not contain a definition for 'Pending'
        // The error indicates that 'Pending' is not a valid member of the 'RemediationState' enum.
        // Based on the provided enum definition, the correct value to use is 'NotStarted'.

        plan.Status = new RemediationStatus { State = RemediationState.NotStarted }; // Updated to use 'NotStarted'

        // Add suggested actions as steps
        foreach (var action in analysisResult.SuggestedActions)
        {
            var step = new RemediationStep
            {
                StepId = Guid.NewGuid().ToString(),
                Name = action.Description,
                Description = action.Description,
                Type = Models.Remediation.RemediationStepType.Execution,
                Status = new RemediationStatus { State = RemediationState.NotStarted }, // Updated to use 'State'
                Action = new RemediationAction
                {
                    ActionId = Guid.NewGuid().ToString(),
                    Name = action.ActionType,
                    Description = action.Description,
                    Type = action.ActionType,
                    Status = new RemediationStatus { State = RemediationState.NotStarted } // Updated to use 'State'
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

    Task<Models.Validation.RemediationValidationResult> IRemediationExecutor.ValidateRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(analysis);
        ArgumentNullException.ThrowIfNull(context);

        var plan = CreateRemediationPlan(analysis);
        return _validator.ValidatePlanAsync(plan, context);
    }
} 
