namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents the scope of an impact.
/// </summary>
public enum ImpactScope
{
    /// <summary>
    /// Impact is isolated to a single component.
    /// </summary>
    Isolated = 0,

    /// <summary>
    /// Impact affects directly connected components.
    /// </summary>
    Connected = 1,

    /// <summary>
    /// Impact affects components within the same module.
    /// </summary>
    Module = 2,

    /// <summary>
    /// Impact affects multiple modules.
    /// </summary>
    MultiModule = 3,

    /// <summary>
    /// Impact affects the entire service.
    /// </summary>
    Service = 4,

    /// <summary>
    /// Impact affects multiple services.
    /// </summary>
    MultiService = 5,

    /// <summary>
    /// Impact affects the entire system.
    /// </summary>
    System = 6,

    /// <summary>
    /// Impact affects external dependencies.
    /// </summary>
    External = 7
} 