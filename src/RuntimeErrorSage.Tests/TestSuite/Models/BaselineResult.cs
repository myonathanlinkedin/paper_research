namespace RuntimeErrorSage.Tests.TestSuite.Models;

/// <summary>
/// Results from baseline execution
/// </summary>
public class BaselineResult
{
    /// <summary>
    /// Unique identifier for the result
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Name of the baseline test
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Duration of the test in milliseconds
    /// </summary>
    public long DurationMs { get; set; }

    /// <summary>
    /// Memory usage in bytes
    /// </summary>
    public long MemoryUsage { get; set; }

    /// <summary>
    /// CPU usage percentage
    /// </summary>
    public double CpuUsage { get; set; }

    /// <summary>
    /// Whether the test passed
    /// </summary>
    public bool Passed { get; set; }

    /// <summary>
    /// Error message if the test failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Additional metadata for the result
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 
