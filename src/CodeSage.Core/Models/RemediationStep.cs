using System;
using System.Collections.Generic;

namespace CodeSage.Core.Models
{
    public class RemediationStep
    {
        public string StepId { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new();
        public RemediationStatus Status { get; set; } = RemediationStatus.Pending;
        public string? ErrorMessage { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int RetryCount { get; set; }
        public int MaxRetries { get; set; } = 3;
        public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(5);
        public Dictionary<string, object> Metadata { get; set; } = new();

        public RemediationStep()
        {
        }

        public RemediationStep(string description, string action)
        {
            Description = description;
            Action = action;
        }

        public void Start()
        {
            Status = RemediationStatus.InProgress;
            StartedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            Status = RemediationStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void Fail(string errorMessage)
        {
            Status = RemediationStatus.Failed;
            ErrorMessage = errorMessage;
            CompletedAt = DateTime.UtcNow;
        }

        public void Retry()
        {
            if (RetryCount < MaxRetries)
            {
                RetryCount++;
                Status = RemediationStatus.Pending;
                StartedAt = null;
                CompletedAt = null;
                ErrorMessage = null;
            }
            else
            {
                Status = RemediationStatus.Failed;
                ErrorMessage = "Max retries exceeded.";
            }
        }
    }
} 