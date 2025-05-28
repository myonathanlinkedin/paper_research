using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents a related error.
    /// </summary>
    public class RelatedError
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error ID.
        /// </summary>
        public string ErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        public ErrorRelationshipType RelationshipType { get; set; }

        /// <summary>
        /// Gets or sets the confidence level of the relationship (0-1).
        /// </summary>
        public double ConfidenceLevel { get; set; }

        /// <summary>
        /// Gets or sets the component ID.
        /// </summary>
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error severity.
        /// </summary>
        public SeverityLevel Severity { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the error.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional context.
        /// </summary>
        public Dictionary<string, object> AdditionalContext { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the relationship description.
        /// </summary>
        public string RelationshipDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether this is a root cause.
        /// </summary>
        public bool IsRootCause { get; set; }

        /// <summary>
        /// Gets or sets whether this is a symptom.
        /// </summary>
        public bool IsSymptom { get; set; }

        /// <summary>
        /// Gets or sets whether this is the primary error.
        /// </summary>
        public bool IsPrimary { get; set; }

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

