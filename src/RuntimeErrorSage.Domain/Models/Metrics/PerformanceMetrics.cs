using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Metrics;

/// <summary>
/// Represents performance metrics for the system.
/// </summary>
public class PerformanceMetrics
{
    /// <summary>
    /// Gets or sets the timestamp when these metrics were collected.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the CPU usage percentage.
    /// </summary>
    public double CpuUsage { get; set; }

    /// <summary>
    /// Gets or sets the memory usage in bytes.
    /// </summary>
    public long MemoryUsage { get; set; }

    /// <summary>
    /// Gets or sets the response time in milliseconds.
    /// </summary>
    public double ResponseTime { get; set; }

    /// <summary>
    /// Gets or sets the throughput (requests per second).
    /// </summary>
    public double Throughput { get; set; }

    /// <summary>
    /// Gets or sets the error rate (percentage).
    /// </summary>
    public double ErrorRate { get; set; }

    /// <summary>
    /// Gets or sets the latency in milliseconds.
    /// </summary>
    public double Latency { get; set; }

    /// <summary>
    /// Gets or sets the number of active connections.
    /// </summary>
    public int ActiveConnections { get; set; }

    /// <summary>
    /// Gets or sets the queue length.
    /// </summary>
    public int QueueLength { get; set; }

    /// <summary>
    /// Gets or sets custom metrics.
    /// </summary>
    public Dictionary<string, double> CustomMetrics { get; set; } = new();

    /// <summary>
    /// Gets or sets the collection interval in seconds.
    /// </summary>
    public int CollectionInterval { get; set; }

    /// <summary>
    /// Gets or sets the total processing time.
    /// </summary>
    public TimeSpan TotalProcessingTime { get; set; }

    /// <summary>
    /// Gets or sets the resource usage by phase.
    /// </summary>
    public Dictionary<string, MetricsResourceUsage> PhaseResourceUsage { get; set; } = new Dictionary<string, MetricsResourceUsage>();
}
