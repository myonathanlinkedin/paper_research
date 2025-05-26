using System;

namespace RuntimeErrorSage.Core.Models.Common
{
    /// <summary>
    /// Represents a time range for metrics aggregation.
    /// </summary>
    public class TimeRange
    {
        /// <summary>
        /// Gets or sets the start time of the range.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end time of the range.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Gets the duration of the time range.
        /// </summary>
        public TimeSpan Duration => End - Start;

        /// <summary>
        /// Creates a new time range.
        /// </summary>
        public TimeRange()
        {
            Start = DateTime.UtcNow;
            End = DateTime.UtcNow;
        }

        /// <summary>
        /// Creates a new time range with the specified start and end times.
        /// </summary>
        public TimeRange(DateTime start, DateTime end)
        {
            if (end < start)
            {
                throw new ArgumentException("End time must be greater than or equal to start time.", nameof(end));
            }
            Start = start;
            End = end;
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
            return time >= Start && time <= End;
        }

        /// <summary>
        /// Checks if this range overlaps with another range.
        /// </summary>
        /// <param name="other">The other time range to check.</param>
        /// <returns>True if the ranges overlap; otherwise, false.</returns>
        public bool Overlaps(TimeRange other)
        {
            return Start <= other.End && other.Start <= End;
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

            var start = Start > other.Start ? Start : other.Start;
            var end = End < other.End ? End : other.End;

            return new TimeRange(start, end);
        }
    }
} 
