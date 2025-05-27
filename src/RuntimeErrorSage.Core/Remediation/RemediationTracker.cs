using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Execution;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Tracks remediation execution status and history.
/// </summary>
public class RemediationTracker : IRemediationTracker, IDisposable
{
    private readonly ILogger<RemediationTracker> _logger;
    private readonly ConcurrentDictionary<string, RemediationExecution> _executions = new();
    private readonly ConcurrentDictionary<string, RemediationMetrics> _metrics = new();
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, RemediationActionExecution>> _actionExecutions = new();
    private readonly Dictionary<string, List<RemediationStep>> _stepHistory;
    private bool _disposed;

    public RemediationTracker(ILogger<RemediationTracker> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _stepHistory = new Dictionary<string, List<RemediationStep>>();
    }

    public Task<RemediationStatusEnum> GetStatusAsync(string planId)
    {
        if (_executions.TryGetValue(planId, out var execution))
        {
            var status = execution.Status switch
            {
                RemediationExecutionStatus.Running => RemediationStatusEnum.InProgress,
                RemediationExecutionStatus.Completed => RemediationStatusEnum.Completed,
                RemediationExecutionStatus.Failed => RemediationStatusEnum.Failed,
                RemediationExecutionStatus.Cancelled => RemediationStatusEnum.Cancelled,
                RemediationExecutionStatus.Partial => RemediationStatusEnum.InProgress,
                _ => RemediationStatusEnum.NotStarted
            };
            return Task.FromResult(status);
        }

        return Task.FromResult(RemediationStatusEnum.NotStarted);
    }

    public Task UpdateStatusAsync(string remediationId, RemediationStatusEnum status, string? message = null)
    {
        if (_executions.TryGetValue(remediationId, out var execution))
        {
            // Map RemediationStatus to RemediationExecutionStatus
            execution.Status = status switch
            {
                RemediationStatusEnum.InProgress => RemediationExecutionStatus.Running,
                RemediationStatusEnum.Completed => RemediationExecutionStatus.Completed,
                RemediationStatusEnum.Failed => RemediationExecutionStatus.Failed,
                RemediationStatusEnum.Cancelled => RemediationExecutionStatus.Cancelled,
                _ => RemediationExecutionStatus.Unknown
            };

            if (message != null)
            {
                execution.Error = message;
            }

            if (status is RemediationStatusEnum.Completed or RemediationStatusEnum.Failed or RemediationStatusEnum.Cancelled)
            {
                execution.EndTime = DateTime.UtcNow;
            }
        }

        return Task.CompletedTask;
    }

    public async Task RecordStepAsync(string remediationId, RemediationStep step)
    {
        ArgumentNullException.ThrowIfNull(remediationId);
        ArgumentNullException.ThrowIfNull(step);
        ThrowIfDisposed();

        try
        {
            if (!_stepHistory.ContainsKey(remediationId))
            {
                _stepHistory[remediationId] = new List<RemediationStep>();
            }

            _stepHistory[remediationId].Add(step);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording step for remediation {RemediationId}", remediationId);
            throw;
        }
    }

    public async Task<IEnumerable<RemediationStep>> GetStepHistoryAsync(string remediationId)
    {
        ArgumentNullException.ThrowIfNull(remediationId);
        ThrowIfDisposed();

        if (_stepHistory.TryGetValue(remediationId, out var steps))
        {
            return steps.OrderBy(s => s.StartTime);
        }

        return Enumerable.Empty<RemediationStep>();
    }

    public Task<RemediationMetrics> GetMetricsAsync(string remediationId)
    {
        return Task.FromResult(_metrics.GetOrAdd(remediationId, _ => new RemediationMetrics
        {
            MetricsId = Guid.NewGuid().ToString(),
            RemediationId = remediationId,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow,
            Timestamp = DateTime.UtcNow
        }));
    }

    public Task RecordMetricsAsync(string planId, RemediationMetrics metrics)
    {
        _metrics.AddOrUpdate(
            planId,
            metrics,
            (_, _) => metrics);

        return Task.CompletedTask;
    }

    public Task TrackRemediationAsync(RemediationExecution execution)
    {
        _executions.AddOrUpdate(
            execution.CorrelationId,
            execution,
            (_, _) => execution);

        if (execution.Metrics != null)
        {
            _metrics.AddOrUpdate(
                execution.CorrelationId,
                execution.Metrics,
                (_, _) => execution.Metrics);
        }

        return Task.CompletedTask;
    }

    public Task<RemediationExecution> GetExecutionAsync(string remediationId)
    {
        return Task.FromResult(_executions.GetOrAdd(remediationId, _ => new RemediationExecution
        {
            CorrelationId = remediationId,
            StartTime = DateTime.UtcNow,
            Status = RemediationExecutionStatus.Unknown
        }));
    }

    public Task<IEnumerable<RemediationExecution>> GetExecutionHistoryAsync()
    {
        return Task.FromResult<IEnumerable<RemediationExecution>>(_executions.Values.ToList());
    }

    public Task TrackActionStartAsync(string planId, string actionId)
    {
        var actionExecutions = _actionExecutions.GetOrAdd(planId, _ => new ConcurrentDictionary<string, RemediationActionExecution>());
        
        var actionExecution = new RemediationActionExecution
        {
            ActionId = actionId,
            StartTime = DateTime.UtcNow,
            Status = RemediationActionStatus.InProgress
        };
        
        actionExecutions.AddOrUpdate(actionId, actionExecution, (_, _) => actionExecution);
        
        _logger.LogInformation("Action {ActionId} started for plan {PlanId}", actionId, planId);
        
        return Task.CompletedTask;
    }

    public Task TrackActionCompletionAsync(string planId, string actionId, bool success, string? errorMessage = null)
    {
        if (_actionExecutions.TryGetValue(planId, out var actionExecutions) && 
            actionExecutions.TryGetValue(actionId, out var actionExecution))
        {
            actionExecution.EndTime = DateTime.UtcNow;
            actionExecution.Status = success ? RemediationActionStatus.Completed : RemediationActionStatus.Failed;
            
            if (errorMessage != null)
            {
                actionExecution.ErrorMessage = errorMessage;
            }
            
            _logger.LogInformation("Action {ActionId} completed for plan {PlanId} with status {Success}", 
                actionId, planId, success ? "Success" : "Failure");
        }
        else
        {
            _logger.LogWarning("Attempted to track completion of unknown action {ActionId} for plan {PlanId}", 
                actionId, planId);
        }
        
        return Task.CompletedTask;
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RemediationTracker));
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        GC.SuppressFinalize(this);
    }
} 