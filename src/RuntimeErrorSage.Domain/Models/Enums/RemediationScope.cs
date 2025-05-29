namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Represents the scope of a remediation action.
/// </summary>
public enum RemediationScope
{
    /// <summary>
    /// Remediation affects a single file.
    /// </summary>
    File,

    /// <summary>
    /// Remediation affects a single class.
    /// </summary>
    Class,

    /// <summary>
    /// Remediation affects a single method.
    /// </summary>
    Method,

    /// <summary>
    /// Remediation affects a single property.
    /// </summary>
    Property,

    /// <summary>
    /// Remediation affects a single namespace.
    /// </summary>
    Namespace,

    /// <summary>
    /// Remediation affects a single project.
    /// </summary>
    Project,

    /// <summary>
    /// Remediation affects the entire solution.
    /// </summary>
    Solution,

    /// <summary>
    /// Remediation scope is unknown.
    /// </summary>
    Unknown
} 
