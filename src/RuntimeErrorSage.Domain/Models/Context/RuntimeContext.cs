using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Context
{
    /// <summary>
    /// Represents a runtime context for error analysis.
    /// </summary>
    public class RuntimeContext
    {
        /// <summary>
        /// Gets or sets the unique identifier for this context.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the context metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public string StackTrace { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;
    }
} 
