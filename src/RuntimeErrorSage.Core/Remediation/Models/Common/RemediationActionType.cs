namespace RuntimeErrorSage.Core.Remediation.Models.Common;

/// <summary>
/// Defines the types of remediation actions.
/// </summary>
public enum RemediationActionType
{
    /// <summary>
    /// A code fix action.
    /// </summary>
    CodeFix,

    /// <summary>
    /// A configuration update action.
    /// </summary>
    ConfigUpdate,

    /// <summary>
    /// A resource allocation action.
    /// </summary>
    ResourceAllocation,

    /// <summary>
    /// A service restart action.
    /// </summary>
    ServiceRestart,

    /// <summary>
    /// A cache clear action.
    /// </summary>
    CacheClear,

    /// <summary>
    /// A database update action.
    /// </summary>
    DatabaseUpdate,

    /// <summary>
    /// A dependency update action.
    /// </summary>
    DependencyUpdate,

    /// <summary>
    /// A custom action.
    /// </summary>
    Custom
} 