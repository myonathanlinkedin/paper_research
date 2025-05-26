using System;

namespace RuntimeErrorSage.Core.Models;

/// <summary>
/// Represents a time range with start and end times.
/// </summary>
public class TimeRange
{
    /// <summary>
    /// Gets or sets the start time of the range.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the range.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Gets the duration of the time range.
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;

    /// <summary>
    /// Initializes a new instance of the TimeRange class.
    /// </summary>
    public TimeRange()
    {
        StartTime = DateTime.UtcNow;
        EndTime = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the TimeRange class with specified start and end times.
    /// </summary>
    /// <param name="startTime">The start time of the range.</param>
    /// <param name="endTime">The end time of the range.</param>
    public TimeRange(DateTime startTime, DateTime endTime)
    {
        if (endTime < startTime)
        {
            throw new ArgumentException("End time must be greater than or equal to start time.", nameof(endTime));
        }

        StartTime = startTime;
        EndTime = endTime;
    }

    /// <summary>
    /// Checks if a given time falls within this range.
    /// </summary>
    /// <param name="time">The time to check.</param>
    /// <returns>True if the time falls within the range; otherwise, false.</returns>
    public bool Contains(DateTime time)
    {
        return time >= StartTime && time <= EndTime;
    }

    /// <summary>
    /// Checks if this range overlaps with another range.
    /// </summary>
    /// <param name="other">The other time range to check.</param>
    /// <returns>True if the ranges overlap; otherwise, false.</returns>
    public bool Overlaps(TimeRange other)
    {
        return StartTime <= other.EndTime && other.StartTime <= EndTime;
    }

    /// <summary>
    /// Gets the intersection of this range with another range.
    /// </summary>
    /// <param name="other">The other time range.</param>
    /// <returns>A new TimeRange representing the intersection, or null if there is no intersection.</returns>
    public TimeRange? GetIntersection(TimeRange other)
    {
        if (!Overlaps(other))
        {
            return null;
        }

        var start = StartTime > other.StartTime ? StartTime : other.StartTime;
        var end = EndTime < other.EndTime ? EndTime : other.EndTime;

        return new TimeRange(start, end);
    }
} 
