using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents error propagation information.
/// </summary>
public class ErrorPropagation
{
    /// <summary>
    /// Gets or sets the unique identifier of the propagation.
    /// </summary>
    public string PropagationId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the source error identifier.
    /// </summary>
    public string SourceErrorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the propagation path.
    /// </summary>
    public List<string> PropagationPath { get; set; } = new();

    /// <summary>
    /// Gets or sets the affected components.
    /// </summary>
    public List<string> AffectedComponents { get; set; } = new();

    /// <summary>
    /// Gets or sets the propagation depth.
    /// </summary>
    public int PropagationDepth { get; set; }

    /// <summary>
    /// Gets or sets the propagation direction.
    /// </summary>
    public PropagationDirection Direction { get; set; }

    /// <summary>
    /// Gets or sets the propagation type.
    /// </summary>
    public RelationshipType PropagationType { get; set; }

    /// <summary>
    /// Gets or sets the propagation severity.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the confidence score.
    /// </summary>
    public double ConfidenceScore { get; set; }

    /// <summary>
    /// Gets or sets the impact weight.
    /// </summary>
    public double ImpactWeight { get; set; }

    /// <summary>
    /// Gets or sets when the propagation was detected.
    /// </summary>
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets when the propagation was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets whether the propagation is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets any additional properties.
    /// </summary>
    public Dictionary<string, object> Properties { get; set; } = new();
}

/// <summary>
/// Defines the direction of error propagation.
/// </summary>
public enum PropagationDirection
{
    /// <summary>
    /// Propagation flows upstream.
    /// </summary>
    Upstream,

    /// <summary>
    /// Propagation flows downstream.
    /// </summary>
    Downstream,

    /// <summary>
    /// Propagation flows bidirectionally.
    /// </summary>
    Bidirectional,

    /// <summary>
    /// Propagation flows laterally.
    /// </summary>
    Lateral,

    /// <summary>
    /// Propagation direction is unknown.
    /// </summary>
    Unknown
} 