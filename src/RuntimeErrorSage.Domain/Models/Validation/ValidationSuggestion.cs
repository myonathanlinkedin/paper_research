using RuntimeErrorSage.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RuntimeErrorSage.Application.Models.Validation;

/// <summary>
/// Represents a validation suggestion.
/// </summary>
public sealed class ValidationSuggestion
{
    private readonly Dictionary<string, object> _metadata;

    /// <summary>
    /// Gets the unique identifier of the suggestion.
    /// </summary>
    public string SuggestionId { get; }

    /// <summary>
    /// Gets the suggestion message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the suggestion code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the suggestion source.
    /// </summary>
    public string Source { get; }

    /// <summary>
    /// Gets the suggestion priority.
    /// </summary>
    public ValidationPriority Priority { get; }

    /// <summary>
    /// Gets the suggestion impact.
    /// </summary>
    public ValidationImpact Impact { get; }

    /// <summary>
    /// Gets the suggestion category.
    /// </summary>
    public ValidationCategory Category { get; }

    /// <summary>
    /// Gets the suggestion scope.
    /// </summary>
    public ValidationScope Scope { get; }

    /// <summary>
    /// Gets when the suggestion was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets any additional metadata.
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata => new ReadOnlyDictionary<string, object>(_metadata);

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationSuggestion"/> class.
    /// </summary>
    /// <param name="message">The suggestion message.</param>
    /// <param name="code">The suggestion code.</param>
    /// <param name="source">The suggestion source.</param>
    /// <param name="priority">The suggestion priority.</param>
    /// <param name="impact">The suggestion impact.</param>
    /// <param name="category">The suggestion category.</param>
    /// <param name="scope">The suggestion scope.</param>
    /// <param name="metadata">Optional metadata dictionary.</param>
    /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
    public ValidationSuggestion(
        string message,
        string code,
        string source,
        ValidationPriority priority,
        ValidationImpact impact,
        ValidationCategory category,
        ValidationScope scope,
        Dictionary<string, object> metadata = null)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(source);

        SuggestionId = Guid.NewGuid().ToString();
        Message = message;
        Code = code;
        Source = source;
        Priority = priority;
        Impact = impact;
        Category = category;
        Scope = scope;
        CreatedAt = DateTime.UtcNow;
        _metadata = metadata ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// Creates a new suggestion with additional metadata.
    /// </summary>
    /// <param name="key">The metadata key.</param>
    /// <param name="value">The metadata value.</param>
    /// <returns>A new suggestion with the added metadata.</returns>
    /// <exception cref="ArgumentException">Thrown when key is null or whitespace.</exception>
    public ValidationSuggestion WithMetadata(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Metadata key cannot be null or whitespace", nameof(key));
        }

        var newMetadata = new Dictionary<string, object>(_metadata) { [key] = value };
        return new ValidationSuggestion(
            Message,
            Code,
            Source,
            Priority,
            Impact,
            Category,
            Scope,
            newMetadata);
    }

    /// <summary>
    /// Gets a metadata value from the suggestion.
    /// </summary>
    /// <param name="key">The metadata key.</param>
    /// <returns>The metadata value, or null if not found.</returns>
    /// <exception cref="ArgumentException">Thrown when key is null or whitespace.</exception>
    public object GetMetadata(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Metadata key cannot be null or whitespace", nameof(key));
        }

        return _metadata.TryGetValue(key, out var value) ? value : null;
    }
} 
