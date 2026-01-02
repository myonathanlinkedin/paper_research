using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Application.Health.Exceptions;
using RuntimeErrorSage.Application.Health.Interfaces;
using RuntimeErrorSage.Domain.Models.Health;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Options;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using RuntimeErrorSage.Application.Health.Models;

namespace RuntimeErrorSage.Application.Health;

/// <summary>
/// Represents a health status report
/// </summary>
public class HealthStatus
{
    /// <summary>
    /// Gets or sets the service name
    /// </summary>
    public string ServiceName { get; set; }
    
    /// <summary>
    /// Gets or sets whether the service is healthy
    /// </summary>
    public bool IsHealthy { get; set; }
    
    /// <summary>
    /// Gets or sets the health score (0-1)
    /// </summary>
    public double HealthScore { get; set; }
    
    /// <summary>
    /// Gets or sets the metrics
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; }
    
    /// <summary>
    /// Gets or sets the prediction
    /// </summary>
    public HealthPrediction Prediction { get; set; }
    
    /// <summary>
    /// Gets or sets the timestamp
    /// </summary>
    public DateTime Timestamp { get; set; }
}

public class ServiceHealthCheck : IHealthCheck, IDisposable
{
    private readonly ILogger<ServiceHealthCheck> _logger;
    private readonly HttpClient _httpClient;
    private readonly ServiceHealthCheckOptions _options;
    private readonly ConcurrentDictionary<string, ServiceHealthStatus> _serviceStatus;
    private readonly Timer _healthCheckTimer;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly object _lock = new();
    private readonly ConcurrentDictionary<string, Queue<HealthMetric>> _metricHistory;
    private readonly ConcurrentDictionary<string, IHealthCheckProvider> _customHealthChecks;
    private readonly object _metricLock = new();
    private readonly RuntimeErrorSageOptions _RuntimeErrorSageOptions;
    private readonly ConcurrentDictionary<string, DateTime> _lastCheckTime;
    private readonly ConcurrentDictionary<string, string> _errorMessages;
    private readonly IEnumerable<ICustomHealthCheck> _customChecks;

    public ServiceHealthCheck(
        ILogger<ServiceHealthCheck> logger,
        HttpClient httpClient,
        IOptions<ServiceHealthCheckOptions> options,
        IOptions<RuntimeErrorSageOptions> RuntimeErrorSageOptions,
        IEnumerable<ICustomHealthCheck> customChecks)
    {
        _logger = logger;
        _httpClient = httpClient;
        _options = options.Value;
        _RuntimeErrorSageOptions = RuntimeErrorSageOptions.Value;
        _serviceStatus = new();
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _metricHistory = new();
        _customHealthChecks = new();
        _lastCheckTime = new();
        _errorMessages = new();
        _customChecks = customChecks;

        // Initialize service status
        foreach (var endpoint in _options.ServiceEndpoints)
        {
            _serviceStatus[endpoint] = new ServiceHealthStatus
            {
                Endpoint = endpoint,
                IsHealthy = false,
                LastCheck = DateTime.MinValue,
                ConsecutiveFailures = 0,
                ConsecutiveSuccesses = 0
            };
        }

        // Start health check timer
        _healthCheckTimer = new Timer(
            CheckServicesHealthAsync,
            null,
            TimeSpan.Zero,
            _options.HealthCheckInterval);

        // Initialize metric history
        foreach (var metric in _options.MetricThresholds.Keys)
        {
            _metricHistory[metric] = new Queue<HealthMetric>();
        }

        if (customChecks != null)
        {
            foreach (var check in customChecks)
            {
                _customHealthChecks[check.Name] = (IHealthCheckProvider)check;
            }
        }
    }

    public async Task<List<string>> GetHealthyServicesAsync()
    {
        return _serviceStatus
            .Where(s => s.Value.IsHealthy)
            .Select(s => s.Value.Endpoint)
            .ToList();
    }

    private async void CheckServicesHealthAsync(object? state)
    {
        try
        {
            var tasks = _options.ServiceEndpoints
                .Select(CheckServiceHealthAsync)
                .ToList();

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during health check cycle");
        }
    }

    private async Task CheckServiceHealthAsync(string endpoint)
    {
        try
        {
            using var cts = new CancellationTokenSource(_options.HealthCheckTimeout);
            var response = await _httpClient.GetAsync($"{endpoint}/health", cts.Token);

            lock (_lock)
            {
                var status = _serviceStatus[endpoint];
                status.LastCheck = DateTime.UtcNow;

                if (response.IsSuccessStatusCode)
                {
                    status.ConsecutiveSuccesses++;
                    status.ConsecutiveFailures = 0;

                    if (status.ConsecutiveSuccesses >= _options.HealthyThreshold)
                    {
                        if (!status.IsHealthy)
                        {
                            _logger.LogInformation("Service {Service} is now healthy", endpoint);
                        }
                        status.IsHealthy = true;
                    }
                }
                else
                {
                    status.ConsecutiveFailures++;
                    status.ConsecutiveSuccesses = 0;

                    if (status.ConsecutiveFailures >= _options.UnhealthyThreshold)
                    {
                        if (status.IsHealthy)
                        {
                            _logger.LogWarning("Service {Service} is now unhealthy", endpoint);
                        }
                        status.IsHealthy = false;
                    }
                }

                _serviceStatus[endpoint] = status;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking health of service {Service}", endpoint);

            lock (_lock)
            {
                var status = _serviceStatus[endpoint];
                status.LastCheck = DateTime.UtcNow;
                status.ConsecutiveFailures++;
                status.ConsecutiveSuccesses = 0;

                if (status.ConsecutiveFailures >= _options.UnhealthyThreshold)
                {
                    if (status.IsHealthy)
                    {
                        _logger.LogWarning("Service {Service} is now unhealthy", endpoint);
                    }
                    status.IsHealthy = false;
                }

                _serviceStatus[endpoint] = status;
            }
        }
    }

    public async Task<HealthStatus> GetServiceHealthAsync(string serviceName)
    {
        try
        {
            var metrics = await CollectMetricsAsync(serviceName);
            var healthScore = CalculateHealthScore(metrics);
            var prediction = _options.EnablePredictiveAnalysis ? 
                await PredictHealthTrendAsync(serviceName, metrics) : null;

            var serviceStatus = _serviceStatus.GetValueOrDefault(serviceName);
            var isHealthy = serviceStatus?.IsHealthy ?? false;

            return new HealthStatus
            {
                ServiceName = serviceName,
                IsHealthy = isHealthy && healthScore >= _options.HealthScoreThreshold,
                HealthScore = healthScore,
                Metrics = metrics,
                Prediction = prediction,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting health status for service {Service}", serviceName);
            throw new HealthCheckException("Failed to get service health", ex);
        }
    }

    private async Task<Dictionary<string, double>> CollectMetricsAsync(string serviceName)
    {
        var metrics = new Dictionary<string, double>();
        var tasks = new List<Task>();

        // Collect system metrics
        tasks.Add(CollectSystemMetricsAsync(metrics));

        // Collect service-specific metrics
        tasks.Add(CollectServiceMetricsAsync(serviceName, metrics));

        // Run custom health checks
        foreach (var check in _customHealthChecks.Values)
        {
            tasks.Add(CollectCustomMetricsAsync(check, metrics));
        }

        await Task.WhenAll(tasks);
        return metrics;
    }

    private async Task CollectSystemMetricsAsync(Dictionary<string, double> metrics)
    {
        var process = Process.GetCurrentProcess();
        var drive = new DriveInfo(Path.GetPathRoot(Environment.CurrentDirectory)!);

        metrics["cpu.usage"] = await GetCpuUsageAsync(process);
        metrics["memory.usage"] = process.WorkingSet64 / (double)drive.TotalSize * 100;
        metrics["disk.usage"] = (drive.TotalSize - drive.AvailableFreeSpace) / (double)drive.TotalSize * 100;
        metrics["thread.count"] = process.Threads.Count;
        metrics["handle.count"] = process.HandleCount;
        metrics["private.memory"] = process.PrivateMemorySize64;
        metrics["virtual.memory"] = process.VirtualMemorySize64;
    }

    private async Task CollectServiceMetricsAsync(string serviceName, Dictionary<string, double> metrics)
    {
        try
        {
            using var cts = new CancellationTokenSource(_options.HealthCheckTimeout);
            var response = await _httpClient.GetAsync($"{serviceName}/metrics", cts.Token);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var serviceMetrics = await JsonSerializer.DeserializeAsync<Dictionary<string, double>>(stream, _jsonOptions);
                if (serviceMetrics != null)
                {
                    foreach (var metric in serviceMetrics)
                    {
                        metrics[$"service.{metric.Key}"] = metric.Value;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error collecting service metrics for {Service}", serviceName);
        }
    }

    private async Task CollectCustomMetricsAsync(IHealthCheckProvider check, Dictionary<string, double> metrics)
    {
        try
        {
            var customMetrics = await check.GetMetricsAsync();
            foreach (var metric in customMetrics)
            {
                metrics[$"custom.{check.Name}.{metric.Key}"] = metric.Value;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error collecting custom metrics from {Check}", check.Name);
        }
    }

    private double CalculateHealthScore(Dictionary<string, double> metrics)
    {
        var score = 1.0;
        var weightSum = 0.0;

        foreach (var (metric, value) in metrics)
        {
            if (_options.MetricThresholds.TryGetValue(metric, out var threshold))
            {
                var weight = GetMetricWeight(metric);
                var metricScore = CalculateMetricScore(value, threshold);
                score *= Math.Pow(metricScore, weight);
                weightSum += weight;
            }
        }

        return weightSum > 0 ? Math.Pow(score, 1.0 / weightSum) : 1.0;
    }

    private double GetMetricWeight(string metric)
    {
        // Assign weights based on metric importance
        return metric switch
        {
            "cpu.usage" => 2.0,
            "memory.usage" => 2.0,
            "error.rate" => 3.0,
            "response.time" => 2.0,
            _ => 1.0
        };
    }

    private double CalculateMetricScore(double value, double threshold)
    {
        // Normalize metric value to a score between 0 and 1
        return Math.Max(0, 1 - value / threshold);
    }

    private async Task<HealthPrediction?> PredictHealthTrendAsync(
        string serviceName,
        Dictionary<string, double> currentMetrics)
    {
        try
        {
            // Update metric history
            lock (_metricLock)
            {
                foreach (var (metric, value) in currentMetrics)
                {
                    if (_metricHistory.TryGetValue(metric, out var history))
                    {
                        history.Enqueue(new HealthMetric
                        {
                            Value = value,
                            Timestamp = DateTime.UtcNow
                        });

                        // Trim old metrics
                        while (history.Count > _options.MinDataPointsForPrediction)
                        {
                            history.Dequeue();
                        }
                    }
                }
            }

            // Check if we have enough data points
            if (!HasEnoughDataPoints())
            {
                return null;
            }

            // Calculate trends for each metric
            var trends = new Dictionary<string, MetricTrend>();
            foreach (var (metric, history) in _metricHistory)
            {
                if (history.Count >= _options.MinDataPointsForPrediction)
                {
                    trends[metric] = CalculateMetricTrend(history);
                }
            }

            // Predict future health score
            var prediction = PredictFutureHealthScore(trends, currentMetrics);

            // Convert MetricTrend dictionary to double dictionary for HealthPrediction
            var trendsDouble = trends.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.CurrentValue // Extract the current value from MetricTrend
            );

            return new HealthPrediction
            {
                PredictedHealthScore = prediction,
                TimeToUnhealthy = CalculateTimeToUnhealthy(trends, currentMetrics),
                Confidence = CalculatePredictionConfidence(trends),
                Trends = trendsDouble
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error predicting health trend for service {Service}", serviceName);
            return null;
        }
    }

    private bool HasEnoughDataPoints()
    {
        return _metricHistory.Values.All(h => h.Count >= _options.MinDataPointsForPrediction);
    }

    private MetricTrend CalculateMetricTrend(Queue<HealthMetric> history)
    {
        var points = history.ToList();
        var n = points.Count;
        var sumX = 0.0;
        var sumY = 0.0;
        var sumXY = 0.0;
        var sumX2 = 0.0;

        for (int i = 0; i < n; i++)
        {
            var x = (points[i].Timestamp - points[0].Timestamp).TotalMinutes;
            var y = points[i].Value;
            sumX += x;
            sumY += y;
            sumXY += x * y;
            sumX2 += x * x;
        }

        var slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
        var intercept = (sumY - slope * sumX) / n;

        return new MetricTrend
        {
            Slope = slope,
            Intercept = intercept,
            CurrentValue = points.Last().Value,
            ChangeRate = slope
        };
    }

    private double PredictFutureHealthScore(
        Dictionary<string, MetricTrend> trends,
        Dictionary<string, double> currentMetrics)
    {
        var futureMetrics = new Dictionary<string, double>(currentMetrics);
        var predictionTime = DateTime.UtcNow.Add(_options.PredictionWindow);

        foreach (var (metric, trend) in trends)
        {
            if (currentMetrics.TryGetValue(metric, out var currentValue))
            {
                var minutesToPrediction = (predictionTime - DateTime.UtcNow).TotalMinutes;
                futureMetrics[metric] = trend.Intercept + trend.Slope * minutesToPrediction;
            }
        }

        return CalculateHealthScore(futureMetrics);
    }

    private TimeSpan? CalculateTimeToUnhealthy(
        Dictionary<string, MetricTrend> trends,
        Dictionary<string, double> currentMetrics)
    {
        var minTimeToUnhealthy = double.MaxValue;

        foreach (var (metric, trend) in trends)
        {
            if (_options.MetricThresholds.TryGetValue(metric, out var threshold) &&
                currentMetrics.TryGetValue(metric, out var currentValue))
            {
                if (trend.Slope > 0 && currentValue < threshold)
                {
                    var timeToThreshold = (threshold - currentValue) / trend.Slope;
                    if (timeToThreshold > 0 && timeToThreshold < minTimeToUnhealthy)
                    {
                        minTimeToUnhealthy = timeToThreshold;
                    }
                }
            }
        }

        return minTimeToUnhealthy < double.MaxValue ?
            TimeSpan.FromMinutes(minTimeToUnhealthy) : null;
    }

    private double CalculatePredictionConfidence(Dictionary<string, MetricTrend> trends)
    {
        // Calculate confidence based on:
        // 1. Number of data points
        // 2. Trend stability
        // 3. Metric coverage
        var dataPointConfidence = Math.Min(1.0, 
            _metricHistory.Values.Average(h => h.Count) / _options.MinDataPointsForPrediction);

        var trendStability = trends.Values.Average(t => 
            Math.Exp(-Math.Abs(t.Slope))); // Exponential decay based on slope

        var metricCoverage = (double)trends.Count / _options.MetricThresholds.Count;

        return (dataPointConfidence + trendStability + metricCoverage) / 3.0;
    }

    private async Task<double> GetCpuUsageAsync(Process process)
    {
        var startTime = DateTime.UtcNow;
        var startCpuUsage = process.TotalProcessorTime;
        await Task.Delay(1000);

        var endTime = DateTime.UtcNow;
        var endCpuUsage = process.TotalProcessorTime;
        var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
        var totalMsPassed = (endTime - startTime).TotalMilliseconds * Environment.ProcessorCount;

        return cpuUsedMs / totalMsPassed * 100.0;
    }

    public void Dispose()
    {
        _healthCheckTimer.Dispose();
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Running service health checks");

        var componentResults = new Dictionary<string, HealthReportEntry>();
        var overallStatus = Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy;

        foreach (var customCheck in _customChecks)
        {
            try
            {
                var isHealthy = await customCheck.CheckHealthAsync();
                var status = isHealthy ? 
                    Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy : 
                    Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy;
                    
                componentResults[customCheck.Name] = new HealthReportEntry(
                    status,
                    $"{customCheck.Name} is {(isHealthy ? "healthy" : "unhealthy")}",
                    TimeSpan.Zero, // TODO: Measure duration
                    null, // TODO: Add exception if any
                    null);

                if (status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy)
                {
                    overallStatus = Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running custom health check: {Name}", customCheck.Name);
                componentResults[customCheck.Name] = new HealthReportEntry(
                    Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
                    $"Error running {customCheck.Name}: {ex.Message}",
                    TimeSpan.Zero, // TODO: Measure duration
                    ex,
                    null);
                overallStatus = Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy;
            }
        }

        // Aggregate results
        var description = $"Service is {overallStatus.ToString().ToLower()}";
        var report = new HealthReport(componentResults, TimeSpan.Zero); // TODO: Measure total duration

        return new HealthCheckResult(
            overallStatus,
            description,
            null,
            report.Entries.ToDictionary(e => e.Key, e => e.Value as object)); // Use the component results
    }
} 

