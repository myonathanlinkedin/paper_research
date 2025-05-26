using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents a remediation action.
    /// </summary>
    public class RemediationAction
    {
        /// <summary>
        /// Gets or sets the action identifier.
        /// </summary>
        public string ActionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the action description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        public RemediationActionType Type { get; set; }

        /// <summary>
        /// Gets or sets the action priority.
        /// </summary>
        public RemediationActionPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the action severity.
        /// </summary>
        public RemediationActionSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the action impact scope.
        /// </summary>
        public RemediationActionImpactScope ImpactScope { get; set; }

        /// <summary>
        /// Gets or sets the action parameters.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the action requires manual intervention.
        /// </summary>
        public bool RequiresManualIntervention { get; set; }

        /// <summary>
        /// Gets or sets whether the action can be rolled back.
        /// </summary>
        public bool CanRollback { get; set; }

        /// <summary>
        /// Gets or sets the action dependencies.
        /// </summary>
        public List<string> Dependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the action timeout in seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 300;

        /// <summary>
        /// Gets or sets the action retry count.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the action retry delay in seconds.
        /// </summary>
        public int RetryDelaySeconds { get; set; } = 30;

        /// <summary>
        /// Gets or sets the action validation rules.
        /// </summary>
        public List<ValidationRule> ValidationRules { get; set; } = new();

        /// <summary>
        /// Gets or sets the action rollback steps.
        /// </summary>
        public List<RollbackStep> RollbackSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the action metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Specifies the type of remediation action.
    /// </summary>
    public enum RemediationActionType
    {
        /// <summary>
        /// Automated action.
        /// </summary>
        Automated,

        /// <summary>
        /// Manual action.
        /// </summary>
        Manual,

        /// <summary>
        /// Hybrid action.
        /// </summary>
        Hybrid
    }

    /// <summary>
    /// Specifies the priority of a remediation action.
    /// </summary>
    public enum RemediationActionPriority
    {
        /// <summary>
        /// Low priority.
        /// </summary>
        Low,

        /// <summary>
        /// Medium priority.
        /// </summary>
        Medium,

        /// <summary>
        /// High priority.
        /// </summary>
        High,

        /// <summary>
        /// Critical priority.
        /// </summary>
        Critical
    }

    /// <summary>
    /// Specifies the severity of a remediation action.
    /// </summary>
    public enum RemediationActionSeverity
    {
        /// <summary>
        /// Low severity.
        /// </summary>
        Low,

        /// <summary>
        /// Medium severity.
        /// </summary>
        Medium,

        /// <summary>
        /// High severity.
        /// </summary>
        High,

        /// <summary>
        /// Critical severity.
        /// </summary>
        Critical
    }

    /// <summary>
    /// Specifies the impact scope of a remediation action.
    /// </summary>
    public enum RemediationActionImpactScope
    {
        /// <summary>
        /// Local impact.
        /// </summary>
        Local,

        /// <summary>
        /// Component impact.
        /// </summary>
        Component,

        /// <summary>
        /// Service impact.
        /// </summary>
        Service,

        /// <summary>
        /// System impact.
        /// </summary>
        System
    }

    /// <summary>
    /// Represents the status of a remediation action.
    /// </summary>
    public enum ActionStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        Cancelled,
        TimedOut,
        Unknown
    }

    /// <summary>
    /// Represents a validation rule for a remediation action.
    /// </summary>
    public class ValidationRule
    {
        /// <summary>
        /// Gets or sets the rule identifier.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the rule description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rule condition.
        /// </summary>
        public string Condition { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rule parameters.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();
    }

    /// <summary>
    /// Represents an execution step for a remediation action.
    /// </summary>
    public class ExecutionStep
    {
        /// <summary>
        /// Gets or sets the step identifier.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the step description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the step command.
        /// </summary>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the step parameters.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the step order.
        /// </summary>
        public int Order { get; set; }
    }

    /// <summary>
    /// Represents a retry policy for a remediation action.
    /// </summary>
    public class RetryPolicy
    {
        /// <summary>
        /// Gets or sets the maximum number of retries.
        /// </summary>
        public int MaxRetries { get; set; }

        /// <summary>
        /// Gets or sets the delay between retries.
        /// </summary>
        public TimeSpan RetryDelay { get; set; }

        /// <summary>
        /// Gets or sets whether to use exponential backoff.
        /// </summary>
        public bool UseExponentialBackoff { get; set; }

        /// <summary>
        /// Gets or sets the maximum delay between retries.
        /// </summary>
        public TimeSpan? MaxRetryDelay { get; set; }
    }

    /// <summary>
    /// Represents a rollback step for a remediation action.
    /// </summary>
    public class RollbackStep
    {
        /// <summary>
        /// Gets or sets the step identifier.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the step description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the step command.
        /// </summary>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the step parameters.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the step order.
        /// </summary>
        public int Order { get; set; }
    }
} 