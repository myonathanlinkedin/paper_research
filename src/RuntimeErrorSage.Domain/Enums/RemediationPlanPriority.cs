using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the priority levels for remediation plans.
/// </summary>
public enum RemediationPlanPriority
{
    /// <summary>
    /// The plan has low priority.
    /// </summary>
    Low,

    /// <summary>
    /// The plan has medium priority.
    /// </summary>
    Medium,

    /// <summary>
    /// The plan has high priority.
    /// </summary>
    High,

    /// <summary>
    /// The plan has critical priority.
    /// </summary>
    Critical,

    /// <summary>
    /// The plan has unknown priority.
    /// </summary>
    Unknown
} 







