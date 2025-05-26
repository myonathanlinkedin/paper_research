using System;
using System.Collections.Generic;

namespace CodeSage.Core.Models
{
    public class RemediationPlan
    {
        public string PlanId { get; set; } = Guid.NewGuid().ToString();
        public string Context { get; set; } = string.Empty;
        public List<RemediationStep> Steps { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExecutedAt { get; set; }
        public RemediationStatus Status { get; set; } = RemediationStatus.Pending;
        public string? ErrorMessage { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
        public int CurrentStepIndex { get; set; }
        public bool IsValidated { get; set; }
        public string? ValidationMessage { get; set; }

        public RemediationPlan()
        {
        }

        public RemediationPlan(string context, List<RemediationStep> steps)
        {
            Context = context;
            Steps = steps;
        }

        public void AddStep(RemediationStep step)
        {
            Steps.Add(step);
        }

        public void RemoveStep(int index)
        {
            if (index >= 0 && index < Steps.Count)
            {
                Steps.RemoveAt(index);
            }
        }

        public void MoveStep(int fromIndex, int toIndex)
        {
            if (fromIndex >= 0 && fromIndex < Steps.Count && toIndex >= 0 && toIndex < Steps.Count)
            {
                var step = Steps[fromIndex];
                Steps.RemoveAt(fromIndex);
                Steps.Insert(toIndex, step);
            }
        }

        public void Validate()
        {
            IsValidated = true;
            ValidationMessage = "Plan validated successfully.";
        }

        public void Invalidate(string message)
        {
            IsValidated = false;
            ValidationMessage = message;
        }
    }
} 