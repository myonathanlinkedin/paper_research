using System.Collections.ObjectModel;
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
        public string ActionId { get; }

        /// <summary>
        /// Gets or sets the timestamp of the result.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets or sets whether the action was successful.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets or sets the error that occurred, if any.
        /// </summary>
        public Exception Error { get; }

        /// <summary>
        /// Gets or sets the execution time in milliseconds.
        /// </summary>
        public long ExecutionTimeMs { get; }

        /// <summary>
        /// Gets or sets the validation result.
        /// </summary>
        public ValidationResult ValidationResult { get; }

        /// <summary>
        /// Gets or sets the execution result.
        /// </summary>
        public ExecutionResult ExecutionResult { get; }

        /// <summary>
        /// Gets or sets additional data associated with the result.
        /// </summary>
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the status of the action.
        /// </summary>
        public RemediationStatusEnum Status { get; }

        /// <summary>
        /// Gets or sets the output of the action.
        /// </summary>
        public string Output { get; } = string.Empty;

        /// <summary>
        /// Gets or sets any warnings that occurred during execution.
        /// </summary>
        public IReadOnlyCollection<Warnings> Warnings { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the metrics collected during execution.
        /// </summary>
        public Dictionary<string, object> Metrics { get; set; } = new Dictionary<string, object>();
    }
} 





