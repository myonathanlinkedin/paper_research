using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Metrics
{
    /// <summary>
    /// Contains metrics information for a remediation execution step.
    /// </summary>
    public class StepMetrics
    {
        /// <summary>
        /// Gets or sets the step ID.
        /// </summary>
        public string StepId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the execution ID.
        /// </summary>
        public string ExecutionId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the step name.
        /// </summary>
        public string StepName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the step type.
        /// </summary>
        public string StepType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the step status.
        /// </summary>
        public ActionStatus Status { get; set; } = ActionStatus.Unknown;

        /// <summary>
        /// Gets or sets the step start time.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the step end time.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the step duration in milliseconds.
        /// </summary>
        public double DurationMs { get; set; }

        /// <summary>
        /// Gets or sets the error message if the step failed.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the resource usage at the start of the step.
        /// </summary>
        public MetricsResourceUsage StartResourceUsage { get; set; } = new MetricsResourceUsage();

        /// <summary>
        /// Gets or sets the resource usage at the end of the step.
        /// </summary>
        public MetricsResourceUsage EndResourceUsage { get; set; } = new MetricsResourceUsage();

        /// <summary>
        /// Gets or sets additional metadata.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets step-specific metrics.
        /// </summary>
        public Dictionary<string, double> Metrics { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Gets whether the step was successful.
        /// </summary>
        public bool IsSuccessful => Status == ActionStatus.Completed;

        /// <summary>
        /// Gets the CPU usage.
        /// </summary>
        public double CpuUsage { get; set; } = 0.0;

        /// <summary>
        /// Gets the memory usage.
        /// </summary>
        public double MemoryUsage { get; set; } = 0.0;

        /// <summary>
        /// Gets the latency.
        /// </summary>
        public TimeSpan Latency { get; set; } = TimeSpan.Zero;

        // TODO: Implement memory and CPU usage differences if needed   
        ///// <summary>
        ///// Gets the memory usage difference.
        ///// </summary>
        //public long MemoryUsageDiff => EndResourceUsage.MemoryUsageBytes - StartResourceUsage.MemoryUsageBytes;

        ///// <summary>
        ///// Gets the CPU usage difference.
        ///// </summary>
        //public double CpuUsageDiff => EndResourceUsage.CpuUsagePercent - StartResourceUsage.CpuUsagePercent;

        /// <summary>
        /// Creates a new step metrics instance.
        /// </summary>
        /// <param name="stepName">The name of the step.</param>
        /// <param name="executionId">The execution ID.</param>
        /// <returns>A new step metrics instance.</returns>
        public static StepMetrics Create(string stepName, string executionId)
        {
            return new StepMetrics
            {
                StepName = stepName,
                ExecutionId = executionId,
                StartTime = DateTime.UtcNow,
                Status = ActionStatus.InProgress
            };
        }

        /// <summary>
        /// Completes the step metrics.
        /// </summary>
        /// <param name="status">The step status.</param>
        /// <param name="errorMessage">The error message if the step failed.</param>
        public void Complete(ActionStatus status, string errorMessage = "")
        {
            Status = status;
            ErrorMessage = errorMessage;
            EndTime = DateTime.UtcNow;
            DurationMs = (EndTime.Value - StartTime).TotalMilliseconds;
        }

        public StepMetrics()
        {
            StepId = Guid.NewGuid().ToString();
            ExecutionId = string.Empty;
            StepName = string.Empty;
            StepType = string.Empty;
            StartTime = DateTime.UtcNow;
            ErrorMessage = string.Empty;
            StartResourceUsage = new MetricsResourceUsage();
            EndResourceUsage = new MetricsResourceUsage();
            Metadata = new Dictionary<string, string>();
            Metrics = new Dictionary<string, double>();
            CpuUsage = 0.0;
            MemoryUsage = 0.0;
            Latency = TimeSpan.Zero;
            Status = ActionStatus.Unknown;
            DurationMs = 0.0;
            EndTime = null;
        }
    }
} 
