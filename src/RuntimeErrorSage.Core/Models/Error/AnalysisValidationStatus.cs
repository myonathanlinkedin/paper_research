namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Defines the validation status of error analysis.
/// </summary>
public enum AnalysisValidationStatus
{
    Validated,
    Pending,
    Failed,
    Skipped,
    Unknown
} 