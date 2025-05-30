using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Execution
{
    /// <summary>
    /// Represents the execution of a remediation action.
    /// </summary>
    public class RemediationActionExecution
    {
        /// <summary>
        /// Gets or sets the unique identifier for this execution.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the action ID that was executed.
        /// </summary>
        public string ActionId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string ActionName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        public string ActionType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the execution.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the execution started.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the execution ended.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the execution.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets whether the execution was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the error message if the execution failed.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the stack trace if the execution failed.
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the retry count if the execution was retried.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets whether the execution was retried.
        /// </summary>
        public bool WasRetried { get; set; }

        /// <summary>
        /// Gets or sets the timeout value for the execution.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets whether the execution timed out.
        /// </summary>
        public bool TimedOut { get; set; }

        /// <summary>
        /// Gets or sets the execution parameters.
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the execution results.
        /// </summary>
        public Dictionary<string, string> Results { get; set; } = new();

        /// <summary>
        /// Gets or sets additional metadata about the execution.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
} 
