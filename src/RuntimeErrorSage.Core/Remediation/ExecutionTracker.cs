using System;
using System.Collections.Concurrent;
using RuntimeErrorSage.Model.Models.Execution;

namespace RuntimeErrorSage.Model.Remediation
{
    public class ExecutionTracker
    {
        private readonly ConcurrentDictionary<string, RemediationExecution> _executions = new();

        public RemediationExecution GetOrAddExecution(string remediationId)
        {
            return _executions.GetOrAdd(remediationId, _ => new RemediationExecution
            {
                CorrelationId = remediationId,
                StartTime = DateTime.UtcNow,
                Status = RemediationExecutionStatus.Unknown
            });
        }

        public void AddOrUpdateExecution(RemediationExecution execution)
        {
            _executions.AddOrUpdate(
                execution.CorrelationId,
                execution,
                (_, _) => execution);
        }

        public ConcurrentDictionary<string, RemediationExecution> GetAllExecutions()
        {
            return _executions;
        }
    }
} 
