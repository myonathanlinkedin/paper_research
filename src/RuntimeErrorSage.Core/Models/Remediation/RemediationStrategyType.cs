namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents the type of remediation strategy.
/// </summary>
public enum RemediationStrategyType
{
    /// <summary>
    /// Automatic remediation without human intervention.
    /// </summary>
    Automatic = 0,

    /// <summary>
    /// Semi-automatic remediation requiring minimal human intervention.
    /// </summary>
    SemiAutomatic = 1,

    /// <summary>
    /// Manual remediation requiring human intervention.
    /// </summary>
    Manual = 2,

    /// <summary>
    /// Preventive remediation to avoid future errors.
    /// </summary>
    Preventive = 3,

    /// <summary>
    /// Reactive remediation to address existing errors.
    /// </summary>
    Reactive = 4,

    /// <summary>
    /// Adaptive remediation that learns from past experiences.
    /// </summary>
    Adaptive = 5,

    /// <summary>
    /// Temporary remediation as a workaround.
    /// </summary>
    Temporary = 6,

    /// <summary>
    /// Permanent remediation to fix the root cause.
    /// </summary>
    Permanent = 7
} 