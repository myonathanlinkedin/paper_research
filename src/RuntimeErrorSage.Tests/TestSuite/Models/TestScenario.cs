namespace RuntimeErrorSage.Tests.TestSuite.Models;

/// <summary>
/// Represents a test scenario for error analysis
/// </summary>
public class TestScenario
{
    /// <summary>
    /// Unique identifier for the scenario
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Name of the scenario
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the scenario
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Category of the scenario (e.g., "Database", "HTTP", "Memory")
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Expected error type
    /// </summary>
    public string ExpectedErrorType { get; set; } = string.Empty;

    /// <summary>
    /// Expected error message
    /// </summary>
    public string ExpectedErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Expected remediation action
    /// </summary>
    public string ExpectedRemediation { get; set; } = string.Empty;

    /// <summary>
    /// Setup action to prepare the scenario
    /// </summary>
    public Func<Task>? SetupAction { get; set; }

    /// <summary>
    /// Action that triggers the error
    /// </summary>
    public Func<Task>? TriggerAction { get; set; }

    /// <summary>
    /// Cleanup action to restore state
    /// </summary>
    public Func<Task>? CleanupAction { get; set; }

    /// <summary>
    /// Additional metadata for the scenario
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 
