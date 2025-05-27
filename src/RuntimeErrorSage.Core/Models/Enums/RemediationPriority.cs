namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the priority level of a remediation action.
/// </summary>
public enum RemediationPriority
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
    /// Medium priority - should be addressed soon.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// High priority - should be addressed as soon as possible.
    /// </summary>
    High = 3,

    /// <summary>
    /// Critical priority - must be addressed immediately.
    /// </summary>
    Critical = 4,

    /// <summary>
    /// Emergency priority - requires immediate attention and may impact system stability.
    /// </summary>
    Emergency = 5
} 