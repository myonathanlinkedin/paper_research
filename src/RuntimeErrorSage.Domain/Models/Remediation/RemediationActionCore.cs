using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RollbackStatus = RuntimeErrorSage.Domain.Models.Remediation.RollbackStatus;
using RuntimeErrorSage.Domain.Interfaces;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents the core properties and state of a remediation action.
    /// </summary>
    public class RemediationActionCore
    {
        private readonly List<RemediationActionExecution> _executions = new();
        private readonly Dictionary<string, object> _metadata = new();

        /// <summary>
        /// Gets or sets the unique identifier of the action.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the unique identifier of the action (alias for Id).
        /// </summary>
        public string ActionId
        {
            get => Id;
            set => Id = value;
        }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the action.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        public string ActionType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public ErrorContext Context { get; set; }

        /// <summary>
        /// Gets or sets the priority of the action.
        /// </summary>
        public RemediationPriority Priority { get; set; }

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
        public IReadOnlyDictionary<string, object> Metadata => _metadata;

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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the creation date of the action (alias for CreatedAt).
        /// </summary>
        public DateTime CreatedDate
        {
            get => CreatedAt;
            set => CreatedAt = value;
        }

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
        private RemediationSeverity _severity;
        public RemediationSeverity Severity => _severity;

        /// <summary>
        /// Gets or sets the complexity of the action.
        /// </summary>
        public RemediationComplexity Complexity { get; set; } = new RemediationComplexity();

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
        public RollbackStatus RollbackStatus { get; set; } = new() { Status = RollbackStatusEnum.NotAttempted };

        /// <summary>
        /// Gets or sets whether the action can be rolled back.
        /// </summary>
        public bool CanRollback { get; set; }

        /// <summary>
        /// Gets or sets the rollback action if available.
        /// </summary>
        public IRemediationAction RollbackAction { get; set; }

        /// <summary>
        /// Gets or sets the last executed date of the action.
        /// </summary>
        public DateTime? LastExecutedAt { get; set; }

        /// <summary>
        /// Gets or sets the executions for the action.
        /// </summary>
        public IReadOnlyList<RemediationActionExecution> Executions => _executions;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationActionCore"/> class.
        /// </summary>
        public RemediationActionCore()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            Description = string.Empty;
            ActionType = string.Empty;
            Prerequisites = new List<string>();
            Dependencies = new List<string>();
            Parameters = new Dictionary<string, object>();
            _metadata = new Dictionary<string, object>();
            Tags = new List<string>();
            Version = string.Empty;
            Author = string.Empty;
            CreatedAt = DateTime.UtcNow;
            LastModifiedDate = DateTime.UtcNow;
            Category = string.Empty;
            Subcategory = string.Empty;
            _severity = RemediationSeverity.None;
            ErrorMessage = string.Empty;
            StackTrace = string.Empty;
            Output = string.Empty;
            ValidationResults = new List<ValidationResult>();
            RollbackStatus = new() { Status = RollbackStatusEnum.NotAttempted };
            CanRollback = false;
            RollbackAction = null;
            Priority = RemediationPriority.Normal;
            Status = RemediationStatusEnum.NotStarted;
            RequiresManualApproval = false;
            ExecutionTimeoutMs = 0;
            RetryCount = 0;
            RetryDelayMs = 0;
            IsEnabled = true;
            IsVisible = true;
            EstimatedDuration = TimeSpan.Zero;
            ActualDuration = TimeSpan.Zero;
            StartTime = DateTime.UtcNow;
            EndTime = null;
            Context = null;
            LastExecutedAt = null;
        }

        /// <summary>
        /// Adds an execution to the action.
        /// </summary>
        /// <param name="execution">The execution to add.</param>
        public void AddExecution(RemediationActionExecution execution)
        {
            if (execution == null)
                throw new ArgumentNullException(nameof(execution));

            _executions.Add(execution);
            LastExecutedAt = DateTime.UtcNow;
        }

        public void AddMetadata(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            _metadata[key] = value;
        }

        public object? GetMetadata(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _metadata.TryGetValue(key, out var value) ? value : null;
        }

        public void SetMetadata(Dictionary<string, object> metadata)
        {
            _metadata.Clear();
            foreach (var kvp in metadata)
            {
                _metadata[kvp.Key] = kvp.Value;
            }
        }

        public void SetSeverity(RemediationSeverity severity)
        {
            _severity = severity;
        }

        public void SetComplexity(RemediationComplexity complexity)
        {
            Complexity = complexity;
        }

        public void SetRollbackStatus(RollbackStatus status)
        {
            RollbackStatus = status;
        }

        public void SetPriority(RemediationPriority priority)
        {
            Priority = priority;
        }

        public void SetImpact(RemediationImpactLevel impact)
        {
            Impact = (ImpactLevel)impact;
        }

        public void SetRiskLevel(RiskLevel riskLevel)
        {
            RiskLevel = riskLevel;
        }

        public void SetStatus(RemediationStatusEnum status)
        {
            Status = status;
        }
    }
} 


