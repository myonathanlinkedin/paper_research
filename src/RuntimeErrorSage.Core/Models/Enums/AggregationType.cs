namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the type of metric aggregation.
/// </summary>
public enum AggregationType
{
    /// <summary>
    /// No aggregation.
    /// </summary>
    None = 0,

    /// <summary>
    /// Sum of values.
    /// </summary>
    Sum = 1,

    /// <summary>
    /// Average of values.
    /// </summary>
    Average = 2,

    /// <summary>
    /// Minimum value.
    /// </summary>
    Min = 3,

    /// <summary>
    /// Maximum value.
    /// </summary>
    Max = 4,

    /// <summary>
    /// Count of values.
    /// </summary>
    Count = 5,

    /// <summary>
    /// First value in the series.
    /// </summary>
    First = 6,

    /// <summary>
    /// Last value in the series.
    /// </summary>
    Last = 7,

    /// <summary>
    /// Standard deviation of values.
    /// </summary>
    StandardDeviation = 8,

    /// <summary>
    /// Percentile of values.
    /// </summary>
    Percentile = 9
} 