using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents the result of a remediation action.
    /// </summary>
    public class ActionResult
    {
        /// <summary>
        /// Gets or sets the ID of the action.
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the result.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets whether the action was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error that occurred, if any.
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// Gets or sets the execution time in milliseconds.
        /// </summary>
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// Gets or sets the validation result.
        /// </summary>
        public ValidationResult ValidationResult { get; set; }

        /// <summary>
        /// Gets or sets the execution result.
        /// </summary>
        public ExecutionResult ExecutionResult { get; set; }

        /// <summary>
        /// Gets or sets additional data associated with the result.
        /// </summary>
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the status of the action.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the output of the action.
        /// </summary>
        public string Output { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any warnings that occurred during execution.
        /// </summary>
        public List<string> Warnings { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the metrics collected during execution.
        /// </summary>
        public Dictionary<string, object> Metrics { get; set; } = new Dictionary<string, object>();
    }
} 