using System.Collections.ObjectModel;
using System;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Error
{
    /// <summary>
    /// Represents a relationship between two errors.
    /// </summary>
    public class ErrorRelationship
    {
        /// <summary>
        /// Gets or sets the unique identifier of the relationship.
        /// </summary>
        public string RelationshipId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the source error ID.
        /// </summary>
        public string SourceErrorId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the target error ID.
        /// </summary>
        public string TargetErrorId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        public ErrorRelationshipType RelationshipType { get; } = ErrorRelationshipType.Unknown;

        /// <summary>
        /// Gets or sets the relationship strength (0-1).
        /// </summary>
        public double Strength { get; } = 0.0;

        /// <summary>
        /// Gets or sets the confidence level of the relationship (0-1).
        /// </summary>
        public double Confidence { get; } = 0.0;

        /// <summary>
        /// Gets or sets the description of the relationship.
        /// </summary>
        public string Description { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the relationship.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets whether the relationship is bidirectional.
        /// </summary>
        public bool IsBidirectional { get; }

        /// <summary>
        /// Creates a new error relationship.
        /// </summary>
        /// <param name="sourceErrorId">The source error ID.</param>
        /// <param name="targetErrorId">The target error ID.</param>
        /// <param name="type">The relationship type.</param>
        /// <param name="strength">The relationship strength.</param>
        /// <returns>The error relationship.</returns>
        public static ErrorRelationship Create(string sourceErrorId, string targetErrorId, ErrorRelationshipType type, double strength = 0.5)
        {
            return new ErrorRelationship
            {
                SourceErrorId = sourceErrorId,
                TargetErrorId = targetErrorId,
                RelationshipType = type,
                Strength = strength,
                Confidence = 0.8,
                Timestamp = DateTime.UtcNow
            };
        }
    }
} 






