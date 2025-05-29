using RuntimeErrorSage.Model.Models.Remediation.Interfaces;
using RuntimeErrorSage.Model.Models.Remediation;
namespace RuntimeErrorSage.Model.Models.Enums;

/// <summary>
/// Represents the scope of impact for a remediation action.
/// </summary>
public enum RemediationActionImpactScope
{
    /// <summary>
    /// No impact scope.
    /// </summary>
    None,

    /// <summary>
    /// Component-level impact.
    /// </summary>
    Component,

    /// <summary>
    /// Module-level impact.
    /// </summary>
    Module,

    /// <summary>
    /// Service-level impact.
    /// </summary>
    Service,

    /// <summary>
    /// System-level impact.
    /// </summary>
    System,

    /// <summary>
    /// Global impact.
    /// </summary>
    Global
} 



