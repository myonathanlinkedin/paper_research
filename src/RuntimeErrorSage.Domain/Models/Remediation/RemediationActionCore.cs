using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Validation;
using RuntimeErrorSage.Application.Models.Error;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents the core properties and state of a remediation action.
    /// </summary>
    public class RemediationActionCore
    {
        /// <summary>
        /// Gets or sets the unique identifier of the action.
        /// </summary>
        public string ActionId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string Name { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the action.
        /// </summary>
        public string Description { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the context in which the action should be executed.
        /// </summary>
        public ErrorContext Context { get; }

        /// <summary>
        /// Gets or sets the priority of the action.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Gets or sets the impact level of the action.
        /// </summary>
        public ImpactLevel Impact { get; }

        /// <summary>
        /// Gets or sets the risk level of the action.
        /// </summary>
        public RiskLevel RiskLevel { get; }

        /// <summary>
        /// Gets or sets the status of the action.
        /// </summary>
        public RemediationStatusEnum Status { get; }

        /// <summary>
        /// Gets or sets whether the action requires manual approval.
        /// </summary>
        public bool RequiresManualApproval { get; }

        /// <summary>
        /// Gets or sets the prerequisites for the action.
        /// </summary>
        public IReadOnlyCollection<Prerequisites> Prerequisites { get; } = new();

        /// <summary>
        /// Gets or sets the dependencies for the action.
        /// </summary>
        public IReadOnlyCollection<Dependencies> Dependencies { get; } = new();

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
        public IReadOnlyCollection<Tags> Tags { get; } = new();

        /// <summary>
        /// Gets or sets the version of the action.
        /// </summary>
        public string Version { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the author of the action.
        /// </summary>
        public string Author { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the creation date of the action.
        /// </summary>
        public DateTime CreatedDate { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last modified date of the action.
        /// </summary>
        public DateTime LastModifiedDate { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the execution timeout in milliseconds.
        /// </summary>
        public int ExecutionTimeoutMs { get; } = 300000; // 5 minutes default

        /// <summary>
        /// Gets or sets the retry count for the action.
        /// </summary>
        public int RetryCount { get; } = 3;

        /// <summary>
        /// Gets or sets the retry delay in milliseconds.
        /// </summary>
        public int RetryDelayMs { get; } = 30000; // 30 seconds default

        /// <summary>
        /// Gets or sets whether the action is enabled.
        /// </summary>
        public bool IsEnabled { get; } = true;

        /// <summary>
        /// Gets or sets whether the action is visible.
        /// </summary>
        public bool IsVisible { get; } = true;

        /// <summary>
        /// Gets or sets the category of the action.
        /// </summary>
        public string Category { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the subcategory of the action.
        /// </summary>
        public string Subcategory { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the severity of the action.
        /// </summary>
        public string Severity { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the complexity of the action.
        /// </summary>
        public string Complexity { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the estimated duration of the action.
        /// </summary>
        public TimeSpan EstimatedDuration { get; }

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
        public string ErrorMessage { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the stack trace if the action failed.
        /// </summary>
        public string StackTrace { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the output of the action.
        /// </summary>
        public string Output { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation results for the action.
        /// </summary>
        public IReadOnlyCollection<ValidationResults> ValidationResults { get; } = new();

        /// <summary>
        /// Gets or sets the rollback status.
        /// </summary>
        public RollbackStatus? RollbackStatus { get; set; }

        /// <summary>
        /// Gets or sets whether the action can be rolled back.
        /// </summary>
        public bool CanRollback { get; }

        /// <summary>
        /// Gets or sets the rollback action if available.
        /// </summary>
        public IRemediationAction RollbackAction { get; }
    }
} 







