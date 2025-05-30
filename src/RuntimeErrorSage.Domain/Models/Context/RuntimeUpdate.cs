using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Context
{
    /// <summary>
    /// Represents an update to a runtime context.
    /// </summary>
    public class RuntimeUpdate
    {
        /// <summary>
        /// Gets or sets the update identifier.
        /// </summary>
        public string UpdateId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp of the update.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the type of update.
        /// </summary>
        public string UpdateType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the update data.
        /// </summary>
        public Dictionary<string, object> UpdateData { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the source of the update.
        /// </summary>
        public string Source { get; set; } = string.Empty;
    }
} 