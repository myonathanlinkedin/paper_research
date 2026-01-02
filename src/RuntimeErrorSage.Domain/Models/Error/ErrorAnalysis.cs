using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Domain.Models.Error;

/// <summary>
/// Represents an analysis of an error occurrence.
/// </summary>
public class ErrorAnalysis
{
    /// <summary>
    /// Gets or sets the unique identifier of the error analysis.
    /// </summary>
    public string ErrorId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the category of the error.
    /// </summary>
    public ErrorCategory Category { get; set; }

    /// <summary>
    /// Gets or sets the severity of the error.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the details of the error.
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the frequency of the error.
    /// </summary>
    public int Frequency { get; set; }

    /// <summary>
    /// Gets or sets the error scope.
    /// </summary>
    public ErrorScope Scope { get; set; }

    /// <summary>
    /// Gets or sets the source of the error.
    /// </summary>
    public ErrorSource Source { get; set; }

    /// <summary>
    /// Gets or sets the stack trace of the error.
    /// </summary>
    public string StackTrace { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string ErrorType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error location.
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the analysis.
    /// </summary>
    public AnalysisStatus Status { get; set; }

    /// <summary>
    /// Gets or sets any metadata associated with the error.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets whether the error is actionable.
    /// </summary>
    public bool IsActionable { get; set; }

    /// <summary>
    /// Gets or sets whether the error is transient.
    /// </summary>
    public bool IsTransient { get; set; }

    /// <summary>
    /// Gets or sets whether the error has been resolved.
    /// </summary>
    public bool IsResolved { get; set; }

    /// <summary>
    /// Gets or sets the related errors.
    /// </summary>
    public List<RelatedError> RelatedErrors { get; set; } = new List<RelatedError>();

    /// <summary>
    /// Gets or sets the timestamp when the analysis was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the timestamp when the analysis was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

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
    public IReadOnlyList<string> RemediationSteps { get; }

    /// <summary>
    /// Gets or sets a summary of the error analysis.
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the suggested remediation actions.
    /// </summary>
    public List<RemediationAction> SuggestedActions { get; set; } = new();

    /// <summary>
    /// Gets or sets the primary remediation action.
    /// </summary>
    public RemediationAction RemediationAction { get; set; }

    /// <summary>
    /// Gets or sets the error message (alias for Message).
    /// </summary>
    public string ErrorMessage
    {
        get => Message;
        set => Message = value;
    }

    /// <summary>
    /// Gets or sets contextual insights about the error.
    /// </summary>
    public Dictionary<string, string> ContextualInsights { get; set; } = new();

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
