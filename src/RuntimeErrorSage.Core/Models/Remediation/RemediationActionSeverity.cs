namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Defines the severity levels for remediation actions.
/// </summary>
public enum RemediationActionSeverity
{
    /// <summary>
    /// Information level severity.
    /// </summary>
    Information = 0,

    /// <summary>
    /// Warning level severity.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Moderate level severity.
    /// </summary>
    Moderate = 2,

    /// <summary>
    /// Major level severity.
    /// </summary>
    Major = 3,

    /// <summary>
    /// Critical level severity.
    /// </summary>
    Critical = 4,

    /// <summary>
    /// Blocking level severity.
    /// </summary>
    Blocking = 5
} 