using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents a step in a remediation process.
    /// </summary>
    public class RemediationStep
    {
        /// <summary>
        /// Gets or sets the unique identifier for this step.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the step identifier.
        /// </summary>
        public string StepId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the step.
        /// </summary>
        public string Name { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the step.
        /// </summary>
        public RemediationStepType StepType { get; }

        /// <summary>
        /// Gets or sets the type of the step (legacy).
        /// </summary>
        public string Type { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the step.
        /// </summary>
        public string Description { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the order of the step.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Gets or sets whether the step is critical.
        /// </summary>
        public bool IsCritical { get; }

        /// <summary>
        /// Gets or sets the status of the step.
        /// </summary>
        public RemediationStepStatus Status { get; }

        /// <summary>
        /// Gets or sets the start time of the step.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the step.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the error message if the step failed.
        /// </summary>
        public string ErrorMessage { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the action associated with this step.
        /// </summary>
        public RemediationAction Action { get; }

        /// <summary>
        /// Gets or sets the parameters for this step.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();
    }
} 






