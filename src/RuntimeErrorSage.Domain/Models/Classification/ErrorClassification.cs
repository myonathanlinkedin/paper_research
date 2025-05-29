using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Classification
{
    /// <summary>
    /// Represents the classification of an error.
    /// </summary>
    public class ErrorClassification
    {
        /// <summary>
        /// Gets or sets the unique identifier of the classification.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; }

        /// <summary>
        /// Gets or sets the error category.
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets or sets the confidence level of the classification.
        /// </summary>
        public double Confidence { get; }

        /// <summary>
        /// Gets or sets the severity level.
        /// </summary>
        public int Severity { get; }

        /// <summary>
        /// Gets or sets the timestamp of the classification.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets additional metadata about the classification.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
} 





