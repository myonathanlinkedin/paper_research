using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Models.Execution
{
    /// <summary>
    /// Represents the status of a rollback operation.
    /// </summary>
    public class RollbackStatus
    {
        private readonly List<RollbackStep> _steps = new();
        private readonly List<RollbackError> _errors = new();
        private readonly Dictionary<string, object> _metadata = new();

        /// <summary>
        /// Gets or sets the rollback ID.
        /// </summary>
        public string RollbackId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets when the rollback started.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets when the rollback ended.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the rollback status.
        /// </summary>
        public RollbackExecutionStatus Status { get; set; }

        /// <summary>
        /// Gets the rollback steps.
        /// </summary>
        public IReadOnlyList<RollbackStep> Steps => _steps;

        /// <summary>
        /// Gets the rollback errors.
        /// </summary>
        public IReadOnlyList<RollbackError> Errors => _errors;

        /// <summary>
        /// Gets the rollback metadata.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata => _metadata;

        /// <summary>
        /// Gets or sets any error message.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets whether the rollback was successful.
        /// </summary>
        public bool IsSuccessful => Status == RollbackExecutionStatus.Completed && string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Gets the duration of the rollback in seconds.
        /// </summary>
        public double? DurationSeconds => EndTime.HasValue ? (EndTime.Value - StartTime).TotalSeconds : null;

        /// <summary>
        /// Adds a step to the rollback.
        /// </summary>
        public void AddStep(RollbackStep step) => _steps.Add(step);

        /// <summary>
        /// Adds an error to the rollback.
        /// </summary>
        public void AddError(RollbackError error) => _errors.Add(error);

        /// <summary>
        /// Adds metadata to the rollback.
        /// </summary>
        public void AddMetadata(string key, object value) => _metadata[key] = value;
    }
} 
