using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents a recognized error pattern.
    /// </summary>
    public class ErrorPattern
    {
        /// <summary>
        /// Gets or sets the unique identifier for this pattern.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the pattern.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of error this pattern represents.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the pattern.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the frequency of occurrence.
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// Gets or sets the confidence level of the pattern match.
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the context information associated with this pattern.
        /// </summary>
        public Dictionary<string, object> Context { get; set; } = new();

        /// <summary>
        /// Gets or sets the timestamp when this pattern was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the severity level of errors matching this pattern.
        /// </summary>
        public SeverityLevel Severity { get; set; }

        /// <summary>
        /// Gets or sets the list of known remediation strategies for this pattern.
        /// </summary>
        public List<string> RemediationStrategies { get; set; } = new();

        /// <summary>
        /// Gets or sets whether this pattern is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the operation name associated with this pattern.
        /// </summary>
        public string OperationName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the last occurrence.
        /// </summary>
        public DateTime LastOccurrence { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the number of times this pattern has occurred.
        /// </summary>
        public int OccurrenceCount { get; set; }

        /// <summary>
        /// Gets or sets the service name associated with this pattern.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the pattern type.
        /// </summary>
        public string PatternType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the pattern metadata.
        /// </summary>
        public Dictionary<string, object> PatternMetadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the pattern notes.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the pattern ID.
        /// </summary>
        public string PatternId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the first occurrence timestamp.
        /// </summary>
        public DateTime FirstOccurrence { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the analysis result associated with this pattern.
        /// </summary>
        public ErrorAnalysisResult? Analysis { get; set; }
    }
} 
