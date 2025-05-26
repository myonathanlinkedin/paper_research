using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Options;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MetricsStepMetrics = RuntimeErrorSage.Core.Models.Metrics.StepMetrics;
using SeverityLevel = RuntimeErrorSage.Core.Models.Validation.SeverityLevel;
using ValidationError = RuntimeErrorSage.Core.Models.Validation.ValidationError;
using ValidationResult = RuntimeErrorSage.Core.Models.Validation.ValidationResult;
using ValidationWarning = RuntimeErrorSage.Core.Models.Validation.ValidationWarning;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Collects and aggregates remediation metrics.
/// </summary>
public sealed class RemediationMetricsCollector : IRemediationMetricsCollector, IDisposable
{
    private readonly ILogger<RemediationMetricsCollector> _logger;
    private readonly RemediationMetricsCollectorOptions _options;
    private readonly Process _currentProcess;
    private readonly Timer _collectionTimer;
    private readonly ConcurrentDictionary<string, List<RemediationMetrics>> _metricsHistory = new();
    private readonly ConcurrentDictionary<string, List<MetricsStepMetrics>> _stepMetricsHistory = new();
    private readonly object _lock = new();
    private readonly CancellationTokenSource _cts = new();
    private bool _disposed;

    private static readonly Action<ILogger, string, Exception?> LogMetricsError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(CollectMetricsAsync)),
            "Error collecting metrics: {Message}");

    public RemediationMetricsCollector(
        ILogger<RemediationMetricsCollector> logger,
        IOptions<RemediationMetricsCollectorOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _currentProcess = Process.GetCurrentProcess();

        // Setup collection timer with dueTime and period in milliseconds
        _collectionTimer = new Timer(
            async _ => await CollectMetricsAsync().ConfigureAwait(false),
            null,
            0, // Start immediately
            (int)_options.CollectionInterval.TotalMilliseconds);
    }

    private async Task CollectMetricsAsync()
    {
        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);
            cts.CancelAfter(_options.CollectionTimeout);

            var metrics = new Dictionary<string, object>();

            // Collect system metrics
            var systemMetrics = await CollectSystemMetricsAsync(cts.Token).ConfigureAwait(false);
            foreach (var (key, value) in systemMetrics)
            {
                metrics[key] = value;
            }

            // Collect process metrics
            var processMetrics = CollectProcessMetrics();
            foreach (var (key, value) in processMetrics)
            {
                metrics[key] = value;
            }

            StoreMetricsInHistory(metrics);
        }
        catch (OperationCanceledException) when (!_cts.Token.IsCancellationRequested)
        {
            LogMetricsError(_logger, "Metrics collection timed out", null);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            LogMetricsError(_logger, ex.Message, ex);
        }
    }

    public Task RecordRemediationMetricsAsync(RemediationMetrics metrics)
    {
        ArgumentNullException.ThrowIfNull(metrics);
        ThrowIfDisposed();

        lock (_lock)
        {
            if (!_metricsHistory.TryGetValue(metrics.RemediationId, out var history))
            {
                history = new List<RemediationMetrics>();
                _metricsHistory[metrics.RemediationId] = history;
            }
            history.Add(metrics);
        }

        return Task.CompletedTask;
    }

    public async Task RecordStepMetricsAsync(CoreStepMetrics metrics)
    {
        ArgumentNullException.ThrowIfNull(metrics);
        ThrowIfDisposed();

        try
        {
            var stepMetrics = new MetricsStepMetrics
            {
                StepId = metrics.StepId,
                RemediationId = metrics.RemediationId,
                StartTime = metrics.StartTime,
                EndTime = metrics.EndTime,
                Status = metrics.Status,
                Duration = metrics.Duration,
                Metrics = metrics.Metrics.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Labels = metrics.Labels.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            };

            _stepMetricsHistory.AddOrUpdate(
                metrics.RemediationId,
                new List<MetricsStepMetrics> { stepMetrics },
                (_, list) =>
                {
                    list.Add(stepMetrics);
                    return list;
                });

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording step metrics for remediation {RemediationId}", metrics.RemediationId);
            throw;
        }
    }

    public async Task<Dictionary<string, List<MetricValue>>> GetMetricsHistoryAsync(string remediationId)
    {
        ArgumentNullException.ThrowIfNull(remediationId);
        ThrowIfDisposed();

        var history = new Dictionary<string, List<MetricValue>>();

        if (_metricsHistory.TryGetValue(remediationId, out var metrics))
        {
            foreach (var metric in metrics)
            {
                foreach (var (key, value) in metric.Values)
                {
                    if (!history.ContainsKey(key))
                    {
                        history[key] = new List<MetricValue>();
                    }

                    history[key].Add(new MetricValue
                    {
                        Value = value,
                        Timestamp = metric.Timestamp,
                        Labels = metric.Labels
                    });
                }
            }
        }

        return history;
    }

    public Task<Dictionary<string, AggregatedMetrics>> GetAggregatedMetricsAsync(TimeRange timeRange)
    {
        ArgumentNullException.ThrowIfNull(timeRange);
        ThrowIfDisposed();

        var result = new Dictionary<string, AggregatedMetrics>();

        foreach (var (remediationId, history) in _metricsHistory)
        {
            var filteredMetrics = history
                .Where(m => m.Timestamp >= timeRange.Start && m.Timestamp <= timeRange.End)
                .ToList();

            foreach (var metrics in filteredMetrics)
            {
                foreach (var (key, value) in metrics.Values)
                {
                    if (!result.TryGetValue(key, out var aggregated))
                    {
                        aggregated = new AggregatedMetrics
                        {
                            MetricName = key,
                            StartTime = timeRange.Start,
                            EndTime = timeRange.End,
                            Labels = new Dictionary<string, string>(metrics.Labels)
                        };
                        result[key] = aggregated;
                    }

                    var values = filteredMetrics
                        .Where(m => m.Values.ContainsKey(key))
                        .Select(m => m.Values[key])
                        .ToList();

                    if (values.Count > 0)
                    {
                        aggregated.Sum = values.Sum();
                        aggregated.Count = values.Count;
                        aggregated.Min = values.Min();
                        aggregated.Max = values.Max();
                        aggregated.Average = aggregated.Sum / aggregated.Count;
                    }
                }
            }
        }

        return Task.FromResult(result);
    }

    private async Task<Dictionary<string, double>> CollectSystemMetricsAsync(CancellationToken cancellationToken)
    {
        var metrics = new Dictionary<string, double>();

        try
        {
            metrics["cpu_usage"] = await GetCpuUsageAsync(cancellationToken).ConfigureAwait(false);
            metrics["memory_usage"] = await GetMemoryUsageAsync(cancellationToken).ConfigureAwait(false);
            metrics["disk_usage"] = await GetDiskUsageAsync(cancellationToken).ConfigureAwait(false);

            if (_options.EnableDetailedMetrics)
            {
                metrics["cpu_frequency"] = await Task.Run(() => GetCpuFrequency(), cancellationToken).ConfigureAwait(false);
                metrics["total_memory"] = await Task.Run(() => GetTotalMemory(), cancellationToken).ConfigureAwait(false);
                metrics["available_memory"] = await Task.Run(() => GetAvailableMemory(), cancellationToken).ConfigureAwait(false);
                metrics["total_disk_space"] = await Task.Run(() => GetTotalDiskSpace(), cancellationToken).ConfigureAwait(false);
                metrics["free_disk_space"] = await Task.Run(() => GetFreeDiskSpace(), cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            LogMetricsError(_logger, ex.Message, ex);
        }

        return metrics;
    }

    private Dictionary<string, double> CollectProcessMetrics()
    {
        var metrics = new Dictionary<string, double>();

        try
        {
            metrics["process_cpu_usage"] = _currentProcess.TotalProcessorTime.TotalMilliseconds;
            metrics["process_memory_usage"] = _currentProcess.WorkingSet64;
            metrics["process_thread_count"] = _currentProcess.Threads.Count;
            metrics["process_handle_count"] = _currentProcess.HandleCount;

            if (_options.EnableDetailedMetrics)
            {
                metrics["process_privileged_time"] = _currentProcess.PrivilegedProcessorTime.TotalMilliseconds;
                metrics["process_user_time"] = _currentProcess.UserProcessorTime.TotalMilliseconds;
                metrics["process_peak_memory"] = _currentProcess.PeakWorkingSet64;
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            LogMetricsError(_logger, ex.Message, ex);
        }

        return metrics;
    }

    private static async Task<double> GetCpuUsageAsync(CancellationToken cancellationToken)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            using var process = Process.GetCurrentProcess();
            var startTime = DateTime.UtcNow;
            var startCpuUsage = process.TotalProcessorTime;

            await Task.Delay(1000, cancellationToken).ConfigureAwait(false);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = process.TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds * Environment.ProcessorCount;

            return (cpuUsedMs / totalMsPassed) * 100.0;
        }

        return 0.0; // Implement for other platforms
    }

    private static async Task<double> GetMemoryUsageAsync(CancellationToken cancellationToken)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var info = new PerformanceCounter("Memory", "Available MBytes", true);
            var totalMemory = await Task.Run(() => info.NextValue(), cancellationToken).ConfigureAwait(false);
            return (totalMemory / Environment.SystemPageSize) * 100.0;
        }

        return 0.0; // Implement for other platforms
    }

    private static async Task<double> GetDiskUsageAsync(CancellationToken cancellationToken)
    {
        var drive = new DriveInfo(Path.GetPathRoot(Environment.CurrentDirectory)!);
        var totalSize = drive.TotalSize;
        var freeSpace = drive.AvailableFreeSpace;
        return ((totalSize - freeSpace) / (double)totalSize) * 100.0;
    }

    private static double GetCpuFrequency() => 0.0; // Implement platform-specific logic
    private static long GetTotalMemory() => 0L; // Implement platform-specific logic
    private static long GetAvailableMemory() => 0L; // Implement platform-specific logic
    private static long GetTotalDiskSpace() => 0L; // Implement platform-specific logic
    private static long GetFreeDiskSpace() => 0L; // Implement platform-specific logic

    private void StoreMetricsInHistory(Dictionary<string, object> metrics)
    {
        var remediationMetrics = new RemediationMetrics
        {
            MetricsId = Guid.NewGuid().ToString(),
            RemediationId = Guid.NewGuid().ToString(), // System-wide metrics
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow,
            Timestamp = DateTime.UtcNow
        };

        foreach (var (key, value) in metrics)
        {
            if (value is IConvertible convertible)
            {
                remediationMetrics.AddValue(key, Convert.ToDouble(convertible, System.Globalization.CultureInfo.InvariantCulture));
            }
            else
            {
                remediationMetrics.AddLabel(key, value?.ToString() ?? string.Empty);
            }
        }

        lock (_lock)
        {
            if (!_metricsHistory.TryGetValue(remediationMetrics.RemediationId, out var history))
            {
                history = new List<RemediationMetrics>();
                _metricsHistory[remediationMetrics.RemediationId] = history;
            }
            history.Add(remediationMetrics);
        }
    }

    private Dictionary<string, double> CalculateMetricTrends()
    {
        var trends = new Dictionary<string, double>();

        foreach (var (remediationId, history) in _metricsHistory)
        {
            if (history.Count < 2) continue;

            var orderedMetrics = history.OrderBy(m => m.Timestamp).ToList();
            var first = orderedMetrics.First();
            var last = orderedMetrics.Last();
            var timeSpan = (last.Timestamp - first.Timestamp).TotalSeconds;

            foreach (var (key, value) in last.Values)
            {
                if (first.Values.TryGetValue(key, out var firstValue))
                {
                    var trend = (value - firstValue) / timeSpan;
                    trends[key] = trend;
                }
            }
        }

        return trends;
    }

    public Task<ValidationResult> ValidateMetricsAsync(RemediationMetrics metrics)
    {
        ArgumentNullException.ThrowIfNull(metrics);
        ThrowIfDisposed();

        var errors = new List<ValidationError>();
        var warnings = new List<ValidationWarning>();

        foreach (var (key, value) in metrics.Values)
        {
            if (_options.MetricThresholds.TryGetValue(key, out var threshold) && value > threshold)
            {
                errors.Add(new ValidationError
                {
                    Code = "MetricThresholdExceeded",
                    Message = $"Metric '{key}' value {value} exceeds threshold {threshold}",
                    Severity = SeverityLevel.Warning,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        var result = new ValidationResult
        {
            IsValid = errors.Count == 0,
            Timestamp = DateTime.UtcNow,
            Errors = new ReadOnlyCollection<ValidationError>(errors),
            Warnings = new ReadOnlyCollection<ValidationWarning>(warnings)
        };

        return Task.FromResult(result);
    }

    public void Dispose()
    {
        if (_disposed) return;

        _cts.Cancel();
        _collectionTimer.Dispose();
        _currentProcess.Dispose();
        _cts.Dispose();

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RemediationMetricsCollector));
    }

    public Task RecordMetricAsync(string remediationId, string metricName, object value)
    {
        var metrics = new RemediationMetrics
        {
            MetricsId = Guid.NewGuid().ToString(),
            RemediationId = remediationId,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow,
            Timestamp = DateTime.UtcNow
        };

        if (value is IConvertible convertible)
        {
            metrics.Values[metricName] = Convert.ToDouble(convertible, System.Globalization.CultureInfo.InvariantCulture);
        }
        else
        {
            metrics.Labels[metricName] = value?.ToString() ?? string.Empty;
        }

        return RecordRemediationMetricsAsync(metrics);
    }

    public Task<RemediationMetrics> GetRemediationMetricsAsync(string remediationId)
    {
        if (_metricsHistory.TryGetValue(remediationId, out var history) && history.Any())
        {
            return Task.FromResult(history.Last());
        }

        return Task.FromResult(new RemediationMetrics
        {
            MetricsId = Guid.NewGuid().ToString(),
            RemediationId = remediationId,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow,
            Timestamp = DateTime.UtcNow
        });
    }

    public async Task<RemediationMetrics> GetAggregatedMetricsAsync(string remediationId, TimeRange timeRange)
    {
        var metrics = new RemediationMetrics
        {
            MetricsId = Guid.NewGuid().ToString(),
            RemediationId = remediationId,
            StartTime = timeRange.Start,
            EndTime = timeRange.End,
            Timestamp = DateTime.UtcNow
        };

        if (_metricsHistory.TryGetValue(remediationId, out var history))
        {
            var filteredMetrics = history.Where(m =>
                m.Timestamp >= timeRange.Start && m.Timestamp <= timeRange.End);

            foreach (var metric in filteredMetrics)
            {
                foreach (var (key, value) in metric.Values)
                {
                    if (!metrics.Values.ContainsKey(key))
                    {
                        metrics.Values[key] = value;
                    }
                    else
                    {
                        metrics.Values[key] += value;
                    }
                }

                foreach (var (key, value) in metric.Labels)
                {
                    metrics.Labels[key] = value;
                }
            }
        }

        return metrics;
    }

    public Task<Dictionary<string, object>> CollectMetricsAsync(ErrorContext context)
    {
        var metrics = new Dictionary<string, object>();

        // Collect system metrics
        var systemMetrics = await CollectSystemMetricsAsync().ConfigureAwait(false);
        foreach (var (key, value) in systemMetrics)
        {
            metrics[$"system.{key}"] = value;
        }

        // Collect process metrics
        var processMetrics = CollectProcessMetrics();
        foreach (var (key, value) in processMetrics)
        {
            metrics[$"process.{key}"] = value;
        }

        // Collect network metrics
        var networkMetrics = await CollectNetworkMetricsAsync(context).ConfigureAwait(false);
        foreach (var (key, value) in networkMetrics)
        {
            metrics[$"network.{key}"] = value;
        }

        // Collect error metrics
        var errorMetrics = await CollectErrorMetricsAsync(context).ConfigureAwait(false);
        foreach (var (key, value) in errorMetrics)
        {
            metrics[$"error.{key}"] = value;
        }

        return Task.FromResult(metrics);
    }

    Task<Dictionary<string, List<Models.Metrics.MetricValue>>> IRemediationMetricsCollector.GetMetricsHistoryAsync(string remediationId)
    {
        ArgumentNullException.ThrowIfNull(remediationId);
        ThrowIfDisposed();

        var history = new Dictionary<string, List<Models.Metrics.MetricValue>>();

        if (_metricsHistory.TryGetValue(remediationId, out var metrics))
        {
            foreach (var metric in metrics)
            {
                foreach (var (key, value) in metric.Values)
                {
                    if (!history.ContainsKey(key))
                    {
                        history[key] = new List<Models.Metrics.MetricValue>();
                    }

                    history[key].Add(new Models.Metrics.MetricValue
                    {
                        Value = value,
                        Timestamp = metric.Timestamp
                    });
                }
            }
        }

        return Task.FromResult(history);
    }

    Task<RemediationMetrics> IRemediationMetricsCollector.GetAggregatedMetricsAsync(TimeRange range)
    {
        ArgumentNullException.ThrowIfNull(range);
        ThrowIfDisposed();

        var aggregatedMetrics = new RemediationMetrics
        {
            MetricsId = Guid.NewGuid().ToString(),
            StartTime = range.Start,
            EndTime = range.End,
            Timestamp = DateTime.UtcNow
        };

        foreach (var history in _metricsHistory.Values)
        {
            var filteredMetrics = history.Where(m =>
                m.Timestamp >= range.Start && m.Timestamp <= range.End);

            foreach (var metric in filteredMetrics)
            {
                foreach (var (key, value) in metric.Values)
                {
                    if (!aggregatedMetrics.Values.ContainsKey(key))
                    {
                        aggregatedMetrics.Values[key] = value;
                    }
                    else
                    {
                        aggregatedMetrics.Values[key] += value;
                    }
                }

                foreach (var (key, value) in metric.Labels)
                {
                    aggregatedMetrics.Labels[key] = value;
                }
            }
        }

        return Task.FromResult(aggregatedMetrics);
    }
} 
