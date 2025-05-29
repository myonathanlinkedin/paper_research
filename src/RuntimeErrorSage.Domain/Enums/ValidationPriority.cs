namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the priority levels for validation.
/// </summary>
public enum ValidationPriority
{
    /// <summary>
    /// Unknown validation priority.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Low validation priority.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Normal validation priority.
    /// </summary>
    Normal = 2,

    /// <summary>
    /// Medium validation priority.
    /// </summary>
    Medium = 3,

    /// <summary>
    /// High validation priority.
    /// </summary>
    High = 4,

    /// <summary>
    /// Critical validation priority.
    /// </summary>
    Critical = 5,

    /// <summary>
    /// Emergency validation priority.
    /// </summary>
    Emergency = 6
} 

