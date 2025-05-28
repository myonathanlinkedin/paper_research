using System;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents a relationship between two errors.
    /// </summary>
    public class ErrorRelationship
    {
        /// <summary>
        /// Gets or sets the ID of the source error.
        /// </summary>
        public string SourceErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of the target error.
        /// </summary>
        public string TargetErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the relationship was created.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the type of relationship.
        /// </summary>
        public ErrorRelationshipType Type { get; set; } = ErrorRelationshipType.None;

        /// <summary>
        /// Gets or sets the strength of the relationship.
        /// </summary>
        public double Strength { get; set; } = 0.0;
    }
} 