using System;
using System.Collections.Generic;

namespace CodeSage.Core.Models.Metrics
{
    public class RemediationMetrics
    {
        public string RemediationId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan Duration => EndTime.HasValue ? EndTime.Value - StartTime : TimeSpan.Zero;
        public bool IsSuccessful { get; set; }
        public int StepCount { get; set; }
        public int CompletedSteps { get; set; }
        public int FailedSteps { get; set; }
        public ResourceUsage StartResourceUsage { get; set; }
        public ResourceUsage EndResourceUsage { get; set; }
        public Dictionary<string, double> CustomMetrics { get; set; }
        public List<string> Errors { get; set; }
        public Dictionary<string, TimeSpan> StepDurations { get; set; }
    }
} 