using System;
using System.Collections.Concurrent;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Remediation
{
    public class MetricsTracker
    {
        private readonly ConcurrentDictionary<string, RemediationMetrics> _metrics = new();

        public RemediationMetrics GetOrAddMetrics(string remediationId)
        {
            return _metrics.GetOrAdd(remediationId, _ => new RemediationMetrics
            {
                MetricsId = Guid.NewGuid().ToString(),
                RemediationId = remediationId,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow
            });
        }

        public void AddOrUpdateMetrics(string planId, RemediationMetrics metrics)
        {
            _metrics.AddOrUpdate(
                planId,
                metrics,
                (_, _) => metrics);
        }

        public ConcurrentDictionary<string, RemediationMetrics> GetAllMetrics()
        {
            return _metrics;
        }
    }
} 
