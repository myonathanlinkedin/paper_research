using System;

namespace RuntimeErrorSage.Core.Extensions
{
    /// <summary>
    /// Extension methods for TimeSpan objects in the RuntimeErrorSage codebase.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Gets the total milliseconds from a nullable TimeSpan.
        /// </summary>
        /// <param name="timeSpan">The nullable TimeSpan.</param>
        /// <returns>The total milliseconds, or 0 if the TimeSpan is null.</returns>
        public static double GetTotalMilliseconds(this TimeSpan? timeSpan)
        {
            return timeSpan?.TotalMilliseconds ?? 0;
        }

        /// <summary>
        /// Gets the total seconds from a nullable TimeSpan.
        /// </summary>
        /// <param name="timeSpan">The nullable TimeSpan.</param>
        /// <returns>The total seconds, or 0 if the TimeSpan is null.</returns>
        public static double GetTotalSeconds(this TimeSpan? timeSpan)
        {
            return timeSpan?.TotalSeconds ?? 0;
        }

        /// <summary>
        /// Gets the total minutes from a nullable TimeSpan.
        /// </summary>
        /// <param name="timeSpan">The nullable TimeSpan.</param>
        /// <returns>The total minutes, or 0 if the TimeSpan is null.</returns>
        public static double GetTotalMinutes(this TimeSpan? timeSpan)
        {
            return timeSpan?.TotalMinutes ?? 0;
        }

        /// <summary>
        /// Gets the total hours from a nullable TimeSpan.
        /// </summary>
        /// <param name="timeSpan">The nullable TimeSpan.</param>
        /// <returns>The total hours, or 0 if the TimeSpan is null.</returns>
        public static double GetTotalHours(this TimeSpan? timeSpan)
        {
            return timeSpan?.TotalHours ?? 0;
        }

        /// <summary>
        /// Gets the total days from a nullable TimeSpan.
        /// </summary>
        /// <param name="timeSpan">The nullable TimeSpan.</param>
        /// <returns>The total days, or 0 if the TimeSpan is null.</returns>
        public static double GetTotalDays(this TimeSpan? timeSpan)
        {
            return timeSpan?.TotalDays ?? 0;
        }

        /// <summary>
        /// Converts a double value to a TimeSpan representing milliseconds.
        /// </summary>
        /// <param name="milliseconds">The milliseconds value.</param>
        /// <returns>A TimeSpan representing the specified milliseconds.</returns>
        public static TimeSpan FromMilliseconds(this double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Converts a double value to a TimeSpan representing seconds.
        /// </summary>
        /// <param name="seconds">The seconds value.</param>
        /// <returns>A TimeSpan representing the specified seconds.</returns>
        public static TimeSpan FromSeconds(this double seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Calculates the TimeSpan between two DateTimes, handling nulls safely.
        /// </summary>
        /// <param name="end">The end DateTime.</param>
        /// <param name="start">The start DateTime.</param>
        /// <returns>The TimeSpan between end and start, or null if either is null.</returns>
        public static TimeSpan? DurationBetween(this DateTime? end, DateTime start)
        {
            return end.HasValue ? end.Value - start : null;
        }
        
        /// <summary>
        /// Calculates the TimeSpan between two DateTimes, handling nulls safely.
        /// </summary>
        /// <param name="end">The end DateTime.</param>
        /// <param name="start">The start DateTime.</param>
        /// <returns>The TimeSpan between end and start, or TimeSpan.Zero if either is null.</returns>
        public static TimeSpan DurationBetweenOrZero(this DateTime? end, DateTime start)
        {
            return end.HasValue ? end.Value - start : TimeSpan.Zero;
        }
    }
} 
