using System;
using System.Collections.Concurrent;
using RuntimeErrorSage.Core.Models.Execution;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Remediation
{
    public class ActionExecutionTracker
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, RemediationActionExecution>> _actionExecutions = new();

        public void TrackActionStart(string planId, string actionId)
        {
            var actionExecutions = _actionExecutions.GetOrAdd(planId, _ => new ConcurrentDictionary<string, RemediationActionExecution>());
            var actionExecution = new RemediationActionExecution
            {
                ActionId = actionId,
                StartTime = DateTime.UtcNow,
                Status = RemediationActionStatus.InProgress
            };
            actionExecutions.AddOrUpdate(actionId, actionExecution, (_, _) => actionExecution);
        }

        public void TrackActionCompletion(string planId, string actionId, bool success, string? errorMessage = null)
        {
            if (_actionExecutions.TryGetValue(planId, out var actionExecutions) &&
                actionExecutions.TryGetValue(actionId, out var actionExecution))
            {
                actionExecution.EndTime = DateTime.UtcNow;
                actionExecution.Status = success ? RemediationActionStatus.Completed : RemediationActionStatus.Failed;
                if (errorMessage != null)
                {
                    actionExecution.ErrorMessage = errorMessage;
                }
            }
        }
    }
} 
