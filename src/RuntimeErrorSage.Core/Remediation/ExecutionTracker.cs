using System;
using System.Collections.Concurrent;
using RuntimeErrorSage.Domain.Models.Execution;

namespace RuntimeErrorSage.Application.Remediation
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
