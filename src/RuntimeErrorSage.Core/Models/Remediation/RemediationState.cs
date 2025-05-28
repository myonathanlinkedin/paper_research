using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the state information of a remediation process.
    /// </summary>
    public class RemediationStateInfo
    {
        /// <summary>
        /// Gets or sets the unique identifier for this state.
        /// </summary>
        public string StateId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the status of the remediation.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the current step of the remediation.
        /// </summary>
        public string CurrentStep { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the progress of the remediation (0-100).
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Gets or sets the start time of the remediation.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the remediation.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the error message if the remediation failed.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the metadata of the remediation.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the completed steps.
        /// </summary>
        public List<string> CompletedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the failed steps.
        /// </summary>
        public List<string> FailedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the pending steps.
        /// </summary>
        public List<string> PendingSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the remediation was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets whether the remediation has been rolled back.
        /// </summary>
        public bool IsRolledBack { get; set; }

        /// <summary>
        /// Gets or sets the rollback error message if the rollback failed.
        /// </summary>
        public string RollbackErrorMessage { get; set; } = string.Empty;
    }
} 
