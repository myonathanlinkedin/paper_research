using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the priority level for error prevention measures.
/// </summary>
public enum PreventionPriority
{
    /// <summary>
    /// No priority assigned.
    /// </summary>
    None = 0,

    /// <summary>
    /// Low priority - can be addressed when convenient.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Medium priority - should be addressed in the near future.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// High priority - should be addressed soon.
    /// </summary>
    High = 3,

    /// <summary>
    /// Critical priority - must be addressed immediately.
    /// </summary>
    Critical = 4,

    /// <summary>
    /// Emergency priority - requires immediate attention to prevent system failure.
    /// </summary>
    Emergency = 5
} 





