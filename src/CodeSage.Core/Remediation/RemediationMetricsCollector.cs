using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CodeSage.Core.Interfaces;
using CodeSage.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeSage.Core.Models.Error;
using CodeSage.Core.Models.Metrics;
using CodeSage.Core.Remediation.Interfaces;

namespace CodeSage.Core.Remediation;

// Configuration for RemediationMetricsCollectorOptions is loaded from appsettings.json via IOptions<RemediationMetricsCollectorOptions>.
// See the corresponding section in appsettings.json for configuration keys.

// Example appsettings.json section:
// "RemediationMetricsCollectorOptions": {
//   "CollectionInterval": "00:00:01",
//   "CollectionTimeout": "00:05:00",
//   "EnableDetailedMetrics": true,
//   "MetricThresholds": {
//     "cpu.usage": 80.0,
//     "memory.usage": 85.0,
//     "disk.usage": 90.0,
//     "network.latency": 100.0,
//     "error.rate": 5.0
//   }
// }

public class RemediationMetricsCollectorOptions
{
    /// <summary>
    /// Gets or sets the interval for metrics collection. Loaded from configuration.
    /// </summary>
    public TimeSpan CollectionInterval { get; set; }
    /// <summary>
    /// Gets or sets the timeout for metrics collection. Loaded from configuration.
    /// </summary>
    public TimeSpan CollectionTimeout { get; set; }
    /// <summary>
    /// Gets or sets whether to enable detailed metrics. Loaded from configuration.
    /// </summary>
    public bool EnableDetailedMetrics { get; set; }
    /// <summary>
    /// Gets or sets the metric thresholds. Loaded from configuration.
    /// </summary>
    public Dictionary<string, double> MetricThresholds { get; set; }
}

public class RemediationMetricsCollector : IRemediationMetricsCollector, IDisposable
{
    private readonly ILogger<RemediationMetricsCollector> _logger;
    private readonly RemediationMetricsCollectorOptions _options;
    private readonly Process _currentProcess;
    private readonly Timer _collectionTimer;
    private readonly Dictionary<string, List<MetricValue>> _metricsHistory;
    private readonly object _lock = new();

    public RemediationMetricsCollector(
        ILogger<RemediationMetricsCollector> logger,
        IOptions<RemediationMetricsCollectorOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _currentProcess = Process.GetCurrentProcess();
        _metricsHistory = new Dictionary<string, List<MetricValue>>();

        // Setup collection timer
        _collectionTimer = new Timer(
            async _ => await CollectMetricsAsync(),
            null,
            TimeSpan.Zero,
            _options.CollectionInterval);
    }

    public async Task<Dictionary<string, object>> CollectMetricsAsync(ErrorContext context)
    {
        _logger.LogInformation("Collecting remediation metrics for correlation ID: {CorrelationId}", context.CorrelationId);

        try
        {
            var metrics = new Dictionary<string, object>
            {
                ["Timestamp"] = DateTime.UtcNow,
                ["ServiceName"] = context.ServiceName,
                ["OperationName"] = context.OperationName,
                ["ErrorType"] = context.Exception?.GetType().Name,
                ["CorrelationId"] = context.CorrelationId
            };

            // Add any additional metrics from context
            if (context.Metadata != null)
            {
                foreach (var kvp in context.Metadata)
                {
                    metrics[$"Context_{kvp.Key}"] = kvp.Value;
                }
            }

            return metrics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error collecting metrics for context {CorrelationId}", context.CorrelationId);
            throw;
        }
    }

    public async Task RecordMetricAsync(string remediationId, string metricName, object value)
    {
        _logger.LogInformation("Recording metric {MetricName} with value {Value} for remediation {RemediationId}", metricName, value, remediationId);
        try
        {
            if (!_metricsHistory.ContainsKey(remediationId))
            {
                _metricsHistory[remediationId] = new List<MetricValue>();
            }

            _metricsHistory[remediationId].Add(new MetricValue
            {
                Name = metricName,
                Value = value,
                Timestamp = DateTime.UtcNow
            });

            _logger.LogDebug("Recorded metric {MetricName} for remediation {RemediationId}", metricName, remediationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording metric {MetricName} for remediation {RemediationId}", metricName, remediationId);
            throw;
        }
    }

    public async Task<Dictionary<string, List<MetricValue>>> GetMetricsHistoryAsync(string remediationId)
    {
        _logger.LogInformation("Retrieving metric history for remediation {RemediationId}", remediationId);
        try
        {
            if (_metricsHistory.TryGetValue(remediationId, out var history))
            {
                return new Dictionary<string, List<MetricValue>>
                {
                    [remediationId] = history
                };
            }
            return new Dictionary<string, List<MetricValue>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting metrics history for remediation {RemediationId}", remediationId);
            throw;
        }
    }

    private async Task<Dictionary<string, object>> CollectSystemMetricsAsync()
    {
        var metrics = new Dictionary<string, object>();

        try
        {
            // CPU usage
            var cpuUsage = await GetCpuUsageAsync();
            metrics["cpu.usage"] = cpuUsage;
            metrics["cpu.usage.threshold"] = _options.MetricThresholds["cpu.usage"];

            // Memory usage
            var memoryUsage = await GetMemoryUsageAsync();
            metrics["memory.usage"] = memoryUsage;
            metrics["memory.usage.threshold"] = _options.MetricThresholds["memory.usage"];

            // Disk usage
            var diskUsage = await GetDiskUsageAsync();
            metrics["disk.usage"] = diskUsage;
            metrics["disk.usage.threshold"] = _options.MetricThresholds["disk.usage"];

            if (_options.EnableDetailedMetrics)
            {
                // CPU details
                metrics["cpu.cores"] = Environment.ProcessorCount;
                metrics["cpu.frequency"] = GetCpuFrequency();

                // Memory details
                metrics["memory.total"] = GetTotalMemory();
                metrics["memory.available"] = GetAvailableMemory();
                metrics["memory.cached"] = GetCachedMemory();

                // Disk details
                metrics["disk.total"] = GetTotalDiskSpace();
                metrics["disk.free"] = GetFreeDiskSpace();
                metrics["disk.reads"] = GetDiskReads();
                metrics["disk.writes"] = GetDiskWrites();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error collecting system metrics");
        }

        return metrics;
    }

    private async Task<Dictionary<string, object>> CollectProcessMetricsAsync()
    {
        var metrics = new Dictionary<string, object>();

        try
        {
            _currentProcess.Refresh();

            // Process CPU
            metrics["process.cpu"] = _currentProcess.TotalProcessorTime.TotalMilliseconds;
            metrics["process.cpu.user"] = _currentProcess.UserProcessorTime.TotalMilliseconds;
            metrics["process.cpu.privileged"] = _currentProcess.PrivilegedProcessorTime.TotalMilliseconds;

            // Process Memory
            metrics["process.memory.working"] = _currentProcess.WorkingSet64;
            metrics["process.memory.private"] = _currentProcess.PrivateMemorySize64;
            metrics["process.memory.virtual"] = _currentProcess.VirtualMemorySize64;

            // Process Threads
            metrics["process.threads"] = _currentProcess.Threads.Count;
            metrics["process.handles"] = _currentProcess.HandleCount;

            if (_options.EnableDetailedMetrics)
            {
                // Process Details
                metrics["process.startTime"] = _currentProcess.StartTime;
                metrics["process.uptime"] = (DateTime.Now - _currentProcess.StartTime).TotalSeconds;
                metrics["process.priority"] = _currentProcess.BasePriority;
                metrics["process.responding"] = !_currentProcess.Responding;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error collecting process metrics");
        }

        return metrics;
    }

    private async Task<Dictionary<string, object>> CollectNetworkMetricsAsync(ErrorContext context)
    {
        var metrics = new Dictionary<string, object>();

        try
        {
            // Network latency
            var latency = await GetNetworkLatencyAsync(context.ServiceName);
            metrics["network.latency"] = latency;
            metrics["network.latency.threshold"] = _options.MetricThresholds["network.latency"];

            if (_options.EnableDetailedMetrics)
            {
                // Network details
                metrics["network.connections"] = GetNetworkConnections();
                metrics["network.bytes.sent"] = GetNetworkBytesSent();
                metrics["network.bytes.received"] = GetNetworkBytesReceived();
                metrics["network.packets.sent"] = GetNetworkPacketsSent();
                metrics["network.packets.received"] = GetNetworkPacketsReceived();
                metrics["network.errors"] = GetNetworkErrors();
            }
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
            // Error rate
            var errorRate = await GetErrorRateAsync(context);
            metrics["error.rate"] = errorRate;
            metrics["error.rate.threshold"] = _options.MetricThresholds["error.rate"];

            if (_options.EnableDetailedMetrics)
            {
                // Error details
                metrics["error.count"] = GetErrorCount(context);
                metrics["error.distribution"] = GetErrorDistribution(context);
                metrics["error.impact"] = GetErrorImpact(context);
                metrics["error.recovery"] = GetErrorRecoveryTime(context);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error collecting error metrics");
        }

        return metrics;
    }

    private void StoreMetricsInHistory(Dictionary<string, object> metrics)
    {
        var timestamp = DateTime.UtcNow;
        foreach (var (key, value) in metrics)
        {
            if (value is IConvertible convertible)
            {
                var metricValue = new MetricValue
                {
                    Timestamp = timestamp,
                    Value = Convert.ToDouble(convertible)
                };

                _metricsHistory.AddOrUpdate(
                    key,
                    new List<MetricValue> { metricValue },
                    (_, list) =>
                    {
                        lock (_lock)
                        {
                            list.Add(metricValue);
                            // Keep only last hour of metrics
                            var cutoff = timestamp.AddHours(-1);
                            list.RemoveAll(m => m.Timestamp < cutoff);
                            return list;
                        }
                    });
            }
        }
    }

    private Dictionary<string, double> CalculateMetricTrends()
    {
        var trends = new Dictionary<string, double>();

        foreach (var (key, values) in _metricsHistory)
        {
            if (values.Count >= 2)
            {
                var recentValues = values
                    .OrderByDescending(v => v.Timestamp)
                    .Take(10)
                    .ToList();

                if (recentValues.Count >= 2)
                {
                    var firstValue = recentValues.Last().Value;
                    var lastValue = recentValues.First().Value;
                    var timeSpan = recentValues.First().Timestamp - recentValues.Last().Timestamp;

                    if (timeSpan.TotalSeconds > 0)
                    {
                        var trend = (lastValue - firstValue) / timeSpan.TotalSeconds;
                        trends[key] = trend;
                    }
                }
            }
        }

        return trends;
    }

    private async Task<double> GetCpuUsageAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = _currentProcess.TotalProcessorTime;
            await Task.Delay(1000);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = _currentProcess.TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds * Environment.ProcessorCount;

            return (cpuUsedMs / totalMsPassed) * 100.0;
        }
        else
        {
            // Implement for other platforms
            return 0.0;
        }
    }

    private async Task<double> GetMemoryUsageAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var computerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
            var totalPhysicalMemory = computerInfo.TotalPhysicalMemory;
            var availablePhysicalMemory = computerInfo.AvailablePhysicalMemory;
            return ((totalPhysicalMemory - availablePhysicalMemory) / (double)totalPhysicalMemory) * 100.0;
        }
        else
        {
            // Implement for other platforms
            return 0.0;
        }
    }

    private async Task<double> GetDiskUsageAsync()
    {
        var drive = new DriveInfo(Path.GetPathRoot(Environment.CurrentDirectory)!);
        var totalSize = drive.TotalSize;
        var freeSpace = drive.AvailableFreeSpace;
        return ((totalSize - freeSpace) / (double)totalSize) * 100.0;
    }

    private async Task<double> GetNetworkLatencyAsync(string serviceName)
    {
        // Implement network latency measurement
        await Task.Delay(100); // Simulate measurement
        return 50.0; // Example value
    }

    private async Task<double> GetErrorRateAsync(ErrorContext context)
    {
        // Implement error rate calculation
        await Task.Delay(100); // Simulate calculation
        return 2.5; // Example value
    }

    private double GetCpuFrequency()
    {
        // Implement CPU frequency retrieval
        return 0.0;
    }

    private long GetTotalMemory()
    {
        // Implement total memory retrieval
        return 0;
    }

    private long GetAvailableMemory()
    {
        // Implement available memory retrieval
        return 0;
    }

    private long GetCachedMemory()
    {
        // Implement cached memory retrieval
        return 0;
    }

    private long GetTotalDiskSpace()
    {
        // Implement total disk space retrieval
        return 0;
    }

    private long GetFreeDiskSpace()
    {
        // Implement free disk space retrieval
        return 0;
    }

    private long GetDiskReads()
    {
        // Implement disk reads retrieval
        return 0;
    }

    private long GetDiskWrites()
    {
        // Implement disk writes retrieval
        return 0;
    }

    private int GetNetworkConnections()
    {
        // Implement network connections retrieval
        return 0;
    }

    private long GetNetworkBytesSent()
    {
        // Implement network bytes sent retrieval
        return 0;
    }

    private long GetNetworkBytesReceived()
    {
        // Implement network bytes received retrieval
        return 0;
    }

    private long GetNetworkPacketsSent()
    {
        // Implement network packets sent retrieval
        return 0;
    }

    private long GetNetworkPacketsReceived()
    {
        // Implement network packets received retrieval
        return 0;
    }

    private int GetNetworkErrors()
    {
        // Implement network errors retrieval
        return 0;
    }

    private int GetErrorCount(ErrorContext context)
    {
        // Implement error count retrieval
        return 0;
    }

    private Dictionary<string, int> GetErrorDistribution(ErrorContext context)
    {
        // Implement error distribution retrieval
        return new();
    }

    private double GetErrorImpact(ErrorContext context)
    {
        // Implement error impact calculation
        return 0.0;
    }

    private TimeSpan GetErrorRecoveryTime(ErrorContext context)
    {
        // Implement error recovery time retrieval
        return TimeSpan.Zero;
    }

    public void Dispose()
    {
        _collectionTimer.Dispose();
        _currentProcess.Dispose();
    }
}

public class MetricValue
{
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
    public string Name { get; set; }
}

public class MetricsCollectionException : Exception
{
    public MetricsCollectionException(string message, Exception inner) : base(message, inner) { }
    public MetricsCollectionException(string message) : base(message) { }
} 