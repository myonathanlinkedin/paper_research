using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Execution;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using CommonRemediationStatus = RuntimeErrorSage.Core.Models.Common.RemediationStatus;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Tracks remediation execution status and history.
/// </summary>
public class RemediationTracker : IRemediationTracker, IDisposable
{
    private readonly ILogger<RemediationTracker> _logger;
    private readonly ConcurrentDictionary<string, RemediationExecution> _executions = new();
    private readonly ConcurrentDictionary<string, RemediationMetrics> _metrics = new();
    private readonly Dictionary<string, List<RemediationStep>> _stepHistory;
    private bool _disposed;

    public RemediationTracker(ILogger<RemediationTracker> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _stepHistory = new Dictionary<string, List<RemediationStep>>();
    }

    public Task<CommonRemediationStatus> GetStatusAsync(string planId)
    {
        if (_executions.TryGetValue(planId, out var execution))
        {
            var status = execution.Status switch
            {
                RemediationExecutionStatus.Running => CommonRemediationStatus.Running,
                RemediationExecutionStatus.Completed => CommonRemediationStatus.Completed,
                RemediationExecutionStatus.Failed => CommonRemediationStatus.Failed,
                RemediationExecutionStatus.Cancelled => CommonRemediationStatus.Cancelled,
                RemediationExecutionStatus.Partial => CommonRemediationStatus.Partial,
                _ => CommonRemediationStatus.Unknown
            };
            return Task.FromResult(status);
        }

        return Task.FromResult(CommonRemediationStatus.Unknown);
    }

    public Task UpdateStatusAsync(string remediationId, CommonRemediationStatus status, string? message = null)
    {
        if (_executions.TryGetValue(remediationId, out var execution))
        {
            // Map RemediationStatus to RemediationExecutionStatus
            execution.Status = status switch
            {
                CommonRemediationStatus.Running => RemediationExecutionStatus.Running,
                CommonRemediationStatus.Completed => RemediationExecutionStatus.Completed,
                CommonRemediationStatus.Failed => RemediationExecutionStatus.Failed,
                CommonRemediationStatus.Cancelled => RemediationExecutionStatus.Cancelled,
                CommonRemediationStatus.Partial => RemediationExecutionStatus.Partial,
                _ => RemediationExecutionStatus.Unknown
            };

            if (message != null)
            {
                execution.Error = message;
            }

            if (status is CommonRemediationStatus.Completed or CommonRemediationStatus.Failed or CommonRemediationStatus.Cancelled)
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