using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents the result of analyzing a runtime error.
    /// </summary>
    public class ErrorAnalysisResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for this analysis.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the ID of the analyzed error.
        /// </summary>
        public string ErrorId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the root cause of the error.
        /// </summary>
        public string RootCause { get; set; }

        /// <summary>
        /// Gets or sets the error classification.
        /// </summary>
        public string Classification { get; set; }

        /// <summary>
        /// Gets or sets the error severity.
        /// </summary>
        public ErrorSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the error category.
        /// </summary>
        public ErrorCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the impact level.
        /// </summary>
        public ImpactLevel Impact { get; set; }

        /// <summary>
        /// Gets or sets the confidence score of the analysis.
        /// </summary>
        public double ConfidenceScore { get; set; }

        /// <summary>
        /// Gets or sets the list of related errors.
        /// </summary>
        public List<RelatedError> RelatedErrors { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of affected components.
        /// </summary>
        public List<string> AffectedComponents { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of potential fixes.
        /// </summary>
        public List<PotentialFix> PotentialFixes { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of prevention measures.
        /// </summary>
        public List<PreventionMeasure> PreventionMeasures { get; set; } = new();

        /// <summary>
        /// Gets or sets the dependency graph analysis.
        /// </summary>
        public GraphAnalysisResult GraphAnalysis { get; set; }

        /// <summary>
        /// Gets or sets additional analysis metrics.
        /// </summary>
        public Dictionary<string, double> Metrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the correlation ID for tracing purposes.
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the confidence level.
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the suggested remediation actions.
        /// </summary>
        public List<RemediationAction> SuggestedActions { get; set; } = new();

        /// <summary>
        /// Gets or sets additional metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the natural language explanation.
        /// </summary>
        public string NaturalLanguageExplanation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the analysis duration.
        /// </summary>
        public TimeSpan AnalysisDuration { get; set; }

        /// <summary>
        /// Gets or sets the analysis status.
        /// </summary>
        public AnalysisStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the analysis version.
        /// </summary>
        public string AnalysisVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the analysis model used.
        /// </summary>
        public string AnalysisModel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the analysis parameters.
        /// </summary>
        public Dictionary<string, object> AnalysisParameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis dependencies.
        /// </summary>
        public List<string> AnalysisDependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis tags.
        /// </summary>
        public Dictionary<string, string> AnalysisTags { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis metrics.
        /// </summary>
        public Dictionary<string, double> AnalysisMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis context.
        /// </summary>
        public Dictionary<string, object> AnalysisContext { get; set; } = new();
    }

    /// <summary>
    /// Represents a related error.
    /// </summary>
    public class RelatedError
    {
        /// <summary>
        /// Gets or sets the ID of the related error.
        /// </summary>
        public string ErrorId { get; set; }

        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        public string RelationType { get; set; }

        /// <summary>
        /// Gets or sets the similarity score.
        /// </summary>
        public double SimilarityScore { get; set; }

        /// <summary>
        /// Gets or sets additional relationship details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();
    }

    /// <summary>
    /// Represents a potential fix for an error.
    /// </summary>
    public class PotentialFix
    {
        /// <summary>
        /// Gets or sets the unique identifier for this fix.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the description of the fix.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the implementation steps.
        /// </summary>
        public List<string> Steps { get; set; } = new();

        /// <summary>
        /// Gets or sets the estimated complexity.
        /// </summary>
        public FixComplexity Complexity { get; set; }

        /// <summary>
        /// Gets or sets the estimated success probability.
        /// </summary>
        public double SuccessProbability { get; set; }

        /// <summary>
        /// Gets or sets the potential risks.
        /// </summary>
        public List<string> Risks { get; set; } = new();
    }

    /// <summary>
    /// Represents a prevention measure.
    /// </summary>
    public class PreventionMeasure
    {
        /// <summary>
        /// Gets or sets the unique identifier for this measure.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the description of the measure.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the implementation steps.
        /// </summary>
        public List<string> Steps { get; set; } = new();

        /// <summary>
        /// Gets or sets the priority level.
        /// </summary>
        public PreventionPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the estimated effectiveness.
        /// </summary>
        public double Effectiveness { get; set; }

        /// <summary>
        /// Gets or sets the implementation cost.
        /// </summary>
        public ImplementationCost Cost { get; set; }
    }

    /// <summary>
    /// Represents the complexity of a fix.
    /// </summary>
    public enum FixComplexity
    {
        Low,
        Medium,
        High,
        VeryHigh
    }

    /// <summary>
    /// Represents the priority of a prevention measure.
    /// </summary>
    public enum PreventionPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    /// <summary>
    /// Represents the implementation cost.
    /// </summary>
    public enum ImplementationCost
    {
        Low,
        Medium,
        High,
        VeryHigh
    }

    /// <summary>
    /// Represents the status of an error analysis.
    /// </summary>
    public enum AnalysisStatus
    {
        /// <summary>
        /// The analysis is pending.
        /// </summary>
        Pending,

        /// <summary>
        /// The analysis is in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// The analysis has completed successfully.
        /// </summary>
        Completed,

        /// <summary>
        /// The analysis has failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The analysis has been cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The analysis has timed out.
        /// </summary>
        TimedOut,

        /// <summary>
        /// The analysis status is unknown.
        /// </summary>
        Unknown
    }
} 