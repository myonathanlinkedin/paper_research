using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Remediation.Models.Common;

/// <summary>
/// Represents a remediation plan.
/// </summary>
public class RemediationPlan
{
    /// <summary>
    /// Gets or sets the unique identifier of the plan.
    /// </summary>
    public string PlanId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the plan.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the plan.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the plan.
    /// </summary>
    public RemediationPlanStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the priority of the plan.
    /// </summary>
    public RemediationPlanPriority Priority { get; set; }

    /// <summary>
    /// Gets or sets the type of the plan.
    /// </summary>
    public RemediationPlanType Type { get; set; }

    /// <summary>
    /// Gets or sets the category of the plan.
    /// </summary>
    public RemediationPlanCategory Category { get; set; }

    /// <summary>
    /// Gets or sets the scope of the plan.
    /// </summary>
    public RemediationPlanScope Scope { get; set; }

    /// <summary>
    /// Gets or sets the impact of the plan.
    /// </summary>
    public RemediationPlanImpact Impact { get; set; }

    /// <summary>
    /// Gets or sets the complexity of the plan.
    /// </summary>
    public RemediationPlanComplexity Complexity { get; set; }

    /// <summary>
    /// Gets or sets the risk level of the plan.
    /// </summary>
    public RemediationPlanRiskLevel RiskLevel { get; set; }

    /// <summary>
    /// Gets or sets the urgency of the plan.
    /// </summary>
    public RemediationPlanUrgency Urgency { get; set; }

    /// <summary>
    /// Gets or sets the effort required for the plan.
    /// </summary>
    public RemediationPlanEffort Effort { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp of the plan.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last modification timestamp of the plan.
    /// </summary>
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the scheduled execution timestamp of the plan.
    /// </summary>
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// Gets or sets the actual execution timestamp of the plan.
    /// </summary>
    public DateTime? ExecutedAt { get; set; }

    /// <summary>
    /// Gets or sets the completion timestamp of the plan.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets the list of actions in the plan.
    /// </summary>
    public List<RemediationAction> Actions { get; set; } = new();

    /// <summary>
    /// Gets or sets the metadata of the plan.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the version of the plan.
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// Gets or sets the author of the plan.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the dependencies of the plan.
    /// </summary>
    public Dictionary<string, string> Dependencies { get; set; } = new();

    /// <summary>
    /// Gets or sets the requirements of the plan.
    /// </summary>
    public Dictionary<string, string> Requirements { get; set; } = new();

    /// <summary>
    /// Gets or sets whether the plan is deprecated.
    /// </summary>
    public bool IsDeprecated { get; set; }

    /// <summary>
    /// Gets or sets the reason for deprecation.
    /// </summary>
    public string? DeprecationReason { get; set; }

    /// <summary>
    /// Gets or sets the replacement strategy.
    /// </summary>
    public string? ReplacementStrategy { get; set; }
} 