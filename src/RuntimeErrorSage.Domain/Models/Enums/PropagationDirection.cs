namespace RuntimeErrorSage.Application.Models.Enums;

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
