using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Contains metrics information for a remediation execution.
    /// </summary>
    public class RemediationMetrics
    {
        /// <summary>
        /// Gets or sets the execution ID.
        /// </summary>
        public string ExecutionId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the execution start time.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the execution end time.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the execution duration in milliseconds.
        /// </summary>
        public double DurationMs { get; set; }

        /// <summary>
        /// Gets or sets the number of actions executed.
        /// </summary>
        public int ActionCount { get; set; }

        /// <summary>
        /// Gets or sets the number of successful actions.
        /// </summary>
        public int SuccessfulActionCount { get; set; }

        /// <summary>
        /// Gets or sets the number of failed actions.
        /// </summary>
        public int FailedActionCount { get; set; }

        /// <summary>
        /// Gets or sets the resource usage at the start of the execution.
        /// </summary>
        public ResourceUsage StartResourceUsage { get; set; } = new ResourceUsage();

        /// <summary>
        /// Gets or sets the resource usage at the end of the execution.
        /// </summary>
        public ResourceUsage EndResourceUsage { get; set; } = new ResourceUsage();

        /// <summary>
        /// Gets or sets the step metrics for the execution.
        /// </summary>
        public List<StepMetrics> StepMetrics { get; set; } = new List<StepMetrics>();

        /// <summary>
        /// Gets or sets additional metadata.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets the success rate as a percentage.
        /// </summary>
        public double SuccessRate => ActionCount > 0 ? (double)SuccessfulActionCount / ActionCount * 100 : 0;

        /// <summary>
        /// Gets the memory usage difference.
        /// </summary>
        public long MemoryUsageDiff => EndResourceUsage.MemoryUsageBytes - StartResourceUsage.MemoryUsageBytes;

        /// <summary>
        /// Gets the CPU usage difference.
        /// </summary>
        public double CpuUsageDiff => EndResourceUsage.CpuUsagePercent - StartResourceUsage.CpuUsagePercent;

        /// <summary>
        /// Creates a new metrics instance for a specific action.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="parentExecutionId">The parent execution ID.</param>
        /// <returns>A new metrics instance.</returns>
        public static RemediationMetrics CreateForAction(string actionName, string parentExecutionId)
        {
            return new RemediationMetrics
            {
                ExecutionId = Guid.NewGuid().ToString(),
                CorrelationId = parentExecutionId,
                StartTime = DateTime.UtcNow,
                Metadata = new Dictionary<string, string>
                {
                    { "ActionName", actionName },
                    { "ParentExecutionId", parentExecutionId }
                }
            };
        }
    }

    /// <summary>
    /// Represents resource usage metrics.
    /// </summary>
    public class ResourceUsage
    {
        /// <summary>
        /// Gets or sets the CPU usage percentage.
        /// </summary>
        public double CpuUsagePercent { get; set; }

        /// <summary>
        /// Gets or sets the memory usage in bytes.
        /// </summary>
        public double MemoryUsageBytes { get; set; }

        /// <summary>
        /// Gets or sets the disk usage in bytes.
        /// </summary>
        public double DiskUsage { get; set; }

        /// <summary>
        /// Gets or sets the network usage in bytes.
        /// </summary>
        public double NetworkUsage { get; set; }
    }
} 