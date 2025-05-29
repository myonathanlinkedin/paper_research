using System.Collections.ObjectModel;
using System;
using RuntimeErrorSage.Application.Models.Error;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Results from running a test scenario
/// </summary>
public class TestResult
{
    /// <summary>
    /// Unique identifier for the result
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// ID of the scenario that was run
    /// </summary>
    public string ScenarioId { get; } = string.Empty;

    /// <summary>
    /// Name of the scenario that was run
    /// </summary>
    public string ScenarioName { get; } = string.Empty;

    /// <summary>
    /// Whether the test passed
    /// </summary>
    public bool Passed { get; }

    /// <summary>
    /// Error message if the test failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Error analysis results
    /// </summary>
    public ErrorAnalysis? ErrorAnalysis { get; set; }

    /// <summary>
    /// Start time of the test
    /// </summary>
    public DateTime StartTime { get; }

    /// <summary>
    /// End time of the test
    /// </summary>
    public DateTime EndTime { get; }

    /// <summary>
    /// Duration of the test
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;
} 




