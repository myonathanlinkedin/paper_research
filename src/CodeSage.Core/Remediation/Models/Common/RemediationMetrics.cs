using System;
using System.Collections.Generic;

namespace CodeSage.Core.Remediation.Models.Common
{
    public class RemediationMetrics
    {
        public string MetricsId { get; set; } = Guid.NewGuid().ToString();
        public string RemediationId { get; set; } = string.Empty;
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime? EndTime { get; set; }
        public TimeSpan? Duration => EndTime.HasValue ? EndTime.Value - StartTime : null;
        public ResourceUsage ResourceUsage { get; set; } = new();
        public int TotalSteps { get; set; }
        public int CompletedSteps { get; set; }
        public int FailedSteps { get; set; }
        public int SkippedSteps { get; set; }
        public int RetryCount { get; set; }
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, TimeSpan> StepDurations { get; set; } = new();
        public Dictionary<string, int> StepRetries { get; set; } = new();
        public Dictionary<string, string> StepErrors { get; set; } = new();
        public Dictionary<string, object> AdditionalMetrics { get; set; } = new();

        public void RecordStepDuration(string stepId, TimeSpan duration)
        {
            StepDurations[stepId] = duration;
        }

        public void RecordStepRetry(string stepId)
        {
            if (!StepRetries.ContainsKey(stepId))
            {
                StepRetries[stepId] = 0;
            }
            StepRetries[stepId]++;
            RetryCount++;
        }

        public void RecordStepError(string stepId, string errorMessage)
        {
            StepErrors[stepId] = errorMessage;
            FailedSteps++;
        }

        public void RecordStepCompletion(string stepId)
        {
            CompletedSteps++;
        }

        public void RecordStepSkipped(string stepId)
        {
            SkippedSteps++;
        }

        public void Complete()
        {
            EndTime = DateTime.UtcNow;
        }
    }

    public class ResourceUsage
    {
        public double CpuUsage { get; set; }
        public long MemoryUsageBytes { get; set; }
        public int ThreadCount { get; set; }
        public TimeSpan ProcessingTime { get; set; }
        public Dictionary<string, double> CustomMetrics { get; set; } = new();
    }
} 