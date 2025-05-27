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
using RuntimeErrorSage.Core.Remediation;

namespace RuntimeErrorSage.Core.Remediation;

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
        if (_executionTracker.TryGetValue(remediationId, out var execution))
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
            await _stepHistoryTracker.AddStepAsync(remediationId, step);
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

        return await _stepHistoryTracker.GetStepHistoryAsync(remediationId);
    }

    public Task<RemediationMetrics> GetMetricsAsync(string remediationId)
    {
        return _metricsTracker.GetMetricsAsync(remediationId);
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