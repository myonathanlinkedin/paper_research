using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Interfaces.MCP;
using RuntimeErrorSage.Core.Remediation.Models.Execution;
using RuntimeErrorSage.Core.Remediation.Models.Validation;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Remediation.Models.Common;
using RuntimeErrorSage.Core.Interfaces;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Executes remediation strategies for error contexts.
/// </summary>
public class RemediationExecutor : IRemediationExecutor
{
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

    public async Task<RemediationResult> ExecuteStrategyAsync(
        string strategyName,
        ErrorContext context,
        Dictionary<string, string>? parameters = null)
    {
        if (string.IsNullOrEmpty(strategyName))
        {
            throw new ArgumentException("Strategy name cannot be null or empty", nameof(strategyName));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            // Get strategy
            var strategy = await _registry.GetStrategyAsync(strategyName);
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
            var contextValidation = await _validator.ValidateRemediationAsync(context);
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
            var strategyValidation = await strategy.ValidateAsync(context);
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
            await _tracker.UpdateStatusAsync(remediationId, RemediationStatus.Running);

            // Execute strategy
            var startTime = DateTime.UtcNow;
            var result = await strategy.ExecuteAsync(context);
            var endTime = DateTime.UtcNow;

            // Record metrics
            var metrics = new RemediationMetrics
            {
                RemediationId = remediationId,
                StrategyName = strategyName,
                ErrorType = context.ErrorType,
                StartTime = startTime,
                EndTime = endTime,
                Duration = endTime - startTime,
                Success = result.Success,
                ErrorMessage = result.Error
            };

            await _metricsCollector.RecordRemediationMetricsAsync(metrics);

            // Update status
            await _tracker.UpdateStatusAsync(
                remediationId,
                result.Success ? RemediationStatus.Completed : RemediationStatus.Failed,
                result.Message);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing strategy {StrategyName}", strategyName);
            return new RemediationResult
            {
                Success = false,
                Message = $"Error executing strategy: {ex.Message}",
                Error = ex.ToString(),
                Timestamp = DateTime.UtcNow
            };
        }
    }

    public async Task<RemediationResult> ExecuteStrategiesForErrorTypeAsync(
        string errorType,
        ErrorContext context,
        Dictionary<string, Dictionary<string, string>>? strategyParameters = null)
    {
        if (string.IsNullOrEmpty(errorType))
        {
            throw new ArgumentException("Error type cannot be null or empty", nameof(errorType));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            // Get strategies for error type
            var strategies = await _registry.GetStrategiesForErrorTypeAsync(errorType);
            if (!strategies.Any())
            {
                return new RemediationResult
                {
                    Success = false,
                    Message = $"No strategies found for error type '{errorType}'",
                    Timestamp = DateTime.UtcNow
                };
            }

            // Validate context
            var contextValidation = await _validator.ValidateRemediationAsync(context);
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

            // Execute strategies in priority order
            foreach (var strategy in strategies)
            {
                // Set parameters if provided
                if (strategyParameters != null &&
                    strategyParameters.TryGetValue(strategy.Name, out var parameters))
                {
                    foreach (var (key, value) in parameters)
                    {
                        strategy.Parameters[key] = value;
                    }
                }

                // Validate strategy
                var strategyValidation = await strategy.ValidateAsync(context);
                if (!strategyValidation.IsValid)
                {
                    _logger.LogWarning(
                        "Strategy {StrategyName} validation failed: {Errors}",
                        strategy.Name,
                        string.Join(", ", strategyValidation.Errors));
                    continue;
                }

                // Update status
                await _tracker.UpdateStatusAsync(remediationId, RemediationStatus.Running);

                // Execute strategy
                var startTime = DateTime.UtcNow;
                var result = await strategy.ExecuteAsync(context);
                var endTime = DateTime.UtcNow;

                // Record metrics
                var metrics = new RemediationMetrics
                {
                    RemediationId = remediationId,
                    StrategyName = strategy.Name,
                    ErrorType = errorType,
                    StartTime = startTime,
                    EndTime = endTime,
                    Duration = endTime - startTime,
                    Success = result.Success,
                    ErrorMessage = result.Error
                };

                await _metricsCollector.RecordRemediationMetricsAsync(metrics);
                results.Add(result);

                // If strategy succeeded, stop execution
                if (result.Success)
                {
                    await _tracker.UpdateStatusAsync(
                        remediationId,
                        RemediationStatus.Completed,
                        $"Strategy '{strategy.Name}' succeeded: {result.Message}");
                    break;
                }
            }

            // Determine overall result
            var overallSuccess = results.Any(r => r.Success);
            var lastResult = results.LastOrDefault();

            await _tracker.UpdateStatusAsync(
                remediationId,
                overallSuccess ? RemediationStatus.Completed : RemediationStatus.Failed,
                overallSuccess
                    ? $"Remediation succeeded with strategy '{lastResult?.Message}'"
                    : "All strategies failed");

            return new RemediationResult
            {
                Success = overallSuccess,
                Message = overallSuccess
                    ? $"Remediation succeeded with strategy '{lastResult?.Message}'"
                    : "All strategies failed",
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing strategies for error type {ErrorType}", errorType);
            return new RemediationResult
            {
                Success = false,
                Message = $"Error executing strategies: {ex.Message}",
                Error = ex.ToString(),
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

        await _tracker.TrackRemediationAsync(execution);

        try
        {
            // Validate the remediation plan (derived from analysis)
            var plan = CreateRemediationPlan(analysis); // Create a plan from the analysis result
            var validationResult = await _validator.ValidatePlanAsync(plan, context);

            if (!validationResult.IsSuccessful)
            {
                execution.Status = RemediationExecutionStatus.Failed;
                execution.Error = $"Remediation plan validation failed: {validationResult.Message}";
                execution.Validation = validationResult;
                await _tracker.TrackRemediationAsync(execution);
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
                    var strategyValidation = await _validator.ValidateStrategyAsync(strategy, context);
                    if (!strategyValidation.IsSuccessful)
                    {
                        _logger.LogWarning("Strategy {Strategy} validation failed: {Message}", strategy.Name, strategyValidation.Message);
                        continue; // Skip this strategy if validation fails
                    }

                    _logger.LogInformation("Executing remediation strategy: {Strategy}", strategy.Name);
                    var strategyExecution = await strategy.ExecuteAsync(context);
                    execution.ExecutedActions.Add(strategyExecution);

                    if (strategyExecution.IsSuccessful)
                    {
                        execution.Status = RemediationExecutionStatus.Completed; // Mark as completed if any strategy succeeds
                        _logger.LogInformation("Remediation strategy {Strategy} completed successfully.", strategy.Name);
                        break; // Stop on first successful strategy (or continue for multiple?)
                    }
                    else
                    {
                        _logger.LogError("Remediation strategy {Strategy} failed: {Error}", strategy.Name, strategyExecution.Error);
                    }
                }
                catch (Exception strategyEx)
                {
                    _logger.LogError(strategyEx, "Exception during remediation strategy execution: {Strategy}", strategy.Name);
                    execution.ExecutedActions.Add(new RemediationActionExecution // Log strategy exception as a failed action
                    {
                        ActionName = $"{strategy.Name} Execution",
                        Status = RemediationActionStatus.Failed,
                        Error = strategyEx.Message,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow // Assuming immediate failure
                    });
                }
            }

            // Final status based on executed actions
            if (execution.Status != RemediationExecutionStatus.Completed)
            {
                 if (execution.ExecutedActions.Any(a => a.Status == RemediationActionStatus.Completed))
                 {
                      execution.Status = RemediationExecutionStatus.Partial; // Partially successful if some actions passed
                 }
                 else
                 {
                      execution.Status = RemediationExecutionStatus.Failed; // Failed if no strategy succeeded
                      execution.Error = execution.ExecutedActions.LastOrDefault()?.Error ?? "No successful remediation strategy found.";
                 }
            }

            execution.EndTime = DateTime.UtcNow;

            // Collect metrics
            // This would ideally be done after a successful remediation or to analyze failures
            // For simplicity, collecting general metrics here
            execution.Metrics = await _metricsCollector.CollectMetricsAsync(context); // Assuming CollectMetricsAsync returns RemediationMetrics
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during remediation execution");
            execution.Status = RemediationExecutionStatus.Failed;
            execution.Error = ex.Message;
            execution.EndTime = DateTime.UtcNow;
        }
        finally
        {
            await _tracker.TrackRemediationAsync(execution); // Final tracking update
        }

        return execution;
    }

    public Task<RemediationExecution?> GetRemediationStatusAsync(string remediationId)
    {
        // This would typically retrieve status from the tracker
        _logger.LogInformation("GetRemediationStatusAsync not implemented");
        return Task.FromResult<RemediationExecution?>(null);
    }

    public Task<bool> CancelRemediationAsync(string remediationId)
    {
        // Implement cancellation logic
        _logger.LogInformation("CancelRemediationAsync not implemented");
        return Task.FromResult(false);
    }

    public Task<RemediationValidationResult> ValidateRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context)
    {
        // This would typically involve the validator
         var plan = CreateRemediationPlan(analysis); // Create a plan from the analysis result
        return _validator.ValidatePlanAsync(plan, context);
    }

    public Task<List<RemediationExecution>> GetExecutionHistoryAsync(string remediationId)
    {
         // This would typically retrieve history from the tracker or storage
        _logger.LogInformation("GetExecutionHistoryAsync not implemented");
        return Task.FromResult(new List<RemediationExecution>());
    }

    public Task<RemediationMetrics> GetExecutionMetricsAsync(string remediationId)
    {
        // This would typically retrieve metrics from the tracker or storage
        _logger.LogInformation("GetExecutionMetricsAsync not implemented");
        return Task.FromResult(new RemediationMetrics { }); // Return empty metrics for now
    }

     private RemediationPlan CreateRemediationPlan(ErrorAnalysisResult analysisResult)
    {
        // Convert analysis result to a remediation plan
        // This is a simplified mapping
        var plan = new RemediationPlan
        {
            Context = analysisResult.RootCause, // Using root cause as context for simplicity
            IsValidated = false // Needs explicit validation
        };

        // Add suggested actions as steps
        plan.Steps.AddRange(analysisResult.SuggestedActions.Select(action => new RemediationStep
        {
            Description = action.Description,
            Action = action.ActionType, // Using ActionType as the action identifier
            Parameters = action.Parameters,
            MaxRetries = 3 // Default retries
        }));

        return plan;
    }
} 
