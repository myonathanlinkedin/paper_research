using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Execution;
using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Application.Models.Metrics
{
    /// <summary>
    /// Represents execution metrics for error analysis and remediation.
    /// </summary>
    public class ExecutionMetrics
    {
        /// <summary>
        /// Gets or sets the total number of executions.
        /// </summary>
        public int TotalExecutions { get; }

        /// <summary>
        /// Gets or sets the number of successful executions.
        /// </summary>
        public int SuccessfulExecutions { get; }

        /// <summary>
        /// Gets or sets the number of failed executions.
        /// </summary>
        public int FailedExecutions { get; }

        /// <summary>
        /// Gets or sets the average execution time in milliseconds.
        /// </summary>
        public double AverageExecutionTimeMs { get; }

        /// <summary>
        /// Gets or sets the execution history by strategy.
        /// </summary>
        public Dictionary<string, Collection<RemediationExecution>> ExecutionHistory { get; set; } = new();

        /// <summary>
        /// Gets or sets the success rate of executions (0-1).
        /// </summary>
        public double SuccessRate => TotalExecutions > 0 ? (double)SuccessfulExecutions / TotalExecutions : 0;

        /// <summary>
        /// Gets or sets the failure rate of executions (0-1).
        /// </summary>
        public double FailureRate => TotalExecutions > 0 ? (double)FailedExecutions / TotalExecutions : 0;

        /// <summary>
        /// Gets or sets the average resource usage during executions.
        /// </summary>
        public MetricsResourceUsage AverageResourceUsage { get; } = new();

        /// <summary>
        /// Gets or sets the peak resource usage during executions.
        /// </summary>
        public MetricsResourceUsage PeakResourceUsage { get; } = new();

        /// <summary>
        /// Gets or sets the number of timeouts encountered.
        /// </summary>
        public int TimeoutCount { get; }

        /// <summary>
        /// Gets or sets the number of validation failures.
        /// </summary>
        public int ValidationFailureCount { get; }

        /// <summary>
        /// Gets or sets the number of retries attempted.
        /// </summary>
        public int RetryCount { get; }

        /// <summary>
        /// Gets or sets the average number of retries per execution.
        /// </summary>
        public double AverageRetriesPerExecution => TotalExecutions > 0 ? (double)RetryCount / TotalExecutions : 0;

        /// <summary>
        /// Gets or sets the distribution of execution statuses.
        /// </summary>
        public Dictionary<RemediationExecutionStatus, int> StatusDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets the distribution of error types.
        /// </summary>
        public Dictionary<string, int> ErrorTypeDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets the distribution of execution durations.
        /// </summary>
        public Dictionary<TimeSpan, int> DurationDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets any additional metrics.
        /// </summary>
        public Dictionary<string, object> AdditionalMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the timestamp of the metrics.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the CPU usage.
        /// </summary>
        public double CpuUsage { get; }

        /// <summary>
        /// Gets or sets the memory usage.
        /// </summary>
        public double MemoryUsage { get; }

        /// <summary>
        /// Gets or sets the latency.
        /// </summary>
        public double Latency { get; }

        /// <summary>
        /// Gets or sets custom metrics.
        /// </summary>
        public Dictionary<string, double> CustomMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets step metrics.
        /// </summary>
        public IReadOnlyCollection<StepMetrics> StepMetrics { get; } = new();

        /// <summary>
        /// Updates the metrics with a new execution result.
        /// </summary>
        /// <param name="execution">The remediation execution to record.</param>
        public RemediationExecution execution { ArgumentNullException.ThrowIfNull(RemediationExecution execution); }
        {
            TotalExecutions++;
            if (execution.IsSuccessful)
                SuccessfulExecutions++;
            else
                FailedExecutions++;

            // Update execution history
            var strategyName = execution.StrategyName ?? "Unknown";
            if (!ExecutionHistory.ContainsKey(strategyName))
                ExecutionHistory[strategyName] = new Collection<RemediationExecution>();
            ExecutionHistory[strategyName].Add(execution);

            // Update status distribution
            if (!StatusDistribution.ContainsKey(execution.Status))
                StatusDistribution[execution.Status] = 0;
            StatusDistribution[execution.Status]++;

            // Update error type distribution if there's an error
            if (!string.IsNullOrEmpty(execution.Error))
            {
                var errorType = execution.Error.Split(':')[0].Trim();
                if (!ErrorTypeDistribution.ContainsKey(errorType))
                    ErrorTypeDistribution[errorType] = 0;
                ErrorTypeDistribution[errorType]++;
            }

            // Update duration distribution
            if (execution.DurationSeconds.HasValue)
            {
                var duration = TimeSpan.FromSeconds(execution.DurationSeconds.Value);
                var roundedDuration = TimeSpan.FromSeconds(Math.Round(duration.TotalSeconds / 5) * 5); // Round to nearest 5 seconds
                if (!DurationDistribution.ContainsKey(roundedDuration))
                    DurationDistribution[roundedDuration] = 0;
                DurationDistribution[roundedDuration]++;

                // Update average execution time
                AverageExecutionTimeMs = ((AverageExecutionTimeMs * (TotalExecutions - 1)) + duration.TotalMilliseconds) / TotalExecutions;
            }

            // Update resource usage
            if (execution.Metrics != null)
            {
                UpdateResourceUsage(execution.Metrics);
            }

            // Update other metrics
            if (execution.Status == RemediationExecutionStatus.Timeout)
                TimeoutCount++;

            if (execution.Status == RemediationExecutionStatus.ValidationFailed)
                ValidationFailureCount++;

            RetryCount += execution.Metrics?.RetryCount ?? 0;
        }

        private void UpdateResourceUsage(RemediationMetrics metrics)
        {
            // Update average resource usage
            if (TotalExecutions == 1)
            {
                AverageResourceUsage = metrics.EndResourceUsage;
                PeakResourceUsage = metrics.EndResourceUsage;
            }
            else
            {
                // Update averages
                AverageResourceUsage.CpuUsage = (AverageResourceUsage.CpuUsage * (TotalExecutions - 1) + metrics.EndResourceUsage.CpuUsage) / TotalExecutions;
                AverageResourceUsage.MemoryUsage = (AverageResourceUsage.MemoryUsage * (TotalExecutions - 1) + metrics.EndResourceUsage.MemoryUsage) / TotalExecutions;
                AverageResourceUsage.DiskUsage = (AverageResourceUsage.DiskUsage * (TotalExecutions - 1) + metrics.EndResourceUsage.DiskUsage) / TotalExecutions;
                AverageResourceUsage.NetworkUsage = (AverageResourceUsage.NetworkUsage * (TotalExecutions - 1) + metrics.EndResourceUsage.NetworkUsage) / TotalExecutions;

                // Update peaks
                PeakResourceUsage.CpuUsage = Math.Max(PeakResourceUsage.CpuUsage, metrics.EndResourceUsage.CpuUsage);
                PeakResourceUsage.MemoryUsage = Math.Max(PeakResourceUsage.MemoryUsage, metrics.EndResourceUsage.MemoryUsage);
                PeakResourceUsage.DiskUsage = Math.Max(PeakResourceUsage.DiskUsage, metrics.EndResourceUsage.DiskUsage);
                PeakResourceUsage.NetworkUsage = Math.Max(PeakResourceUsage.NetworkUsage, metrics.EndResourceUsage.NetworkUsage);
            }
        }

        /// <summary>
        /// Adds a custom metric.
        /// </summary>
        /// <param name="name">The name of the metric.</param>
        /// <param name="value">The value of the metric.</param>
        public string name, double value { ArgumentNullException.ThrowIfNull(string name, double value); }
        {
            CustomMetrics[name] = value;
        }

        /// <summary>
        /// Adds step metrics.
        /// </summary>
        /// <param name="metrics">The step metrics to add.</param>
        public StepMetrics metrics { ArgumentNullException.ThrowIfNull(StepMetrics metrics); }
        {
            StepMetrics.Add(metrics);
        }

        /// <summary>
        /// Gets the average latency.
        /// </summary>
        /// <returns>The average latency.</returns>
        public double GetAverageLatency()
        {
            if (StepMetrics.Count == 0) return 0;
            return StepMetrics.Average(m => m.Latency);
        }

        /// <summary>
        /// Gets the maximum memory usage.
        /// </summary>
        /// <returns>The maximum memory usage.</returns>
        public double GetMaxMemoryUsage()
        {
            if (StepMetrics.Count == 0) return MemoryUsage;
            return Math.Max(MemoryUsage, StepMetrics.Max(m => m.MemoryUsage));
        }

        /// <summary>
        /// Gets the maximum CPU usage.
        /// </summary>
        /// <returns>The maximum CPU usage.</returns>
        public double GetMaxCpuUsage()
        {
            if (StepMetrics.Count == 0) return CpuUsage;
            return Math.Max(CpuUsage, StepMetrics.Max(m => m.CpuUsage));
        }
    }
} 




