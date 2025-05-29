using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Models.Context
{
    /// <summary>
    /// Represents metadata for a runtime context.
    /// </summary>
    public class ContextMetadata
    {
        /// <summary>
        /// Gets or sets the unique identifier for this metadata.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp when this metadata was created.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the name of the context.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the context.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the context.
        /// </summary>
        public string ContextType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source of the context.
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional properties for this metadata.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the tags for this metadata.
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the correlation ID for tracing.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether this context is persistent.
        /// </summary>
        public bool IsPersistent { get; set; }

        /// <summary>
        /// Gets or sets the expiration time for this metadata.
        /// </summary>
        public DateTime? ExpiresAt { get; set; }
    }
} 
