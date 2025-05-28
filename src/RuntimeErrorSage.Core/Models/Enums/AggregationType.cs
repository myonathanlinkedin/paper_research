namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the type of aggregation for metrics collection.
/// </summary>
public enum AggregationType
{
    /// <summary>
    /// Sum of values.
    /// </summary>
    Sum = 0,
    
    /// <summary>
    /// Average of values.
    /// </summary>
    Average = 1,
    
    /// <summary>
    /// Maximum value.
    /// </summary>
    Max = 2,
    
    /// <summary>
    /// Minimum value.
    /// </summary>
    Min = 3,
    
    /// <summary>
    /// Count of values.
    /// </summary>
    Count = 4
} 