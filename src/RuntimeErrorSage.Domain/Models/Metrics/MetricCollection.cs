using System;
using System.Collections.Generic;
using System.Linq;

namespace RuntimeErrorSage.Domain.Models.Metrics
{
    /// <summary>
    /// Represents a collection of metric values.
    /// </summary>
    public class MetricCollection
    {
        /// <summary>
        /// Gets the metric values in the collection.
        /// </summary>
        public List<MetricValue> Values { get; }

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
            Values = new List<MetricValue>();
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
                throw new ArgumentNullException(nameof(value));
            }

            Values.Add(value);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets all metric values with the specified name.
        /// </summary>
        /// <param name="name">The name of the metrics to get.</param>
        /// <returns>A list of metric values with the specified name.</returns>
        public List<MetricValue> GetMetricsByName(string name)
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
        /// <param name="tag">The tag to search for.</param>
        /// <returns>A list of metric values with the specified tag.</returns>
        public List<MetricValue> GetMetricsByTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException("Tag cannot be null or empty.", nameof(tag));
            }

            return Values.Where(v => v.Tags.Contains(tag)).ToList();
        }

        /// <summary>
        /// Gets all metric values collected within the specified time range.
        /// </summary>
        /// <param name="startTime">The start time of the range.</param>
        /// <param name="endTime">The end time of the range.</param>
        /// <returns>A list of metric values collected within the specified time range.</returns>
        public List<MetricValue> GetMetricsByTimeRange(DateTime startTime, DateTime endTime)
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
                    g => Convert.ToDouble(g.Average(v => Convert.ToDouble(v.Value)))
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
                    g => Convert.ToDouble(g.Max(v => Convert.ToDouble(v.Value)))
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
                    g => Convert.ToDouble(g.Min(v => Convert.ToDouble(v.Value)))
                );
        }
    }
} 
