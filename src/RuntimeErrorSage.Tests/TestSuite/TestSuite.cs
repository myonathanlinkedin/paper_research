using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Runtime.Interfaces;
using RuntimeErrorSage.Tests.TestSuite.Models;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Main test suite for RuntimeErrorSage
/// </summary>
public class TestSuite
{
    private readonly IErrorAnalyzer _errorAnalyzer;
    private readonly TestScenarioRunner _scenarioRunner;
    private readonly PerformanceTestRunner _performanceRunner;
    private readonly BaselineComparisonTests _baselineComparison;
    private readonly List<TestScenario> _scenarios;

    public TestSuite(IErrorAnalyzer errorAnalyzer)
    {
        _errorAnalyzer = errorAnalyzer;
        _scenarioRunner = new TestScenarioRunner(errorAnalyzer);
        _performanceRunner = new PerformanceTestRunner(errorAnalyzer);
        var runtimeService = new Moq.Mock<RuntimeErrorSage.Application.Runtime.Interfaces.IRuntimeErrorSageService>().Object;
        var standardizedScenarios = new Moq.Mock<StandardizedErrorScenarios>(runtimeService).Object;
        var realWorldScenarios = new Moq.Mock<RealWorldErrorCases>(runtimeService).Object;
        _baselineComparison = new BaselineComparisonTests(runtimeService, standardizedScenarios, realWorldScenarios, errorAnalyzer);
        _scenarios = new List<TestScenario>();
    }

    /// <summary>
    /// Adds a test scenario to the suite
    /// </summary>
    public void AddScenario(TestScenario scenario)
    {
        _scenarios.Add(scenario);
    }

    /// <summary>
    /// Runs all test scenarios
    /// </summary>
    public async Task<TestSuiteResults> RunAllAsync()
    {
        var results = new TestSuiteResults
        {
            StartTime = DateTime.UtcNow
        };

        foreach (var scenario in _scenarios)
        {
            var testResult = await _scenarioRunner.RunScenarioAsync(scenario);
            results.TestResults.Add(testResult);

            if (scenario.Metadata.TryGetValue("PerformanceTest", out var isPerformanceTest) && 
                isPerformanceTest is bool isPerf && isPerf)
            {
                var perfMetrics = await _performanceRunner.RunTestAsync(
                    scenario.Name,
                    async () => await _scenarioRunner.RunScenarioAsync(scenario));
                results.PerformanceMetrics.Add(perfMetrics);
            }

            if (scenario.Metadata.TryGetValue("BaselineComparison", out var isBaseline) && 
                isBaseline is bool isBase && isBase)
            {
                var comparison = await _baselineComparison.CompareResultsAsync(
                    scenario.Name,
                    async () => await _scenarioRunner.RunScenarioAsync(scenario),
                    async () => await _scenarioRunner.RunScenarioAsync(scenario));
                results.BaselineComparisons.Add(comparison);
            }
        }

        results.EndTime = DateTime.UtcNow;
        return results;
    }

    /// <summary>
    /// Gets all test scenarios
    /// </summary>
    public IReadOnlyList<TestScenario> GetScenarios() => _scenarios;
} 
