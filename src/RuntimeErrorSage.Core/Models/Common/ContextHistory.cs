using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Common
{
    /// <summary>
    /// Represents the history of changes to an error context.
    /// </summary>
    public class ContextHistory
    {
        /// <summary>
        /// Gets or sets the unique identifier of the history entry.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the context identifier.
        /// </summary>
        public string ContextId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the change occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the type of change that occurred.
        /// </summary>
        public ContextChangeType ChangeType { get; set; }

        /// <summary>
        /// Gets or sets the previous state of the context.
        /// </summary>
        public Dictionary<string, object> PreviousState { get; set; }

        /// <summary>
        /// Gets or sets the new state of the context.
        /// </summary>
        public Dictionary<string, object> NewState { get; set; }

        /// <summary>
        /// Gets or sets the user or system that made the change.
        /// </summary>
        public string ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets additional metadata about the change.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Defines the types of changes that can occur to a context.
    /// </summary>
    public enum ContextChangeType
    {
        /// <summary>
        /// The context was created.
        /// </summary>
        Created,

        /// <summary>
        /// The context was updated.
        /// </summary>
        Updated,

        /// <summary>
        /// The context was enriched with additional data.
        /// </summary>
        Enriched,

        /// <summary>
        /// The context was validated.
        /// </summary>
        Validated,

        /// <summary>
        /// The context was analyzed.
        /// </summary>
        Analyzed,

        /// <summary>
        /// The context was remediated.
        /// </summary>
        Remediated,

        /// <summary>
        /// The context was archived.
        /// </summary>
        Archived,

        /// <summary>
        /// The context was deleted.
        /// </summary>
        Deleted
    }
} 