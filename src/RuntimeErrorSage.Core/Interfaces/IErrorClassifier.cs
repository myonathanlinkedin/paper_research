using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Interfaces;

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