using System;

namespace RuntimeErrorSage.Tests.TestSuite.Models;

/// <summary>
/// Performance metric for test execution.
/// </summary>
public class PerformanceMetric
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PerformanceMetric"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="durationMs">The duration in milliseconds.</param>
    /// <param name="memoryUsage">The memory usage in bytes.</param>
    /// <param name="cpuUsage">The CPU usage percentage.</param>
    /// <param name="passed">Whether the test passed.</param>
    /// <param name="errorMessage">The error message if the test failed.</param>
    public PerformanceMetric(
        string name,
        double durationMs,
        long memoryUsage,
        double cpuUsage,
        bool passed = true,
        string errorMessage = null)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        if (durationMs < 0)
            throw new ArgumentException("Duration cannot be negative.", nameof(durationMs));

        if (memoryUsage < 0)
            throw new ArgumentException("Memory usage cannot be negative.", nameof(memoryUsage));

        if (cpuUsage < 0 || cpuUsage > 100)
            throw new ArgumentException("CPU usage must be between 0 and 100.", nameof(cpuUsage));

        Name = name;
        DurationMs = durationMs;
        MemoryUsage = memoryUsage;
        CpuUsage = cpuUsage;
        Passed = passed;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the duration in milliseconds.
    /// </summary>
    public double DurationMs { get; }

    /// <summary>
    /// Gets the memory usage in bytes.
    /// </summary>
    public long MemoryUsage { get; }

    /// <summary>
    /// Gets the CPU usage percentage.
    /// </summary>
    public double CpuUsage { get; }

    /// <summary>
    /// Gets a value indicating whether the test passed.
    /// </summary>
    public bool Passed { get; }

    /// <summary>
    /// Gets the error message if the test failed.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Validates the metric.
    /// </summary>
    /// <returns>True if the metric is valid; otherwise, false.</returns>
    public bool Validate()
    {
        if (string.IsNullOrEmpty(Name))
            return false;

        if (DurationMs < 0)
            return false;

        if (MemoryUsage < 0)
            return false;

        if (CpuUsage < 0 || CpuUsage > 100)
            return false;

        if (!Passed && string.IsNullOrEmpty(ErrorMessage))
            return false;

        return true;
    }
} 