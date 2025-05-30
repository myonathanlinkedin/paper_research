using System;
using System.Linq;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents a time window for availability.
    /// </summary>
    public class AvailabilityWindow
    {
        /// <summary>
        /// Gets or sets the start time of the window.
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the window.
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Gets or sets the time zone for the availability window.
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets whether the window repeats.
        /// </summary>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// Gets or sets the recurrence pattern if the window repeats.
        /// </summary>
        public string RecurrencePattern { get; set; }

        /// <summary>
        /// Gets or sets the recurrence interval if the window repeats.
        /// </summary>
        public int RecurrenceInterval { get; set; }

        /// <summary>
        /// Gets or sets the recurrence frequency if the window repeats.
        /// </summary>
        public string RecurrenceFrequency { get; set; }

        /// <summary>
        /// Gets or sets the recurrence days of week if the window repeats.
        /// </summary>
        public string RecurrenceDaysOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the recurrence days of month if the window repeats.
        /// </summary>
        public string RecurrenceDaysOfMonth { get; set; }

        /// <summary>
        /// Gets or sets the recurrence months of year if the window repeats.
        /// </summary>
        public string RecurrenceMonthsOfYear { get; set; }

        /// <summary>
        /// Gets or sets the recurrence end date if the window repeats.
        /// </summary>
        public DateTime? RecurrenceEndDate { get; set; }

        /// <summary>
        /// Gets or sets the recurrence end after occurrences if the window repeats.
        /// </summary>
        public int? RecurrenceEndAfterOccurrences { get; set; }

        /// <summary>
        /// Gets or sets whether the recurrence never ends.
        /// </summary>
        public bool RecurrenceNeverEnds { get; set; }

        /// <summary>
        /// Gets or sets the recurrence start date if the window repeats.
        /// </summary>
        public DateTime? RecurrenceStartDate { get; set; }

        /// <summary>
        /// Gets or sets the recurrence time zone if the window repeats.
        /// </summary>
        public string RecurrenceTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the recurrence week of month if the window repeats.
        /// </summary>
        public string RecurrenceWeekOfMonth { get; set; }

        /// <summary>
        /// Gets or sets the recurrence year if the window repeats.
        /// </summary>
        public int? RecurrenceYear { get; set; }

        /// <summary>
        /// Gets or sets the recurrence month if the window repeats.
        /// </summary>
        public int? RecurrenceMonth { get; set; }

        /// <summary>
        /// Gets or sets the recurrence day if the window repeats.
        /// </summary>
        public int? RecurrenceDay { get; set; }

        /// <summary>
        /// Gets or sets the recurrence hour if the window repeats.
        /// </summary>
        public int? RecurrenceHour { get; set; }

        /// <summary>
        /// Gets or sets the recurrence minute if the window repeats.
        /// </summary>
        public int? RecurrenceMinute { get; set; }

        /// <summary>
        /// Gets or sets the recurrence second if the window repeats.
        /// </summary>
        public int? RecurrenceSecond { get; set; }

        /// <summary>
        /// Gets or sets the recurrence millisecond if the window repeats.
        /// </summary>
        public int? RecurrenceMillisecond { get; set; }

        /// <summary>
        /// Gets or sets the recurrence day of week if the window repeats.
        /// </summary>
        public DayOfWeek? RecurrenceDayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the recurrence day of year if the window repeats.
        /// </summary>
        public int? RecurrenceDayOfYear { get; set; }

        /// <summary>
        /// Gets or sets the recurrence week of year if the window repeats.
        /// </summary>
        public int? RecurrenceWeekOfYear { get; set; }

        /// <summary>
        /// Gets or sets the recurrence quarter if the window repeats.
        /// </summary>
        public int? RecurrenceQuarter { get; set; }

        /// <summary>
        /// Gets or sets the recurrence semester if the window repeats.
        /// </summary>
        public int? RecurrenceSemester { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal year if the window repeats.
        /// </summary>
        public int? RecurrenceFiscalYear { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal quarter if the window repeats.
        /// </summary>
        public int? RecurrenceFiscalQuarter { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal month if the window repeats.
        /// </summary>
        public int? RecurrenceFiscalMonth { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal week if the window repeats.
        /// </summary>
        public int? RecurrenceFiscalWeek { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal day if the window repeats.
        /// </summary>
        public int? RecurrenceFiscalDay { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal day of week if the window repeats.
        /// </summary>
        public DayOfWeek? RecurrenceFiscalDayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal day of year if the window repeats.
        /// </summary>
        public int? RecurrenceFiscalDayOfYear { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal week of year if the window repeats.
        /// </summary>
        public int? RecurrenceFiscalWeekOfYear { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal quarter of year if the window repeats.
        /// </summary>
        public int? RecurrenceFiscalQuarterOfYear { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal semester of year if the window repeats.
        /// </summary>
        public int? RecurrenceFiscalSemesterOfYear { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal year start if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalYearStart { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal year end if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalYearEnd { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal quarter start if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalQuarterStart { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal quarter end if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalQuarterEnd { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal month start if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalMonthStart { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal month end if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalMonthEnd { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal week start if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalWeekStart { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal week end if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalWeekEnd { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal day start if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalDayStart { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal day end if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalDayEnd { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal day of week start if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalDayOfWeekStart { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal day of week end if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalDayOfWeekEnd { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal day of year start if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalDayOfYearStart { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal day of year end if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalDayOfYearEnd { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal week of year start if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalWeekOfYearStart { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal week of year end if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalWeekOfYearEnd { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal quarter of year start if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalQuarterOfYearStart { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal quarter of year end if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalQuarterOfYearEnd { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal semester of year start if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalSemesterOfYearStart { get; set; }

        /// <summary>
        /// Gets or sets the recurrence fiscal semester of year end if the window repeats.
        /// </summary>
        public DateTime? RecurrenceFiscalSemesterOfYearEnd { get; set; }

        public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek>();

        /// <summary>
        /// Checks if a given time falls within this window.
        /// </summary>
        /// <param name="time">The time to check.</param>
        /// <returns>True if the time is within the window, false otherwise.</returns>
        public bool IsInWindow(DateTime time)
        {
            var startTime = TimeSpan.Parse(StartTime);
            var endTime = TimeSpan.Parse(EndTime);
            var currentTime = time.TimeOfDay;

            // Parse RecurrenceDaysOfWeek as comma-separated list
            var daysOfWeek = new HashSet<DayOfWeek>();
            if (!string.IsNullOrEmpty(RecurrenceDaysOfWeek))
            {
                foreach (var day in RecurrenceDaysOfWeek.Split(','))
                {
                    if (Enum.TryParse(day.Trim(), out DayOfWeek dow))
                        daysOfWeek.Add(dow);
                }
            }

            return currentTime >= startTime && currentTime <= endTime && daysOfWeek.Contains(time.DayOfWeek);
        }

        public bool IsWithinWindow(DateTime time, string timezone)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            var localTime = TimeZoneInfo.ConvertTime(time, tz);
            var startTime = TimeSpan.Parse(StartTime);
            var endTime = TimeSpan.Parse(EndTime);
            var currentTime = localTime.TimeOfDay;

            return currentTime >= startTime && currentTime <= endTime && 
                   DaysOfWeek.Contains(localTime.DayOfWeek);
        }

        public bool IsAvailable(DayOfWeek dayOfWeek, TimeSpan timeOfDay)
        {
            if (!DaysOfWeek.Contains(dayOfWeek))
                return false;

            var startTime = TimeSpan.Parse(StartTime);
            var endTime = TimeSpan.Parse(EndTime);

            return timeOfDay >= startTime && timeOfDay <= endTime;
        }

        public bool IsAvailable(DateTime time)
        {
            return IsAvailable(time.DayOfWeek, time.TimeOfDay);
        }
    }
} 
