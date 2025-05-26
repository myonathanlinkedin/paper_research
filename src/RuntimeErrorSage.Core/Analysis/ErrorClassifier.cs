using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Analysis;

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
                Category = DetermineCategory(analysisResult),
                Subcategory = DetermineSubcategory(analysisResult),
                ErrorType = context.Exception?.GetType().Name ?? context.ErrorType ?? "Unknown",
                Severity = DetermineSeverity(analysisResult),
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

    private string DetermineCategory(ErrorAnalysisResult analysis)
    {
        if (analysis.Details != null && analysis.Details.TryGetValue("RootCause", out var rootCauseObj) && rootCauseObj is string rootCause)
        {
            if (rootCause.Contains("database", StringComparison.OrdinalIgnoreCase))
                return "Database";
            if (rootCause.Contains("network", StringComparison.OrdinalIgnoreCase))
                return "Network";
            if (rootCause.Contains("file", StringComparison.OrdinalIgnoreCase))
                return "FileSystem";
            if (rootCause.Contains("memory", StringComparison.OrdinalIgnoreCase))
                return "Resource";
        }
        return "General";
    }

    private string DetermineSubcategory(ErrorAnalysisResult analysis)
    {
        if (analysis.Details != null && analysis.Details.TryGetValue("RootCause", out var rootCauseObj) && rootCauseObj is string rootCause)
        {
            if (rootCause.Contains("timeout", StringComparison.OrdinalIgnoreCase))
                return "Timeout";
            if (rootCause.Contains("permission", StringComparison.OrdinalIgnoreCase))
                return "Permission";
            if (rootCause.Contains("connection", StringComparison.OrdinalIgnoreCase))
                return "Connection";
        }
        return "Unknown";
    }

    private ErrorSeverity DetermineSeverity(ErrorAnalysisResult analysis)
    {
        if (analysis.Details != null && analysis.Details.TryGetValue("severity", out var severityValue))
        {
            double severity = 0.0;
            if (severityValue is double d)
                severity = d;
            else if (severityValue is float f)
                severity = f;
            else if (severityValue is int i)
                severity = i;
            else if (severityValue is string s && double.TryParse(s, out var parsed))
                severity = parsed;
            if (severity >= 0.8) return ErrorSeverity.Critical;
            if (severity >= 0.6) return ErrorSeverity.High;
            if (severity >= 0.4) return ErrorSeverity.Medium;
            if (severity >= 0.2) return ErrorSeverity.Low;
        }
        return ErrorSeverity.Unknown;
    }
} 