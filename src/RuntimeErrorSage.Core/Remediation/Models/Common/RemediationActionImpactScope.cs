namespace RuntimeErrorSage.Core.Remediation.Models.Common;

/// <summary>
/// Defines the impact scope of remediation actions.
/// </summary>
public enum RemediationActionImpactScope
{
    /// <summary>
    /// Global impact scope.
    /// </summary>
    Global,

    /// <summary>
    /// Service-level impact scope.
    /// </summary>
    Service,

    /// <summary>
    /// Component-level impact scope.
    /// </summary>
    Component,

    /// <summary>
    /// Operation-level impact scope.
    /// </summary>
    Operation,

    /// <summary>
    /// Instance-level impact scope.
    /// </summary>
    Instance
} 