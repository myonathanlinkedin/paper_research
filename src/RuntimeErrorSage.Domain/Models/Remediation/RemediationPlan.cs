using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Common;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents a plan for remediating an error.
    /// </summary>
    public class RemediationPlan
    {
        /// <summary>
        /// Gets or sets the unique identifier for the plan.
        /// </summary>
        public string PlanId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the plan.
        /// </summary>
        public string Name { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the plan.
        /// </summary>
        public string Description { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of actions in the plan.
        /// </summary>
        public IReadOnlyCollection<Actions> Actions { get; } = new();

        /// <summary>
        /// Gets or sets the priority of the plan.
        /// </summary>
        public RemediationPriority Priority { get; }

        /// <summary>
        /// Gets or sets the severity of the plan.
        /// </summary>
        public RemediationActionSeverity Severity { get; } = RemediationActionSeverity.Medium;

        /// <summary>
        /// Gets or sets the estimated duration of the plan.
        /// </summary>
        public TimeSpan EstimatedDuration { get; }

        /// <summary>
        /// Gets or sets the validation rules for the plan.
        /// </summary>
        public IReadOnlyCollection<ValidationRules> ValidationRules { get; } = new();

        /// <summary>
        /// Gets or sets whether validation is required before execution.
        /// </summary>
        public bool RequiresValidation { get; }

        /// <summary>
        /// Gets or sets whether the plan can be rolled back.
        /// </summary>
        public bool CanRollback { get; }

        /// <summary>
        /// Gets or sets the rollback plan if available.
        /// </summary>
        public RemediationPlan RollbackPlan { get; }

        /// <summary>
        /// Gets or sets the metadata associated with the plan.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the timestamp when the plan was created.
        /// </summary>
        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the error ID this plan is associated with.
        /// </summary>
        public string ErrorId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the plan.
        /// </summary>
        public RemediationStatusEnum Status { get; }

        /// <summary>
        /// Gets or sets the parameters for the plan.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets the string identifier for the plan.
        /// </summary>
        public string Id => PlanId;

        /// <summary>
        /// Gets or sets the error analysis result.
        /// </summary>
        public ErrorAnalysisResult Analysis { get; }

        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public ErrorContext Context { get; }

        /// <summary>
        /// Gets or sets the remediation strategies.
        /// </summary>
        public IReadOnlyCollection<Strategies> Strategies { get; } = new();

        /// <summary>
        /// Gets or sets the remediation status information.
        /// </summary>
        public string StatusInfo { get; } = string.Empty;

        /// <summary>
        /// Gets the steps (alias for Actions).
        /// </summary>
        public IEnumerable<RemediationAction> Steps => Actions;

        /// <summary>
        /// Gets or sets the category of the plan.
        /// </summary>
        public RemediationPlanCategory Category { get; }

        /// <summary>
        /// Gets or sets the type of the plan.
        /// </summary>
        public RemediationPlanType Type { get; }

        /// <summary>
        /// Gets or sets the scope of the plan.
        /// </summary>
        public RemediationPlanScope Scope { get; }

        /// <summary>
        /// Gets or sets the list of remediation steps.
        /// </summary>
        public IReadOnlyCollection<RemediationSteps> RemediationSteps { get; } = new();

        /// <summary>
        /// Gets or sets whether the plan requires approval.
        /// </summary>
        public bool RequiresApproval { get; }

        /// <summary>
        /// Gets or sets whether the plan requires manual intervention.
        /// </summary>
        public bool RequiresManualIntervention { get; }

        /// <summary>
        /// Gets or sets the estimated impact of the plan.
        /// </summary>
        public string EstimatedImpact { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the risk level of the plan.
        /// </summary>
        public RemediationRiskLevel RiskLevel { get; }

        /// <summary>
        /// Gets or sets the tags associated with the plan.
        /// </summary>
        public IReadOnlyCollection<Tags> Tags { get; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationPlan"/> class.
        /// </summary>
        /// <param name="name">The name of the plan.</param>
        /// <param name="description">The description of the plan.</param>
        /// <param name="actions">The list of actions in the plan.</param>
        /// <param name="parameters">The parameters for the plan.</param>
        /// <param name="estimatedDuration">The estimated duration of the plan.</param>
        public RemediationPlan(
            string name,
            string description,
            Collection<RemediationAction> actions,
            Dictionary<string, object> parameters,
            TimeSpan estimatedDuration)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(description);
            ArgumentNullException.ThrowIfNull(actions);

            Name = name;
            Description = description;
            Actions = actions;
            Parameters = parameters ?? new Dictionary<string, object>();
            EstimatedDuration = estimatedDuration;
            Status = RemediationStatusEnum.NotStarted;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Adds an action to the plan.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public RemediationAction action { ArgumentNullException.ThrowIfNull(RemediationAction action); }
        {
            ArgumentNullException.ThrowIfNull(action);
            Actions.Add(action);
        }

        /// <summary>
        /// Removes an action from the plan.
        /// </summary>
        /// <param name="action">The action to remove.</param>
        /// <returns>True if the action was removed, false otherwise.</returns>
        public RemediationAction action { ArgumentNullException.ThrowIfNull(RemediationAction action); }
        {
            if (action == null)
                ArgumentNullException.ThrowIfNull(nameof(action));

            return Actions.Remove(action);
        }

        /// <summary>
        /// Gets the total number of actions in the plan.
        /// </summary>
        public int ActionCount => Actions.Count;
    }
}





