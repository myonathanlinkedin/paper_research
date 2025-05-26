using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the status of a remediation operation.
    /// </summary>
    public class RemediationStatus
    {
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public List<string> Steps { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
        public ExecutionMetrics Metrics { get; set; } = new();

        public void AddStep(string step)
        {
            Steps.Add(step);
        }

        public void AddMetadata(string key, object value)
        {
            Metadata[key] = value;
        }

        public void Complete()
        {
            EndTime = DateTime.UtcNow;
            Status = "Completed";
        }

        public void Fail(string error)
        {
            EndTime = DateTime.UtcNow;
            Status = "Failed";
            Description = error;
        }

        public static RemediationStatus Started()
        {
            return new RemediationStatus
            {
                Status = "Started",
                StartTime = DateTime.UtcNow
            };
        }

        public static RemediationStatus InProgress()
        {
            return new RemediationStatus
            {
                Status = "InProgress",
                StartTime = DateTime.UtcNow
            };
        }
    }
} 