namespace RuntimeErrorSage.Tests.TestSuite.Models;

/// <summary>
/// Performance metrics for test execution
/// </summary>
public class PerformanceMetrics
{
    /// <summary>
    /// Unique identifier for the metrics
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Name of the test
    /// </summary>
    public string TestName { get; set; } = string.Empty;

    /// <summary>
    /// Execution time in milliseconds
    /// </summary>
    public double ExecutionTimeMs { get; set; }

    /// <summary>
    /// Memory usage in bytes
    /// </summary>
    public long MemoryUsageBytes { get; set; }

    /// <summary>
    /// CPU usage percentage
    /// </summary>
    public double CpuUsagePercentage { get; set; }

    /// <summary>
    /// Number of garbage collections
    /// </summary>
    public int GarbageCollections { get; set; }

    /// <summary>
    /// Thread pool utilization
    /// </summary>
    public double ThreadPoolUtilization { get; set; }

    /// <summary>
    /// Additional metadata for the metrics
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 
