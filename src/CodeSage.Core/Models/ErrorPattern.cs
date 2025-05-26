using System;
using System.Collections.Generic;

namespace CodeSage.Core.Models
{
    /// <summary>
    /// Represents an error pattern for pattern recognition and matching.
    /// </summary>
    public class ErrorPattern
    {
        /// <summary>
        /// Gets or sets the unique identifier of the pattern.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the service name associated with the pattern.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error type (e.g., exception type).
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation name where the error occurred.
        /// </summary>
        public string OperationName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first occurrence timestamp.
        /// </summary>
        public DateTime FirstOccurrence { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last occurrence timestamp.
        /// </summary>
        public DateTime LastOccurrence { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the number of times this pattern has occurred.
        /// </summary>
        public int OccurrenceCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the context information for the pattern.
        /// </summary>
        public Dictionary<string, object> Context { get; set; } = new();

        /// <summary>
        /// Gets or sets the pattern type (e.g., known, anomaly).
        /// </summary>
        public string PatternType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confidence score for the pattern match.
        /// </summary>
        public float Confidence { get; set; } = 1.0f;
    }
} 