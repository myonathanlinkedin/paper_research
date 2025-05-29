using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Metrics;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Models.Execution
{
    /// <summary>
    /// Represents the execution status and details of a remediation action.
    /// </summary>
    public class RemediationExecution
    {
        private readonly Collection<RemediationActionExecution> _executedActions = new();
        private readonly Collection<ValidationResult> _validationResults = new();
        private readonly Dictionary<string, object> _metadata = new();

        /// <summary>
        /// Gets or sets the unique identifier for this execution.
        /// </summary>
        public string ExecutionId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the remediation ID.
        /// </summary>
        public string RemediationId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the error ID.
        /// </summary>
        public string ErrorId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the remediation was started.
        /// </summary>
        public DateTime StartTime { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the remediation was completed.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the current status of the remediation execution.
        /// </summary>
        public RemediationStatusEnum Status { get; } = RemediationStatusEnum.NotStarted;

        /// <summary>
        /// Gets or sets whether the execution was successful.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets or sets any error message if execution failed.
        /// </summary>
        public string ErrorMessage { get; } = string.Empty;

        /// <summary>
        /// Gets the list of actions that were executed.
        /// </summary>
        public IReadOnlyCollection<RemediationActionExecution> ExecutedActions => _executedActions;

        /// <summary>
        /// Gets or sets any error that occurred during remediation.
        /// </summary>
        public string Error { get; }

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
        public RemediationMetrics Metrics { get; }

        /// <summary>
        /// Gets the validation results for this remediation execution.
        /// </summary>
        public IReadOnlyCollection<ValidationResult> ValidationResults => _validationResults;

        /// <summary>
        /// Gets or sets the rollback status if the execution was rolled back.
        /// </summary>
        public RollbackExecutionDetails? RollbackDetails { get; set; }

        /// <summary>
        /// Gets or sets the rollback status.
        /// </summary>
        public RollbackStatus? RollbackStatus { get; set; }

        /// <summary>
        /// Gets or sets the validation result.
        /// </summary>
        public ValidationResult Validation { get; }

        /// <summary>
        /// Gets or sets the list of executed steps.
        /// </summary>
        public IReadOnlyCollection<ExecutedSteps> ExecutedSteps { get; } = new();

        /// <summary>
        /// Gets or sets the list of failed steps.
        /// </summary>
        public IReadOnlyCollection<FailedSteps> FailedSteps { get; } = new();

        /// <summary>
        /// Gets or sets the list of actions executed during remediation.
        /// </summary>
        public IReadOnlyCollection<Actions> Actions { get; } = new Collection<RemediationActionExecution>();

        /// <summary>
        /// Adds an executed action to the collection.
        /// </summary>
        public RemediationActionExecution action { ArgumentNullException.ThrowIfNull(RemediationActionExecution action); }
        {
            if (action == null)
            {
                ArgumentNullException.ThrowIfNull(nameof(action));
            }

            _executedActions.Add(action);
        }

        /// <summary>
        /// Adds a validation result to the collection.
        /// </summary>
        public ValidationResult result { ArgumentNullException.ThrowIfNull(ValidationResult result); } => _validationResults.Add(result);

        /// <summary>
        /// Adds metadata to the collection.
        /// </summary>
        public string key, object value { ArgumentNullException.ThrowIfNull(string key, object value); } => _metadata[key] = value;
    }
}







