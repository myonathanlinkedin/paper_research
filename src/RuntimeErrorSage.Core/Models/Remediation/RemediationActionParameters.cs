using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents the parameters for a remediation action.
/// </summary>
public class RemediationActionParameters
{
    /// <summary>
    /// Gets or sets the dictionary of parameters.
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the timeout in seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of retries.
    /// </summary>
    public int MaxRetries { get; set; }

    /// <summary>
    /// Gets or sets the delay in seconds between retries.
    /// </summary>
    public int RetryDelaySeconds { get; set; }

    /// <summary>
    /// Gets or sets whether manual approval is required.
    /// </summary>
    public bool RequiresManualApproval { get; set; }

    /// <summary>
    /// Gets or sets the confirmation message for manual approval.
    /// </summary>
    public string ConfirmationMessage { get; set; }

    /// <summary>
    /// Gets or sets the list of dependencies.
    /// </summary>
    public List<string> Dependencies { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the list of prerequisites.
    /// </summary>
    public List<string> Prerequisites { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the severity of the action.
    /// </summary>
    public RemediationActionSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the risk level of the action.
    /// </summary>
    public RemediationRiskLevel RiskLevel { get; set; }

    /// <summary>
    /// Gets or sets the impact scope of the action.
    /// </summary>
    public RemediationActionImpactScope ImpactScope { get; set; }

    /// <summary>
    /// Gets or sets the list of warnings.
    /// </summary>
    public List<string> Warnings { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the context of the action.
    /// </summary>
    public string Context { get; set; }

    /// <summary>
    /// Gets or sets the strategy name.
    /// </summary>
    public string StrategyName { get; set; }

    /// <summary>
    /// Gets or sets the order of the action.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets whether the action is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the action is required.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets whether the action is optional.
    /// </summary>
    public bool IsOptional { get; set; }

    /// <summary>
    /// Gets or sets whether the action is conditional.
    /// </summary>
    public bool IsConditional { get; set; }

    /// <summary>
    /// Gets or sets the condition for the action.
    /// </summary>
    public string Condition { get; set; }

    /// <summary>
    /// Gets or sets whether the action is parallel.
    /// </summary>
    public bool IsParallel { get; set; }

    /// <summary>
    /// Gets or sets whether the action is sequential.
    /// </summary>
    public bool IsSequential { get; set; }

    /// <summary>
    /// Gets or sets whether the action is asynchronous.
    /// </summary>
    public bool IsAsynchronous { get; set; }

    /// <summary>
    /// Gets or sets whether the action is synchronous.
    /// </summary>
    public bool IsSynchronous { get; set; }

    /// <summary>
    /// Gets or sets whether the action is idempotent.
    /// </summary>
    public bool IsIdempotent { get; set; }

    /// <summary>
    /// Gets or sets whether the action is reversible.
    /// </summary>
    public bool IsReversible { get; set; }

    /// <summary>
    /// Gets or sets whether the action is atomic.
    /// </summary>
    public bool IsAtomic { get; set; }

    /// <summary>
    /// Gets or sets whether the action is transactional.
    /// </summary>
    public bool IsTransactional { get; set; }
} 