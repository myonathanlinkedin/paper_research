using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Metrics;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Scenarios for test suite execution.
/// </summary>
public class TestSuiteScenarios
{
    private readonly List<ErrorScenario> _scenarios;
    private readonly Dictionary<string, List<ErrorScenario>> _scenarioGroups;
    private readonly Dictionary<string, int> _errorDistribution;
    private readonly Dictionary<string, int> _sourceDistribution;
    private readonly Dictionary<double, int> _confidenceDistribution;
    private readonly Dictionary<string, double> _successRateByErrorType;
    private readonly Dictionary<string, double> _successRateBySource;
    private readonly Dictionary<string, double> _averageConfidenceByErrorType;
    private readonly Dictionary<string, double> _averageConfidenceBySource;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuiteScenarios"/> class.
    /// </summary>
    /// <param name="scenarios">The scenarios.</param>
    public TestSuiteScenarios(IEnumerable<ErrorScenario> scenarios)
    {
        _scenarios = scenarios?.ToList() ?? throw new ArgumentNullException(nameof(scenarios));
        _scenarioGroups = new Dictionary<string, List<ErrorScenario>>();
        _errorDistribution = new Dictionary<string, int>();
        _sourceDistribution = new Dictionary<string, int>();
        _confidenceDistribution = new Dictionary<double, int>();
        _successRateByErrorType = new Dictionary<string, double>();
        _successRateBySource = new Dictionary<string, double>();
        _averageConfidenceByErrorType = new Dictionary<string, double>();
        _averageConfidenceBySource = new Dictionary<string, double>();
        GroupScenarios();
        UpdateDistributions();
        UpdateSuccessRates();
        UpdateConfidence();
    }

    /// <summary>
    /// Gets the scenarios.
    /// </summary>
    public IReadOnlyList<ErrorScenario> Scenarios => _scenarios;

    /// <summary>
    /// Gets the scenario groups.
    /// </summary>
    public IReadOnlyDictionary<string, List<ErrorScenario>> ScenarioGroups => _scenarioGroups;

    /// <summary>
    /// Gets the error types.
    /// </summary>
    public IReadOnlyList<string> ErrorTypes => _scenarios.Select(s => s.ErrorType).Distinct().ToList();

    /// <summary>
    /// Gets the sources.
    /// </summary>
    public IReadOnlyList<string> Sources => _scenarios.Select(s => s.Source).Distinct().ToList();

    /// <summary>
    /// Gets the error distribution.
    /// </summary>
    public IReadOnlyDictionary<string, int> ErrorDistribution => _errorDistribution;

    /// <summary>
    /// Gets the source distribution.
    /// </summary>
    public IReadOnlyDictionary<string, int> SourceDistribution => _sourceDistribution;

    /// <summary>
    /// Gets the confidence distribution.
    /// </summary>
    public IReadOnlyDictionary<double, int> ConfidenceDistribution => _confidenceDistribution;

    /// <summary>
    /// Gets the success rate by error type.
    /// </summary>
    public IReadOnlyDictionary<string, double> SuccessRateByErrorType => _successRateByErrorType;

    /// <summary>
    /// Gets the success rate by source.
    /// </summary>
    public IReadOnlyDictionary<string, double> SuccessRateBySource => _successRateBySource;

    /// <summary>
    /// Gets the average confidence by error type.
    /// </summary>
    public IReadOnlyDictionary<string, double> AverageConfidenceByErrorType => _averageConfidenceByErrorType;

    /// <summary>
    /// Gets the average confidence by source.
    /// </summary>
    public IReadOnlyDictionary<string, double> AverageConfidenceBySource => _averageConfidenceBySource;

    /// <summary>
    /// Groups the scenarios.
    /// </summary>
    private void GroupScenarios()
    {
        foreach (var scenario in _scenarios)
        {
            var group = GetScenarioGroup(scenario);
            if (!_scenarioGroups.ContainsKey(group))
            {
                _scenarioGroups[group] = new List<ErrorScenario>();
            }
            _scenarioGroups[group].Add(scenario);
        }
    }

    /// <summary>
    /// Gets the scenario group.
    /// </summary>
    /// <param name="scenario">The scenario.</param>
    /// <returns>The scenario group.</returns>
    private static string GetScenarioGroup(ErrorScenario scenario)
    {
        if (scenario == null)
            throw new ArgumentNullException(nameof(scenario));

        return $"{scenario.ErrorType}_{scenario.Source}";
    }

    /// <summary>
    /// Gets the scenarios by error type.
    /// </summary>
    /// <param name="errorType">The error type.</param>
    /// <returns>The scenarios.</returns>
    public IReadOnlyList<ErrorScenario> GetScenariosByErrorType(string errorType)
    {
        if (string.IsNullOrEmpty(errorType))
            throw new ArgumentException("Error type cannot be null or empty.", nameof(errorType));

        return _scenarios.Where(s => s.ErrorType == errorType).ToList();
    }

    /// <summary>
    /// Gets the scenarios by source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns>The scenarios.</returns>
    public IReadOnlyList<ErrorScenario> GetScenariosBySource(string source)
    {
        if (string.IsNullOrEmpty(source))
            throw new ArgumentException("Source cannot be null or empty.", nameof(source));

        return _scenarios.Where(s => s.Source == source).ToList();
    }

    /// <summary>
    /// Gets the scenarios by group.
    /// </summary>
    /// <param name="group">The group.</param>
    /// <returns>The scenarios.</returns>
    public IReadOnlyList<ErrorScenario> GetScenariosByGroup(string group)
    {
        if (string.IsNullOrEmpty(group))
            throw new ArgumentException("Group cannot be null or empty.", nameof(group));

        return _scenarioGroups.TryGetValue(group, out var scenarios) ? scenarios : Array.Empty<ErrorScenario>();
    }

    /// <summary>
    /// Gets the success rate.
    /// </summary>
    /// <returns>The success rate.</returns>
    public double GetSuccessRate()
    {
        return _scenarios.Count(s => s.Analysis?.IsValid ?? false) / (double)_scenarios.Count;
    }

    /// <summary>
    /// Gets the average confidence.
    /// </summary>
    /// <returns>The average confidence.</returns>
    public double GetAverageConfidence()
    {
        return _scenarios.Average(s => s.Analysis?.Confidence ?? 0);
    }

    /// <summary>
    /// Gets the scenario by name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The scenario.</returns>
    public ErrorScenario GetScenarioByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        return _scenarios.Find(s => s.Name == name) 
            ?? throw new KeyNotFoundException($"Scenario '{name}' not found.");
    }

    /// <summary>
    /// Updates the distributions.
    /// </summary>
    private void UpdateDistributions()
    {
        foreach (var scenario in _scenarios)
        {
            UpdateDistributions(scenario);
        }
    }

    /// <summary>
    /// Updates the success rates.
    /// </summary>
    private void UpdateSuccessRates()
    {
        _successRateByErrorType.Clear();
        _successRateBySource.Clear();

        // Calculate success rate by error type
        foreach (var errorType in _errorDistribution.Keys)
        {
            var scenarios = _scenarios.Where(s => s.ErrorType == errorType).ToList();
            if (scenarios.Count > 0)
            {
                _successRateByErrorType[errorType] = scenarios.Count(s => s.Analysis?.IsValid ?? false) / (double)scenarios.Count;
            }
        }

        // Calculate success rate by source
        foreach (var source in _sourceDistribution.Keys)
        {
            var scenarios = _scenarios.Where(s => s.Source == source).ToList();
            if (scenarios.Count > 0)
            {
                _successRateBySource[source] = scenarios.Count(s => s.Analysis?.IsValid ?? false) / (double)scenarios.Count;
            }
        }
    }

    /// <summary>
    /// Updates the confidence.
    /// </summary>
    private void UpdateConfidence()
    {
        _averageConfidenceByErrorType.Clear();
        _averageConfidenceBySource.Clear();

        // Calculate average confidence by error type
        foreach (var errorType in _errorDistribution.Keys)
        {
            var scenarios = _scenarios.Where(s => s.ErrorType == errorType).ToList();
            if (scenarios.Count > 0)
            {
                _averageConfidenceByErrorType[errorType] = scenarios.Average(s => s.Analysis?.Confidence ?? 0);
            }
        }

        // Calculate average confidence by source
        foreach (var source in _sourceDistribution.Keys)
        {
            var scenarios = _scenarios.Where(s => s.Source == source).ToList();
            if (scenarios.Count > 0)
            {
                _averageConfidenceBySource[source] = scenarios.Average(s => s.Analysis?.Confidence ?? 0);
            }
        }
    }

    /// <summary>
    /// Updates the distributions.
    /// </summary>
    /// <param name="scenario">The scenario.</param>
    private void UpdateDistributions(ErrorScenario scenario)
    {
        // Update error distribution
        if (!_errorDistribution.ContainsKey(scenario.ErrorType))
        {
            _errorDistribution[scenario.ErrorType] = 0;
        }
        _errorDistribution[scenario.ErrorType]++;

        // Update source distribution
        if (!_sourceDistribution.ContainsKey(scenario.Source))
        {
            _sourceDistribution[scenario.Source] = 0;
        }
        _sourceDistribution[scenario.Source]++;

        // Update confidence distribution
        var confidence = scenario.Analysis?.Confidence ?? 0;
        var roundedConfidence = Math.Round(confidence, 2);
        if (!_confidenceDistribution.ContainsKey(roundedConfidence))
        {
            _confidenceDistribution[roundedConfidence] = 0;
        }
        _confidenceDistribution[roundedConfidence]++;
    }
} 
