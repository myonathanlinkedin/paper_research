namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the risk level associated with a remediation action.
/// </summary>
public enum RemediationRiskLevel
{
    /// <summary>
    /// No risk associated with the remediation.
    /// </summary>
    None = 0,

    /// <summary>
    /// Low risk - minimal impact if remediation fails.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Medium risk - moderate impact if remediation fails.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// High risk - significant impact if remediation fails.
    /// </summary>
    High = 3,

    /// <summary>
    /// Critical risk - severe impact if remediation fails.
    /// </summary>
    Critical = 4,

    /// <summary>
    /// Extreme risk - catastrophic impact if remediation fails.
    /// </summary>
    Extreme = 5
} 