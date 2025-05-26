using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Models.Execution
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
        /// Gets or sets the unique identifier of the remediation execution.
        /// </summary>
        public string ExecutionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation ID of the error context.
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
        public RemediationExecutionStatus Status { get; set; }

        /// <summary>
        /// Gets the list of actions that were executed.
        /// </summary>
        public IReadOnlyList<RemediationActionExecution> ExecutedActions => _executedActions;

        /// <summary>
        /// Gets or sets any error that occurred during remediation.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets the additional execution metadata.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata => _metadata;

        /// <summary>
        /// Gets whether the remediation was successful.
        /// </summary>
        public bool IsSuccessful => Status == RemediationExecutionStatus.Completed && string.IsNullOrEmpty(Error);

        /// <summary>
        /// Gets the total duration of the remediation in seconds.
        /// </summary>
        public double? DurationSeconds => EndTime.HasValue ? (EndTime.Value - StartTime).TotalSeconds : null;

        /// <summary>
        /// Gets or sets the metrics for this remediation execution.
        /// </summary>
        public RemediationMetrics? Metrics { get; set; }

        /// <summary>
        /// Gets the validation results for this remediation execution.
        /// </summary>
        public IReadOnlyList<ValidationResult> ValidationResults => _validationResults;

        /// <summary>
        /// Gets or sets the rollback status if the execution was rolled back.
        /// </summary>
        public RollbackStatus? RollbackStatus { get; set; }

        /// <summary>
        /// Gets or sets the validation result.
        /// </summary>
        public ValidationResult? Validation { get; set; }

        /// <summary>
        /// Adds an executed action to the collection.
        /// </summary>
        public void AddExecutedAction(RemediationActionExecution action) => _executedActions.Add(action);

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
