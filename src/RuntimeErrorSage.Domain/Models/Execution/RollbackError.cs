using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Execution
{
    /// <summary>
    /// Represents an error that occurred during a rollback operation.
    /// </summary>
    public class RollbackError
    {
        /// <summary>
        /// Gets or sets the unique identifier for this error.
        /// </summary>
        public string ErrorId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the step ID where the error occurred.
        /// </summary>
        public string StepId { get; set; }

        /// <summary>
        /// Gets or sets the action ID where the error occurred.
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the error details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the error occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets whether the error is critical.
        /// </summary>
        public bool IsCritical { get; set; }

        /// <summary>
        /// Gets or sets the exception that caused the error.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets any additional metadata for the error.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 
