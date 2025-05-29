namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the risk level of a remediation action.
/// </summary>
public enum RemediationRisk
{
    /// <summary>
    /// No risk - remediation has no potential negative impact.
    /// </summary>
    None,

    /// <summary>
    /// Low risk - remediation has minimal potential negative impact.
    /// </summary>
    Low,

    /// <summary>
    /// Medium risk - remediation has moderate potential negative impact.
    /// </summary>
    Medium,

    /// <summary>
    /// High risk - remediation has significant potential negative impact.
    /// </summary>
    High,

    /// <summary>
    /// Critical risk - remediation has major potential negative impact.
    /// </summary>
    Critical,

    /// <summary>
    /// Risk level is unknown.
    /// </summary>
    Unknown
} 
