using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Result of test suite execution.
/// </summary>
public class TestSuiteResult
{
    private readonly List<ErrorScenario> _scenarios;
    private readonly List<PerformanceMetric> _metrics;
    private readonly Dictionary<string, object> _metadata;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuiteResult"/> class.
    /// </summary>
    public TestSuiteResult()
    {
        _scenarios = new List<ErrorScenario>();
        _metrics = new List<PerformanceMetric>();
        _metadata = new Dictionary<string, object>();
    }

    /// <summary>
    /// Gets the scenarios.
    /// </summary>
    public IReadOnlyList<ErrorScenario> Scenarios => _scenarios;

    /// <summary>
    /// Gets the metrics.
    /// </summary>
    public IReadOnlyList<PerformanceMetric> Metrics => _metrics;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata => _metadata;

    /// <summary>
    /// Gets a value indicating whether the test suite passed.
    /// </summary>
    public bool Passed => _scenarios.All(s => s.Analysis?.IsValid ?? false);

    /// <summary>
    /// Gets the duration.
    /// </summary>
    public TimeSpan Duration => TimeSpan.FromMilliseconds(_metrics.Sum(m => m.DurationMs));

    /// <summary>
    /// Gets the memory usage.
    /// </summary>
    public long MemoryUsage => _metrics.Sum(m => m.MemoryUsage);

    /// <summary>
    /// Gets the CPU usage.
    /// </summary>
    public double CpuUsage => _metrics.Average(m => m.CpuUsage);

    /// <summary>
    /// Gets the success rate.
    /// </summary>
    public double SuccessRate => _scenarios.Count(s => s.Analysis?.IsValid ?? false) / (double)_scenarios.Count;

    /// <summary>
    /// Gets the average confidence.
    /// </summary>
    public double AverageConfidence => _scenarios.Average(s => s.Analysis?.Confidence ?? 0);

    /// <summary>
    /// Gets the error distribution.
    /// </summary>
    public Dictionary<string, int> ErrorDistribution => _scenarios
        .GroupBy(s => s.ErrorType)
        .ToDictionary(g => g.Key, g => g.Count());

    /// <summary>
    /// Gets the source distribution.
    /// </summary>
    public Dictionary<string, int> SourceDistribution => _scenarios
        .GroupBy(s => s.Source)
        .ToDictionary(g => g.Key, g => g.Count());

    /// <summary>
    /// Gets the confidence distribution.
    /// </summary>
    public Dictionary<double, int> ConfidenceDistribution => _scenarios
        .Where(s => s.Analysis != null)
        .GroupBy(s => Math.Round(s.Analysis.Confidence, 2))
        .ToDictionary(g => g.Key, g => g.Count());

    /// <summary>
    /// Gets the success rate by error type.
    /// </summary>
    public Dictionary<string, double> SuccessRateByErrorType => _scenarios
        .GroupBy(s => s.ErrorType)
        .ToDictionary(g => g.Key, g => g.Count(s => s.Analysis?.IsValid ?? false) / (double)g.Count());

    /// <summary>
    /// Gets the success rate by source.
    /// </summary>
    public Dictionary<string, double> SuccessRateBySource => _scenarios
        .GroupBy(s => s.Source)
        .ToDictionary(g => g.Key, g => g.Count(s => s.Analysis?.IsValid ?? false) / (double)g.Count());

    /// <summary>
    /// Gets the average confidence by error type.
    /// </summary>
    public Dictionary<string, double> AverageConfidenceByErrorType => _scenarios
        .GroupBy(s => s.ErrorType)
        .ToDictionary(g => g.Key, g => g.Average(s => s.Analysis?.Confidence ?? 0));

    /// <summary>
    /// Gets the average confidence by source.
    /// </summary>
    public Dictionary<string, double> AverageConfidenceBySource => _scenarios
        .GroupBy(s => s.Source)
        .ToDictionary(g => g.Key, g => g.Average(s => s.Analysis?.Confidence ?? 0));

    /// <summary>
    /// Adds a scenario.
    /// </summary>
    /// <param name="scenario">The scenario.</param>
    public void AddScenario(ErrorScenario scenario)
    {
        if (scenario == null)
            throw new ArgumentNullException(nameof(scenario));

        _scenarios.Add(scenario);
    }

    /// <summary>
    /// Adds a metric.
    /// </summary>
    /// <param name="metric">The metric.</param>
    public void AddMetric(PerformanceMetric metric)
    {
        if (metric == null)
            throw new ArgumentNullException(nameof(metric));

        _metrics.Add(metric);
    }

    /// <summary>
    /// Adds metadata.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void AddMetadata(string key, object value)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        _metadata[key] = value;
    }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The metadata value.</returns>
    public object GetMetadata(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        return _metadata.TryGetValue(key, out var value) ? value : null;
    }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="key">The key.</param>
    /// <returns>The metadata value.</returns>
    public T GetMetadata<T>(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        if (!_metadata.TryGetValue(key, out var value))
            return default;

        return value is T typedValue ? typedValue : default;
    }
} 