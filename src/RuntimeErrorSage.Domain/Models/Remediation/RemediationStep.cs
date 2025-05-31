using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents a single step in a remediation execution.
    /// </summary>
    public class RemediationStep
    {
        /// <summary>
        /// Gets or sets the unique identifier for the step.
        /// </summary>
        public string StepId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the step.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the step.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the associated action.
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the type of the action being performed.
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the step started.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the step completed.
        /// </summary>
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the step completed.
        /// Alias for CompletionTime to maintain compatibility with existing code.
        /// </summary>
        public DateTime? EndTime 
        { 
            get => CompletionTime; 
            set => CompletionTime = value;
        }

        /// <summary>
        /// Gets or sets the status of the step.
        /// </summary>
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// Gets or sets any additional messages related to the step.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets additional details about the step.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets error message if the step failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the step can be rolled back.
        /// </summary>
        public bool CanRollback { get; set; }

        /// <summary>
        /// Gets or sets the order of the step in the execution sequence.
        /// </summary>
        public int Order { get; set; }
    }
} 
