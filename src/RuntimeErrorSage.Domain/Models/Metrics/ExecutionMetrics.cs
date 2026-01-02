using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Metrics
{
    /// <summary>
    /// Represents metrics collected during the execution of a remediation action.
    /// </summary>
    public class ExecutionMetrics
    {
        /// <summary>
        /// Gets or sets the unique identifier for these metrics.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the ID of the action these metrics are associated with.
        /// </summary>
        public string ActionId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start time of the execution.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time of the execution.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the execution.
        /// </summary>
        public TimeSpan Duration => EndTime.HasValue ? EndTime.Value - StartTime : TimeSpan.Zero;

        /// <summary>
        /// Gets or sets whether the execution was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the error message if the execution failed.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the number of steps executed.
        /// </summary>
        public int StepsExecuted { get; set; }

        /// <summary>
        /// Gets or sets the number of steps failed.
        /// </summary>
        public int StepsFailed { get; set; }

        /// <summary>
        /// Gets or sets the memory usage in bytes.
        /// </summary>
        public long MemoryUsageBytes { get; set; }

        /// <summary>
        /// Gets or sets the CPU time used in milliseconds.
        /// </summary>
        public long CpuTimeMs { get; set; }

        /// <summary>
        /// Gets or sets the number of database operations performed.
        /// </summary>
        public int DatabaseOperations { get; set; }

        /// <summary>
        /// Gets or sets the number of network calls made.
        /// </summary>
        public int NetworkCalls { get; set; }

        /// <summary>
        /// Gets or sets additional execution data.
        /// </summary>
        public Dictionary<string, object> Data { get; set; } = new();

        /// <summary>
        /// Marks the execution as completed.
        /// </summary>
        /// <param name="success">Whether the execution was successful.</param>
        /// <param name="errorMessage">The error message if the execution failed.</param>
        public void Complete(bool success, string? errorMessage = null)
        {
            EndTime = DateTime.UtcNow;
            IsSuccess = success;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Adds a data point to the metrics.
        /// </summary>
        /// <param name="key">The data point key.</param>
        /// <param name="value">The data point value.</param>
        public void AddData(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            }

            Data[key] = value;
        }
    }
} 
