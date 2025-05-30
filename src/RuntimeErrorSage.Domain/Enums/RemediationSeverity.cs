namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the severity level of a remediation action.
/// </summary>
public enum RemediationSeverity
{
    /// <summary>
    /// No severity level specified.
    /// </summary>
    None,

    /// <summary>
    /// Low severity - minimal impact on system.
    /// </summary>
    Low,

    /// <summary>
    /// Medium severity - moderate impact on system.
    /// </summary>
    Medium,

    /// <summary>
    /// High severity - significant impact on system.
    /// </summary>
    High,

    /// <summary>
    /// Critical severity - severe impact on system.
    /// </summary>
    Critical
} 
