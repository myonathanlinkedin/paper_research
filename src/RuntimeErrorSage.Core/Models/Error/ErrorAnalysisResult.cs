using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents the result of an error analysis.
    /// </summary>
    public class ErrorAnalysisResult
    {
        /// <summary>
        /// Gets or sets the analysis identifier.
        /// </summary>
        public string AnalysisId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp when the analysis was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the analysis was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the error identifier.
        /// </summary>
        public string ErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error context identifier.
        /// </summary>
        public string ContextId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the analysis status.
        /// </summary>
        public AnalysisStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the analysis score.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Gets or sets the analysis confidence.
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the analysis threshold.
        /// </summary>
        public double Threshold { get; set; }

        /// <summary>
        /// Gets or sets the analysis duration in milliseconds.
        /// </summary>
        public double DurationMs { get; set; }

        /// <summary>
        /// Gets or sets the analysis error message.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the analysis error code.
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the analysis error details.
        /// </summary>
        public string? ErrorDetails { get; set; }

        /// <summary>
        /// Gets or sets the analysis error stack trace.
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the analysis error inner exception.
        /// </summary>
        public Exception? InnerException { get; set; }

        /// <summary>
        /// Gets or sets the analysis tags.
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis owner.
        /// </summary>
        public string? Owner { get; set; }

        /// <summary>
        /// Gets or sets the analysis assignee.
        /// </summary>
        public string? Assignee { get; set; }

        /// <summary>
        /// Gets or sets the analysis reviewer.
        /// </summary>
        public string? Reviewer { get; set; }

        /// <summary>
        /// Gets or sets the analysis approver.
        /// </summary>
        public string? Approver { get; set; }

        /// <summary>
        /// Gets or sets the analysis resource usage.
        /// </summary>
        public ResourceUsage ResourceUsage { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis validation results.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis remediation results.
        /// </summary>
        public List<RemediationResult> RemediationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis connection status.
        /// </summary>
        public ConnectionStatus ConnectionStatus { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis context history.
        /// </summary>
        public List<AnalysisContextHistory> ContextHistory { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis status.
        /// </summary>
        public enum AnalysisStatus
        {
            /// <summary>
            /// The analysis has been created.
            /// </summary>
            Created,

            /// <summary>
            /// The analysis is being validated.
            /// </summary>
            Validating,

            /// <summary>
            /// The analysis has been validated.
            /// </summary>
            Validated,

            /// <summary>
            /// The analysis validation has failed.
            /// </summary>
            ValidationFailed,

            /// <summary>
            /// The analysis is being approved.
            /// </summary>
            Approving,

            /// <summary>
            /// The analysis has been approved.
            /// </summary>
            Approved,

            /// <summary>
            /// The analysis approval has failed.
            /// </summary>
            ApprovalFailed,

            /// <summary>
            /// The analysis is being scheduled.
            /// </summary>
            Scheduling,

            /// <summary>
            /// The analysis has been scheduled.
            /// </summary>
            Scheduled,

            /// <summary>
            /// The analysis scheduling has failed.
            /// </summary>
            SchedulingFailed,

            /// <summary>
            /// The analysis is being executed.
            /// </summary>
            Executing,

            /// <summary>
            /// The analysis has been executed.
            /// </summary>
            Executed,

            /// <summary>
            /// The analysis execution has failed.
            /// </summary>
            ExecutionFailed,

            /// <summary>
            /// The analysis is being rolled back.
            /// </summary>
            RollingBack,

            /// <summary>
            /// The analysis has been rolled back.
            /// </summary>
            RolledBack,

            /// <summary>
            /// The analysis rollback has failed.
            /// </summary>
            RollbackFailed,

            /// <summary>
            /// The analysis is being cancelled.
            /// </summary>
            Cancelling,

            /// <summary>
            /// The analysis has been cancelled.
            /// </summary>
            Cancelled,

            /// <summary>
            /// The analysis cancellation has failed.
            /// </summary>
            CancellationFailed,

            /// <summary>
            /// The analysis is being paused.
            /// </summary>
            Pausing,

            /// <summary>
            /// The analysis has been paused.
            /// </summary>
            Paused,

            /// <summary>
            /// The analysis pause has failed.
            /// </summary>
            PauseFailed,

            /// <summary>
            /// The analysis is being resumed.
            /// </summary>
            Resuming,

            /// <summary>
            /// The analysis has been resumed.
            /// </summary>
            Resumed,

            /// <summary>
            /// The analysis resume has failed.
            /// </summary>
            ResumeFailed,

            /// <summary>
            /// The analysis is being completed.
            /// </summary>
            Completing,

            /// <summary>
            /// The analysis has been completed.
            /// </summary>
            Completed,

            /// <summary>
            /// The analysis completion has failed.
            /// </summary>
            CompletionFailed,

            /// <summary>
            /// The analysis is unknown.
            /// </summary>
            Unknown
        }

        /// <summary>
        /// Gets or sets the analysis context history.
        /// </summary>
        public class AnalysisContextHistory
        {
            /// <summary>
            /// Gets or sets the history identifier.
            /// </summary>
            public string HistoryId { get; set; } = Guid.NewGuid().ToString();

            /// <summary>
            /// Gets or sets the history timestamp.
            /// </summary>
            public DateTime Timestamp { get; set; }

            /// <summary>
            /// Gets or sets the history action.
            /// </summary>
            public string Action { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the history details.
            /// </summary>
            public string? Details { get; set; }

            /// <summary>
            /// Gets or sets the history user.
            /// </summary>
            public string? User { get; set; }

            /// <summary>
            /// Gets or sets the history tags.
            /// </summary>
            public List<string> Tags { get; set; } = new();

            /// <summary>
            /// Gets or sets the history metadata.
            /// </summary>
            public Dictionary<string, object> Metadata { get; set; } = new();
        }
    }
} 