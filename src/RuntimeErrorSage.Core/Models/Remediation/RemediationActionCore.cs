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
        /// Gets or sets the action to execute.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the context of the action.
        /// </summary>
        public Dictionary<string, object> Context { get; set; } = new();

        /// <summary>
        /// Gets or sets the priority of the action.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the impact of the action.
        /// </summary>
        public RemediationActionSeverity Impact { get; set; }

        /// <summary>
        /// Gets or sets the risk level of the action.
        /// </summary>
        public RemediationRiskLevel RiskLevel { get; set; }

        /// <summary>
        /// Gets or sets the status of the action.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the parameters of the action.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the error type of the action.
        /// </summary>
        public string ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the error message of the action.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the stack trace of the action.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the potential issues associated with the action.
        /// </summary>
        public List<string> PotentialIssues { get; set; } = new();

        /// <summary>
        /// Gets or sets the mitigation steps for the action.
        /// </summary>
        public List<string> MitigationSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the timestamp when the action was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the action was executed.
        /// </summary>
        public DateTime? ExecutedAt { get; set; }

        /// <summary>
        /// Gets or sets whether the action is executed.
        /// </summary>
        public bool IsExecuted { get; set; }

        /// <summary>
        /// Gets or sets the execution result of the action.
        /// </summary>
        public string ExecutionResult { get; set; }

        /// <summary>
        /// Gets or sets the state of the action.
        /// </summary>
        public RemediationState State { get; set; } = new();

        /// <summary>
        /// Gets or sets the metadata of the action.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the action requires manual approval.
        /// </summary>
        public bool RequiresManualApproval { get; set; }

        /// <summary>
        /// Gets or sets whether the action can be rolled back.
        /// </summary>
        public bool CanRollback => RollbackAction != null;

        /// <summary>
        /// Gets or sets the rollback action.
        /// </summary>
        public RemediationAction? RollbackAction { get; set; }

        /// <summary>
        /// Gets or sets the dependencies of the action.
        /// </summary>
        public List<string> Dependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the prerequisites of the action.
        /// </summary>
        public List<string> Prerequisites { get; set; } = new();

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the impact scope of this action.
        /// </summary>
        public RemediationActionImpactScope ImpactScope { get; set; }

        /// <summary>
        /// Gets or sets the confirmation message for manual approval.
        /// </summary>
        public string ConfirmationMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timeout for this action in seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 300;

        /// <summary>
        /// Gets or sets the maximum number of retries for this action.
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// Gets or sets the delay between retries in seconds.
        /// </summary>
        public int RetryDelaySeconds { get; set; } = 30;

        /// <summary>
        /// Gets or sets any warnings associated with this action.
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Gets or sets the duration of the action execution in milliseconds.
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// Gets or sets the duration of the action in milliseconds.
        /// </summary>
        public double DurationMs => (ExecutedAt.HasValue && ExecutedAt.Value > CreatedAt) ? (ExecutedAt.Value - CreatedAt).TotalMilliseconds : 0;

        /// <summary>
        /// Gets or sets any error that occurred during action execution.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the result of the action execution.
        /// </summary>
        public Dictionary<string, object> Result { get; set; } = new();

        /// <summary>
        /// Gets or sets the name of the strategy that created this action.
        /// </summary>
        public string StrategyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the action was started.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the action was completed.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets whether the action was successful.
        /// </summary>
        public bool IsSuccessful => Status == RemediationStatusEnum.Success && string.IsNullOrEmpty(Error);

        /// <summary>
        /// Gets or sets the severity of the action.
        /// </summary>
        public RemediationActionSeverity Severity { get; set; } = RemediationActionSeverity.Medium;

        /// <summary>
        /// Gets or sets the risk level of the action.
        /// </summary>
        public RemediationRiskLevel Risk { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the action was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets whether the action is completed.
        /// </summary>
        public bool IsCompleted => Status == RemediationStatusEnum.Success || Status == RemediationStatusEnum.Failed;

        /// <summary>
        /// Updates the state of the action.
        /// </summary>
        /// <param name="newState">The new state.</param>
        /// <param name="errorMessage">Optional error message.</param>
        public void UpdateState(RemediationState newState, string errorMessage = null)
        {
            State = newState;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Error = errorMessage;
            }
            LastUpdated = DateTime.UtcNow;
        }
    }
} 