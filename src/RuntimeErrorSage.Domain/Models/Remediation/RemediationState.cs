using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents the state information of a remediation process.
    /// </summary>
    public class RemediationStateInfo
    {
        /// <summary>
        /// Gets or sets the unique identifier for this state.
        /// </summary>
        public string StateId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the status of the remediation.
        /// </summary>
        public RemediationStatusEnum Status { get; }

        /// <summary>
        /// Gets or sets the current step of the remediation.
        /// </summary>
        public string CurrentStep { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the progress of the remediation (0-100).
        /// </summary>
        public int Progress { get; }

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
        public string ErrorMessage { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the metadata of the remediation.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the completed steps.
        /// </summary>
        public IReadOnlyCollection<CompletedSteps> CompletedSteps { get; } = new();

        /// <summary>
        /// Gets or sets the failed steps.
        /// </summary>
        public IReadOnlyCollection<FailedSteps> FailedSteps { get; } = new();

        /// <summary>
        /// Gets or sets the pending steps.
        /// </summary>
        public IReadOnlyCollection<PendingSteps> PendingSteps { get; } = new();

        /// <summary>
        /// Gets or sets whether the remediation was successful.
        /// </summary>
        public bool IsSuccessful { get; }

        /// <summary>
        /// Gets or sets whether the remediation has been rolled back.
        /// </summary>
        public bool IsRolledBack { get; }

        /// <summary>
        /// Gets or sets the rollback error message if the rollback failed.
        /// </summary>
        public string RollbackErrorMessage { get; } = string.Empty;
    }
} 






