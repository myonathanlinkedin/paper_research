using System;
using System.Linq;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents a time window during which a remediation action is available.
    /// </summary>
    public class AvailabilityWindow
    {
        /// <summary>
        /// Gets or sets the start time of the window.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the window.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the days of the week when the window is active.
        /// </summary>
        public DayOfWeek[] DaysOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the time zone for the window.
        /// </summary>
        public TimeZoneInfo TimeZone { get; set; }

        /// <summary>
        /// Checks if a given time falls within this window.
        /// </summary>
        /// <param name="time">The time to check.</param>
        /// <returns>True if the time is within the window, false otherwise.</returns>
        public bool IsInWindow(DateTime time)
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(time, TimeZone);
            return localTime >= StartTime && localTime <= EndTime && DaysOfWeek.Contains(localTime.DayOfWeek);
        }
    }
} 