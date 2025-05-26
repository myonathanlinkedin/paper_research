using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the rollback status of a remediation step.
    /// </summary>
    public class StepRollbackStatus
    {
        /// <summary>
        /// Gets or sets whether the rollback was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the start time of the rollback.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the rollback.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets the duration of the rollback in seconds.
        /// </summary>
        public double? DurationSeconds => EndTime.HasValue ? (EndTime.Value - StartTime).TotalSeconds : null;

        /// <summary>
        /// Gets or sets any error message if the rollback failed.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the rollback actions performed.
        /// </summary>
        public List<string> RollbackActions { get; set; } = new();

        /// <summary>
        /// Gets or sets any failed rollback actions.
        /// </summary>
        public List<string> FailedActions { get; set; } = new();
    }
} 