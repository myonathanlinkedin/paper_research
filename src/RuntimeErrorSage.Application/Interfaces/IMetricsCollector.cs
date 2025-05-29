using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Common;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Metrics.Interfaces;

/// <summary>
/// Interface for collecting metrics.
/// </summary>
public interface IMetricsCollector
{
    /// <summary>
    /// Gets whether the collector is enabled.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Gets the collector name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the collector version.
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Records a metric value.
    /// </summary>
    /// <param name="name">The metric name.</param>
    /// <param name="value">The metric value.</param>
    Task RecordMetricAsync(string name, double value);

    /// <summary>
    /// Gets metric values.
    /// </summary>
    /// <param name="name">The metric name.</param>
    /// <param name="range">The time range.</param>
    /// <returns>The metric values.</returns>
    Task<IEnumerable<double>> GetMetricValuesAsync(string name, TimeRange range);

    /// <summary>
    /// Gets aggregated metric values.
    /// </summary>
    /// <param name="name">The metric name.</param>
    /// <param name="range">The time range.</param>
    /// <param name="aggregation">The aggregation type.</param>
    /// <returns>The aggregated metric value.</returns>
    Task<double> GetAggregatedMetricAsync(string name, TimeRange range, AggregationType aggregation);

    /// <summary>
    /// Calculates the health score of a component node.
    /// </summary>
    /// <param name="node">The dependency node.</param>
    /// <returns>The health score (0.0 to 1.0).</returns>
    Task<double> CalculateComponentHealthAsync(Models.Graph.DependencyNode node);

    /// <summary>
    /// Calculates the reliability score of a component node.
    /// </summary>
    /// <param name="node">The dependency node.</param>
    /// <returns>The reliability score (0.0 to 1.0).</returns>
    Task<double> CalculateReliabilityAsync(Models.Graph.DependencyNode node);
} 






