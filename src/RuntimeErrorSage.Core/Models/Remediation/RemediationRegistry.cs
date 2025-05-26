using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents a registry of remediation strategies.
/// </summary>
public class RemediationRegistry
{
    /// <summary>
    /// Gets or sets the unique identifier of the registry.
    /// </summary>
    public string RegistryId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the registry.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the registry.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the registered strategies.
    /// </summary>
    public List<RemediationStrategy> Strategies { get; set; } = new();

    /// <summary>
    /// Gets or sets the registry metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents a remediation strategy.
/// </summary>
public class RemediationStrategy
{
    /// <summary>
    /// Gets or sets the unique identifier of the strategy.
    /// </summary>
    public string StrategyId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the strategy.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the strategy.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the type of the strategy.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the priority of the strategy.
    /// </summary>
    public RemediationActionPriority Priority { get; set; }

    /// <summary>
    /// Gets or sets the impact scope of the strategy.
    /// </summary>
    public RemediationActionImpactScope ImpactScope { get; set; }

    /// <summary>
    /// Gets or sets the severity of the strategy.
    /// </summary>
    public RemediationActionSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the conditions for applying the strategy.
    /// </summary>
    public List<RemediationCondition> Conditions { get; set; } = new();

    /// <summary>
    /// Gets or sets the actions to execute for the strategy.
    /// </summary>
    public List<RemediationAction> Actions { get; set; } = new();

    /// <summary>
    /// Gets or sets the strategy metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents a condition for applying a remediation strategy.
/// </summary>
public class RemediationCondition
{
    /// <summary>
    /// Gets or sets the unique identifier of the condition.
    /// </summary>
    public string ConditionId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the condition.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the condition.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the type of the condition.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the expression for the condition.
    /// </summary>
    public string Expression { get; set; }

    /// <summary>
    /// Gets or sets the parameters for the condition.
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();

    /// <summary>
    /// Gets or sets the condition metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 