namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the dependency status of a remediation action.
/// </summary>
public enum RemediationDependencyStatus
{
    /// <summary>
    /// No dependencies.
    /// </summary>
    None,

    /// <summary>
    /// Waiting for dependencies.
    /// </summary>
    Waiting,

    /// <summary>
    /// Dependencies satisfied.
    /// </summary>
    Satisfied,

    /// <summary>
    /// Dependencies failed.
    /// </summary>
    Failed,

    /// <summary>
    /// Dependency status is unknown.
    /// </summary>
    Unknown
} 