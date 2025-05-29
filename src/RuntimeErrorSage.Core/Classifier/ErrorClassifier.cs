using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Model.Classifier.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Graph;
using RuntimeErrorSage.Model.Models.Interfaces;

namespace RuntimeErrorSage.Model.Classifier;

public class ErrorClassifier : IErrorClassifier
{
    private readonly ILogger<ErrorClassifier> _logger;
    private readonly IModel _model;

    public ErrorClassifier(ILogger<ErrorClassifier> logger, IModel model)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _model = model ?? throw new ArgumentNullException(nameof(model));
    }

    public bool IsEnabled => true;
    public string Name => "DefaultErrorClassifier";
    public string Version => "1.0";

    public async Task<ErrorClassification> ClassifyAsync(ErrorContext context)
    {
        return await ExecuteWithErrorHandlingAsync(async () =>
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var analysisResult = await _model.AnalyzeErrorAsync(context);
            double confidence = 0.0;
            if (analysisResult.Details != null && analysisResult.Details.TryGetValue("Confidence", out var confObj) && confObj is double confVal)
                confidence = confVal;
            return new ErrorClassification
            {
                Category = CategoryDetermination.DetermineCategory(analysisResult),
                Subcategory = SubcategoryDetermination.DetermineSubcategory(analysisResult),
                ErrorType = context.Exception?.GetType().Name ?? context.ErrorType ?? "Unknown",
                Severity = SeverityDetermination.DetermineSeverity(analysisResult),
                Confidence = confidence
            };
        }, "Error classifying error context");
    }

    public async Task<double> GetConfidenceScoreAsync(ErrorContext context)
    {
        return await ExecuteWithErrorHandlingAsync(async () =>
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var analysisResult = await _model.AnalyzeErrorAsync(context);
            if (analysisResult.Details != null && analysisResult.Details.TryGetValue("Confidence", out var confObj) && confObj is double confVal)
                return confVal;
            return 0.0;
        }, "Error getting confidence score");
    }

    public async Task<ErrorPatternCollection> GetSimilarPatternsAsync(ErrorContext context)
    {
        return await ExecuteWithErrorHandlingAsync(async () =>
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var analysisResult = await _model.AnalyzeErrorAsync(context);
            var patterns = new ErrorPatternCollection
            {
                Patterns = new List<ErrorPattern>(),
                TotalCount = 0,
                MatchScores = new Dictionary<string, double>()
            };

            // analysisResult.SimilarPatterns does not exist, so we cannot add any patterns here
            // If you want to use patterns from Details or another property, add logic here

            return patterns;
        }, "Error getting similar patterns");
    }

    public async Task<double> CalculateErrorProbabilityAsync(DependencyNode node)
    {
        return await ExecuteWithErrorHandlingAsync(async () =>
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            // Only use ErrorProbability if Health and Reliability do not exist
            var probability = node.ErrorProbability;
            return Math.Max(0.0, Math.Min(1.0, probability));
        }, "Error calculating error probability");
    }

    private async Task<T> ExecuteWithErrorHandlingAsync<T>(Func<Task<T>> action, string errorMessage)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, errorMessage);
            throw;
        }
    }
} 
