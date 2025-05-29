using System.Collections.ObjectModel;
using System;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Graph;

/// <summary>
/// Represents a relationship between two components in a system.
/// </summary>
public class ComponentRelationship
{
    /// <summary>
    /// Gets or sets the unique identifier of the relationship.
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the source component.
    /// </summary>
    public string SourceComponent { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the target component.
    /// </summary>
    public string TargetComponent { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the relationship type.
    /// </summary>
    public RelationshipType RelationshipType { get; }

    /// <summary>
    /// Gets or sets the relationship strength (0-1).
    /// </summary>
    public double Strength { get; }

    /// <summary>
    /// Gets or sets the timestamp of the relationship.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the description of the relationship.
    /// </summary>
    public string Description { get; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the relationship is bi-directional.
    /// </summary>
    public bool IsBidirectional { get; }
} 






