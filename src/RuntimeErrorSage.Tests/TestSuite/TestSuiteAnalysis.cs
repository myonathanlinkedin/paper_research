using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Metrics;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Analysis of test suite execution.
/// </summary>
public class TestSuiteAnalysis
{
    private readonly TestSuiteResult _result;
    private readonly TestSuiteMetrics _metrics;
    private readonly TestSuiteScenarios _scenarios;
    private readonly Dictionary<string, object> _insights;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuiteAnalysis"/> class.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="metrics">The metrics.</param>
    /// <param name="scenarios">The scenarios.</param>
    public TestSuiteAnalysis(TestSuiteResult result, TestSuiteMetrics metrics, TestSuiteScenarios scenarios)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
        _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        _scenarios = scenarios ?? throw new ArgumentNullException(nameof(scenarios));
        _insights = new Dictionary<string, object>();
        Analyze();
    }

    /// <summary>
    /// Gets the result.
    /// </summary>
    public TestSuiteResult Result => _result;

    /// <summary>
    /// Gets the metrics.
    /// </summary>
    public TestSuiteMetrics Metrics => _metrics;

    /// <summary>
    /// Gets the scenarios.
    /// </summary>
    public TestSuiteScenarios Scenarios => _scenarios;

    /// <summary>
    /// Gets the insights.
    /// </summary>
    public IReadOnlyDictionary<string, object> Insights => _insights;

    /// <summary>
    /// Analyzes the test suite.
    /// </summary>
    private void Analyze()
    {
        AnalyzePerformance();
        AnalyzeScenarios();
        AnalyzeMetrics();
        AnalyzeTrends();
        GenerateInsights();
    }

    /// <summary>
    /// Analyzes the performance.
    /// </summary>
    private void AnalyzePerformance()
    {
        var performance = new Dictionary<string, object>
        {
            ["Duration"] = new
            {
                Average = _metrics.AverageDuration.TotalMilliseconds,
                Min = _metrics.MinDuration.TotalMilliseconds,
                Max = _metrics.MaxDuration.TotalMilliseconds
            },
            ["Memory"] = new
            {
                Average = _metrics.AverageMemoryUsage,
                Min = _metrics.MinMemoryUsage,
                Max = _metrics.MaxMemoryUsage
            },
            ["Cpu"] = new
            {
                Average = _metrics.AverageCpuUsage,
                Min = _metrics.MinCpuUsage,
                Max = _metrics.MaxCpuUsage
            }
        };

        _insights["Performance"] = performance;
    }

    /// <summary>
    /// Analyzes the scenarios.
    /// </summary>
    private void AnalyzeScenarios()
    {
        var scenarios = new Dictionary<string, object>
        {
            ["ErrorTypes"] = _scenarios.ErrorDistribution,
            ["Sources"] = _scenarios.SourceDistribution,
            ["Confidence"] = _scenarios.ConfidenceDistribution,
            ["SuccessRate"] = new
            {
                Overall = _scenarios.GetSuccessRate(),
                ByErrorType = _scenarios.SuccessRateByErrorType,
                BySource = _scenarios.SuccessRateBySource
            },
            ["Confidence"] = new
            {
                Overall = _scenarios.GetAverageConfidence(),
                ByErrorType = _scenarios.AverageConfidenceByErrorType,
                BySource = _scenarios.AverageConfidenceBySource
            }
        };

        _insights["Scenarios"] = scenarios;
    }

    /// <summary>
    /// Analyzes the metrics.
    /// </summary>
    private void AnalyzeMetrics()
    {
        var metrics = new Dictionary<string, object>
        {
            ["Distribution"] = _metrics.MetricDistribution,
            ["SuccessRate"] = _metrics.MetricSuccessRate,
            ["Trends"] = _metrics.TrendAnalysis
        };

        _insights["Metrics"] = metrics;
    }

    /// <summary>
    /// Analyzes the trends.
    /// </summary>
    private void AnalyzeTrends()
    {
        var trends = new Dictionary<string, object>
        {
            ["Performance"] = new
            {
                Duration = _metrics.GetMetricTrend("Duration"),
                Memory = _metrics.GetMetricTrend("Memory"),
                Cpu = _metrics.GetMetricTrend("Cpu")
            },
            ["Scenarios"] = new
            {
                SuccessRate = _metrics.GetMetricTrend("SuccessRate"),
                Confidence = _metrics.GetMetricTrend("Confidence")
            }
        };

        _insights["Trends"] = trends;
    }

    /// <summary>
    /// Generates insights.
    /// </summary>
    private void GenerateInsights()
    {
        var insights = new List<string>();

        // Performance insights
        if (_metrics.AverageDuration > TimeSpan.FromSeconds(1))
        {
            insights.Add($"Test suite execution is slow (average duration: {_metrics.AverageDuration.TotalSeconds:F2}s)");
        }

        if (_metrics.AverageMemoryUsage > 100 * 1024 * 1024) // 100 MB
        {
            insights.Add($"Test suite uses high memory (average: {_metrics.AverageMemoryUsage / (1024 * 1024):F2} MB)");
        }

        if (_metrics.AverageCpuUsage > 50)
        {
            insights.Add($"Test suite uses high CPU (average: {_metrics.AverageCpuUsage:F2}%)");
        }

        // Scenario insights
        var successRate = _scenarios.GetSuccessRate();
        if (successRate < 0.8)
        {
            insights.Add($"Test suite has low success rate ({successRate:P2})");
        }

        var confidence = _scenarios.GetAverageConfidence();
        if (confidence < 0.7)
        {
            insights.Add($"Test suite has low confidence ({confidence:P2})");
        }

        // Error type insights
        foreach (var (errorType, rate) in _scenarios.SuccessRateByErrorType)
        {
            if (rate < 0.8)
            {
                insights.Add($"Low success rate for error type '{errorType}' ({rate:P2})");
            }
        }

        // Source insights
        foreach (var (source, rate) in _scenarios.SuccessRateBySource)
        {
            if (rate < 0.8)
            {
                insights.Add($"Low success rate for source '{source}' ({rate:P2})");
            }
        }

        // Trend insights
        foreach (var (metric, trend) in _metrics.TrendAnalysis)
        {
            if (trend > 0.1)
            {
                insights.Add($"Increasing trend for metric '{metric}' ({trend:F2})");
            }
            else if (trend < -0.1)
            {
                insights.Add($"Decreasing trend for metric '{metric}' ({trend:F2})");
            }
        }

        _insights["Insights"] = insights;
    }

    /// <summary>
    /// Gets the insight.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The insight.</returns>
    public object GetInsight(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        return _insights.TryGetValue(key, out var value) ? value : null;
    }

    /// <summary>
    /// Gets the insights.
    /// </summary>
    /// <returns>The insights.</returns>
    public IReadOnlyList<string> GetInsights()
    {
        return _insights.TryGetValue("Insights", out var value) && value is List<string> insights
            ? insights
            : Array.Empty<string>();
    }
} 
