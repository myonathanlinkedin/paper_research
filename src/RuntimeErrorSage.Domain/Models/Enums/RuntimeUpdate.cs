using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Represents the type of runtime update.
/// </summary>
public enum RuntimeUpdate
{
    /// <summary>
    /// No update.
    /// </summary>
    None,

    /// <summary>
    /// Configuration update.
    /// </summary>
    Configuration,

    /// <summary>
    /// State update.
    /// </summary>
    State,

    /// <summary>
    /// Health update.
    /// </summary>
    Health,

    /// <summary>
    /// Metrics update.
    /// </summary>
    Metrics,

    /// <summary>
    /// Error update.
    /// </summary>
    Error,

    /// <summary>
    /// Remediation update.
    /// </summary>
    Remediation
} 




