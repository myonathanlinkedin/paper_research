using System;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents a relationship between components in the system.
/// </summary>
public class ComponentRelationship
{
    /// <summary>
    /// Gets or sets the unique identifier for the relationship.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the source component.
    /// </summary>
    public string SourceComponent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target component.
    /// </summary>
    public string TargetComponent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of relationship.
    /// </summary>
    public RelationshipType RelationshipType { get; set; }

    /// <summary>
    /// Gets or sets the strength of the relationship (0-1).
    /// </summary>
    public double Strength { get; set; }

    /// <summary>
    /// Gets or sets the metadata associated with the relationship.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp when the relationship was created.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets whether the relationship is bidirectional.
    /// </summary>
    public bool IsBidirectional { get; set; }

    /// <summary>
    /// Gets or sets the description of the relationship.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the weight of the relationship.
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Gets or sets the confidence level of the relationship (0-1).
    /// </summary>
    public double Confidence { get; set; }
} 