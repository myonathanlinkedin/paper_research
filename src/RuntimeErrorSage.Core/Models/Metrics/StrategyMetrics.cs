using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Metrics
{
    /// <summary>
    /// Represents metrics for a remediation strategy.
    /// </summary>
    public class StrategyMetrics
    {
        /// <summary>
        /// Gets or sets the name of the strategy.
        /// </summary>
        public string StrategyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total number of executions.
        /// </summary>
        public int ExecutionCount { get; set; }

        /// <summary>
        /// Gets or sets the number of successful executions.
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// Gets or sets the number of failed executions.
        /// </summary>
        public int FailureCount { get; set; }

        /// <summary>
        /// Gets or sets the total duration of all executions in milliseconds.
        /// </summary>
        public double TotalDurationMs { get; set; }

        /// <summary>
        /// Gets or sets the average duration of executions in milliseconds.
        /// </summary>
        public double AverageDurationMs { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the last execution.
        /// </summary>
        public DateTime? LastExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the success rate of the strategy (0-1).
        /// </summary>
        public double SuccessRate { get; set; }

        /// <summary>
        /// Gets or sets the failure rate of the strategy (0-1).
        /// </summary>
        public double FailureRate { get; set; }

        /// <summary>
        /// Gets or sets the count of errors by type.
        /// </summary>
        public Dictionary<string, int> ErrorCounts { get; set; } = new();

        /// <summary>
        /// Gets or sets the average resource usage during executions.
        /// </summary>
        public ResourceUsage AverageResourceUsage { get; set; } = new();

        /// <summary>
        /// Gets or sets the peak resource usage during executions.
        /// </summary>
        public ResourceUsage PeakResourceUsage { get; set; } = new();

        /// <summary>
        /// Gets or sets the number of retries attempted.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the average number of retries per execution.
        /// </summary>
        public double AverageRetriesPerExecution { get; set; }

        /// <summary>
        /// Gets or sets the number of timeouts encountered.
        /// </summary>
        public int TimeoutCount { get; set; }

        /// <summary>
        /// Gets or sets the number of validation failures.
        /// </summary>
        public int ValidationFailureCount { get; set; }

        /// <summary>
        /// Gets or sets any additional metrics.
        /// </summary>
        public Dictionary<string, object> AdditionalMetrics { get; set; } = new();

        /// <summary>
        /// Updates the metrics with a new execution result.
        /// </summary>
        /// <param name="durationMs">The duration of the execution in milliseconds.</param>
        /// <param name="isSuccessful">Whether the execution was successful.</param>
        /// <param name="errorType">The type of error if any.</param>
        /// <param name="resourceUsage">The resource usage during execution.</param>
        /// <param name="retryCount">The number of retries attempted.</param>
        /// <param name="isTimeout">Whether the execution timed out.</param>
        /// <param name="isValidationFailure">Whether the execution failed validation.</param>
        public void UpdateMetrics(
            double durationMs,
            bool isSuccessful,
            string? errorType = null,
            ResourceUsage? resourceUsage = null,
            int retryCount = 0,
            bool isTimeout = false,
            bool isValidationFailure = false)
        {
            ExecutionCount++;
            if (isSuccessful)
                SuccessCount++;
            else
                FailureCount++;

            TotalDurationMs += durationMs;
            AverageDurationMs = TotalDurationMs / ExecutionCount;
            LastExecutionTime = DateTime.UtcNow;

            SuccessRate = (double)SuccessCount / ExecutionCount;
            FailureRate = (double)FailureCount / ExecutionCount;

            if (!string.IsNullOrEmpty(errorType))
            {
                if (!ErrorCounts.ContainsKey(errorType))
                    ErrorCounts[errorType] = 0;
                ErrorCounts[errorType]++;
            }

            if (resourceUsage != null)
            {
                UpdateResourceUsage(resourceUsage);
            }

            RetryCount += retryCount;
            AverageRetriesPerExecution = (double)RetryCount / ExecutionCount;

            if (isTimeout)
                TimeoutCount++;

            if (isValidationFailure)
                ValidationFailureCount++;
        }

        private void UpdateResourceUsage(ResourceUsage usage)
        {
            // Update average resource usage
            if (ExecutionCount == 1)
            {
                AverageResourceUsage = usage;
                PeakResourceUsage = usage;
            }
            else
            {
                // Update averages
                AverageResourceUsage.CpuUsage = (AverageResourceUsage.CpuUsage * (ExecutionCount - 1) + usage.CpuUsage) / ExecutionCount;
                AverageResourceUsage.MemoryUsage = (AverageResourceUsage.MemoryUsage * (ExecutionCount - 1) + usage.MemoryUsage) / ExecutionCount;
                AverageResourceUsage.DiskUsage = (AverageResourceUsage.DiskUsage * (ExecutionCount - 1) + usage.DiskUsage) / ExecutionCount;
                AverageResourceUsage.NetworkUsage = (AverageResourceUsage.NetworkUsage * (ExecutionCount - 1) + usage.NetworkUsage) / ExecutionCount;

                // Update peaks
                PeakResourceUsage.CpuUsage = Math.Max(PeakResourceUsage.CpuUsage, usage.CpuUsage);
                PeakResourceUsage.MemoryUsage = Math.Max(PeakResourceUsage.MemoryUsage, usage.MemoryUsage);
                PeakResourceUsage.DiskUsage = Math.Max(PeakResourceUsage.DiskUsage, usage.DiskUsage);
                PeakResourceUsage.NetworkUsage = Math.Max(PeakResourceUsage.NetworkUsage, usage.NetworkUsage);
            }
        }
    }
} 