namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Defines the effort levels of remediation plans.
/// </summary>
public enum RemediationPlanEffort
{
    /// <summary>
    /// The plan requires low effort.
    /// </summary>
    Low,

    /// <summary>
    /// The plan requires medium effort.
    /// </summary>
    Medium,

    /// <summary>
    /// The plan requires high effort.
    /// </summary>
    High,

    /// <summary>
    /// The plan requires critical effort.
    /// </summary>
    Critical,

    /// <summary>
    /// The plan has unknown effort.
    /// </summary>
    Unknown
} 

