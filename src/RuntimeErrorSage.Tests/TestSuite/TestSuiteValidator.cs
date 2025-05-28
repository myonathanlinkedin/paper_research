using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Validates test suite results.
/// </summary>
public class TestSuiteValidator
{
    private readonly TestSuiteResult _result;
    private readonly Dictionary<string, object> _thresholds;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuiteValidator"/> class.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="thresholds">The thresholds.</param>
    public TestSuiteValidator(TestSuiteResult result, Dictionary<string, object> thresholds)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
        _thresholds = thresholds ?? throw new ArgumentNullException(nameof(thresholds));
    }

    /// <summary>
    /// Gets the result.
    /// </summary>
    public TestSuiteResult Result => _result;

    /// <summary>
    /// Gets the thresholds.
    /// </summary>
    public IReadOnlyDictionary<string, object> Thresholds => _thresholds;

    /// <summary>
    /// Validates the result.
    /// </summary>
    /// <returns>The validation result.</returns>
    public ValidationResult Validate()
    {
        var errors = new List<string>();
        var warnings = new List<string>();

        // Validate scenarios
        foreach (var scenario in _result.Scenarios)
        {
            if (!ValidateScenario(scenario, out var scenarioErrors, out var scenarioWarnings))
            {
                errors.AddRange(scenarioErrors);
                warnings.AddRange(scenarioWarnings);
            }
        }

        // Validate metrics
        foreach (var metric in _result.Metrics)
        {
            if (!ValidateMetric(metric, out var metricErrors, out var metricWarnings))
            {
                errors.AddRange(metricErrors);
                warnings.AddRange(metricWarnings);
            }
        }

        // Validate duration
        if (_thresholds.TryGetValue("MaxDuration", out var maxDuration) && maxDuration is TimeSpan maxDurationValue)
        {
            if (_result.Duration > maxDurationValue)
            {
                errors.Add($"Test suite duration ({_result.Duration.TotalSeconds:F2}s) exceeds maximum threshold ({maxDurationValue.TotalSeconds:F2}s)");
            }
        }

        // Validate memory usage
        if (_thresholds.TryGetValue("MaxMemoryUsage", out var maxMemory) && maxMemory is long maxMemoryValue)
        {
            var totalMemory = _result.Metrics.Sum(m => m.MemoryUsage);
            if (totalMemory > maxMemoryValue)
            {
                errors.Add($"Total memory usage ({totalMemory / 1024:F2} KB) exceeds maximum threshold ({maxMemoryValue / 1024:F2} KB)");
            }
        }

        // Validate CPU usage
        if (_thresholds.TryGetValue("MaxCpuUsage", out var maxCpu) && maxCpu is double maxCpuValue)
        {
            var maxCpuUsage = _result.Metrics.Max(m => m.CpuUsage);
            if (maxCpuUsage > maxCpuValue)
            {
                errors.Add($"Maximum CPU usage ({maxCpuUsage:F2}%) exceeds threshold ({maxCpuValue:F2}%)");
            }
        }

        return new ValidationResult
        {
            IsValid = errors.Count == 0,
            Errors = errors,
            Warnings = warnings
        };
    }

    /// <summary>
    /// Validates a scenario.
    /// </summary>
    /// <param name="scenario">The scenario.</param>
    /// <param name="errors">The errors.</param>
    /// <param name="warnings">The warnings.</param>
    /// <returns>True if the scenario is valid; otherwise, false.</returns>
    private bool ValidateScenario(ErrorScenario scenario, out List<string> errors, out List<string> warnings)
    {
        errors = new List<string>();
        warnings = new List<string>();

        if (scenario.Analysis == null)
        {
            errors.Add($"Scenario '{scenario.Name}' has no analysis");
            return false;
        }

        if (string.IsNullOrEmpty(scenario.Analysis.Summary))
        {
            warnings.Add($"Scenario '{scenario.Name}' has no analysis summary");
        }

        if (_thresholds.TryGetValue("MinConfidence", out var minConfidence) && minConfidence is double minConfidenceValue)
        {
            if (scenario.Analysis.Confidence < minConfidenceValue)
            {
                errors.Add($"Scenario '{scenario.Name}' has low confidence ({scenario.Analysis.Confidence:F2})");
            }
        }

        return errors.Count == 0;
    }

    /// <summary>
    /// Validates a metric.
    /// </summary>
    /// <param name="metric">The metric.</param>
    /// <param name="errors">The errors.</param>
    /// <param name="warnings">The warnings.</param>
    /// <returns>True if the metric is valid; otherwise, false.</returns>
    private bool ValidateMetric(PerformanceMetric metric, out List<string> errors, out List<string> warnings)
    {
        errors = new List<string>();
        warnings = new List<string>();

        if (_thresholds.TryGetValue("MaxMetricDuration", out var maxDuration) && maxDuration is TimeSpan maxDurationValue)
        {
            if (metric.Duration > maxDurationValue)
            {
                errors.Add($"Metric '{metric.Name}' duration ({metric.Duration.TotalMilliseconds:F2} ms) exceeds maximum threshold ({maxDurationValue.TotalMilliseconds:F2} ms)");
            }
        }

        if (_thresholds.TryGetValue("MaxMetricMemory", out var maxMemory) && maxMemory is long maxMemoryValue)
        {
            if (metric.MemoryUsage > maxMemoryValue)
            {
                errors.Add($"Metric '{metric.Name}' memory usage ({metric.MemoryUsage / 1024:F2} KB) exceeds maximum threshold ({maxMemoryValue / 1024:F2} KB)");
            }
        }

        if (_thresholds.TryGetValue("MaxMetricCpu", out var maxCpu) && maxCpu is double maxCpuValue)
        {
            if (metric.CpuUsage > maxCpuValue)
            {
                errors.Add($"Metric '{metric.Name}' CPU usage ({metric.CpuUsage:F2}%) exceeds maximum threshold ({maxCpuValue:F2}%)");
            }
        }

        return errors.Count == 0;
    }
} 
