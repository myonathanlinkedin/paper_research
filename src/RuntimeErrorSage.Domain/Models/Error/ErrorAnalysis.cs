using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Error;

/// <summary>
/// Represents an analysis of an error occurrence.
/// </summary>
public class ErrorAnalysis
{
    /// <summary>
    /// Gets or sets the unique identifier of the error analysis.
    /// </summary>
    public string ErrorId { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the category of the error.
    /// </summary>
    public ErrorCategory Category { get; }

    /// <summary>
    /// Gets or sets the severity of the error.
    /// </summary>
    public ErrorSeverity Severity { get; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the details of the error.
    /// </summary>
    public string Details { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the frequency of the error.
    /// </summary>
    public int Frequency { get; }

    /// <summary>
    /// Gets or sets the error scope.
    /// </summary>
    public ErrorScope Scope { get; }

    /// <summary>
    /// Gets or sets the source of the error.
    /// </summary>
    public ErrorSource Source { get; }

    /// <summary>
    /// Gets or sets the stack trace of the error.
    /// </summary>
    public string StackTrace { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string ErrorType { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string ErrorCode { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the error location.
    /// </summary>
    public string Location { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the analysis.
    /// </summary>
    public AnalysisStatus Status { get; } = AnalysisStatus.NotStarted;

    /// <summary>
    /// Gets or sets any metadata associated with the error.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets whether the error is actionable.
    /// </summary>
    public bool IsActionable { get; }

    /// <summary>
    /// Gets or sets whether the error is transient.
    /// </summary>
    public bool IsTransient { get; }

    /// <summary>
    /// Gets or sets whether the error has been resolved.
    /// </summary>
    public bool IsResolved { get; }

    /// <summary>
    /// Gets or sets the related errors.
    /// </summary>
    public IReadOnlyCollection<RelatedErrors> RelatedErrors { get; } = new Collection<RelatedError>();

    /// <summary>
    /// Gets or sets the timestamp when the analysis was created.
    /// </summary>
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the timestamp when the analysis was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the root cause.
    /// </summary>
    public string RootCause { get; }

    /// <summary>
    /// Gets the confidence level (0-1).
    /// </summary>
    public double Confidence { get; }

    /// <summary>
    /// Gets the remediation steps.
    /// </summary>
    public IReadOnlyCollection<string> RemediationSteps { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorAnalysis"/> class.
    /// </summary>
    /// <param name="errorType">The error type.</param>
    /// <param name="rootCause">The root cause.</param>
    /// <param name="confidence">The confidence level (0-1).</param>
    /// <param name="remediationSteps">The remediation steps.</param>
    public ErrorAnalysis(
        string errorType,
        string rootCause,
        double confidence,
        IEnumerable<string> remediationSteps)
    {
        ArgumentNullException.ThrowIfNull(errorType);
        ArgumentNullException.ThrowIfNull(rootCause);
        ArgumentNullException.ThrowIfNull(remediationSteps);

        ErrorType = errorType;
        RootCause = rootCause;
        Confidence = confidence;
        RemediationSteps = remediationSteps.ToList();
    }

    /// <summary>
    /// Gets a value indicating whether the analysis is valid.
    /// </summary>
    public bool IsValid => Validate();

    /// <summary>
    /// Validates the analysis.
    /// </summary>
    /// <returns>True if the analysis is valid; otherwise, false.</returns>
    public bool Validate()
    {
        if (string.IsNullOrEmpty(ErrorType))
            return false;

        if (string.IsNullOrEmpty(RootCause))
            return false;

        if (Confidence < 0 || Confidence > 1)
            return false;

        if (RemediationSteps == null || !RemediationSteps.Any())
            return false;

        return true;
    }
} 





