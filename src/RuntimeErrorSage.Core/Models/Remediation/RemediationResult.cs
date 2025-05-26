using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the result of a remediation operation.
    /// </summary>
    public class RemediationResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the result.
        /// </summary>
        public string ResultId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the remediation action that was executed.
        /// </summary>
        public RemediationAction Action { get; set; }

        /// <summary>
        /// Gets or sets whether the remediation was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the status of the remediation.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the start time of the remediation.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the remediation.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the remediation in milliseconds.
        /// </summary>
        public double DurationMs => EndTime.HasValue ? (EndTime.Value - StartTime).TotalMilliseconds : 0;

        /// <summary>
        /// Gets or sets the error message if the remediation failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the exception that occurred during remediation.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the validation results for the remediation.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the metrics collected during remediation.
        /// </summary>
        public RemediationMetrics Metrics { get; set; }

        /// <summary>
        /// Gets or sets the rollback result if the remediation was rolled back.
        /// </summary>
        public RemediationResult RollbackResult { get; set; }

        /// <summary>
        /// Gets or sets the number of retries attempted.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets whether the remediation was rolled back.
        /// </summary>
        public bool WasRolledBack { get; set; }

        /// <summary>
        /// Gets or sets the impact of the remediation.
        /// </summary>
        public RemediationImpact Impact { get; set; }

        /// <summary>
        /// Gets or sets additional metadata about the remediation.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Represents the impact of a remediation operation.
    /// </summary>
    public class RemediationImpact
    {
        /// <summary>
        /// Gets or sets the scope of the impact.
        /// </summary>
        public RemediationActionImpactScope Scope { get; set; }

        /// <summary>
        /// Gets or sets the severity of the impact.
        /// </summary>
        public RemediationActionSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the affected components.
        /// </summary>
        public List<string> AffectedComponents { get; set; } = new();

        /// <summary>
        /// Gets or sets the affected services.
        /// </summary>
        public List<string> AffectedServices { get; set; } = new();

        /// <summary>
        /// Gets or sets the affected operations.
        /// </summary>
        public List<string> AffectedOperations { get; set; } = new();

        /// <summary>
        /// Gets or sets the affected users.
        /// </summary>
        public List<string> AffectedUsers { get; set; } = new();

        /// <summary>
        /// Gets or sets the estimated downtime in milliseconds.
        /// </summary>
        public double EstimatedDowntimeMs { get; set; }

        /// <summary>
        /// Gets or sets the estimated recovery time in milliseconds.
        /// </summary>
        public double EstimatedRecoveryTimeMs { get; set; }

        /// <summary>
        /// Gets or sets the risk level of the impact.
        /// </summary>
        public RemediationActionPriority RiskLevel { get; set; }

        /// <summary>
        /// Gets or sets additional impact details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();
    }
} 