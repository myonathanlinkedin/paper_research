using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Analysis; // For GraphAnalysisResult, RemediationAnalysis, etc.
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;

namespace RuntimeErrorSage.Tests.TestSuite.Models;

/// <summary>
/// Error scenario for test execution.
/// </summary>
public class ErrorScenario
{
    private readonly Dictionary<string, object> _metadata;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorScenario"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="errorType">The error type.</param>
    /// <param name="source">The source.</param>
    /// <param name="error">The error.</param>
    /// <param name="analysis">The analysis.</param>
    public ErrorScenario(
        string name,
        string errorType,
        string source,
        RuntimeError error,
        ErrorAnalysis analysis = null)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        if (string.IsNullOrEmpty(errorType))
            throw new ArgumentException("Error type cannot be null or empty.", nameof(errorType));

        if (string.IsNullOrEmpty(source))
            throw new ArgumentException("Source cannot be null or empty.", nameof(source));

        ArgumentNullException.ThrowIfNull(error);

        Name = name;
        ErrorType = errorType;
        Source = source;
        Error = error;
        Analysis = analysis;
        _metadata = new Dictionary<string, object>();
    }

    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string ErrorType { get; set; }

    /// <summary>
    /// Gets or sets the source.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    public RuntimeError Error { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the stack trace.
    /// </summary>
    public string StackTrace { get; set; }

    /// <summary>
    /// Gets or sets the analysis.
    /// </summary>
    public ErrorAnalysis Analysis { get; set; }

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
    /// Executes the scenario asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task<Exception> ExecuteAsync()
    {
        // Default implementation - throws the error from the Error property
        if (Error != null)
        {
            return await Task.FromException<Exception>(new Exception(Error.Message));
        }
        return await Task.FromException<Exception>(new InvalidOperationException("No error defined in scenario"));
    }

    /// <summary>
    /// Executes the scenario synchronously.
    /// </summary>
    /// <returns>The exception that was thrown.</returns>
    public Exception Execute()
    {
        if (Error != null)
        {
            throw new Exception(Error.Message);
        }
        throw new InvalidOperationException("No error defined in scenario");
    }

    /// <summary>
    /// Validates the scenario.
    /// </summary>
    /// <returns>True if the scenario is valid; otherwise, false.</returns>
    public bool Validate()
    {
        if (string.IsNullOrEmpty(Name))
            return false;

        if (string.IsNullOrEmpty(ErrorType))
            return false;

        if (string.IsNullOrEmpty(Source))
            return false;

        if (Error == null)
            return false;

        return true;
    }
} 
