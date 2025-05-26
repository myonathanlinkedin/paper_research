using System;

namespace CodeSage.Core.Models;

public class TimeRange
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration => EndTime - StartTime;

    public TimeRange()
    {
        StartTime = DateTime.UtcNow;
        EndTime = DateTime.UtcNow;
    }

    public TimeRange(DateTime startTime, DateTime endTime)
    {
        if (endTime < startTime)
        {
            throw new ArgumentException("End time must be greater than or equal to start time", nameof(endTime));
        }

        StartTime = startTime;
        EndTime = endTime;
    }
} 