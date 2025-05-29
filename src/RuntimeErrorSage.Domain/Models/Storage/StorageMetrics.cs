using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Storage;

/// <summary>
/// Represents storage-related metrics.
/// </summary>
public class StorageMetrics
{
    /// <summary>
    /// Gets or sets the total storage size in bytes.
    /// </summary>
    public long TotalSizeBytes { get; }

    /// <summary>
    /// Gets or sets the used storage size in bytes.
    /// </summary>
    public long UsedSizeBytes { get; }

    /// <summary>
    /// Gets or sets the available storage size in bytes.
    /// </summary>
    public long AvailableSizeBytes { get; }

    /// <summary>
    /// Gets or sets the number of stored items.
    /// </summary>
    public long ItemCount { get; }

    /// <summary>
    /// Gets or sets the cache hit rate.
    /// </summary>
    public double CacheHitRate { get; }

    /// <summary>
    /// Gets or sets the cache miss rate.
    /// </summary>
    public double CacheMissRate { get; }

    /// <summary>
    /// Gets or sets the average operation latency in milliseconds.
    /// </summary>
    public double AverageLatencyMs { get; }

    /// <summary>
    /// Gets or sets the timestamp of the metrics.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets additional metrics.
    /// </summary>
    public Dictionary<string, double> AdditionalMetrics { get; set; } = new();

    /// <summary>
    /// Gets or sets additional metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 






