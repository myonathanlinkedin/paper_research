using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the result of executing a remediation step.
    /// </summary>
    public class RemediationStepResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for this result.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the step that was executed.
        /// </summary>
        public RemediationStep Step { get; set; } = new();

        /// <summary>
        /// Gets or sets the status of the step execution.
        /// </summary>
        public RemediationStepStatus Status { get; set; }

        /// <summary>
        /// Gets or sets whether the step was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the start time of the step execution.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time of the step execution.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Gets or sets any error message if the step failed.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets any warnings generated during execution.
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Gets or sets the execution logs.
        /// </summary>
        public List<string> Logs { get; set; } = new();

        /// <summary>
        /// Gets or sets any output produced by the step.
        /// </summary>
        public Dictionary<string, object> Output { get; set; } = new();

        /// <summary>
        /// Gets or sets any validation results.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets whether rollback is available for this step.
        /// </summary>
        public bool CanRollback { get; set; }

        /// <summary>
        /// Gets or sets whether manual intervention is required.
        /// </summary>
        public bool RequiresManualIntervention { get; set; }

        /// <summary>
        /// Gets or sets any retry attempts made.
        /// </summary>
        public int RetryAttempts { get; set; }
    }
} 
