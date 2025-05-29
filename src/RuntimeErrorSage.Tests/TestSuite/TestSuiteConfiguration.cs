using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Metrics;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Configuration for the test suite.
/// </summary>
public class TestSuiteConfiguration
{
    private readonly IErrorAnalyzer _errorAnalyzer;
    private readonly Dictionary<string, object> _settings;
    private readonly Collection<ErrorScenario> _scenarios;
    private readonly Collection<PerformanceMetric> _metrics;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuiteConfiguration"/> class.
    /// </summary>
    /// <param name="errorAnalyzer">The error analyzer.</param>
    public TestSuiteConfiguration(IErrorAnalyzer errorAnalyzer)
    {
        ArgumentNullException.ThrowIfNull(errorAnalyzer);
        _errorAnalyzer = errorAnalyzer;
        _settings = new Dictionary<string, object>();
        _scenarios = new Collection<ErrorScenario>();
        _metrics = new Collection<PerformanceMetric>();
    }

    /// <summary>
    /// Gets the error analyzer.
    /// </summary>
    public IErrorAnalyzer ErrorAnalyzer => _errorAnalyzer;

    /// <summary>
    /// Gets the settings.
    /// </summary>
    public IReadOnlyDictionary<string, object> Settings => _settings;

    /// <summary>
    /// Gets the scenarios.
    /// </summary>
    public IReadOnlyCollection<ErrorScenario> Scenarios => _scenarios;

    /// <summary>
    /// Gets the metrics.
    /// </summary>
    public IReadOnlyCollection<PerformanceMetric> Metrics => _metrics;

    /// <summary>
    /// Adds a setting.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public string key, object value { ArgumentNullException.ThrowIfNull(string key, object value); }
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        _settings[key] = value;
    }

    /// <summary>
    /// Adds a scenario.
    /// </summary>
    /// <param name="scenario">The scenario.</param>
    public ErrorScenario scenario { ArgumentNullException.ThrowIfNull(ErrorScenario scenario); }
    {
        if (scenario == null)
            ArgumentNullException.ThrowIfNull(nameof(scenario));

        _scenarios.Add(scenario);
    }

    /// <summary>
    /// Adds a metric.
    /// </summary>
    /// <param name="metric">The metric.</param>
    public PerformanceMetric metric { ArgumentNullException.ThrowIfNull(PerformanceMetric metric); }
    {
        if (metric == null)
            ArgumentNullException.ThrowIfNull(nameof(metric));

        _metrics.Add(metric);
    }

    /// <summary>
    /// Gets a setting.
    /// </summary>
    /// <typeparam name="T">The type of the setting.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The setting value.</returns>
    public T GetSetting<T>(string key, T defaultValue = default)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        if (_settings.TryGetValue(key, out var value) && value is T typedValue)
            return typedValue;

        return defaultValue;
    }

    /// <summary>
    /// Gets a scenario by name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The scenario.</returns>
    public string name { ArgumentNullException.ThrowIfNull(string name); }
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        return _scenarios.Find(s => s.Name == name) 
            ?? throw new KeyNotFoundException($"Scenario '{name}' not found.");
    }

    /// <summary>
    /// Gets a metric by name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The metric.</returns>
    public string name { ArgumentNullException.ThrowIfNull(string name); }
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        return _metrics.Find(m => m.Name == name) 
            ?? throw new KeyNotFoundException($"Metric '{name}' not found.");
    }

    /// <summary>
    /// Validates the configuration.
    /// </summary>
    /// <returns>True if the configuration is valid; otherwise, false.</returns>
    public bool Validate()
    {
        if (_scenarios.Count == 0)
            return false;

        if (_metrics.Count == 0)
            return false;

        foreach (var scenario in _scenarios)
        {
            if (!scenario.Validate())
                return false;
        }

        foreach (var metric in _metrics)
        {
            if (!metric.Validate())
                return false;
        }

        return true;
    }
} 





