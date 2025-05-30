using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Application.Extensions;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Execution;

namespace RuntimeErrorSage.Application.Examples
{
    /// <summary>
    /// Examples showing how to properly handle nullable TimeSpan properties
    /// </summary>
    public class TimeSpanNullableExtensionsExample
    {
        /// <summary>
        /// Shows correct vs incorrect ways to access TimeSpan? TotalMilliseconds
        /// </summary>
        /// <param name="result">A remediation result with nullable duration</param>
        public void ShowTimeSpanNullableAccess(RemediationResult result)
        {
            TimeSpan? nullableDuration = result.EndTime.HasValue ? result.EndTime.Value - result.StartTime : null;
            
            // INCORRECT: This causes error "'TimeSpan?' does not contain a definition for 'TotalMilliseconds'"
            // double ms = nullableDuration.TotalMilliseconds; 
            
            // CORRECT: Use conditional null handling operator
            double ms1 = nullableDuration?.TotalMilliseconds ?? 0;
            
            // CORRECT: Use HasValue check
            double ms2 = nullableDuration.HasValue ? nullableDuration.Value.TotalMilliseconds : 0;
            
            // CORRECT: Use GetTotalMilliseconds extension method
            double ms3 = nullableDuration.GetTotalMilliseconds();
            
            Console.WriteLine($"Duration in milliseconds: {ms3}");
        }
        
        /// <summary>
        /// Shows how to handle multiple TimeSpan? calculations
        /// </summary>
        /// <param name="executions">List of execution results with nullable durations</param>
        public void CalculateAverageDuration(List<RemediationExecution> executions)
        {
            // INCORRECT: This causes error with nullable TimeSpan
            // var avgDuration = executions.Average(e => e.DurationSeconds);
            
            // CORRECT: Handle null values explicitly
            var avgDuration1 = executions
                .Where(e => e.DurationSeconds.HasValue)
                .Average(e => e.DurationSeconds.Value);
            
            // CORRECT: Use extension method for consistent null handling
            var durations = executions.Select(e => {
                TimeSpan? duration = e.EndTime.HasValue ? e.EndTime.Value - e.StartTime : null;
                return duration;
            }).ToList();
            
            var avgDuration2 = durations.Sum(d => d.GetTotalSeconds()) / durations.Count;
            
            Console.WriteLine($"Average duration: {avgDuration2} seconds");
        }
        
        /// <summary>
        /// Shows how to handle TimeSpan? in calculations
        /// </summary>
        /// <param name="start">Start time</param>
        /// <param name="end">Optional end time</param>
        /// <returns>Duration in milliseconds</returns>
        public double CalculateDuration(DateTime start, DateTime? end)
        {
            TimeSpan? duration = end.HasValue ? end.Value - start : null;
            
            // INCORRECT: This causes error with nullable TimeSpan
            // return duration.TotalMilliseconds;
            
            // CORRECT: Use conditional operator
            return duration?.TotalMilliseconds ?? 0;
            
            // ALTERNATIVE: Use extension method
            // return duration.GetTotalMilliseconds();
        }
    }
} 
