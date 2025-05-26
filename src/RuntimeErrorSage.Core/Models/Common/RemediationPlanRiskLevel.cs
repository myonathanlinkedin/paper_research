namespace RuntimeErrorSage.Core.Models.Common;

/// <summary>
/// Defines the risk levels of remediation plans.
/// </summary>
public enum RemediationPlanRiskLevel
{
    /// <summary>
    /// The plan has low risk.
    /// </summary>
    Low,

    /// <summary>
    /// The plan has medium risk.
    /// </summary>
    Medium,

    /// <summary>
    /// The plan has high risk.
    /// </summary>
    High,

    /// <summary>
    /// The plan has critical risk.
    /// </summary>
    Critical,

    /// <summary>
    /// The plan has unknown risk.
    /// </summary>
    Unknown
} 
