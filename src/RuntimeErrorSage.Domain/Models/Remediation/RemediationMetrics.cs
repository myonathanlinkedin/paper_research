using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Metrics;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents metrics for a remediation operation.
    /// </summary>
    public class RemediationMetrics
    {
        /// <summary>
        /// Gets or sets the unique identifier for the execution.
        /// </summary>
        public string ExecutionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp of the metrics.
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Gets or sets the success rate of the remediation.
        /// </summary>
        public double SuccessRate { get; set; }
        
        /// <summary>
        /// Gets or sets the error rate of the remediation.
        /// </summary>
        public double ErrorRate { get; set; }
        
        /// <summary>
        /// Gets or sets the average execution time in milliseconds.
        /// </summary>
        public long AverageExecutionTimeMs { get; set; }
        
        /// <summary>
        /// Gets or sets the total number of remediations.
        /// </summary>
        public int TotalRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the number of successful remediations.
        /// </summary>
        public int SuccessfulRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the number of failed remediations.
        /// </summary>
        public int FailedRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the number of cancelled remediations.
        /// </summary>
        public int CancelledRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the number of timed out remediations.
        /// </summary>
        public int TimedOutRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the number of validation failed remediations.
        /// </summary>
        public int ValidationFailedRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        public int RetryCount { get; set; }
        
        /// <summary>
        /// Gets or sets the metric values.
        /// </summary>
        public Dictionary<string, object> Values { get; set; } = new Dictionary<string, object>();
        
        /// <summary>
        /// Gets or sets the metric labels.
        /// </summary>
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
        
        /// <summary>
        /// Gets or sets the success flag.
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string? Error { get; set; }
    }
} 
