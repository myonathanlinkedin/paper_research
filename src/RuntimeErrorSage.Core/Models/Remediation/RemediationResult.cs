using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the result of a remediation operation.
    /// </summary>
    public class RemediationResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for the result.
        /// </summary>
        public string ResultId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the remediation identifier.
        /// </summary>
        public string RemediationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error ID.
        /// </summary>
        public string ErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the remediation was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the success status.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the result was created.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the start time of the remediation operation.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time of the remediation operation.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the remediation operation.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the list of errors that occurred during remediation.
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of warnings that occurred during remediation.
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Gets or sets the metadata associated with the result.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the metrics associated with the remediation.
        /// </summary>
        public Dictionary<string, object> Metrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the risk assessment.
        /// </summary>
        public RiskAssessment RiskAssessment { get; set; } = new();

        /// <summary>
        /// Gets or sets the unique identifier of the plan.
        /// </summary>
        public string PlanId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of remediation action results.
        /// </summary>
        public List<RemediationActionResult> Actions { get; set; } = new();

        /// <summary>
        /// Gets or sets the status of the remediation.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the error that occurred during remediation.
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation results.
        /// </summary>
        public ValidationResult Validation { get; set; }

        /// <summary>
        /// Gets or sets the remediation timestamp.
        /// </summary>
        public DateTime RemediationTimestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the action taken.
        /// </summary>
        public string ActionTaken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the remediation context.
        /// </summary>
        public ErrorContext Context { get; set; }

        /// <summary>
        /// Gets or sets the remediation plan.
        /// </summary>
        public RemediationPlan Plan { get; set; }

        /// <summary>
        /// Gets or sets the completed steps.
        /// </summary>
        public List<RemediationStep> CompletedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the failed steps.
        /// </summary>
        public List<RemediationStep> FailedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the rollback status.
        /// </summary>
        public RollbackStatus? RollbackStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a rollback operation.
        /// </summary>
        public bool IsRollback { get; set; }

        /// <summary>
        /// Gets or sets whether the remediation can be rolled back.
        /// </summary>
        public bool CanRollback { get; set; }

        /// <summary>
        /// Gets or sets the rollback plan if available.
        /// </summary>
        public RemediationPlan RollbackPlan { get; set; }

        /// <summary>
        /// Gets or sets the plan name.
        /// </summary>
        public string PlanName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the original execution ID.
        /// </summary>
        public string OriginalExecutionId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action ID.
        /// </summary>
        public string ActionId { get; set; } = string.Empty;

        /// <summary>
        /// Creates a successful remediation result.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <returns>A successful remediation result.</returns>
        public static RemediationResult CreateSuccessResult(string message)
        {
            return new RemediationResult
            {
                Success = true,
                IsSuccessful = true,
                Message = message,
                Status = RemediationStatusEnum.Completed
            };
        }

        /// <summary>
        /// Creates a failed remediation result.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>A failed remediation result.</returns>
        public static RemediationResult Failure(string error)
        {
            return new RemediationResult
            {
                Success = false,
                IsSuccessful = false,
                ErrorMessage = error,
                Error = error,
                Status = RemediationStatusEnum.Failed
            };
        }
    }
} 