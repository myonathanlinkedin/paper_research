using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents a remediation action that can be taken to address an error.
    /// </summary>
    public class RemediationAction
    {
        /// <summary>
        /// Gets or sets the unique identifier of the action.
        /// </summary>
        public string ActionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the action.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        public RemediationActionType Type { get; set; }

        /// <summary>
        /// Gets or sets the priority of the action.
        /// </summary>
        public RemediationActionPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the impact scope of the action.
        /// </summary>
        public RemediationActionImpactScope ImpactScope { get; set; }

        /// <summary>
        /// Gets or sets the severity level of the action.
        /// </summary>
        public RemediationActionSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the parameters required for the action.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation rules for the action.
        /// </summary>
        public List<RemediationActionValidationRule> ValidationRules { get; set; } = new();

        /// <summary>
        /// Gets or sets the rollback action if this action fails.
        /// </summary>
        public RemediationAction RollbackAction { get; set; }

        /// <summary>
        /// Gets or sets the timeout for the action in seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// Gets or sets the maximum number of retries for the action.
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// Gets or sets the delay between retries in seconds.
        /// </summary>
        public int RetryDelaySeconds { get; set; } = 5;

        /// <summary>
        /// Gets or sets whether the action requires confirmation.
        /// </summary>
        public bool RequiresConfirmation { get; set; }

        /// <summary>
        /// Gets or sets the confirmation message.
        /// </summary>
        public string ConfirmationMessage { get; set; }

        /// <summary>
        /// Gets or sets additional metadata for the action.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Defines the types of remediation actions.
    /// </summary>
    public enum RemediationActionType
    {
        /// <summary>
        /// A code fix action.
        /// </summary>
        CodeFix,

        /// <summary>
        /// A configuration update action.
        /// </summary>
        ConfigUpdate,

        /// <summary>
        /// A resource allocation action.
        /// </summary>
        ResourceAllocation,

        /// <summary>
        /// A service restart action.
        /// </summary>
        ServiceRestart,

        /// <summary>
        /// A dependency update action.
        /// </summary>
        DependencyUpdate,

        /// <summary>
        /// A custom action.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Defines the priority levels for remediation actions.
    /// </summary>
    public enum RemediationActionPriority
    {
        /// <summary>
        /// Critical priority.
        /// </summary>
        Critical = 1,

        /// <summary>
        /// High priority.
        /// </summary>
        High = 2,

        /// <summary>
        /// Medium priority.
        /// </summary>
        Medium = 3,

        /// <summary>
        /// Low priority.
        /// </summary>
        Low = 4,

        /// <summary>
        /// No priority.
        /// </summary>
        None = 5
    }

    /// <summary>
    /// Defines the impact scope of remediation actions.
    /// </summary>
    public enum RemediationActionImpactScope
    {
        /// <summary>
        /// Global impact scope.
        /// </summary>
        Global,

        /// <summary>
        /// Service-level impact scope.
        /// </summary>
        Service,

        /// <summary>
        /// Component-level impact scope.
        /// </summary>
        Component,

        /// <summary>
        /// Operation-level impact scope.
        /// </summary>
        Operation,

        /// <summary>
        /// Instance-level impact scope.
        /// </summary>
        Instance
    }

    /// <summary>
    /// Defines the severity levels for remediation actions.
    /// </summary>
    public enum RemediationActionSeverity
    {
        /// <summary>
        /// Critical severity.
        /// </summary>
        Critical,

        /// <summary>
        /// High severity.
        /// </summary>
        High,

        /// <summary>
        /// Medium severity.
        /// </summary>
        Medium,

        /// <summary>
        /// Low severity.
        /// </summary>
        Low,

        /// <summary>
        /// No severity.
        /// </summary>
        None
    }

    /// <summary>
    /// Represents a validation rule for a remediation action.
    /// </summary>
    public class RemediationActionValidationRule
    {
        /// <summary>
        /// Gets or sets the unique identifier of the rule.
        /// </summary>
        public string RuleId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the rule.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the validation condition.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// Gets or sets the error message if validation fails.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets whether the rule is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the priority of the rule.
        /// </summary>
        public RemediationActionPriority Priority { get; set; }
    }
} 