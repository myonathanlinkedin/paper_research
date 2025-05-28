using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents a single metric value with its metadata
    /// </summary>
    public class MetricValue
    {
        /// <summary>
        /// Gets or sets the name of the metric
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the metric
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the metric was recorded
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the unit of measurement for the metric
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the description of the metric
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the source of the metric
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the category of the metric
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the subcategory of the metric
        /// </summary>
        public string Subcategory { get; set; }

        /// <summary>
        /// Gets or sets the tags associated with the metric
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets additional metadata about the metric
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// Initializes a new instance of the MetricValue class
        /// </summary>
        public MetricValue()
        {
            Timestamp = DateTime.UtcNow;
            Tags = Array.Empty<string>();
            Metadata = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the MetricValue class with the specified values
        /// </summary>
        /// <param name="name">The name of the metric</param>
        /// <param name="value">The value of the metric</param>
        public MetricValue(string name, double value) : this()
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the MetricValue class with the specified values
        /// </summary>
        /// <param name="name">The name of the metric</param>
        /// <param name="value">The value of the metric</param>
        /// <param name="unit">The unit of measurement</param>
        public MetricValue(string name, double value, string unit) : this(name, value)
        {
            Unit = unit;
        }

        /// <summary>
        /// Initializes a new instance of the MetricValue class with the specified values
        /// </summary>
        /// <param name="name">The name of the metric</param>
        /// <param name="value">The value of the metric</param>
        /// <param name="unit">The unit of measurement</param>
        /// <param name="description">The description of the metric</param>
        public MetricValue(string name, double value, string unit, string description) : this(name, value, unit)
        {
            Description = description;
        }

        /// <summary>
        /// Returns a string representation of the MetricValue
        /// </summary>
        /// <returns>A string representation of the MetricValue</returns>
        public override string ToString()
        {
            return $"{Name}: {Value} {Unit}";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            if (obj is not MetricValue other)
                return false;

            return Name == other.Name &&
                   Value == other.Value &&
                   Unit == other.Unit &&
                   Timestamp == other.Timestamp;
        }

        /// <summary>
        /// Returns the hash code for the current object
        /// </summary>
        /// <returns>A hash code for the current object</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value, Unit, Timestamp);
        }
    }
} 
