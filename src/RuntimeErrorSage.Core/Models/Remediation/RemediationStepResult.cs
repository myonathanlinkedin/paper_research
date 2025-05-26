using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the result of executing a remediation step.
    /// </summary>
    public class RemediationStepResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the step.
        /// </summary>
        public string StepId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the status of the step execution.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the start time of the step execution.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the step execution.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the step execution in milliseconds.
        /// </summary>
        public double Duration => EndTime.HasValue ? (EndTime.Value - StartTime).TotalMilliseconds : 0;

        /// <summary>
        /// Gets or sets the error message if the step failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the exception that occurred during step execution.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the output produced by the step.
        /// </summary>
        public Dictionary<string, object> Output { get; set; } = new();

        /// <summary>
        /// Gets or sets additional metadata about the step execution.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 