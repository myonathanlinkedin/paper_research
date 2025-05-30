using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Error;

namespace RuntimeErrorSage.Domain.Models.Analysis
{
    /// <summary>
    /// Represents the result of error analysis.
    /// </summary>
    public class ErrorAnalysisResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for this result.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error context that was analyzed.
        /// </summary>
        public ErrorContext ErrorContext { get; set; }

        /// <summary>
        /// Gets or sets the root cause of the error.
        /// </summary>
        public string RootCause { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the impact of the error.
        /// </summary>
        public string Impact { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the severity of the error.
        /// </summary>
        public string Severity { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confidence level of the analysis.
        /// </summary>
        public double ConfidenceLevel { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the analysis was performed.
        /// </summary>
        public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the analysis method used.
        /// </summary>
        public string AnalysisMethod { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the analysis duration.
        /// </summary>
        public TimeSpan AnalysisDuration { get; set; }

        /// <summary>
        /// Gets or sets the analysis findings.
        /// </summary>
        public List<string> Findings { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis recommendations.
        /// </summary>
        public List<string> Recommendations { get; set; } = new();

        /// <summary>
        /// Gets or sets additional metadata about the analysis.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorAnalysisResult"/> class.
        /// </summary>
        public ErrorAnalysisResult()
        {
            ErrorContext = new ErrorContext(new RuntimeError(), "Analysis", DateTime.UtcNow);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorAnalysisResult"/> class.
        /// </summary>
        /// <param name="errorContext">The error context to analyze.</param>
        public ErrorAnalysisResult(ErrorContext errorContext)
        {
            ErrorContext = errorContext ?? throw new ArgumentNullException(nameof(errorContext));
        }
    }
} 
