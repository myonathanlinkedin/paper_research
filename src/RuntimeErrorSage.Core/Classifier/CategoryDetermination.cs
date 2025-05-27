using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Classifier;

/// <summary>
/// Provides functionality for determining error categories based on analysis results.
/// </summary>
public static class CategoryDetermination
{
    /// <summary>
    /// Determines the category of an error based on the analysis result.
    /// </summary>
    /// <param name="analysis">The error analysis result.</param>
    /// <returns>The determined error category.</returns>
    public static string DetermineCategory(ErrorAnalysisResult analysis)
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
} 