namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the scope of impact for a remediation action.
/// </summary>
public enum RemediationActionImpactScope
{
    /// <summary>
    /// No impact scope specified.
    /// </summary>
    None = 0,
    /// <summary>
    /// Local impact.
    /// </summary>
    Local = 1,
    /// <summary>
    /// Module-level impact.
    /// </summary>
    Module = 2,
    /// <summary>
    /// Service-level impact.
    /// </summary>
    Service = 3,
    /// <summary>
    /// System-wide impact.
    /// </summary>
    System = 4,
    /// <summary>
    /// Global impact.
    /// </summary>
    Global = 5
} 
