using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Remediation.Models.Execution;
using RuntimeErrorSage.Core.Remediation.Models.Metrics;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Tracks the status and progress of remediation actions.
    /// </summary>
    public class RemediationTracker : IRemediationTracker
    {
        private readonly ILogger<RemediationTracker> _logger;
        private readonly Dictionary<string, RemediationStatus> _statuses;
        private readonly Dictionary<string, List<RemediationStep>> _stepHistory;
        private readonly Dictionary<string, RemediationMetrics> _metrics;

        public RemediationTracker(ILogger<RemediationTracker> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _statuses = new Dictionary<string, RemediationStatus>();
            _stepHistory = new Dictionary<string, List<RemediationStep>>();
            _metrics = new Dictionary<string, RemediationMetrics>();
        }

        public async Task<RemediationStatus> GetStatusAsync(string remediationId)
        {
            if (string.IsNullOrEmpty(remediationId))
            {
                throw new ArgumentException("Remediation ID cannot be null or empty", nameof(remediationId));
            }

            return await Task.FromResult(_statuses.TryGetValue(remediationId, out var status)
                ? status
                : new RemediationStatus { State = RemediationState.Unknown });
        }

        public async Task UpdateStatusAsync(string remediationId, RemediationStatus status, string? message = null)
        {
            if (string.IsNullOrEmpty(remediationId))
            {
                throw new ArgumentException("Remediation ID cannot be null or empty", nameof(remediationId));
            }

            if (status == null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            try
            {
                _statuses[remediationId] = status;
                if (!string.IsNullOrEmpty(message))
                {
                    _logger.LogInformation("Remediation {RemediationId} status updated to {State}: {Message}",
                        remediationId, status.State, message);
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for remediation {RemediationId}", remediationId);
                throw;
            }
        }

        public async Task<IEnumerable<RemediationStep>> GetStepHistoryAsync(string remediationId)
        {
            if (string.IsNullOrEmpty(remediationId))
            {
                throw new ArgumentException("Remediation ID cannot be null or empty", nameof(remediationId));
            }

            return await Task.FromResult(_stepHistory.TryGetValue(remediationId, out var history)
                ? history
                : Array.Empty<RemediationStep>());
        }

        public async Task RecordMetricsAsync(string remediationId, RemediationMetrics metrics)
        {
            if (string.IsNullOrEmpty(remediationId))
            {
                throw new ArgumentException("Remediation ID cannot be null or empty", nameof(remediationId));
            }

            if (metrics == null)
            {
                throw new ArgumentNullException(nameof(metrics));
            }

            try
            {
                _metrics[remediationId] = metrics;
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording metrics for remediation {RemediationId}", remediationId);
                throw;
            }
        }

        public async Task AddStepAsync(string remediationId, RemediationStep step)
        {
            if (string.IsNullOrEmpty(remediationId))
            {
                throw new ArgumentException("Remediation ID cannot be null or empty", nameof(remediationId));
            }

            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            try
            {
                if (!_stepHistory.TryGetValue(remediationId, out var history))
                {
                    history = new List<RemediationStep>();
                    _stepHistory[remediationId] = history;
                }

                history.Add(step);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding step for remediation {RemediationId}", remediationId);
                throw;
            }
        }
    }
} 