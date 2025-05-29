using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Validation;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents the result of a remediation operation.
    /// </summary>
    public class RemediationResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for this result.
        /// </summary>
        public string ResultId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets whether the remediation was successful.
        /// </summary>
        public bool IsSuccessful { get; }

        /// <summary>
        /// Gets or sets the error ID that was remediated.
        /// </summary>
        public string ErrorId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets a message describing the result.
        /// </summary>
        public string Message { get; } = string.Empty;

        /// <summary>
        /// Gets or sets an error message if the remediation failed.
        /// </summary>
        public string ErrorMessage { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the remediation was executed.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the duration of the remediation.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Gets or sets the status of the remediation.
        /// </summary>
        public RemediationStatusEnum Status { get; } = RemediationStatusEnum.Success;

        /// <summary>
        /// Gets or sets the rollback status.
        /// </summary>
        public Enums.RollbackStatus RollbackStatus { get; set; } = Enums.RollbackStatus.NotAttempted;

        /// <summary>
        /// Gets or sets additional metadata for the result.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets any validation messages associated with this result.
        /// </summary>
        public IReadOnlyCollection<ValidationMessages> ValidationMessages { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the remediation identifier.
        /// </summary>
        public string RemediationId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the success status.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets or sets the start time of the remediation operation.
        /// </summary>
        public DateTime StartTime { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time of the remediation operation.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the list of errors that occurred during remediation.
        /// </summary>
        public IReadOnlyCollection<Errors> Errors { get; } = new();

        /// <summary>
        /// Gets or sets the list of warnings that occurred during remediation.
        /// </summary>
        public IReadOnlyCollection<Warnings> Warnings { get; } = new();

        /// <summary>
        /// Gets or sets the metrics associated with the remediation.
        /// </summary>
        public Dictionary<string, object> Metrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the risk assessment.
        /// </summary>
        public RiskAssessment RiskAssessment { get; } = new();

        /// <summary>
        /// Gets or sets the unique identifier of the plan.
        /// </summary>
        public string PlanId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of remediation action results.
        /// </summary>
        public IReadOnlyCollection<Actions> Actions { get; } = new();

        /// <summary>
        /// Gets or sets the error that occurred during remediation.
        /// </summary>
        public string Error { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation results.
        /// </summary>
        public ValidationResult Validation { get; }

        /// <summary>
        /// Gets or sets the remediation timestamp.
        /// </summary>
        public DateTime RemediationTimestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the action taken.
        /// </summary>
        public string ActionTaken { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the remediation context.
        /// </summary>
        public ErrorContext Context { get; }

        /// <summary>
        /// Gets or sets the remediation plan.
        /// </summary>
        public RemediationPlan Plan { get; }

        /// <summary>
        /// Gets or sets the completed steps.
        /// </summary>
        public IReadOnlyCollection<CompletedSteps> CompletedSteps { get; } = new();

        /// <summary>
        /// Gets or sets the failed steps.
        /// </summary>
        public IReadOnlyCollection<FailedSteps> FailedSteps { get; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether this is a rollback operation.
        /// </summary>
        public bool IsRollback { get; }

        /// <summary>
        /// Gets or sets whether the remediation can be rolled back.
        /// </summary>
        public bool CanRollback { get; }

        /// <summary>
        /// Gets or sets the rollback plan if available.
        /// </summary>
        public RemediationPlan RollbackPlan { get; }

        /// <summary>
        /// Gets or sets the plan name.
        /// </summary>
        public string PlanName { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the original execution ID.
        /// </summary>
        public string OriginalExecutionId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the action ID.
        /// </summary>
        public string ActionId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the remediation is completed.
        /// </summary>
        public bool IsCompleted => Status == RemediationStatusEnum.Success || Status == RemediationStatusEnum.Failed;

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
                Status = RemediationStatusEnum.Success
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

        public RemediationResult(
            ErrorContext context,
            RemediationStatusEnum status,
            string message,
            string error)
        {
            ArgumentNullException.ThrowIfNull(context);

            Context = context;
            Status = status;
            Message = message;
            Error = error;
            Timestamp = DateTime.UtcNow;
            StartTime = DateTime.UtcNow;
        }

        public static RemediationResult CreateSuccess(ErrorContext context, string message = "Remediation completed successfully")
        {
            return new RemediationResult(context, RemediationStatusEnum.Success, message, string.Empty);
        }

        public static RemediationResult CreateFailure(ErrorContext context, string error, string message = "Remediation failed")
        {
            return new RemediationResult(context, RemediationStatusEnum.Failed, message, error);
        }
    }
} 






