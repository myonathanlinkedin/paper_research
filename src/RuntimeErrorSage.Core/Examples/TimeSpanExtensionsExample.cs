using System;
using System.Collections.Generic;
using RuntimeErrorSage.Model.Extensions;
using RuntimeErrorSage.Model.Models.Remediation;

namespace RuntimeErrorSage.Model.Examples
{
    /// <summary>
    /// Example code showing how to use TimeSpanExtensions.
    /// </summary>
    public class TimeSpanExtensionsExample
    {
        /// <summary>
        /// Shows incorrect vs correct ways to access properties of nullable TimeSpan.
        /// </summary>
        /// <param name="result">The remediation action result.</param>
        public void ShowTimeSpanExtensionsExample(RemediationActionResult result)
        {
            // INCORRECT: This might cause null reference exceptions
            // double durationMs = result.Duration?.TotalMilliseconds ?? 0;
            
            // CORRECT: Use the extension method
            double durationMs = result.Duration.GetTotalMilliseconds();
            Console.WriteLine($"Duration in milliseconds: {durationMs}");
            
            // Other extension methods available
            double durationSec = result.Duration.GetTotalSeconds();
            Console.WriteLine($"Duration in seconds: {durationSec}");
            
            double durationMin = result.Duration.GetTotalMinutes();
            Console.WriteLine($"Duration in minutes: {durationMin}");
            
            double durationHours = result.Duration.GetTotalHours();
            Console.WriteLine($"Duration in hours: {durationHours}");
            
            double durationDays = result.Duration.GetTotalDays();
            Console.WriteLine($"Duration in days: {durationDays}");
        }

        /// <summary>
        /// Shows how to handle TimeSpan calculations safely.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time (nullable).</param>
        /// <returns>The duration in milliseconds.</returns>
        public double CalculateDuration(DateTime startTime, DateTime? endTime)
        {
            // If endTime is null, use current time
            TimeSpan? duration = endTime.HasValue ? endTime.Value - startTime : DateTime.UtcNow - startTime;
            
            // INCORRECT: This might cause null reference exceptions
            // return duration?.TotalMilliseconds ?? 0;
            
            // CORRECT: Use the extension method
            return duration.GetTotalMilliseconds();
        }

        /// <summary>
        /// Shows how to handle multiple TimeSpan operations safely.
        /// </summary>
        /// <param name="executionTimes">The list of execution times.</param>
        /// <returns>The average execution time in milliseconds.</returns>
        public double CalculateAverageExecutionTime(List<TimeSpan?> executionTimes)
        {
            if (executionTimes == null || executionTimes.Count == 0)
            {
                return 0;
            }
            
            // INCORRECT: This might cause null reference exceptions
            // double totalMs = executionTimes.Sum(t => t?.TotalMilliseconds ?? 0);
            
            // CORRECT: Use the extension method
            double totalMs = executionTimes.Sum(t => t.GetTotalMilliseconds());
            
            return totalMs / executionTimes.Count;
        }
    }
} 
