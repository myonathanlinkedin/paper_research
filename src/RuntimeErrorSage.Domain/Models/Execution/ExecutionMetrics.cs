using System;

namespace RuntimeErrorSage.Domain.Models.Execution
{
    /// <summary>
    /// Represents metrics for an execution operation.
    /// </summary>
    public class ExecutionMetrics
    {
        /// <summary>
        /// Gets or sets the execution time in milliseconds.
        /// </summary>
        public double ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the number of errors encountered.
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// Gets or sets the number of warnings encountered.
        /// </summary>
        public int WarningCount { get; set; }

        /// <summary>
        /// Gets or sets the start time of the execution.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time of the execution.
        /// </summary>
        public DateTime EndTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the total duration of the execution in milliseconds.
        /// </summary>
        public double Duration => (EndTime - StartTime).TotalMilliseconds;
    }
} 