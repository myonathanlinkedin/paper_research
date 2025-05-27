using RuntimeErrorSage.Core.Analysis.Interfaces;
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
        _baselineComparison = new BaselineComparisonTests(errorAnalyzer);
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

/// <summary>
/// Results from running the test suite
/// </summary>
public class TestSuiteResults
{
    /// <summary>
    /// Unique identifier for the results
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Start time of the test suite
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// End time of the test suite
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Duration of the test suite
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;

    /// <summary>
    /// Results from individual test scenarios
    /// </summary>
    public List<TestResult> TestResults { get; set; } = new();

    /// <summary>
    /// Performance metrics from performance tests
    /// </summary>
    public List<PerformanceMetrics> PerformanceMetrics { get; set; } = new();

    /// <summary>
    /// Results from baseline comparisons
    /// </summary>
    public List<ComparisonResults> BaselineComparisons { get; set; } = new();

    /// <summary>
    /// Whether all tests passed
    /// </summary>
    public bool AllTestsPassed => TestResults.All(r => r.Passed);

    /// <summary>
    /// Number of passed tests
    /// </summary>
    public int PassedTests => TestResults.Count(r => r.Passed);

    /// <summary>
    /// Number of failed tests
    /// </summary>
    public int FailedTests => TestResults.Count(r => !r.Passed);

    /// <summary>
    /// Pass rate as a percentage
    /// </summary>
    public double PassRate => TestResults.Count == 0 ? 0 : (PassedTests / (double)TestResults.Count) * 100;
} 