using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Metrics;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RollbackStatus = RuntimeErrorSage.Domain.Models.Remediation.RollbackStatus;

namespace RuntimeErrorSage.Domain.Models.Execution
{
    /// <summary>
    /// Represents the execution status and details of a remediation action.
    /// </summary>
    public class RemediationExecution
    {
        private readonly List<RemediationActionExecution> _executedActions = new();
        private readonly List<ValidationResult> _validationResults = new();
        private readonly Dictionary<string, object> _metadata = new();

        /// <summary>
        /// Gets or sets the unique identifier for this execution.
        /// </summary>
        public string ExecutionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the remediation ID.
        /// </summary>
        public string RemediationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error ID.
        /// </summary>
        public string ErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the remediation was started.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the remediation was completed.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the current status of the remediation execution.
        /// </summary>
        public RemediationStatusEnum Status { get; set; } = RemediationStatusEnum.NotStarted;

        /// <summary>
        /// Gets or sets whether the execution was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets any error message if execution failed.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets the list of actions that were executed.
        /// </summary>
        public IReadOnlyList<RemediationActionExecution> ExecutedActions => _executedActions;

        /// <summary>
        /// Gets or sets any error that occurred during remediation.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets the additional execution metadata.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata => _metadata;

        /// <summary>
        /// Gets whether the remediation was successful.
        /// </summary>
        public bool IsSuccessful => Status == RemediationStatusEnum.Success && string.IsNullOrEmpty(Error);

        /// <summary>
        /// Gets the total duration of the remediation in seconds.
        /// </summary>
        public double? DurationSeconds => EndTime.HasValue ? (EndTime.Value - StartTime).TotalSeconds : null;

        /// <summary>
        /// Gets or sets the metrics for this remediation execution.
        /// </summary>
        public RemediationMetrics Metrics { get; set; }

        /// <summary>
        /// Gets the validation results for this remediation execution.
        /// </summary>
        public IReadOnlyList<ValidationResult> ValidationResults => _validationResults;

        /// <summary>
        /// Gets or sets the rollback status if the execution was rolled back.
        /// </summary>
        public RollbackExecutionDetails? RollbackDetails { get; set; }

        /// <summary>
        /// Gets or sets the rollback status.
        /// </summary>
        public RollbackStatus RollbackStatus { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation result.
        /// </summary>
        public ValidationResult Validation { get; set; }

        /// <summary>
        /// Gets or sets the list of executed steps.
        /// </summary>
        public List<string> ExecutedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of failed steps.
        /// </summary>
        public List<string> FailedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of actions executed during remediation.
        /// </summary>
        public List<RemediationActionExecution> Actions { get; set; } = new List<RemediationActionExecution>();

        /// <summary>
        /// Adds an executed action to the collection.
        /// </summary>
        public void AddExecutedAction(RemediationActionExecution action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _executedActions.Add(action);
        }

        /// <summary>
        /// Adds a validation result to the collection.
        /// </summary>
        public void AddValidationResult(ValidationResult result) => _validationResults.Add(result);

        /// <summary>
        /// Adds metadata to the collection.
        /// </summary>
        public void AddMetadata(string key, object value) => _metadata[key] = value;
    }
}

