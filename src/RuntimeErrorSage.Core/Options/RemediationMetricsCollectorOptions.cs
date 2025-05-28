using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Options
{
    /// <summary>
    /// Configuration options for the RemediationMetricsCollector.
    /// Loaded from appsettings.json via IOptions<RemediationMetricsCollectorOptions>.
    /// </summary>
    /// <example>
    /// Example appsettings.json section:
    /// "RemediationMetricsCollectorOptions": {
    ///   "CollectionInterval": "00:00:01",
    ///   "CollectionTimeout": "00:05:00",
    ///   "EnableDetailedMetrics": true,
    ///   "MetricThresholds": {
    ///     "cpu.usage": 80.0,
    ///     "memory.usage": 85.0,
    ///     "disk.usage": 90.0,
    ///     "network.latency": 100.0,
    ///     "error.rate": 5.0
    ///   }
    /// }
    /// </example>
    public sealed class RemediationMetricsCollectorOptions
    {
        /// <summary>
        /// Gets a value indicating whether detailed metrics collection is enabled.
        /// </summary>
        public required bool EnableDetailedMetrics { get; init; } = true;

        /// <summary>
        /// Gets the interval at which metrics are collected.
        /// </summary>
        public required TimeSpan CollectionInterval { get; init; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Gets the timeout duration for metrics collection operations.
        /// </summary>
        public required TimeSpan CollectionTimeout { get; init; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Gets the threshold values for different metrics.
        /// </summary>
        public required Dictionary<string, double> MetricThresholds { get; init; } = new()
        {
            ["cpu_usage"] = 80.0,
            ["memory_usage"] = 85.0,
            ["disk_usage"] = 90.0,
            ["network_latency"] = 100.0,
            ["error_rate"] = 5.0
        };

        /// <summary>
        /// Gets or sets the maximum number of metrics to keep in history.
        /// </summary>
        public int MaxMetricsHistory { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the metrics retention period.
        /// </summary>
        public TimeSpan MetricsRetentionPeriod { get; set; } = TimeSpan.FromDays(7);
    }
} 
