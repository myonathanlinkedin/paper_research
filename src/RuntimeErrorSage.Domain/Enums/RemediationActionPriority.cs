using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Remediation;
namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the priority levels for remediation actions.
/// </summary>
public enum RemediationActionPriority
{
    /// <summary>
    /// Critical priority.
    /// </summary>
    Critical = 1,

    /// <summary>
    /// High priority.
    /// </summary>
    High = 2,

    /// <summary>
    /// Medium priority.
    /// </summary>
    Medium = 3,

    /// <summary>
    /// Low priority.
    /// </summary>
    Low = 4,

    /// <summary>
    /// No priority.
    /// </summary>
    None = 5
} 



