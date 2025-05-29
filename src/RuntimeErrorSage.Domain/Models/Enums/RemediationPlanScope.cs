namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Defines the scope of remediation plans.
/// </summary>
public enum RemediationPlanScope
{
    /// <summary>
    /// The plan has local scope.
    /// </summary>
    Local,

    /// <summary>
    /// The plan has global scope.
    /// </summary>
    Global,

    /// <summary>
    /// The plan has system scope.
    /// </summary>
    System,

    /// <summary>
    /// The plan has application scope.
    /// </summary>
    Application,

    /// <summary>
    /// The plan has component scope.
    /// </summary>
    Component,

    /// <summary>
    /// The plan has service scope.
    /// </summary>
    Service,

    /// <summary>
    /// The plan has module scope.
    /// </summary>
    Module,

    /// <summary>
    /// The plan has feature scope.
    /// </summary>
    Feature,

    /// <summary>
    /// The plan has unknown scope.
    /// </summary>
    Unknown
} 

