using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Analysis.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Executes test suites.
/// </summary>
public class TestSuiteExecutor
{
    private readonly IErrorAnalyzer _errorAnalyzer;
    private readonly TestSuiteConfiguration _configuration;
    private readonly List<TestSuiteResult> _results;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuiteExecutor"/> class.
    /// </summary>
    /// <param name="errorAnalyzer">The error analyzer.</param>
    /// <param name="configuration">The configuration.</param>
    public TestSuiteExecutor(IErrorAnalyzer errorAnalyzer, TestSuiteConfiguration configuration)
    {
        _errorAnalyzer = errorAnalyzer ?? throw new ArgumentNullException(nameof(errorAnalyzer));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _results = new List<TestSuiteResult>();
    }

    /// <summary>
    /// Gets the error analyzer.
    /// </summary>
    public IErrorAnalyzer ErrorAnalyzer => _errorAnalyzer;

    /// <summary>
    /// Gets the configuration.
    /// </summary>
    public TestSuiteConfiguration Configuration => _configuration;

    /// <summary>
    /// Gets the results.
    /// </summary>
    public IReadOnlyList<TestSuiteResult> Results => _results;

    /// <summary>
    /// Executes the test suite.
    /// </summary>
    /// <returns>The test suite result.</returns>
    public async Task<TestSuiteResult> ExecuteAsync()
    {
        if (!_configuration.Validate())
            throw new InvalidOperationException("Test suite configuration is invalid.");

        var result = new TestSuiteResult();

        try
        {
            foreach (var scenario in _configuration.Scenarios)
            {
                var scenarioResult = await ExecuteScenarioAsync(scenario).ConfigureAwait(false);
                result.AddScenario(scenarioResult);
            }

            foreach (var metric in _configuration.Metrics)
            {
                var metricResult = await ExecuteMetricAsync(metric).ConfigureAwait(false);
                result.AddMetric(metricResult);
            }

            result.SetPassed();
        }
        catch (Exception ex)
        {
            result.SetFailed(ex.Message);
        }

        _results.Add(result);
        return result;
    }

    /// <summary>
    /// Executes a scenario.
    /// </summary>
    /// <param name="scenario">The scenario.</param>
    /// <returns>The scenario result.</returns>
    private async Task<ErrorScenario> ExecuteScenarioAsync(ErrorScenario scenario)
    {
        if (scenario == null)
            throw new ArgumentNullException(nameof(scenario));

        var context = new ErrorContext
        {
            ErrorType = scenario.ErrorType,
            ErrorMessage = scenario.ErrorMessage,
            StackTrace = scenario.StackTrace,
            Source = scenario.Source,
            Timestamp = DateTime.UtcNow
        };

        var analysis = await _errorAnalyzer.AnalyzeErrorAsync(context).ConfigureAwait(false);
        scenario.Analysis = analysis;

        return scenario;
    }

    /// <summary>
    /// Executes a metric.
    /// </summary>
    /// <param name="metric">The metric.</param>
    /// <returns>The metric result.</returns>
    private async Task<PerformanceMetric> ExecuteMetricAsync(PerformanceMetric metric)
    {
        if (metric == null)
            throw new ArgumentNullException(nameof(metric));

        var startTime = DateTime.UtcNow;
        var startMemory = GC.GetTotalMemory(false);

        await metric.ExecuteAsync().ConfigureAwait(false);

        var endTime = DateTime.UtcNow;
        var endMemory = GC.GetTotalMemory(false);

        metric.Duration = endTime - startTime;
        metric.MemoryUsage = endMemory - startMemory;

        return metric;
    }

    /// <summary>
    /// Gets the latest result.
    /// </summary>
    /// <returns>The latest result.</returns>
    public TestSuiteResult GetLatestResult()
    {
        if (_results.Count == 0)
            throw new InvalidOperationException("No results available.");

        return _results[^1];
    }

    /// <summary>
    /// Gets a result by index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The result.</returns>
    public TestSuiteResult GetResult(int index)
    {
        if (index < 0 || index >= _results.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        return _results[index];
    }

    /// <summary>
    /// Clears the results.
    /// </summary>
    public void ClearResults()
    {
        _results.Clear();
    }
} 