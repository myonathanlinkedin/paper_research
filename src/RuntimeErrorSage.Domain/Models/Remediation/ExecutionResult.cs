using System;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents the result of an action execution.
    /// </summary>
    public class ExecutionResult
    {
        /// <summary>
        /// Gets or sets the ID of the action that was executed.
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the start time of the execution.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the execution.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the status of the execution.
        /// </summary>
        public ExecutionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets whether the execution was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error that occurred during execution, if any.
        /// </summary>
        public Exception Error { get; set; }
    }
} 