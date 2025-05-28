using System;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents a related error with relationship information.
    /// </summary>
    public class RelatedError
    {
        /// <summary>
        /// Gets or sets the ID of the related error.
        /// </summary>
        public string ErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of relationship.
        /// </summary>
        public ErrorRelationshipType RelationshipType { get; set; } = ErrorRelationshipType.None;

        /// <summary>
        /// Gets or sets the strength of the relationship.
        /// </summary>
        public double RelationshipStrength { get; set; } = 0.0;

        /// <summary>
        /// Gets or sets the timestamp when the relationship was created.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
