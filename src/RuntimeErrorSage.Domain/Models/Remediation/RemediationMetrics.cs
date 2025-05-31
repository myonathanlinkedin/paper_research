using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Metrics;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents metrics for a remediation operation.
    /// </summary>
    public class RemediationMetrics
    {
        private long _totalExecutions;
        private long _successfulExecutions;
        private long _failedExecutions;
        private long _rollbackCount;
        private long _successfulRollbacks;
        private long _failedRollbacks;
        private TimeSpan _totalExecutionTime;
        private TimeSpan _totalRollbackTime;
        private readonly Dictionary<RemediationStatusEnum, long> _executionCountsByStatus = new();
        private readonly Dictionary<RemediationStatusEnum, long> _rollbackCountsByStatus = new();
        private readonly Dictionary<string, TimeSpan> _averageTimesByActionType = new();
        private readonly Dictionary<string, double> _successRatesByActionType = new();
        private readonly Dictionary<string, double> _rollbackRatesByActionType = new();
        private readonly List<long> _executionTimes = new();
        private readonly List<long> _memoryUsages = new();

        public long TotalExecutions => _totalExecutions;
        public long SuccessfulExecutions => _successfulExecutions;
        public long FailedExecutions => _failedExecutions;
        public long RollbackCount => _rollbackCount;
        public long SuccessfulRollbacks => _successfulRollbacks;
        public long FailedRollbacks => _failedRollbacks;
        public TimeSpan AverageExecutionTime => _totalExecutions > 0 ? TimeSpan.FromTicks(_totalExecutionTime.Ticks / _totalExecutions) : TimeSpan.Zero;
        public TimeSpan AverageRollbackTime => _rollbackCount > 0 ? TimeSpan.FromTicks(_totalRollbackTime.Ticks / _rollbackCount) : TimeSpan.Zero;
        public IReadOnlyDictionary<RemediationStatusEnum, long> ExecutionCountsByStatus => _executionCountsByStatus;
        public IReadOnlyDictionary<RemediationStatusEnum, long> RollbackCountsByStatus => _rollbackCountsByStatus;
        public IReadOnlyDictionary<string, TimeSpan> AverageTimesByActionType => _averageTimesByActionType;
        public IReadOnlyDictionary<string, double> SuccessRatesByActionType => _successRatesByActionType;
        public IReadOnlyDictionary<string, double> RollbackRatesByActionType => _rollbackRatesByActionType;

        public RemediationMetrics()
        {
            ExecutionId = Guid.NewGuid().ToString();
            Timestamp = DateTime.UtcNow;
            Values = new Dictionary<string, double>();
            Labels = new Dictionary<string, string>();
            EndResourceUsage = new MetricsResourceUsage();
            SuccessRate = 0.0;
            ErrorRate = 0.0;
            AverageExecutionTimeMs = 0;
            TotalRemediations = 0;
            SuccessfulRemediations = 0;
            FailedRemediations = 0;
            CancelledRemediations = 0;
            TimedOutRemediations = 0;
            ValidationFailedRemediations = 0;
            RetryCount = 0;
            Success = false;
            Error = string.Empty;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the execution.
        /// </summary>
        public string ExecutionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp of the metrics.
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Gets or sets the success rate of the remediation.
        /// </summary>
        public double SuccessRate { get; set; }
        
        /// <summary>
        /// Gets or sets the error rate of the remediation.
        /// </summary>
        public double ErrorRate { get; set; }
        
        /// <summary>
        /// Gets or sets the average execution time in milliseconds.
        /// </summary>
        public long AverageExecutionTimeMs { get; set; }
        
        /// <summary>
        /// Gets or sets the total number of remediations.
        /// </summary>
        public int TotalRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the number of successful remediations.
        /// </summary>
        public int SuccessfulRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the number of failed remediations.
        /// </summary>
        public int FailedRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the number of cancelled remediations.
        /// </summary>
        public int CancelledRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the number of timed out remediations.
        /// </summary>
        public int TimedOutRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the number of validation failed remediations.
        /// </summary>
        public int ValidationFailedRemediations { get; set; }
        
        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        public int RetryCount { get; set; }
        
        /// <summary>
        /// Gets or sets the metric values.
        /// </summary>
        public Dictionary<string, double> Values { get; set; } = new Dictionary<string, double>();
        
        /// <summary>
        /// Gets or sets the metric labels.
        /// </summary>
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
        
        /// <summary>
        /// Gets or sets the success flag.
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the end resource usage metrics.
        /// </summary>
        public MetricsResourceUsage EndResourceUsage { get; set; } = new MetricsResourceUsage();

        /// <summary>
        /// Gets or sets the metrics ID.
        /// </summary>
        public string MetricsId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the remediation ID.
        /// </summary>
        public string RemediationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start time of the remediation.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time of the remediation.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the metadata for this metrics object.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        public void IncrementExecutionCount(RemediationStatusEnum status)
        {
            _totalExecutions++;
            if (status == RemediationStatusEnum.Success)
            {
                _successfulExecutions++;
            }
            else if (status == RemediationStatusEnum.Failed)
            {
                _failedExecutions++;
            }

            if (!_executionCountsByStatus.ContainsKey(status))
            {
                _executionCountsByStatus[status] = 0;
            }
            _executionCountsByStatus[status]++;
        }

        public void IncrementRollbackCount(RemediationStatusEnum status)
        {
            _rollbackCount++;
            if (status == RemediationStatusEnum.RolledBack)
            {
                _successfulRollbacks++;
            }
            else if (status == RemediationStatusEnum.RollbackFailed)
            {
                _failedRollbacks++;
            }

            if (!_rollbackCountsByStatus.ContainsKey(status))
            {
                _rollbackCountsByStatus[status] = 0;
            }
            _rollbackCountsByStatus[status]++;
        }

        public void UpdateAverageExecutionTime(TimeSpan executionTime)
        {
            _totalExecutionTime += executionTime;
        }

        public void UpdateAverageRollbackTime(TimeSpan rollbackTime)
        {
            _totalRollbackTime += rollbackTime;
        }

        public void UpdateActionTypeMetrics(string actionType, TimeSpan executionTime, bool success, bool rolledBack)
        {
            if (!_averageTimesByActionType.ContainsKey(actionType))
            {
                _averageTimesByActionType[actionType] = TimeSpan.Zero;
                _successRatesByActionType[actionType] = 0;
                _rollbackRatesByActionType[actionType] = 0;
            }

            _averageTimesByActionType[actionType] = TimeSpan.FromTicks(
                (_averageTimesByActionType[actionType].Ticks + executionTime.Ticks) / 2);

            var totalExecutions = _executionCountsByStatus.GetValueOrDefault(RemediationStatusEnum.Completed, 0) +
                                _executionCountsByStatus.GetValueOrDefault(RemediationStatusEnum.Failed, 0);

            if (totalExecutions > 0)
            {
                _successRatesByActionType[actionType] = (double)_successfulExecutions / totalExecutions;
            }

            if (_rollbackCount > 0)
            {
                _rollbackRatesByActionType[actionType] = (double)_successfulRollbacks / _rollbackCount;
            }
        }

        public void UpdateResourceUsage()
        {
            EndResourceUsage = new MetricsResourceUsage
            {
                CpuUsage = 0.0,
                MemoryUsage = 0.0,
                DiskUsage = 0.0,
                NetworkUsage = 0.0
            };
        }

        public void Clear()
        {
            _totalExecutions = 0;
            _successfulExecutions = 0;
            _failedExecutions = 0;
            _rollbackCount = 0;
            _successfulRollbacks = 0;
            _failedRollbacks = 0;
            _totalExecutionTime = TimeSpan.Zero;
            _totalRollbackTime = TimeSpan.Zero;
            _executionCountsByStatus.Clear();
            _rollbackCountsByStatus.Clear();
            _averageTimesByActionType.Clear();
            _successRatesByActionType.Clear();
            _rollbackRatesByActionType.Clear();
        }

        public void TrackExecutionTime(double executionTime)
        {
            _executionTimes.Add((long)Math.Round(executionTime));
        }

        public void TrackMemoryUsage(double memoryUsage)
        {
            _memoryUsages.Add((long)Math.Round(memoryUsage));
        }
    }
} 
