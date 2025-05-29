using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Metrics;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Metrics for test suite execution.
/// </summary>
public class TestSuiteMetrics
{
    private readonly Collection<PerformanceMetric> _metrics;
    private readonly Dictionary<string, Collection<double>> _metricHistory;
    private readonly Dictionary<string, double> _metricDistribution;
    private readonly Dictionary<string, double> _metricSuccessRate;
    private readonly Dictionary<string, double> _trendAnalysis;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuiteMetrics"/> class.
    /// </summary>
    public TestSuiteMetrics()
    {
        _metrics = new Collection<PerformanceMetric>();
        _metricHistory = new Dictionary<string, Collection<double>>();
        _metricDistribution = new Dictionary<string, double>();
        _metricSuccessRate = new Dictionary<string, double>();
        _trendAnalysis = new Dictionary<string, double>();
    }

    /// <summary>
    /// Gets the average duration.
    /// </summary>
    public TimeSpan AverageDuration => TimeSpan.FromMilliseconds(_metrics.Average(m => m.DurationMs));

    /// <summary>
    /// Gets the minimum duration.
    /// </summary>
    public TimeSpan MinDuration => TimeSpan.FromMilliseconds(_metrics.Min(m => m.DurationMs));

    /// <summary>
    /// Gets the maximum duration.
    /// </summary>
    public TimeSpan MaxDuration => TimeSpan.FromMilliseconds(_metrics.Max(m => m.DurationMs));

    /// <summary>
    /// Gets the average memory usage.
    /// </summary>
    public long AverageMemoryUsage => (long)_metrics.Average(m => m.MemoryUsage);

    /// <summary>
    /// Gets the minimum memory usage.
    /// </summary>
    public long MinMemoryUsage => _metrics.Min(m => m.MemoryUsage);

    /// <summary>
    /// Gets the maximum memory usage.
    /// </summary>
    public long MaxMemoryUsage => _metrics.Max(m => m.MemoryUsage);

    /// <summary>
    /// Gets the average CPU usage.
    /// </summary>
    public double AverageCpuUsage => _metrics.Average(m => m.CpuUsage);

    /// <summary>
    /// Gets the minimum CPU usage.
    /// </summary>
    public double MinCpuUsage => _metrics.Min(m => m.CpuUsage);

    /// <summary>
    /// Gets the maximum CPU usage.
    /// </summary>
    public double MaxCpuUsage => _metrics.Max(m => m.CpuUsage);

    /// <summary>
    /// Gets the metric distribution.
    /// </summary>
    public IReadOnlyDictionary<string, double> MetricDistribution => _metricDistribution;

    /// <summary>
    /// Gets the metric success rate.
    /// </summary>
    public IReadOnlyDictionary<string, double> MetricSuccessRate => _metricSuccessRate;

    /// <summary>
    /// Gets the trend analysis.
    /// </summary>
    public IReadOnlyDictionary<string, double> TrendAnalysis => _trendAnalysis;

    /// <summary>
    /// Adds a metric.
    /// </summary>
    /// <param name="metric">The metric.</param>
    public void AddMetric(PerformanceMetric metric)
    {
        if (metric == null)
            ArgumentNullException.ThrowIfNull(nameof(metric));

        _metrics.Add(metric);
        UpdateMetricHistory(metric);
        UpdateMetricDistribution();
        UpdateMetricSuccessRate();
        UpdateTrendAnalysis();
    }

    /// <summary>
    /// Gets the metric trend.
    /// </summary>
    /// <param name="metricName">The metric name.</param>
    /// <returns>The trend.</returns>
    public double GetMetricTrend(string metricName)
    {
        if (string.IsNullOrEmpty(metricName))
            throw new ArgumentException("Metric name cannot be null or empty.", nameof(metricName));

        return _trendAnalysis.TryGetValue(metricName, out var trend) ? trend : 0;
    }

    /// <summary>
    /// Updates the metric history.
    /// </summary>
    /// <param name="metric">The metric.</param>
    private void UpdateMetricHistory(PerformanceMetric metric)
    {
        UpdateMetricHistoryValue("Duration", metric.DurationMs);
        UpdateMetricHistoryValue("Memory", metric.MemoryUsage);
        UpdateMetricHistoryValue("Cpu", metric.CpuUsage);
    }

    /// <summary>
    /// Updates the metric history value.
    /// </summary>
    /// <param name="metricName">The metric name.</param>
    /// <param name="value">The value.</param>
    private void UpdateMetricHistoryValue(string metricName, double value)
    {
        if (!_metricHistory.ContainsKey(metricName))
        {
            _metricHistory[metricName] = new Collection<double>();
        }

        _metricHistory[metricName].Add(value);
    }

    /// <summary>
    /// Updates the metric distribution.
    /// </summary>
    private void UpdateMetricDistribution()
    {
        _metricDistribution.Clear();

        foreach (var (metricName, values) in _metricHistory)
        {
            var mean = values.Average();
            var variance = values.Select(v => Math.Pow(v - mean, 2)).Average();
            var standardDeviation = Math.Sqrt(variance);

            _metricDistribution[metricName] = standardDeviation / mean;
        }
    }

    /// <summary>
    /// Updates the metric success rate.
    /// </summary>
    private void UpdateMetricSuccessRate()
    {
        _metricSuccessRate.Clear();

        var successCount = _metrics.Count(m => m.Passed);
        var totalCount = _metrics.Count;

        if (totalCount > 0)
        {
            _metricSuccessRate["Overall"] = (double)successCount / totalCount;
        }

        foreach (var metricName in _metricHistory.Keys)
        {
            var successValues = _metrics
                .Where(m => m.Passed)
                .Select(m => GetMetricValue(m, metricName))
                .ToList();

            var totalValues = _metrics
                .Select(m => GetMetricValue(m, metricName))
                .ToList();

            if (totalValues.Count > 0)
            {
                _metricSuccessRate[metricName] = (double)successValues.Count / totalValues.Count;
            }
        }
    }

    /// <summary>
    /// Updates the trend analysis.
    /// </summary>
    private void UpdateTrendAnalysis()
    {
        _trendAnalysis.Clear();

        foreach (var (metricName, values) in _metricHistory)
        {
            if (values.Count < 2)
                continue;

            var x = Enumerable.Range(0, values.Count).Select(i => (double)i).ToArray();
            var y = values.ToArray();

            var slope = CalculateLinearRegressionSlope(x, y);
            _trendAnalysis[metricName] = slope;
        }
    }

    /// <summary>
    /// Gets the metric value.
    /// </summary>
    /// <param name="metric">The metric.</param>
    /// <param name="metricName">The metric name.</param>
    /// <returns>The value.</returns>
    private static double GetMetricValue(PerformanceMetric metric, string metricName)
    {
        return metricName switch
        {
            "Duration" => metric.DurationMs,
            "Memory" => metric.MemoryUsage,
            "Cpu" => metric.CpuUsage,
            _ => throw new ArgumentException($"Unknown metric name: {metricName}", nameof(metricName))
        };
    }

    /// <summary>
    /// Calculates the linear regression slope.
    /// </summary>
    /// <param name="x">The x values.</param>
    /// <param name="y">The y values.</param>
    /// <returns>The slope.</returns>
    private static double CalculateLinearRegressionSlope(double[] x, double[] y)
    {
        if (x.Length != y.Length)
            throw new ArgumentException("Arrays must have the same length.");

        var n = x.Length;
        var sumX = x.Sum();
        var sumY = y.Sum();
        var sumXY = x.Zip(y, (a, b) => a * b).Sum();
        var sumXX = x.Sum(a => a * a);

        return (n * sumXY - sumX * sumY) / (n * sumXX - sumX * sumX);
    }
} 





