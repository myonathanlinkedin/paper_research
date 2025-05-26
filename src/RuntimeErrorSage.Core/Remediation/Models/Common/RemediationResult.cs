using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Validation;
using RemediationResult = RuntimeErrorSage.Core.Models.Remediation.RemediationResult;
using RemediationImpact = RuntimeErrorSage.Core.Models.Remediation.RemediationImpact;

namespace RuntimeErrorSage.Core.Remediation.Models.Common
{
    /// <summary>
    /// Represents the result of a remediation operation.
    /// </summary>
    public class RemediationResult
    {
        /// <summary>
        /// Gets or sets the result identifier.
        /// </summary>
        public string ResultId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp when the result was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the result was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the remediation identifier.
        /// </summary>
        public string RemediationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the plan identifier.
        /// </summary>
        public string PlanId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the remediation was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the remediation status.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the remediation metrics.
        /// </summary>
        public RemediationMetrics Metrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the remediation validation results.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the remediation error message.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the remediation error code.
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the remediation error details.
        /// </summary>
        public string? ErrorDetails { get; set; }

        /// <summary>
        /// Gets or sets the remediation error stack trace.
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the remediation error inner exception.
        /// </summary>
        public Exception? InnerException { get; set; }

        /// <summary>
        /// Gets or sets the remediation start time.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the remediation end time.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the remediation duration in milliseconds.
        /// </summary>
        public double DurationMs { get; set; }

        /// <summary>
        /// Gets or sets the remediation retry count.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the remediation timeout in minutes.
        /// </summary>
        public int TimeoutMinutes { get; set; }

        /// <summary>
        /// Gets or sets the remediation tags.
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// Gets or sets the remediation metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the remediation owner.
        /// </summary>
        public string? Owner { get; set; }

        /// <summary>
        /// Gets or sets the remediation assignee.
        /// </summary>
        public string? Assignee { get; set; }

        /// <summary>
        /// Gets or sets the remediation reviewer.
        /// </summary>
        public string? Reviewer { get; set; }

        /// <summary>
        /// Gets or sets the remediation approver.
        /// </summary>
        public string? Approver { get; set; }

        /// <summary>
        /// Gets or sets the remediation status.
        /// </summary>
        public enum RemediationStatus
        {
            /// <summary>
            /// The remediation has been created.
            /// </summary>
            Created,

            /// <summary>
            /// The remediation is being validated.
            /// </summary>
            Validating,

            /// <summary>
            /// The remediation has been validated.
            /// </summary>
            Validated,

            /// <summary>
            /// The remediation validation has failed.
            /// </summary>
            ValidationFailed,

            /// <summary>
            /// The remediation is being approved.
            /// </summary>
            Approving,

            /// <summary>
            /// The remediation has been approved.
            /// </summary>
            Approved,

            /// <summary>
            /// The remediation approval has failed.
            /// </summary>
            ApprovalFailed,

            /// <summary>
            /// The remediation is being scheduled.
            /// </summary>
            Scheduling,

            /// <summary>
            /// The remediation has been scheduled.
            /// </summary>
            Scheduled,

            /// <summary>
            /// The remediation scheduling has failed.
            /// </summary>
            SchedulingFailed,

            /// <summary>
            /// The remediation is being executed.
            /// </summary>
            Executing,

            /// <summary>
            /// The remediation has been executed.
            /// </summary>
            Executed,

            /// <summary>
            /// The remediation execution has failed.
            /// </summary>
            ExecutionFailed,

            /// <summary>
            /// The remediation is being rolled back.
            /// </summary>
            RollingBack,

            /// <summary>
            /// The remediation has been rolled back.
            /// </summary>
            RolledBack,

            /// <summary>
            /// The remediation rollback has failed.
            /// </summary>
            RollbackFailed,

            /// <summary>
            /// The remediation is being cancelled.
            /// </summary>
            Cancelling,

            /// <summary>
            /// The remediation has been cancelled.
            /// </summary>
            Cancelled,

            /// <summary>
            /// The remediation cancellation has failed.
            /// </summary>
            CancellationFailed,

            /// <summary>
            /// The remediation is being paused.
            /// </summary>
            Pausing,

            /// <summary>
            /// The remediation has been paused.
            /// </summary>
            Paused,

            /// <summary>
            /// The remediation pause has failed.
            /// </summary>
            PauseFailed,

            /// <summary>
            /// The remediation is being resumed.
            /// </summary>
            Resuming,

            /// <summary>
            /// The remediation has been resumed.
            /// </summary>
            Resumed,

            /// <summary>
            /// The remediation resume has failed.
            /// </summary>
            ResumeFailed,

            /// <summary>
            /// The remediation is being completed.
            /// </summary>
            Completing,

            /// <summary>
            /// The remediation has been completed.
            /// </summary>
            Completed,

            /// <summary>
            /// The remediation completion has failed.
            /// </summary>
            CompletionFailed,

            /// <summary>
            /// The remediation is unknown.
            /// </summary>
            Unknown
        }
    }
} 