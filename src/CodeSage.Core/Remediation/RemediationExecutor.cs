using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CodeSage.Core.Models;
using CodeSage.Core.Remediation.Interfaces;
using CodeSage.Core.Interfaces.MCP;
using CodeSage.Core.Models.Error;
using CodeSage.Core.Remediation.Models.Execution;
using CodeSage.Core.Remediation.Models.Validation;
using CodeSage.Core.Models.Metrics;
using CodeSage.Core.Remediation.Models.Common;

namespace CodeSage.Core.Remediation;

public class RemediationExecutor : IRemediationExecutor
{
    private readonly ILogger<RemediationExecutor> _logger;
    private readonly IEnumerable<IRemediationStrategy> _strategies;
    private readonly IRemediationValidator _validator;
    private readonly IRemediationMetricsCollector _metricsCollector;
    private readonly IRemediationTracker _tracker;

    public RemediationExecutor(
        ILogger<RemediationExecutor> logger,
        IEnumerable<IRemediationStrategy> strategies,
        IRemediationValidator validator,
        IRemediationMetricsCollector metricsCollector,
        IRemediationTracker tracker)
    {
        _logger = logger;
        _strategies = strategies;
        _validator = validator;
        _metricsCollector = metricsCollector;
        _tracker = tracker;
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
            var applicableStrategies = _strategies.Where(s => s.CanHandle(analysis)).OrderByDescending(s => s.Priority);

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
                    var strategyExecution = await strategy.ApplyAsync(analysis);
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