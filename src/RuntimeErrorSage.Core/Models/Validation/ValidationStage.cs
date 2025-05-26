namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines the stages of validation.
/// </summary>
public enum ValidationStage
{
    /// <summary>
    /// Initial validation stage.
    /// </summary>
    Initial,

    /// <summary>
    /// Intermediate validation stage.
    /// </summary>
    Intermediate,

    /// <summary>
    /// Final validation stage.
    /// </summary>
    Final,

    /// <summary>
    /// Review validation stage.
    /// </summary>
    Review,

    /// <summary>
    /// Unknown validation stage.
    /// </summary>
    Unknown
} 