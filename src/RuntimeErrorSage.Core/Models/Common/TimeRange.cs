using System;

namespace RuntimeErrorSage.Core.Models.Common
{
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
        /// Initializes a new instance of the <see cref="TimeRange"/> class.
        /// </summary>
        public TimeRange()
        {
            StartTime = DateTime.UtcNow;
            EndTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRange"/> class.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        public TimeRange(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        /// <summary>
        /// Creates a time range for the last specified number of minutes.
        /// </summary>
        public static TimeRange LastMinutes(int minutes)
        {
            var end = DateTime.UtcNow;
            return new TimeRange(end.AddMinutes(-minutes), end);
        }

        /// <summary>
        /// Creates a time range for the last specified number of hours.
        /// </summary>
        public static TimeRange LastHours(int hours)
        {
            var end = DateTime.UtcNow;
            return new TimeRange(end.AddHours(-hours), end);
        }

        /// <summary>
        /// Creates a time range for the last specified number of days.
        /// </summary>
        public static TimeRange LastDays(int days)
        {
            var end = DateTime.UtcNow;
            return new TimeRange(end.AddDays(-days), end);
        }

        /// <summary>
        /// Determines whether a specific time falls within this range.
        /// </summary>
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
} 
