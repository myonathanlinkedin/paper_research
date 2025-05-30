using System;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents an entry in the status history.
    /// </summary>
    public class StatusHistoryEntry
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public RemediationState State { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the progress percentage (0-100).
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// Creates a new history entry with the specified state and message.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="message">The message.</param>
        /// <param name="progress">The progress percentage.</param>
        /// <returns>A new history entry.</returns>
        public static StatusHistoryEntry Create(RemediationState state, string message = null, double progress = 0)
        {
            return new StatusHistoryEntry
            {
                State = state,
                Message = message ?? string.Empty,
                Timestamp = DateTime.UtcNow,
                Progress = progress
            };
        }
    }
} 
