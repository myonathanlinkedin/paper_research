using System.Collections.ObjectModel;
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
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Start time of the test suite
    /// </summary>
    public DateTime StartTime { get; }

    /// <summary>
    /// End time of the test suite
    /// </summary>
    public DateTime EndTime { get; }

    /// <summary>
    /// Duration of the test suite
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;

    /// <summary>
    /// Results from individual test scenarios
    /// </summary>
    public IReadOnlyCollection<TestResults> TestResults { get; } = new();

    /// <summary>
    /// Performance metrics from performance tests
    /// </summary>
    public IReadOnlyCollection<PerformanceMetrics> PerformanceMetrics { get; } = new();

    /// <summary>
    /// Results from baseline comparisons
    /// </summary>
    public IReadOnlyCollection<BaselineComparisons> BaselineComparisons { get; } = new();

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






