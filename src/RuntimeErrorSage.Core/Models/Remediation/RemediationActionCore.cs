using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the core properties and state of a remediation action.
    /// </summary>
    public class RemediationActionCore
    {
        /// <summary>
        /// Gets or sets the unique identifier of the action.
        /// </summary>
        public string ActionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the action.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the context in which the action should be executed.
        /// </summary>
        public string Context { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the priority of the action.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the impact level of the action.
        /// </summary>
        public ImpactLevel Impact { get; set; }

        /// <summary>
        /// Gets or sets the risk level of the action.
        /// </summary>
        public RiskLevel RiskLevel { get; set; }

        /// <summary>
        /// Gets or sets the status of the action.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets whether the action requires manual approval.
        /// </summary>
        public bool RequiresManualApproval { get; set; }

        /// <summary>
        /// Gets or sets the prerequisites for the action.
        /// </summary>
        public List<string> Prerequisites { get; set; } = new();

        /// <summary>
        /// Gets or sets the dependencies for the action.
        /// </summary>
        public List<string> Dependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the parameters for the action.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the metadata for the action.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the tags for the action.
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// Gets or sets the version of the action.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the author of the action.
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the creation date of the action.
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last modified date of the action.
        /// </summary>
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the execution timeout in milliseconds.
        /// </summary>
        public int ExecutionTimeoutMs { get; set; } = 300000; // 5 minutes default

        /// <summary>
        /// Gets or sets the retry count for the action.
        /// </summary>
        public int RetryCount { get; set; } = 3;

        /// <summary>
        /// Gets or sets the retry delay in milliseconds.
        /// </summary>
        public int RetryDelayMs { get; set; } = 30000; // 30 seconds default

        /// <summary>
        /// Gets or sets whether the action is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the action is visible.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Gets or sets the category of the action.
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the subcategory of the action.
        /// </summary>
        public string Subcategory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the severity of the action.
        /// </summary>
        public string Severity { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the complexity of the action.
        /// </summary>
        public string Complexity { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the estimated duration of the action.
        /// </summary>
        public TimeSpan EstimatedDuration { get; set; }

        /// <summary>
        /// Gets or sets the actual duration of the action.
        /// </summary>
        public TimeSpan? ActualDuration { get; set; }

        /// <summary>
        /// Gets or sets the start time of the action.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the action.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the error message if the action failed.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stack trace if the action failed.
        /// </summary>
        public string StackTrace { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the output of the action.
        /// </summary>
        public string Output { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation results for the action.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the rollback status.
        /// </summary>
        public RollbackStatus? RollbackStatus { get; set; }

        /// <summary>
        /// Gets or sets whether the action can be rolled back.
        /// </summary>
        public bool CanRollback { get; set; }

        /// <summary>
        /// Gets or sets the rollback action if available.
        /// </summary>
        public RemediationAction RollbackAction { get; set; }
    }
} 


