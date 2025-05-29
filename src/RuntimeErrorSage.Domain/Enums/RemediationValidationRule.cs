namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the type of remediation validation rule.
/// </summary>
public enum RemediationValidationRule
{
    /// <summary>
    /// No rule.
    /// </summary>
    None,

    /// <summary>
    /// Risk level validation.
    /// </summary>
    RiskLevel,

    /// <summary>
    /// Impact validation.
    /// </summary>
    Impact,

    /// <summary>
    /// Dependency validation.
    /// </summary>
    Dependency,

    /// <summary>
    /// Resource validation.
    /// </summary>
    Resource,

    /// <summary>
    /// Permission validation.
    /// </summary>
    Permission,

    /// <summary>
    /// State validation.
    /// </summary>
    State,

    /// <summary>
    /// Configuration validation.
    /// </summary>
    Configuration,

    /// <summary>
    /// Environment validation.
    /// </summary>
    Environment,

    /// <summary>
    /// Timing validation.
    /// </summary>
    Timing
} 
