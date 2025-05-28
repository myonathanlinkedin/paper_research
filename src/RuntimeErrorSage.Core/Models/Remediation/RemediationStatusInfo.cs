using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents detailed status information for a remediation operation.
    /// </summary>
    public class RemediationStatusInfo
    {
        /// <summary>
        /// Gets or sets the unique identifier for the remediation.
        /// </summary>
        public string RemediationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current status of the remediation.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the start time of the remediation.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the remediation.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the error message if the remediation failed.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the progress percentage of the remediation.
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// Gets or sets the list of completed steps.
        /// </summary>
        public List<string> CompletedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of pending steps.
        /// </summary>
        public List<string> PendingSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of failed steps.
        /// </summary>
        public List<string> FailedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the current step being executed.
        /// </summary>
        public string? CurrentStep { get; set; }

        /// <summary>
        /// Gets or sets the estimated time remaining for completion.
        /// </summary>
        public TimeSpan? EstimatedTimeRemaining { get; set; }

        /// <summary>
        /// Gets or sets the last update time of the status.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets whether the remediation can be rolled back.
        /// </summary>
        public bool CanRollback { get; set; }

        /// <summary>
        /// Gets or sets the rollback status if applicable.
        /// </summary>
        public RollbackStatus? RollbackStatus { get; set; }
    }
} 
