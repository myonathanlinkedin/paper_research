using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Execution;
using RuntimeErrorSage.Domain.Models.Metrics;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Remediation;

namespace RuntimeErrorSage.Application.Remediation;

/// <summary>
/// Tracks remediation execution status and history.
/// </summary>
public class RemediationTracker : IRemediationTracker, IDisposable
{
    private readonly ILogger<RemediationTracker> _logger;
    private readonly ExecutionTracker _executionTracker = new();
    private readonly MetricsTracker _metricsTracker = new();
    private readonly StepHistoryTracker _stepHistoryTracker = new();
    private readonly ActionExecutionTracker _actionExecutionTracker = new();
    private bool _disposed;

    public RemediationTracker(ILogger<RemediationTracker> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<RemediationStatusEnum> GetStatusAsync(string planId)
    {
        if (_executionTracker.TryGetValue(planId, out var execution))
        {
            var status = execution.Status switch
            {
                RemediationExecutionStatus.Running => RemediationStatusEnum.InProgress,
                RemediationExecutionStatus.Completed => RemediationStatusEnum.Success,
                RemediationExecutionStatus.Failed => RemediationStatusEnum.Failed,
                RemediationExecutionStatus.Cancelled => RemediationStatusEnum.Cancelled,
                RemediationExecutionStatus.Partial => RemediationStatusEnum.InProgress,
                _ => RemediationStatusEnum.NotStarted
            };
            return Task.FromResult(status);
        }

        return Task.FromResult(RemediationStatusEnum.NotStarted);
    }

    public Task UpdateStatusAsync(string planId, RemediationStatusEnum status, string? details = null)
    {
        if (_executionTracker.TryGetValue(planId, out var execution))
        {
            // Map RemediationStatus to RemediationExecutionStatus
            execution.Status = status switch
            {
                RemediationStatusEnum.InProgress => RemediationExecutionStatus.Running,
                RemediationStatusEnum.Success => RemediationExecutionStatus.Completed,
                RemediationStatusEnum.Failed => RemediationExecutionStatus.Failed,
                RemediationStatusEnum.Cancelled => RemediationExecutionStatus.Cancelled,
                _ => RemediationExecutionStatus.Unknown
            };

            if (details != null)
            {
                execution.Error = details;
            }

            if (status is RemediationStatusEnum.Success or RemediationStatusEnum.Failed or RemediationStatusEnum.Cancelled)
            {
                execution.EndTime = DateTime.UtcNow;
            }
        }

        return Task.CompletedTask;
    }

    public async Task RecordStepAsync(string planId, RemediationStep step)
    {
        ArgumentNullException.ThrowIfNull(planId);
        ArgumentNullException.ThrowIfNull(step);
        ThrowIfDisposed();

        try
        {
            await _stepHistoryTracker.AddStepAsync(planId, step);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording step for remediation {PlanId}", planId);
            throw;
        }
    }

    public async Task<IEnumerable<RemediationStep>> GetStepHistoryAsync(string planId)
    {
        ArgumentNullException.ThrowIfNull(planId);
        ThrowIfDisposed();

        return await _stepHistoryTracker.GetStepHistoryAsync(planId);
    }

    public Task<RemediationMetrics> GetMetricsAsync(string planId)
    {
        return _metricsTracker.GetMetricsAsync(planId);
    }

    public Task RecordMetricsAsync(string planId, RemediationMetrics metrics)
    {
        return _metricsTracker.RecordMetricsAsync(planId, metrics);
    }

    public Task TrackRemediationAsync(RemediationExecution execution)
    {
        return _executionTracker.TrackExecutionAsync(execution);
    }

    public Task<RemediationExecution> GetExecutionAsync(string remediationId)
    {
        return _executionTracker.GetExecutionAsync(remediationId);
    }

    public Task<IEnumerable<RemediationExecution>> GetExecutionHistoryAsync()
    {
        return _executionTracker.GetExecutionHistoryAsync();
    }

    public Task TrackActionStartAsync(string planId, string actionId)
    {
        return _actionExecutionTracker.TrackActionStartAsync(planId, actionId);
    }

    public Task TrackActionCompletionAsync(string planId, string actionId, bool success, string? errorMessage = null)
    {
        return _actionExecutionTracker.TrackActionCompletionAsync(planId, actionId, success, errorMessage);
    }

    public async Task<bool> IsActionCompletedAsync(string actionId)
    {
        if (_executionTracker.TryGetValue(actionId, out var execution))
        {
            return execution.Status == RemediationStatusEnum.Success || execution.Status == RemediationStatusEnum.Failed;
        }
        return false;
    }

    public async Task<bool> IsPlanCompletedAsync(string planId)
    {
        if (_executionTracker.TryGetValue(planId, out var execution))
        {
            return execution.Status == RemediationStatusEnum.Success || execution.Status == RemediationStatusEnum.Failed;
        }
        return false;
    }

    public async Task<bool> IsExecutionCompletedAsync(string executionId)
    {
        if (_executionTracker.TryGetValue(executionId, out var execution))
        {
            return execution.Status == RemediationStatusEnum.Success || execution.Status == RemediationStatusEnum.Failed;
        }
        return false;
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
