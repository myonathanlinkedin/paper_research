using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Models.Common;
using ImpactScope = RuntimeErrorSage.Core.Remediation.Models.Common.ImpactScope;
using MetricValue = RuntimeErrorSage.Core.Models.Metrics.MetricValue;

namespace RuntimeErrorSage.Core.Remediation.Metrics;

/// <summary>
/// Collects and analyzes metrics for remediation strategies.
/// </summary>
public class RemediationMetricsCollector : IRemediationMetricsCollector
{
    private readonly ILogger<RemediationMetricsCollector> _logger;
    private readonly Dictionary<string, Dictionary<string, List<MetricValue>>> _metricsHistory;
    private readonly Dictionary<string, RemediationMetrics> _remediationMetrics;
    private readonly Dictionary<string, StepMetrics> _stepMetrics;

    public RemediationMetricsCollector(ILogger<RemediationMetricsCollector> logger)
    {
        _logger = logger;
        _metricsHistory = new Dictionary<string, Dictionary<string, List<MetricValue>>>();
        _remediationMetrics = new Dictionary<string, RemediationMetrics>();
        _stepMetrics = new Dictionary<string, StepMetrics>();
    }

    public async Task<Dictionary<string, object>> CollectMetricsAsync(ErrorContext context)
    {
        try
        {
            var metrics = new Dictionary<string, object>
            {
                ["ErrorId"] = context.ErrorId,
                ["Timestamp"] = DateTime.UtcNow,
                ["Severity"] = context.Severity,
                ["Impact"] = context.Impact,
                ["Duration"] = context.Duration,
                ["SuccessRate"] = await CalculateSuccessRateAsync(context.ErrorId),
                ["AverageExecutionTime"] = await CalculateAverageExecutionTimeAsync(context.ErrorId),
                ["ImpactDistribution"] = await GetImpactDistributionAsync(context.ErrorId)
            };

            return metrics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error collecting metrics for context {ErrorId}", context.ErrorId);
            throw;
        }
    }

    public async Task RecordMetricAsync(string remediationId, string metricName, object value)
    {
        try
        {
            var history = _metricsHistory.GetOrAdd(remediationId, _ => new Dictionary<string, List<MetricValue>>());
            var metricHistory = history.GetOrAdd(metricName, _ => new List<MetricValue>());

            metricHistory.Add(new MetricValue
            {
                Timestamp = DateTime.UtcNow,
                Value = value
            });

            _logger.LogInformation("Recorded metric {MetricName} for remediation {RemediationId}", metricName, remediationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording metric {MetricName} for remediation {RemediationId}", metricName, remediationId);
            throw;
        }
    }

    public async Task<Dictionary<string, List<MetricValue>>> GetMetricsHistoryAsync(string remediationId)
    {
        if (_metricsHistory.TryGetValue(remediationId, out var history))
        {
            return await Task.FromResult(history);
        }
        return await Task.FromResult(new Dictionary<string, List<MetricValue>>());
    }

    public async Task RecordRemediationMetricsAsync(RemediationMetrics metrics)
    {
        try
        {
            _remediationMetrics[metrics.RemediationId] = metrics;

            // Record individual metrics
            await RecordMetricAsync(metrics.RemediationId, "Duration", metrics.Duration);
            await RecordMetricAsync(metrics.RemediationId, "SuccessRate", metrics.SuccessRate);
            await RecordMetricAsync(metrics.RemediationId, "Impact", metrics.Impact);
            await RecordMetricAsync(metrics.RemediationId, "Severity", metrics.Severity);

            _logger.LogInformation("Recorded metrics for remediation {RemediationId}", metrics.RemediationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording metrics for remediation {RemediationId}", metrics.RemediationId);
            throw;
        }
    }

    public async Task RecordStepMetricsAsync(StepMetrics metrics)
    {
        try
        {
            _stepMetrics[metrics.StepId] = metrics;

            // Record individual metrics
            await RecordMetricAsync(metrics.StepId, "Duration", metrics.Duration);
            await RecordMetricAsync(metrics.StepId, "Status", metrics.Status);
            await RecordMetricAsync(metrics.StepId, "ErrorType", metrics.ErrorType);

            _logger.LogInformation("Recorded metrics for step {StepId}", metrics.StepId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording metrics for step {StepId}", metrics.StepId);
            throw;
        }
    }

    public async Task<RemediationMetrics> GetRemediationMetricsAsync(string remediationId)
    {
        if (_remediationMetrics.TryGetValue(remediationId, out var metrics))
        {
            return metrics;
        }

        return new RemediationMetrics
        {
            RemediationId = remediationId,
            Timestamp = DateTime.UtcNow,
            Duration = 0,
            SuccessRate = 0,
            Impact = ImpactScope.None,
            Severity = ImpactSeverity.None,
            Status = RemediationStatus.Unknown,
            Steps = new List<StepMetrics>(),
            Metadata = new Dictionary<string, object>()
        };
    }

    public async Task<RemediationMetrics> GetAggregatedMetricsAsync(TimeRange range)
    {
        var metrics = new RemediationMetrics
        {
            RemediationId = "aggregated",
            Timestamp = DateTime.UtcNow,
            Duration = 0,
            SuccessRate = 0,
            Impact = ImpactScope.None,
            Severity = ImpactSeverity.None,
            Status = RemediationStatus.Unknown,
            Steps = new List<StepMetrics>(),
            Metadata = new Dictionary<string, object>()
        };

        var totalDuration = 0.0;
        var totalSuccessRate = 0.0;
        var count = 0;

        foreach (var remediation in _remediationMetrics.Values)
        {
            if (remediation.Timestamp >= range.Start && remediation.Timestamp <= range.End)
            {
                totalDuration += remediation.Duration;
                totalSuccessRate += remediation.SuccessRate;
                count++;

                // Update impact and severity based on highest values
                if (remediation.Impact > metrics.Impact)
                {
                    metrics.Impact = remediation.Impact;
                }
                if (remediation.Severity > metrics.Severity)
                {
                    metrics.Severity = remediation.Severity;
                }

                metrics.Steps.AddRange(remediation.Steps);
            }
        }

        if (count > 0)
        {
            metrics.Duration = totalDuration / count;
            metrics.SuccessRate = totalSuccessRate / count;
        }

        return metrics;
    }

    public async Task<ValidationResult> ValidateMetricsAsync(RemediationMetrics metrics)
    {
        var result = new ValidationResult
        {
            ValidationId = Guid.NewGuid().ToString(),
            IsValid = true,
            Timestamp = DateTime.UtcNow,
            RuleName = "MetricsValidation",
            RuleDescription = "Validates remediation metrics against theoretical thresholds",
            ErrorMessage = null,
            Details = new Dictionary<string, object>(),
            Context = new Dictionary<string, object>(),
            Impact = ImpactScope.None,
            Priority = 0,
            Category = "Metrics",
            Scope = "Remediation",
            Status = ValidationStatus.Success,
            Type = ValidationType.Metrics,
            Mode = ValidationMode.Automatic,
            Phase = ValidationPhase.PostExecution,
            Stage = ValidationStage.Final,
            Level = ValidationLevel.Info,
            DurationMs = 0,
            Metadata = new Dictionary<string, object>()
        };

        try
        {
            // Validate duration
            if (metrics.Duration < 0)
            {
                result.IsValid = false;
                result.ErrorMessage = "Duration cannot be negative";
                result.Status = ValidationStatus.Failed;
                result.Level = ValidationLevel.Error;
            }

            // Validate success rate
            if (metrics.SuccessRate < 0 || metrics.SuccessRate > 1)
            {
                result.IsValid = false;
                result.ErrorMessage = "Success rate must be between 0 and 1";
                result.Status = ValidationStatus.Failed;
                result.Level = ValidationLevel.Error;
            }

            // Validate impact and severity
            if (metrics.Impact == ImpactScope.None && metrics.Severity != ImpactSeverity.None)
            {
                result.IsValid = false;
                result.ErrorMessage = "Impact scope cannot be None when severity is not None";
                result.Status = ValidationStatus.Failed;
                result.Level = ValidationLevel.Warning;
            }

            // Validate steps
            if (metrics.Steps == null || metrics.Steps.Count == 0)
            {
                result.IsValid = false;
                result.ErrorMessage = "No steps recorded";
                result.Status = ValidationStatus.Failed;
                result.Level = ValidationLevel.Warning;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating metrics for remediation {RemediationId}", metrics.RemediationId);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
            result.Status = ValidationStatus.Failed;
            result.Level = ValidationLevel.Error;
            result.Exception = ex;
            return result;
        }
    }

    private async Task<double> CalculateSuccessRateAsync(string remediationId)
    {
        if (_metricsHistory.TryGetValue(remediationId, out var history) &&
            history.TryGetValue("Status", out var statusHistory))
        {
            var successCount = statusHistory.Count(v => v.Value.ToString() == RemediationStatus.Success.ToString());
            return statusHistory.Count > 0 ? (double)successCount / statusHistory.Count : 0;
        }

        return 0;
    }

    private async Task<double> CalculateAverageExecutionTimeAsync(string remediationId)
    {
        if (_metricsHistory.TryGetValue(remediationId, out var history) &&
            history.TryGetValue("Duration", out var durationHistory))
        {
            return durationHistory.Count > 0
                ? durationHistory.Average(v => Convert.ToDouble(v.Value))
                : 0;
        }

        return 0;
    }

    private async Task<Dictionary<ImpactScope, int>> GetImpactDistributionAsync(string remediationId)
    {
        var distribution = new Dictionary<ImpactScope, int>();

        if (_metricsHistory.TryGetValue(remediationId, out var history) &&
            history.TryGetValue("Impact", out var impactHistory))
        {
            foreach (var impact in impactHistory)
            {
                if (impact.Value is ImpactScope scope)
                {
                    distribution[scope] = distribution.GetValueOrDefault(scope) + 1;
                }
            }
        }

        return distribution;
    }
} 