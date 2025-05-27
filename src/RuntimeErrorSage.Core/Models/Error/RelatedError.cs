using System;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents a related error with relationship information.
/// </summary>
public class RelatedError
{
    /// <summary>
    /// Gets or sets the unique identifier of the related error.
    /// </summary>
    public string ErrorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of relationship.
    /// </summary>
    public RelationshipType RelationshipType { get; set; }

    /// <summary>
    /// Gets or sets the strength of the relationship (0.0 to 1.0).
    /// </summary>
    public double RelationshipStrength { get; set; }

    /// <summary>
    /// Gets or sets the distance in the dependency graph.
    /// </summary>
    public int GraphDistance { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the relationship was identified.
    /// </summary>
    public DateTime IdentifiedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the confidence level of the relationship (0.0 to 1.0).
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the description of the relationship.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the related error instance.
    /// </summary>
    public RuntimeError Error { get; set; }
} 