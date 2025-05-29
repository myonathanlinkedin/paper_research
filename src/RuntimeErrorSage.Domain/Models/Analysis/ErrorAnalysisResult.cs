using System;
using System.Collections.Generic;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Model.Models.Analysis
{
    /// <summary>
    /// Represents the result of an error analysis.
    /// </summary>
    public class ErrorAnalysisResult
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public ErrorContext Context { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the possible root causes.
        /// </summary>
        public List<string> PossibleRootCauses { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the probable root cause.
        /// </summary>
        public string ProbableRootCause { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confidence level of the analysis (0-1).
        /// </summary>
        public double ConfidenceLevel { get; set; }

        /// <summary>
        /// Gets or sets the related errors.
        /// </summary>
        public List<RelatedError> RelatedErrors { get; set; } = new List<RelatedError>();

        /// <summary>
        /// Gets or sets the error frequency.
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error severity.
        /// </summary>
        public SeverityLevel ErrorSeverity { get; set; }

        /// <summary>
        /// Gets or sets the component ID.
        /// </summary>
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional context.
        /// </summary>
        public Dictionary<string, object> AdditionalContext { get; set; } = new Dictionary<string, object>();
    }
} 
