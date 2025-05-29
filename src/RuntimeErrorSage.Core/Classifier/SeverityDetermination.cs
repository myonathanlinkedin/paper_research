using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Classifier;

/// <summary>
/// Provides functionality for determining error severity based on analysis results.
/// </summary>
public static class SeverityDetermination
{
    /// <summary>
    /// Determines the severity of an error based on the analysis result.
    /// </summary>
    /// <param name="analysis">The error analysis result.</param>
    /// <returns>The determined error severity.</returns>
    public static ErrorSeverity DetermineSeverity(ErrorAnalysisResult analysis)
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






