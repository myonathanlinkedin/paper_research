using System;

namespace RuntimeErrorSage.Core.Models.Common
{
    /// <summary>
    /// Represents a time range for operations.
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
        /// Gets the duration of the range in milliseconds.
        /// </summary>
        public long DurationMs => (long)(End - Start).TotalMilliseconds;

        /// <summary>
        /// Creates a new time range.
        /// </summary>
        public TimeRange()
        {
            Start = DateTime.UtcNow;
            End = DateTime.UtcNow;
        }

        /// <summary>
        /// Creates a new time range with specified start and end times.
        /// </summary>
        public TimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
} 