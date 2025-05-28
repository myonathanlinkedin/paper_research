using RuntimeErrorSage.Core.Analysis.Interfaces;
using RuntimeErrorSage.Tests.TestSuite.Models;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Runs performance tests and collects metrics
/// </summary>
public class PerformanceTestRunner
{
    private readonly IErrorAnalyzer _errorAnalyzer;
    private readonly List<PerformanceMetrics> _metrics;

    public PerformanceTestRunner(IErrorAnalyzer errorAnalyzer)
    {
        _errorAnalyzer = errorAnalyzer;
        _metrics = new List<PerformanceMetrics>();
    }

    /// <summary>
    /// Runs a performance test and collects metrics
    /// </summary>
    public async Task<PerformanceMetrics> RunTestAsync(
        string testName,
        Func<Task> testAction,
        int iterations = 1)
    {
        var metrics = new PerformanceMetrics
        {
            TestName = testName
        };

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var startMemory = GC.GetTotalMemory(false);
        var startGcCount = GC.CollectionCount(0);

        for (int i = 0; i < iterations; i++)
        {
            await testAction();
        }

        stopwatch.Stop();
        var endMemory = GC.GetTotalMemory(false);
        var endGcCount = GC.CollectionCount(0);

        metrics.ExecutionTimeMs = stopwatch.ElapsedMilliseconds / (double)iterations;
        metrics.MemoryUsageBytes = endMemory - startMemory;
        metrics.GarbageCollections = endGcCount - startGcCount;
        metrics.CpuUsagePercentage = GetCpuUsage();
        metrics.ThreadPoolUtilization = GetThreadPoolUtilization();

        _metrics.Add(metrics);
        return metrics;
    }

    /// <summary>
    /// Gets all collected metrics
    /// </summary>
    public IReadOnlyList<PerformanceMetrics> GetMetrics() => _metrics;

    private double GetCpuUsage()
    {
        // Placeholder implementation - would need proper CPU usage monitoring
        return 0.0;
    }

    private double GetThreadPoolUtilization()
    {
        ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxIoThreads);
        ThreadPool.GetAvailableThreads(out int availableWorkerThreads, out int availableIoThreads);
        
        var workerUtilization = 1.0 - (availableWorkerThreads / (double)maxWorkerThreads);
        var ioUtilization = 1.0 - (availableIoThreads / (double)maxIoThreads);
        
        return (workerUtilization + ioUtilization) / 2.0;
    }
} 
