using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Remediation.Models.Common;

/// <summary>
/// Represents a remediation action.
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
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the action.
    /// </summary>
    public string Description { get; set; } = string.Empty;

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
    /// Gets or sets the severity of the action.
    /// </summary>
    public RemediationActionSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp of the action.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last modification timestamp of the action.
    /// </summary>
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the scheduled execution timestamp of the action.
    /// </summary>
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// Gets or sets the actual execution timestamp of the action.
    /// </summary>
    public DateTime? ExecutedAt { get; set; }

    /// <summary>
    /// Gets or sets the completion timestamp of the action.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets the parameters of the action.
    /// </summary>
    public Dictionary<string, string> Parameters { get; set; } = new();

    /// <summary>
    /// Gets or sets the metadata of the action.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the version of the action.
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// Gets or sets the author of the action.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the dependencies of the action.
    /// </summary>
    public Dictionary<string, string> Dependencies { get; set; } = new();

    /// <summary>
    /// Gets or sets the requirements of the action.
    /// </summary>
    public Dictionary<string, string> Requirements { get; set; } = new();

    /// <summary>
    /// Gets or sets whether the action is deprecated.
    /// </summary>
    public bool IsDeprecated { get; set; }

    /// <summary>
    /// Gets or sets the reason for deprecation.
    /// </summary>
    public string? DeprecationReason { get; set; }

    /// <summary>
    /// Gets or sets the replacement action.
    /// </summary>
    public string? ReplacementAction { get; set; }
} 