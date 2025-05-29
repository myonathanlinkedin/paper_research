namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the impact levels of remediation plans.
/// </summary>
public enum RemediationPlanImpact
{
    /// <summary>
    /// The plan has no impact.
    /// </summary>
    None,

    /// <summary>
    /// The plan has low impact.
    /// </summary>
    Low,

    /// <summary>
    /// The plan has medium impact.
    /// </summary>
    Medium,

    /// <summary>
    /// The plan has high impact.
    /// </summary>
    High,

    /// <summary>
    /// The plan has critical impact.
    /// </summary>
    Critical,

    /// <summary>
    /// The plan has unknown impact.
    /// </summary>
    Unknown
} 

