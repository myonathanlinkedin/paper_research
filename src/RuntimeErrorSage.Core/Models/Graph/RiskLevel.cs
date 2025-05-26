namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents the risk level associated with an impact.
/// </summary>
public enum RiskLevel
{
    /// <summary>
    /// No risk or negligible risk.
    /// </summary>
    None = 0,

    /// <summary>
    /// Low risk with minimal potential impact.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Medium risk requiring attention.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// High risk requiring immediate attention.
    /// </summary>
    High = 3,

    /// <summary>
    /// Critical risk requiring immediate action.
    /// </summary>
    Critical = 4,

    /// <summary>
    /// Extreme risk with potential catastrophic consequences.
    /// </summary>
    Extreme = 5,

    /// <summary>
    /// Unknown risk level due to insufficient information.
    /// </summary>
    Unknown = 6
} 