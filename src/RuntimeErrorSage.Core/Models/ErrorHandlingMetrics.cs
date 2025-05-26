using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Models
{
    /// <summary>
    /// Represents metrics for error handling methods.
    /// Required by research for baseline comparison.
    /// </summary>
    public class ErrorHandlingMetrics
    {
        /// <summary>
        /// Gets or sets the time taken to identify the root cause.
        /// Required by research for performance comparison.
        /// </summary>
        public TimeSpan RootCauseIdentificationTime { get; set; }

        /// <summary>
        /// Gets or sets the time taken to suggest remediation.
        /// Required by research for performance comparison.
        /// </summary>
        public TimeSpan RemediationSuggestionTime { get; set; }

        /// <summary>
        /// Gets or sets the accuracy of root cause identification.
        /// Required by research for accuracy comparison.
        /// </summary>
        public float RootCauseAccuracy { get; set; }

        /// <summary>
        /// Gets or sets the accuracy of remediation suggestions.
        /// Required by research for accuracy comparison.
        /// </summary>
        public float RemediationAccuracy { get; set; }

        /// <summary>
        /// Gets or sets the resource usage during analysis.
        /// Required by research for performance comparison.
        /// </summary>
        public ResourceUsage ResourceUsage { get; set; } = new();

        public string ServiceName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int TotalErrors { get; set; }
        public int AnalyzedErrors { get; set; }
        public int RemediatedErrors { get; set; }
        public int SuccessfulRemediations { get; set; }
        public double AverageAnalysisLatencyMs { get; set; }
        public double AverageRemediationDurationSeconds { get; set; }
        public double ErrorAnalysisAccuracy { get; set; }
        public double RemediationSuccessRate { get; set; }
        public Dictionary<string, int> ErrorTypeCounts { get; set; } = new();
        public Dictionary<string, int> RemediationStrategyCounts { get; set; } = new();
        public Dictionary<string, object> CustomMetrics { get; set; } = new();
    }
} 
