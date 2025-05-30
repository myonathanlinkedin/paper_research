using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Flow
{
    /// <summary>
    /// Represents data flow in the system.
    /// </summary>
    public class DataFlow
    {
        /// <summary>
        /// Gets or sets the unique identifier for this data flow.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the data flow.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source of the data flow.
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the destination of the data flow.
        /// </summary>
        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the data type being flowed.
        /// </summary>
        public string DataType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the flow started.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the flow ended.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the status of the data flow.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the size of the data being flowed.
        /// </summary>
        public long DataSize { get; set; }

        /// <summary>
        /// Gets or sets the throughput of the data flow.
        /// </summary>
        public double Throughput { get; set; }

        /// <summary>
        /// Gets or sets the latency of the data flow.
        /// </summary>
        public TimeSpan Latency { get; set; }

        /// <summary>
        /// Gets or sets whether the data flow was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the error message if the flow failed.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the stack trace if the flow failed.
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// Gets or sets additional metadata about the data flow.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
} 
