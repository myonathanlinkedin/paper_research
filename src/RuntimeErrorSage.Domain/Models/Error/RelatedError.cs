using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Error
{
    /// <summary>
    /// Represents a related error.
    /// </summary>
    public class RelatedError
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error ID.
        /// </summary>
        public string ErrorId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        public ErrorRelationshipType RelationshipType { get; }

        /// <summary>
        /// Gets or sets the confidence level of the relationship (0-1).
        /// </summary>
        public double ConfidenceLevel { get; }

        /// <summary>
        /// Gets or sets the component ID.
        /// </summary>
        public string ComponentId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the error severity.
        /// </summary>
        public SeverityLevel Severity { get; }

        /// <summary>
        /// Gets or sets the timestamp of the error.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets additional context.
        /// </summary>
        public Dictionary<string, object> AdditionalContext { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the relationship description.
        /// </summary>
        public string RelationshipDescription { get; } = string.Empty;

        /// <summary>
        /// Gets or sets whether this is a root cause.
        /// </summary>
        public bool IsRootCause { get; }

        /// <summary>
        /// Gets or sets whether this is a symptom.
        /// </summary>
        public bool IsSymptom { get; }

        /// <summary>
        /// Gets or sets whether this is the primary error.
        /// </summary>
        public bool IsPrimary { get; }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return ErrorId?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is RelatedError other)
            {
                return ErrorId == other.ErrorId;
            }
            return false;
        }
    }
}







