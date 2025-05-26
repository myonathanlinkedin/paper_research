namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines stages for validation operations.
/// </summary>
public enum ValidationStage
{
    /// <summary>
    /// Pre-processing validation stage.
    /// </summary>
    PreProcessing = 0,

    /// <summary>
    /// Input validation stage.
    /// </summary>
    Input = 1,

    /// <summary>
    /// Processing validation stage.
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Output validation stage.
    /// </summary>
    Output = 3,

    /// <summary>
    /// Post-processing validation stage.
    /// </summary>
    PostProcessing = 4,

    /// <summary>
    /// Cleanup validation stage.
    /// </summary>
    Cleanup = 5,

    /// <summary>
    /// Final validation stage.
    /// </summary>
    Final = 6
} 
