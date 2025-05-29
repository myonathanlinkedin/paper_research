using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuntimeErrorSage.Application.Models.Metrics
{
    /// <summary>
    /// Represents a collection of metric values.
    /// </summary>
    public class MetricCollection
    {
        /// <summary>
        /// Gets the metric values in the collection.
        /// </summary>
        public Collection<MetricValue> Values { get; }

        /// <summary>
        /// Gets the timestamp when the collection was created.
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Gets the timestamp when the collection was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricCollection"/> class.
        /// </summary>
        public MetricCollection()
        {
            Values = new Collection<MetricValue>();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Adds a metric value to the collection.
        /// </summary>
        /// <param name="value">The metric value to add.</param>
        public void AddMetric(MetricValue value)
        {
            if (value == null)
            {
                ArgumentNullException.ThrowIfNull(nameof(value));
            }

            Values.Add(value);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets all metric values with the specified name.
        /// </summary>
        /// <param name="name">The name of the metrics to get.</param>
        /// <returns>A list of metric values with the specified name.</returns>
        public Collection<MetricValue> GetMetricsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Metric name cannot be null or empty.", nameof(name));
            }

            return Values.Where(v => v.Name == name).ToList();
        }

        /// <summary>
        /// Gets all metric values with the specified tag.
        /// </summary>
        /// <param name="key">The key of the tag.</param>
        /// <param name="value">The value of the tag.</param>
        /// <returns>A list of metric values with the specified tag.</returns>
        public Collection<MetricValue> GetMetricsByTag(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Tag key cannot be null or empty.", nameof(key));
            }

            return Values.Where(v => v.Tags.ContainsKey(key) && v.Tags[key] == value).ToList();
        }

        /// <summary>
        /// Gets all metric values collected within the specified time range.
        /// </summary>
        /// <param name="startTime">The start time of the range.</param>
        /// <param name="endTime">The end time of the range.</param>
        /// <returns>A list of metric values collected within the specified time range.</returns>
        public Collection<MetricValue> GetMetricsByTimeRange(DateTime startTime, DateTime endTime)
        {
            return Values.Where(v => v.Timestamp >= startTime && v.Timestamp <= endTime).ToList();
        }

        /// <summary>
        /// Gets the latest value for each metric in the collection.
        /// </summary>
        /// <returns>A dictionary mapping metric names to their latest values.</returns>
        public Dictionary<string, MetricValue> GetLatestMetrics()
        {
            return Values
                .GroupBy(v => v.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(v => v.Timestamp).First()
                );
        }

        /// <summary>
        /// Gets the average value for each metric in the collection.
        /// </summary>
        /// <returns>A dictionary mapping metric names to their average values.</returns>
        public Dictionary<string, double> GetAverageMetrics()
        {
            return Values
                .GroupBy(v => v.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.Average(v => v.Value)
                );
        }

        /// <summary>
        /// Gets the maximum value for each metric in the collection.
        /// </summary>
        /// <returns>A dictionary mapping metric names to their maximum values.</returns>
        public Dictionary<string, double> GetMaxMetrics()
        {
            return Values
                .GroupBy(v => v.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.Max(v => v.Value)
                );
        }

        /// <summary>
        /// Gets the minimum value for each metric in the collection.
        /// </summary>
        /// <returns>A dictionary mapping metric names to their minimum values.</returns>
        public Dictionary<string, double> GetMinMetrics()
        {
            return Values
                .GroupBy(v => v.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.Min(v => v.Value)
                );
        }
    }
} 





