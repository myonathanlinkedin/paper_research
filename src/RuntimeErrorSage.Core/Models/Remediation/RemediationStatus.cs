using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the status of a remediation operation.
    /// </summary>
    public class RemediationStatus
    {
        /// <summary>
        /// Gets or sets the state of the remediation.
        /// </summary>
        public RemediationState State { get; set; } = RemediationState.NotStarted;

        /// <summary>
        /// Gets or sets the message describing the status.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets when the status was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the progress percentage (0-100).
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// Gets or sets any error details.
        /// </summary>
        public string ErrorDetails { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any warnings.
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Gets or sets the status history.
        /// </summary>
        public List<StatusHistoryEntry> History { get; set; } = new();

        /// <summary>
        /// Gets or sets any additional status metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Creates a new status with the specified state and message.
        /// </summary>
        /// <param name="state">The remediation state.</param>
        /// <param name="message">The status message.</param>
        /// <returns>A new status instance.</returns>
        public static RemediationStatus Create(RemediationState state, string message = null)
        {
            return new RemediationStatus
            {
                State = state,
                Message = message ?? string.Empty,
                LastUpdated = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a new status with the NotStarted state.
        /// </summary>
        /// <param name="message">The status message.</param>
        /// <returns>A new status instance.</returns>
        public static RemediationStatus NotStarted(string message = null)
        {
            return Create(RemediationState.NotStarted, message);
        }

        /// <summary>
        /// Creates a new status with the InProgress state.
        /// </summary>
        /// <param name="message">The status message.</param>
        /// <param name="progress">The progress percentage (0-100).</param>
        /// <returns>A new status instance.</returns>
        public static RemediationStatus InProgress(string message = null, double progress = 0)
        {
            var status = Create(RemediationState.InProgress, message);
            status.Progress = progress;
            return status;
        }

        /// <summary>
        /// Creates a new status with the Completed state.
        /// </summary>
        /// <param name="message">The status message.</param>
        /// <returns>A new status instance.</returns>
        public static RemediationStatus Completed(string message = null)
        {
            var status = Create(RemediationState.Completed, message);
            status.Progress = 100;
            return status;
        }

        /// <summary>
        /// Creates a new status with the Failed state.
        /// </summary>
        /// <param name="errorDetails">The error details.</param>
        /// <param name="message">The status message.</param>
        /// <returns>A new status instance.</returns>
        public static RemediationStatus Failed(string errorDetails, string message = null)
        {
            var status = Create(RemediationState.Failed, message ?? "Remediation failed");
            status.ErrorDetails = errorDetails ?? string.Empty;
            return status;
        }
    }
} 
