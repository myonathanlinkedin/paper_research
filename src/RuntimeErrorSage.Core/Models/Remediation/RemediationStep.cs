using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents a step in a remediation plan.
    /// </summary>
    public class RemediationStep
    {
        /// <summary>
        /// Gets or sets the step identifier.
        /// </summary>
        public string StepId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the step name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the step description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the step order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the step status.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the step action.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the step parameters.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the step requires manual intervention.
        /// </summary>
        public bool RequiresManualIntervention { get; set; }

        /// <summary>
        /// Gets or sets whether the step can be rolled back.
        /// </summary>
        public bool CanRollback { get; set; }

        /// <summary>
        /// Gets or sets the step dependencies.
        /// </summary>
        public List<string> Dependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the step timeout in seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 300;

        /// <summary>
        /// Gets or sets the step retry count.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the step retry delay in seconds.
        /// </summary>
        public int RetryDelaySeconds { get; set; } = 30;

        /// <summary>
        /// Gets or sets the step start time.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the step end time.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the step error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the step output.
        /// </summary>
        public Dictionary<string, object> Output { get; set; } = new();

        /// <summary>
        /// Gets or sets the step metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the step validation rules.
        /// </summary>
        public List<ValidationRule> ValidationRules { get; set; } = new();

        /// <summary>
        /// Gets or sets the step rollback action.
        /// </summary>
        public string RollbackAction { get; set; }

        /// <summary>
        /// Gets or sets the step rollback parameters.
        /// </summary>
        public Dictionary<string, object> RollbackParameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the step timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
} 