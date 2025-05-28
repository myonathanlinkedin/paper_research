using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents the propagation of an error through components in a system.
/// </summary>
public class ErrorPropagation
{
    /// <summary>
    /// Gets or sets the unique identifier for this propagation.
    /// </summary>
    public string PropagationId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the source error ID.
    /// </summary>
    public string SourceErrorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source component ID.
    /// </summary>
    public string SourceComponentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target component ID.
    /// </summary>
    public string TargetComponentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the propagation path.
    /// </summary>
    public List<string> PropagationPath { get; set; } = new();

    /// <summary>
    /// Gets or sets the relationship type.
    /// </summary>
    public RelationshipType RelationType { get; set; }

    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of propagation.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets whether the propagation is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the propagation metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the affected components.
    /// </summary>
    public List<string> AffectedComponents { get; set; } = new();

    /// <summary>
    /// Gets or sets the propagation impact.
    /// </summary>
    public Dictionary<string, ImpactSeverity> Impact { get; set; } = new();

    /// <summary>
    /// Gets or sets the propagation paths through the system.
    /// </summary>
    public List<List<string>> PropagationPaths { get; set; } = new List<List<string>>();

    /// <summary>
    /// Gets or sets the severity of the error in each component (component id -> severity).
    /// </summary>
    public Dictionary<string, double> ComponentSeverity { get; set; } = new Dictionary<string, double>();

    /// <summary>
    /// Gets or sets the impact score of the error propagation (0-1).
    /// </summary>
    public double ImpactScore { get; set; }

    /// <summary>
    /// Gets or sets the source component where the error originated.
    /// </summary>
    public string SourceComponent { get; set; } = string.Empty;
} 
