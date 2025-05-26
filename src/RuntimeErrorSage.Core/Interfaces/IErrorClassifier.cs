using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for classifying errors.
    /// </summary>
    public interface IErrorClassifier
    {
        /// <summary>
        /// Gets whether the classifier is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the classifier name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the classifier version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Classifies an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The error classification result.</returns>
        Task<ErrorClassification> ClassifyAsync(ErrorContext context);

        /// <summary>
        /// Gets the confidence score for a classification.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The confidence score between 0 and 1.</returns>
        Task<double> GetConfidenceScoreAsync(ErrorContext context);

        /// <summary>
        /// Gets similar error patterns.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The similar error patterns.</returns>
        Task<ErrorPatternCollection> GetSimilarPatternsAsync(ErrorContext context);
    }

    /// <summary>
    /// Represents an error classification.
    /// </summary>
    public class ErrorClassification
    {
        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the error category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the error severity.
        /// </summary>
        public ErrorSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the confidence score.
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets additional classification details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();
    }

    /// <summary>
    /// Represents a collection of error patterns.
    /// </summary>
    public class ErrorPatternCollection
    {
        /// <summary>
        /// Gets or sets the patterns.
        /// </summary>
        public List<ErrorPattern> Patterns { get; set; } = new();

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the match scores.
        /// </summary>
        public Dictionary<string, double> MatchScores { get; set; } = new();
    }

    /// <summary>
    /// Represents an error pattern.
    /// </summary>
    public class ErrorPattern
    {
        /// <summary>
        /// Gets or sets the pattern identifier.
        /// </summary>
        public string PatternId { get; set; }

        /// <summary>
        /// Gets or sets the pattern description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the pattern frequency.
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// Gets or sets the pattern metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 