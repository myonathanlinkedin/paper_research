using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Application.Remediation
{
    public class StepHistoryTracker
    {
        private readonly Dictionary<string, List<RemediationStep>> _stepHistory = new();

        public async Task RecordStepAsync(string remediationId, RemediationStep step)
        {
            if (!_stepHistory.ContainsKey(remediationId))
            {
                _stepHistory[remediationId] = new List<RemediationStep>();
            }
            _stepHistory[remediationId].Add(step);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<RemediationStep>> GetStepHistoryAsync(string remediationId)
        {
            if (_stepHistory.TryGetValue(remediationId, out var steps))
            {
                return steps.OrderBy(s => s.StartTime);
            }
            return Enumerable.Empty<RemediationStep>();
        }
    }
} 
