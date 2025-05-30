namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the priority levels for validation operations.
/// </summary>
public enum ValidationPriority
{
    /// <summary>
    /// No priority specified.
    /// </summary>
    None = 0,

    /// <summary>
    /// Lowest priority level.
    /// </summary>
    Lowest = 1,

    /// <summary>
    /// Low priority level.
    /// </summary>
    Low = 2,

    /// <summary>
    /// Medium priority level.
    /// </summary>
    Medium = 3,

    /// <summary>
    /// High priority level.
    /// </summary>
    High = 4,

    /// <summary>
    /// Highest priority level.
    /// </summary>
    Highest = 5,

    /// <summary>
    /// Critical priority level.
    /// </summary>
    Critical = 6
} 

