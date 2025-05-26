namespace RuntimeErrorSage.Core.Models.Metrics.Enums;

/// <summary>
/// Specifies the type of metric aggregation.
/// </summary>
public enum AggregationType
{
    /// <summary>
    /// Average of values.
    /// </summary>
    Average,

    /// <summary>
    /// Sum of values.
    /// </summary>
    Sum,

    /// <summary>
    /// Minimum value.
    /// </summary>
    Min,

    /// <summary>
    /// Maximum value.
    /// </summary>
    Max,

    /// <summary>
    /// Count of values.
    /// </summary>
    Count
} 