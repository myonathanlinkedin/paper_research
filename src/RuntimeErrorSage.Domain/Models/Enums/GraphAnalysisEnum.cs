using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Represents the type of graph analysis.
/// </summary>
public enum GraphAnalysisType
{
    /// <summary>
    /// No analysis.
    /// </summary>
    None,

    /// <summary>
    /// Impact analysis.
    /// </summary>
    Impact,

    /// <summary>
    /// Critical path analysis.
    /// </summary>
    CriticalPath,

    /// <summary>
    /// Dependency analysis.
    /// </summary>
    Dependency,

    /// <summary>
    /// Root cause analysis.
    /// </summary>
    RootCause,

    /// <summary>
    /// Cycle detection.
    /// </summary>
    CycleDetection,

    /// <summary>
    /// Centrality analysis.
    /// </summary>
    Centrality,

    /// <summary>
    /// Error propagation analysis.
    /// </summary>
    ErrorPropagation,

    /// <summary>
    /// Risk analysis.
    /// </summary>
    Risk
} 






