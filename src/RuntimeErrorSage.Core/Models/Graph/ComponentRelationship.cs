using System;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents a relationship between two components in a system.
/// </summary>
public class ComponentRelationship
{
    /// <summary>
    /// Gets or sets the unique identifier of the relationship.
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
    /// Gets or sets the relationship type.
    /// </summary>
    public RelationshipType RelationshipType { get; set; }

    /// <summary>
    /// Gets or sets the relationship strength (0-1).
    /// </summary>
    public double Strength { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the relationship.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the description of the relationship.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the relationship is bi-directional.
    /// </summary>
    public bool IsBidirectional { get; set; }
} 
