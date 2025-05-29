using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Tests.TestSuite.Models;

namespace RuntimeErrorSage.Tests.TestSuite;

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
