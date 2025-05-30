using System;
using System.Collections.Concurrent;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Execution;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Application.Remediation
{
    public class ActionExecutionTracker
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Domain.Models.Execution.RemediationActionExecution>> _actionExecutions = new();

        public void TrackActionStart(string planId, string actionId)
        {
            var actionExecutions = _actionExecutions.GetOrAdd(planId, _ => new ConcurrentDictionary<string, Domain.Models.Execution.RemediationActionExecution>());
            var actionExecution = new Domain.Models.Execution.RemediationActionExecution
            {
                ActionId = actionId,
                StartTime = DateTime.UtcNow,
                Status = RemediationActionStatus.InProgress.ToString()
            };
            actionExecutions.AddOrUpdate(actionId, actionExecution, (_, _) => actionExecution);
        }

        public void TrackActionCompletion(string planId, string actionId, bool success, string? errorMessage = null)
        {
            if (_actionExecutions.TryGetValue(planId, out var actionExecutions) &&
                actionExecutions.TryGetValue(actionId, out var actionExecution))
            {
                actionExecution.EndTime = DateTime.UtcNow;
                actionExecution.Status = success ? RemediationActionStatus.Completed.ToString() : RemediationActionStatus.Failed.ToString();
                if (errorMessage != null)
                {
                    actionExecution.ErrorMessage = errorMessage;
                }
            }
        }
    }
} 
