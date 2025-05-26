using System;
using System.Collections.Generic;
using System.Linq;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents the result of an error analysis.
/// </summary>
public class ErrorAnalysis
{
    private readonly Dictionary<string, object> _metadata;

    /// <summary>
    /// Gets the error type.
    /// </summary>
    public string ErrorType { get; }

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
        ErrorType = errorType ?? throw new ArgumentNullException(nameof(errorType));
        RootCause = rootCause ?? throw new ArgumentNullException(nameof(rootCause));
        Confidence = confidence;
        RemediationSteps = remediationSteps?.ToList() ?? throw new ArgumentNullException(nameof(remediationSteps));
        _metadata = new Dictionary<string, object>();
    }

    /// <summary>
    /// Gets a value indicating whether the analysis is valid.
    /// </summary>
    public bool IsValid => Validate();

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata => _metadata;

    /// <summary>
    /// Adds metadata.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void AddMetadata(string key, object value)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        _metadata[key] = value;
    }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The metadata value.</returns>
    public object GetMetadata(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        return _metadata.TryGetValue(key, out var value) ? value : null;
    }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="key">The key.</param>
    /// <returns>The metadata value.</returns>
    public T GetMetadata<T>(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        if (!_metadata.TryGetValue(key, out var value))
            return default;

        return value is T typedValue ? typedValue : default;
    }

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