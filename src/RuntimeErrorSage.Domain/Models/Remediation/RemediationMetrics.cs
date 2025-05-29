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
        public RemediationMetrics()
        {
            ExecutionId = Guid.NewGuid().ToString();
            Timestamp = DateTime.UtcNow;
            Values = new Dictionary<string, double>();
            Labels = new Dictionary<string, string>();
            EndResourceUsage = new MetricsResourceUsage();
            SuccessRate = 0.0;
            ErrorRate = 0.0;
            AverageExecutionTimeMs = 0.0;
            TotalRemediations = 0;
            SuccessfulRemediations = 0;
            FailedRemediations = 0;
            CancelledRemediations = 0;
            TimedOutRemediations = 0;
            ValidationFailedRemediations = 0;
            RetryCount = 0;
            Success = false;
            Error = string.Empty;
        }

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
        public Dictionary<string, double> Values { get; set; } = new Dictionary<string, double>();
        
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
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the end resource usage metrics.
        /// </summary>
        public MetricsResourceUsage EndResourceUsage { get; set; } = new MetricsResourceUsage();

        public void UpdateResourceUsage()
        {
            EndResourceUsage = new MetricsResourceUsage
            {
                CpuUsage = 0.0,
                MemoryUsage = 0.0,
                DiskUsage = 0.0,
                NetworkUsage = 0.0
            };
        }
    }
} 
