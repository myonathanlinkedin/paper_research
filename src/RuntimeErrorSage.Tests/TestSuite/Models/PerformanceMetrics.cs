using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Tests.TestSuite.Models;

/// <summary>
/// Performance metrics for test execution
/// </summary>
public class PerformanceMetrics
{
    /// <summary>
    /// Unique identifier for the metrics
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Name of the test
    /// </summary>
    public string TestName { get; } = string.Empty;

    /// <summary>
    /// Execution time in milliseconds
    /// </summary>
    public double ExecutionTimeMs { get; }

    /// <summary>
    /// Memory usage in bytes
    /// </summary>
    public long MemoryUsageBytes { get; }

    /// <summary>
    /// CPU usage percentage
    /// </summary>
    public double CpuUsagePercentage { get; }

    /// <summary>
    /// Number of garbage collections
    /// </summary>
    public int GarbageCollections { get; }

    /// <summary>
    /// Thread pool utilization
    /// </summary>
    public double ThreadPoolUtilization { get; }

    /// <summary>
    /// Additional metadata for the metrics
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 






