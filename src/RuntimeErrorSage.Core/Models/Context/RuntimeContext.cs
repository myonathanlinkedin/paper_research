using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Context
{
    /// <summary>
    /// Represents the runtime context of an application.
    /// </summary>
    public class RuntimeContext
    {
        /// <summary>
        /// Gets or sets the unique identifier for this context.
        /// </summary>
        public string ContextId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        public string ApplicationName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the environment name (e.g., Development, Staging, Production).
        /// </summary>
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the runtime version.
        /// </summary>
        public string RuntimeVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the host information.
        /// </summary>
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when this context was created.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets additional metadata for this context.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the callstack information.
        /// </summary>
        public string Callstack { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source code information.
        /// </summary>
        public string SourceCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the variable state.
        /// </summary>
        public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the correlation ID for tracing.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the runtime context provider.
        /// </summary>
        public string ProviderName { get; set; } = string.Empty;
    }
} 