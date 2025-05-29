using System;
using System.Collections.Generic;
using RuntimeErrorSage.Model.Models.Error;

namespace RuntimeErrorSage.Model.Models.Context
{
    /// <summary>
    /// Represents the history of context changes.
    /// </summary>
    public class ContextHistory
    {
        /// <summary>
        /// Gets or sets the unique identifier of the history.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the context identifier.
        /// </summary>
        public string ContextId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation name.
        /// </summary>
        public string OperationName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the most recent context.
        /// </summary>
        public ErrorContext MostRecentContext { get; set; } = new();

        /// <summary>
        /// Gets or sets the previous state.
        /// </summary>
        public Dictionary<string, object> PreviousState { get; set; } = new();

        /// <summary>
        /// Gets or sets the new state.
        /// </summary>
        public Dictionary<string, object> NewState { get; set; } = new();

        /// <summary>
        /// Gets or sets the metadata associated with the history.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the type of change.
        /// </summary>
        public string ChangeType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the change.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets who made the change.
        /// </summary>
        public string ChangedBy { get; set; } = string.Empty;
    }
} 
