using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Classifier.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Application.Classifier;

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
                ErrorType = context.Error?.ErrorType ?? context.ErrorType ?? "Unknown",
                Severity = ConvertToErrorSeverity(CalculateSeverity(analysisResult)),
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
    
    // Add a local implementation of CalculateSeverity since SeverityDetermination is missing
    private SeverityLevel CalculateSeverity(ErrorAnalysisResult analysisResult)
    {
        if (analysisResult == null)
            return SeverityLevel.Unknown;
            
        // Extract severity data from analysis result
        if (analysisResult.Details != null && 
            analysisResult.Details.TryGetValue("Severity", out var severityObj) && 
            severityObj is string severityStr)
        {
            if (Enum.TryParse<SeverityLevel>(severityStr, true, out var severity))
                return severity;
        }
        
        // Check if we have an impact score in the details
        double impactScore = 0.0;
        if (analysisResult.Details != null && 
            analysisResult.Details.TryGetValue("ImpactScore", out var impactObj) && 
            impactObj is double impact)
        {
            impactScore = impact;
        }
        
        // Default severity based on impact
        return impactScore > 0.8 ? SeverityLevel.Critical :
               impactScore > 0.6 ? SeverityLevel.High :
               impactScore > 0.4 ? SeverityLevel.Medium :
               impactScore > 0.2 ? SeverityLevel.Low :
               SeverityLevel.Unknown;
    }

    private ErrorSeverity ConvertToErrorSeverity(SeverityLevel severity)
    {
        switch (severity)
        {
            case SeverityLevel.Critical:
                return ErrorSeverity.Critical;
            case SeverityLevel.High:
                return ErrorSeverity.High;
            case SeverityLevel.Medium:
                return ErrorSeverity.Medium;
            case SeverityLevel.Low:
                return ErrorSeverity.Low;
            default:
                return ErrorSeverity.Unknown;
        }
    }
} 