using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Remediation;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation;

/// <summary>
/// Represents the context for a remediation action.
/// </summary>
public class RemediationActionContext
{
    /// <summary>
    /// Gets or sets the unique identifier for the context.
    /// </summary>
    public Guid ContextId { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the name of the context.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the description of the context.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets or sets the type of the context.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets or sets the dictionary of context data.
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the list of tags.
    /// </summary>
    public IReadOnlyCollection<Tags> Tags { get; } = new Collection<string>();

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last update timestamp.
    /// </summary>
    public DateTime LastUpdated { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the version of the context.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Gets or sets the source of the context.
    /// </summary>
    public string Source { get; }

    /// <summary>
    /// Gets or sets the priority of the context.
    /// </summary>
    public int Priority { get; }

    /// <summary>
    /// Gets or sets whether the context is active.
    /// </summary>
    public bool IsActive { get; } = true;

    /// <summary>
    /// Gets or sets whether the context is required.
    /// </summary>
    public bool IsRequired { get; }

    /// <summary>
    /// Gets or sets whether the context is optional.
    /// </summary>
    public bool IsOptional { get; }

    /// <summary>
    /// Gets or sets whether the context is conditional.
    /// </summary>
    public bool IsConditional { get; }

    /// <summary>
    /// Gets or sets the condition for the context.
    /// </summary>
    public string Condition { get; }

    /// <summary>
    /// Gets or sets the list of dependencies.
    /// </summary>
    public IReadOnlyCollection<Dependencies> Dependencies { get; } = new Collection<string>();

    /// <summary>
    /// Gets or sets the list of prerequisites.
    /// </summary>
    public IReadOnlyCollection<Prerequisites> Prerequisites { get; } = new Collection<string>();

    /// <summary>
    /// Gets or sets the severity of the context.
    /// </summary>
    public RemediationActionSeverity Severity { get; } = RemediationActionSeverity.Medium;

    /// <summary>
    /// Gets or sets the risk level of the context.
    /// </summary>
    public RemediationRiskLevel RiskLevel { get; }

    /// <summary>
    /// Gets or sets the impact scope of the context.
    /// </summary>
    public RemediationActionImpactScope ImpactScope { get; }

    /// <summary>
    /// Gets or sets the list of warnings.
    /// </summary>
    public IReadOnlyCollection<Warnings> Warnings { get; } = new Collection<string>();

    /// <summary>
    /// Gets or sets the strategy name.
    /// </summary>
    public string StrategyName { get; }

    /// <summary>
    /// Gets or sets the order of the context.
    /// </summary>
    public int Order { get; }

    /// <summary>
    /// Gets or sets whether the context is enabled.
    /// </summary>
    public bool IsEnabled { get; } = true;

    /// <summary>
    /// Gets or sets whether the context is parallel.
    /// </summary>
    public bool IsParallel { get; }

    /// <summary>
    /// Gets or sets whether the context is sequential.
    /// </summary>
    public bool IsSequential { get; }

    /// <summary>
    /// Gets or sets whether the context is asynchronous.
    /// </summary>
    public bool IsAsynchronous { get; }

    /// <summary>
    /// Gets or sets whether the context is synchronous.
    /// </summary>
    public bool IsSynchronous { get; }

    /// <summary>
    /// Gets or sets whether the context is idempotent.
    /// </summary>
    public bool IsIdempotent { get; }

    /// <summary>
    /// Gets or sets whether the context is reversible.
    /// </summary>
    public bool IsReversible { get; }

    /// <summary>
    /// Gets or sets whether the context is atomic.
    /// </summary>
    public bool IsAtomic { get; }

    /// <summary>
    /// Gets or sets whether the context is transactional.
    /// </summary>
    public bool IsTransactional { get; }
} 







