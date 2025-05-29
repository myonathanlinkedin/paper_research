using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the complexity level of a remediation action.
/// </summary>
public enum RemediationComplexity
{
    /// <summary>
    /// Simple complexity - remediation is straightforward and quick to implement.
    /// </summary>
    Simple,

    /// <summary>
    /// Moderate complexity - remediation requires some effort to implement.
    /// </summary>
    Moderate,

    /// <summary>
    /// Complex - remediation requires significant effort to implement.
    /// </summary>
    Complex,

    /// <summary>
    /// Very complex - remediation requires extensive effort to implement.
    /// </summary>
    VeryComplex,

    /// <summary>
    /// Complexity level is unknown.
    /// </summary>
    Unknown
} 






