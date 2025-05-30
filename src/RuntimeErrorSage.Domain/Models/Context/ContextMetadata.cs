using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Context
{
    /// <summary>
    /// Represents metadata for a runtime context.
    /// </summary>
    public class ContextMetadata
    {
        /// <summary>
        /// Gets or sets the context identifier.
        /// </summary>
        public string ContextId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the context type.
        /// </summary>
        public string ContextType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when this metadata was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last modification timestamp.
        /// </summary>
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        public string ApplicationName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the environment (e.g., Development, Staging, Production).
        /// </summary>
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional properties.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    }
} 
