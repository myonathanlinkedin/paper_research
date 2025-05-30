using RuntimeErrorSage.Domain.Models.Error;

namespace RuntimeErrorSage.Application.Classifier;

/// <summary>
/// Provides functionality for determining error subcategories based on analysis results.
/// </summary>
public static class SubcategoryDetermination
{
    /// <summary>
    /// Determines the subcategory of an error based on the analysis result.
    /// </summary>
    /// <param name="analysis">The error analysis result.</param>
    /// <returns>The determined error subcategory.</returns>
    public static string DetermineSubcategory(ErrorAnalysisResult analysis)
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
} 
