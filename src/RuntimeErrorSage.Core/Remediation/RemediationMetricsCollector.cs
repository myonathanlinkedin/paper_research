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
using ValidationSeverity = RuntimeErrorSage.Core.Models.Validation.ValidationSeverity;

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

    public async Task RecordStepMetricsAsync(StepMetrics metrics)
    {
        ArgumentNullException.ThrowIfNull(metrics);
        ThrowIfDisposed();

        var metricsStepMetrics = new MetricsStepMetrics
        {
            StepId = metrics.StepId,
            ActionId = metrics.StepName,
            DurationMs = metrics.DurationMs,
            Status = metrics.Status,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMilliseconds(metrics.DurationMs)
        };

        _stepMetricsHistory.AddOrUpdate(
            metricsStepMetrics.StepId,
            new List<MetricsStepMetrics> { metricsStepMetrics },
            (_, list) => { list.Add(metricsStepMetrics); return list; });
        await Task.CompletedTask;
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
                        history[key] = new List<MetricValue>();
                    history[key].Add(new MetricValue
                    {
                        Name = key,
                        Value = value,
                        Timestamp = metric.Timestamp,
                        Labels = metric.Labels
                    });
                }
            }
        }
        return await Task.FromResult(history);
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

    private async Task<Dictionary<string, object>> CollectSystemMetricsAsync(CancellationToken cancellationToken)
    {
        var metrics = new Dictionary<string, object>
        {
            ["cpu_usage"] = await GetCpuUsageAsync(cancellationToken),
            ["memory_usage"] = await GetMemoryUsageAsync(cancellationToken),
            ["disk_usage"] = await GetDiskUsageAsync(cancellationToken),
            ["cpu_frequency"] = GetCpuFrequency(),
            ["total_memory"] = GetTotalMemory(),
            ["available_memory"] = GetAvailableMemory(),
            ["total_disk_space"] = GetTotalDiskSpace(),
            ["free_disk_space"] = GetFreeDiskSpace()
        };

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
                    Severity = MapSeverity(SeverityLevel.Warning),
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        var result = new ValidationResult(errors, warnings)
        {
            IsValid = errors.Count == 0,
            Message = errors.Count == 0 ? "Metrics valid" : "Metrics validation failed"
        };

        return Task.FromResult(result);
    }

    private static ValidationSeverity MapSeverity(SeverityLevel level)
    {
        return level switch
        {
            SeverityLevel.Info => ValidationSeverity.Info,
            SeverityLevel.Low => ValidationSeverity.Warning,
            SeverityLevel.Medium => ValidationSeverity.Error,
            SeverityLevel.High => ValidationSeverity.Critical,
            SeverityLevel.Critical => ValidationSeverity.Critical,
            _ => ValidationSeverity.Error
        };
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

    public async Task<RemediationMetrics> GetMetricsAsync(string remediationId)
    {
        ArgumentNullException.ThrowIfNull(remediationId);
        ThrowIfDisposed();

        if (_metricsHistory.TryGetValue(remediationId, out var metrics) && metrics.Count > 0)
        {
            return await Task.FromResult(metrics[^1]);
        }

        return await Task.FromResult<RemediationMetrics>(null);
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

    public async Task<Dictionary<string, object>> CollectMetricsAsync(ErrorContext context)
    {
        var metrics = new Dictionary<string, object>();

        // Collect system metrics
        var systemMetrics = await CollectSystemMetricsAsync(_cts.Token).ConfigureAwait(false);
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

        return metrics;
    }

    private async Task<Dictionary<string, object>> CollectNetworkMetricsAsync(ErrorContext context)
    {
        var metrics = new Dictionary<string, object>();
        try
        {
            // Add network metrics collection logic here
            metrics["network_latency"] = 0.0;
            metrics["network_errors"] = 0;
            metrics["network_connections"] = 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error collecting network metrics");
        }
        return metrics;
    }

    private async Task<Dictionary<string, object>> CollectErrorMetricsAsync(ErrorContext context)
    {
        var metrics = new Dictionary<string, object>();
        try
        {
            metrics["error_count"] = 1;
            metrics["error_type"] = context.ErrorType;
            metrics["error_severity"] = context.Severity.ToString();
            metrics["error_timestamp"] = context.Timestamp;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error collecting error metrics");
        }
        return metrics;
    }

    private Dictionary<string, object> CollectProcessMetrics()
    {
        var metrics = new Dictionary<string, object>();
        try
        {
            metrics["process_id"] = _currentProcess.Id;
            metrics["process_cpu_time"] = _currentProcess.TotalProcessorTime.TotalMilliseconds;
            metrics["process_memory"] = _currentProcess.WorkingSet64;
            metrics["process_threads"] = _currentProcess.Threads.Count;
            metrics["process_handles"] = _currentProcess.HandleCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error collecting process metrics");
        }
        return metrics;
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

    public bool IsEnabled => !_disposed;
    public string Name => "RuntimeErrorSage Remediation Metrics Collector";
    public string Version => "1.0.0";
} 
