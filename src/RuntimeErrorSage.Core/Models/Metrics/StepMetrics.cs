using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Models.Metrics
{
    /// <summary>
    /// Represents metrics for a remediation step.
    /// </summary>
    public class StepMetrics
    {
        /// <summary>
        /// Gets or sets the step identifier.
        /// </summary>
        public string StepId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the step name.
        /// </summary>
        public string StepName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action identifier.
        /// </summary>
        public string ActionId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the duration in milliseconds.
        /// </summary>
        public double DurationMs { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error type if any.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message if any.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        public DateTime EndTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the resource usage metrics.
        /// </summary>
        public ResourceUsage ResourceUsage { get; set; } = new();

        /// <summary>
        /// Gets or sets additional metrics.
        /// </summary>
        public Dictionary<string, object> AdditionalMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the number of retries attempted.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets whether the step was rolled back.
        /// </summary>
        public bool WasRolledBack { get; set; }

        /// <summary>
        /// Gets or sets additional metrics data.
        /// </summary>
        public Dictionary<string, object> Data { get; set; } = new();

        /// <summary>
        /// Gets or sets the timestamp when these metrics were recorded.
        /// </summary>
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    }
} 