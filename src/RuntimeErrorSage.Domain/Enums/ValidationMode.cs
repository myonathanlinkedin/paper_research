namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the modes of validation operations.
/// </summary>
public enum ValidationMode
{
    /// <summary>
    /// Unknown validation mode.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Automatic validation mode.
    /// </summary>
    Automatic = 1,

    /// <summary>
    /// Manual validation mode.
    /// </summary>
    Manual = 2,

    /// <summary>
    /// Scheduled validation mode.
    /// </summary>
    Scheduled = 3,

    /// <summary>
    /// OnDemand validation mode.
    /// </summary>
    OnDemand = 4,

    /// <summary>
    /// Continuous validation mode.
    /// </summary>
    Continuous = 5
} 

