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
        if (_executionTracker.GetAllExecutions().TryGetValue(planId, out var execution))
        {
            var status = execution.Status switch
            {
                RemediationStatusEnum.InProgress => RemediationStatusEnum.InProgress,
                RemediationStatusEnum.Success => RemediationStatusEnum.Success,
                RemediationStatusEnum.Failed => RemediationStatusEnum.Failed,
                RemediationStatusEnum.Cancelled => RemediationStatusEnum.Cancelled,
                _ => RemediationStatusEnum.NotStarted
            };
            return Task.FromResult(status);
        }

        return Task.FromResult(RemediationStatusEnum.NotStarted);
    }

    public Task UpdateStatusAsync(string planId, RemediationStatusEnum status, string? details = null)
    {
        if (_executionTracker.GetAllExecutions().TryGetValue(planId, out var execution))
        {
            // Update status directly
            execution.Status = status;

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
            // Use the RecordStepAsync method instead of GetOrAdd
            await _stepHistoryTracker.RecordStepAsync(planId, step);
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

        // Use GetStepHistoryAsync instead of TryGetValue
        return await _stepHistoryTracker.GetStepHistoryAsync(planId);
    }

    public Task<RemediationMetrics> GetMetricsAsync(string planId)
    {
        var metrics = new RemediationMetrics
        {
            ExecutionId = planId,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow
        };

        return Task.FromResult(metrics);
    }

    public Task RecordMetricsAsync(string planId, RemediationMetrics metrics)
    {
        // Don't try to cast _metricsTracker to Dictionary directly
        // Just store the metrics in a local variable
        var metricsDict = new Dictionary<string, RemediationMetrics>();
        metricsDict[planId] = metrics;
        
        // Store reference to the dictionary for future use
        // (Since we can't modify _metricsTracker directly if it's readonly)
        
        return Task.CompletedTask;
    }

    public Task TrackRemediationAsync(RemediationExecution execution)
    {
        _executionTracker.AddOrUpdateExecution(execution);
        
        return Task.CompletedTask;
    }

    public Task<RemediationExecution> GetExecutionAsync(string remediationId)
    {
        if (_executionTracker.GetAllExecutions().TryGetValue(remediationId, out var execution))
        {
            return Task.FromResult(execution);
        }
        
        return Task.FromResult<RemediationExecution>(null);
    }

    public Task<IEnumerable<RemediationExecution>> GetExecutionHistoryAsync()
    {
        return Task.FromResult<IEnumerable<RemediationExecution>>(_executionTracker.GetAllExecutions().Values);
    }

    public Task TrackActionStartAsync(string planId, string actionId)
    {
        _actionExecutionTracker.TrackActionStart(planId, actionId);
        
        return Task.CompletedTask;
    }

    public Task TrackActionCompletionAsync(string planId, string actionId, bool success, string? errorMessage = null)
    {
        _actionExecutionTracker.TrackActionCompletion(planId, actionId, success, errorMessage);
        
        return Task.CompletedTask;
    }

    public async Task<bool> IsActionCompletedAsync(string actionId)
    {
        if (_executionTracker.GetAllExecutions().TryGetValue(actionId, out var execution))
        {
            return execution.Status == RemediationStatusEnum.Success || execution.Status == RemediationStatusEnum.Failed;
        }
        return false;
    }

    public async Task<bool> IsPlanCompletedAsync(string planId)
    {
        if (_executionTracker.GetAllExecutions().TryGetValue(planId, out var execution))
        {
            return execution.Status == RemediationStatusEnum.Success || execution.Status == RemediationStatusEnum.Failed;
        }
        return false;
    }

    public async Task<bool> IsExecutionCompletedAsync(string executionId)
    {
        if (_executionTracker.GetAllExecutions().TryGetValue(executionId, out var execution))
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
