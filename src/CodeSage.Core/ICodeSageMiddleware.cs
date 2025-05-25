using System;
using System.Threading.Tasks;

namespace CodeSage.Core
{
    /// <summary>
    /// Represents the core middleware interface for CodeSage runtime intelligence layer.
    /// </summary>
    public interface ICodeSageMiddleware
    {
        /// <summary>
        /// Intercepts and processes an unhandled exception.
        /// </summary>
        /// <param name="exception">The unhandled exception to process</param>
        /// <param name="context">Additional context information about the error</param>
        /// <returns>A task representing the asynchronous operation with the analysis result</returns>
        Task<ErrorAnalysisResult> ProcessExceptionAsync(Exception exception, ErrorContext context);

        /// <summary>
        /// Attempts to apply automated remediation based on the analysis result.
        /// </summary>
        /// <param name="analysisResult">The result of the error analysis</param>
        /// <returns>A task representing the asynchronous operation with the remediation result</returns>
        Task<RemediationResult> ApplyRemediationAsync(ErrorAnalysisResult analysisResult);
    }

    /// <summary>
    /// Contains contextual information about an error occurrence.
    /// </summary>
    public class ErrorContext
    {
        public string? ServiceName { get; set; }
        public string? OperationName { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Environment { get; set; }
        public string? CorrelationId { get; set; }
    }

    /// <summary>
    /// Represents the result of error analysis performed by CodeSage.
    /// </summary>
    public class ErrorAnalysisResult
    {
        public string? NaturalLanguageExplanation { get; set; }
        public List<string> SuggestedActions { get; set; } = new();
        public Dictionary<string, object>? ContextualData { get; set; }
        public bool CanAutoRemediate { get; set; }
        public string? RemediationStrategy { get; set; }
        public SeverityLevel Severity { get; set; }
    }

    /// <summary>
    /// Represents the result of an attempted remediation.
    /// </summary>
    public class RemediationResult
    {
        public bool Success { get; set; }
        public string? ActionTaken { get; set; }
        public string? AdditionalContext { get; set; }
        public DateTime RemediationTimestamp { get; set; }
    }

    /// <summary>
    /// Represents the severity level of an error.
    /// </summary>
    public enum SeverityLevel
    {
        Low,
        Medium,
        High,
        Critical
    }
} 