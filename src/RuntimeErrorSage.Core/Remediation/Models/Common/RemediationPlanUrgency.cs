namespace RuntimeErrorSage.Core.Remediation.Models.Common;

/// <summary>
/// Defines the urgency levels of remediation plans.
/// </summary>
public enum RemediationPlanUrgency
{
    /// <summary>
    /// The plan has low urgency.
    /// </summary>
    Low,

    /// <summary>
    /// The plan has medium urgency.
    /// </summary>
    Medium,

    /// <summary>
    /// The plan has high urgency.
    /// </summary>
    High,

    /// <summary>
    /// The plan has critical urgency.
    /// </summary>
    Critical,

    /// <summary>
    /// The plan has unknown urgency.
    /// </summary>
    Unknown
} 