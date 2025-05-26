namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines the phases of validation.
/// </summary>
public enum ValidationPhase
{
    /// <summary>
    /// Pre-execution validation phase.
    /// </summary>
    PreExecution,

    /// <summary>
    /// During-execution validation phase.
    /// </summary>
    DuringExecution,

    /// <summary>
    /// Post-execution validation phase.
    /// </summary>
    PostExecution,

    /// <summary>
    /// Continuous validation phase.
    /// </summary>
    Continuous,

    /// <summary>
    /// Unknown validation phase.
    /// </summary>
    Unknown
} 
