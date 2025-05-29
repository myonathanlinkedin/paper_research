using RuntimeErrorSage.Model.Models.Remediation.Interfaces;
using RuntimeErrorSage.Model.Models.Remediation;
namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the severity of a remediation action.
/// </summary>
public enum RemediationActionSeverity
{
    /// <summary>
    /// No severity level assigned.
    /// </summary>
    None,

    /// <summary>
    /// Low severity level.
    /// </summary>
    Low,

    /// <summary>
    /// Medium severity level.
    /// </summary>
    Medium,

    /// <summary>
    /// High severity level.
    /// </summary>
    High,

    /// <summary>
    /// Critical severity level.
    /// </summary>
    Critical
} 


