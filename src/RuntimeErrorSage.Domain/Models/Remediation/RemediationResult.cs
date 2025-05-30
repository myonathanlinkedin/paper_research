using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents the result of a remediation operation.
    /// </summary>
    public class RemediationResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for this result.
        /// </summary>
        public string ResultId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the execution identifier for this result.
        /// </summary>
        public string ExecutionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets whether the remediation was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the error ID that was remediated.
        /// </summary>
        public string ErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a message describing the result.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an error message if the remediation failed.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the remediation was executed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the duration of the remediation.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the status of the remediation.
        /// </summary>
        public RemediationStatusEnum Status { get; set; } = RemediationStatusEnum.Success;

        /// <summary>
        /// Gets or sets the rollback status.
        /// </summary>
        public Domain.Enums.RollbackStatusEnum RollbackStatus { get; set; } = Domain.Enums.RollbackStatusEnum.NotAttempted;

        /// <summary>
        /// Gets or sets additional metadata for the result.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets any validation messages associated with this result.
        /// </summary>
        public List<string> ValidationMessages { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the remediation identifier.
        /// </summary>
        public string RemediationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the success status.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the start time of the remediation operation.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time of the remediation operation.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the list of errors that occurred during remediation.
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of warnings that occurred during remediation.
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Gets or sets the metrics associated with the remediation.
        /// </summary>
        public Dictionary<string, object> Metrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the risk assessment.
        /// </summary>
        public RiskAssessmentModel RiskAssessment { get; set; } = new();

        /// <summary>
        /// Gets or sets the unique identifier of the plan.
        /// </summary>
        public string PlanId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of remediation action results.
        /// </summary>
        public List<RemediationActionResult> Actions { get; set; } = new();

        /// <summary>
        /// Gets or sets the error that occurred during remediation.
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the exception that caused the error.
        /// </summary>
        public Exception Exception { get; set; }

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
        /// Gets or sets whether the remediation is completed.
        /// </summary>
        public bool IsCompleted => Status == RemediationStatusEnum.Success || Status == RemediationStatusEnum.Failed;

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public string StackTrace { get; set; } = string.Empty;

        /// <summary>
        /// Creates a successful remediation result with a message.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <returns>A successful remediation result.</returns>
        public static RemediationResult CreateSuccessResult(string message)
        {
            return new RemediationResult(new ErrorContext(null, string.Empty, DateTime.UtcNow), RemediationStatusEnum.Success, message, string.Empty);
        }

        /// <summary>
        /// Creates a failed remediation result.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>A failed remediation result.</returns>
        public static RemediationResult Failure(string error)
        {
            return new RemediationResult(new ErrorContext(null, string.Empty, DateTime.UtcNow), RemediationStatusEnum.Failed, error, string.Empty);
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RemediationResult()
        {
            Context = new ErrorContext(null, string.Empty, DateTime.UtcNow);
            Status = RemediationStatusEnum.Pending;
        }

        /// <summary>
        /// Constructor with success parameter.
        /// </summary>
        /// <param name="success">Whether the remediation was successful.</param>
        public RemediationResult(bool success)
        {
            Context = new ErrorContext(null, string.Empty, DateTime.UtcNow);
            Status = success ? RemediationStatusEnum.Success : RemediationStatusEnum.Failed;
            IsSuccessful = success;
            Success = success;
        }

        /// <summary>
        /// Sets the success status of this remediation result.
        /// </summary>
        /// <param name="success">Whether the remediation was successful.</param>
        public void SetSuccess(bool success)
        {
            IsSuccessful = success;
            Success = success;
            Status = success ? RemediationStatusEnum.Success : RemediationStatusEnum.Failed;
        }

        /// <summary>
        /// Checks if the remediation was successful.
        /// </summary>
        /// <returns>True if the remediation was successful, false otherwise.</returns>
        public bool WasSuccessful()
        {
            return Status == RemediationStatusEnum.Success;
        }

        public RemediationResult(ErrorContext context, RemediationStatusEnum status, string errorMessage, string stackTrace)
        {
            Context = context;
            Status = status;
            Error = errorMessage;
            this.StackTrace = stackTrace;
        }

        public static RemediationResult CreateSuccess(ErrorContext context, string message = "Remediation completed successfully")
        {
            return new RemediationResult(context, RemediationStatusEnum.Success, message, string.Empty);
        }

        public static RemediationResult CreateFailure(ErrorContext context, string errorMessage, string details = "")
        {
            return new RemediationResult(context, RemediationStatusEnum.Failed, errorMessage, details);
        }

        public static RemediationResult CreateInProgress(ErrorContext context, string message = null)
        {
            return new RemediationResult(context, RemediationStatusEnum.InProgress, message ?? "Remediation in progress", null);
        }

        public static RemediationResult CreatePending(ErrorContext context, string message = null)
        {
            return new RemediationResult(context, RemediationStatusEnum.Pending, message ?? "Remediation pending", null);
        }

        public static RemediationResult CreateRolledBack(ErrorContext context, string message = null, string details = null)
        {
            return new RemediationResult(context, RemediationStatusEnum.RolledBack, message ?? "Remediation rolled back", details);
        }

        public static RemediationResult CreateRollbackFailed(ErrorContext context, string errorMessage, string details = null)
        {
            return new RemediationResult(context, RemediationStatusEnum.RollbackFailed, errorMessage, details);
        }
    }
} 
