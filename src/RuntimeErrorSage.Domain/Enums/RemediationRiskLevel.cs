namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the risk level for a remediation action.
/// </summary>
public enum RemediationRiskLevel
{
    /// <summary>
    /// No risk.
    /// </summary>
    None = 0,

    /// <summary>
    /// Low risk level.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Medium risk level.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// High risk level.
    /// </summary>
    High = 3,

    /// <summary>
    /// Critical risk level.
    /// </summary>
    Critical = 4,
    
    /// <summary>
    /// Unknown risk level.
    /// </summary>
    Unknown = 5
} 
