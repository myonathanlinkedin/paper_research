using System.Collections.ObjectModel;
using RuntimeErrorSage.Domain.Enums;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Validation;

/// <summary>
/// Represents a validation suggestion.
/// </summary>
public class ValidationSuggestion
{
    /// <summary>
    /// Gets or sets the unique identifier of the suggestion.
    /// </summary>
    public string SuggestionId { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the suggestion message.
    /// </summary>
    public string Message { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the suggestion code.
    /// </summary>
    public string Code { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the suggestion source.
    /// </summary>
    public string Source { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the suggestion priority.
    /// </summary>
    public ValidationPriority Priority { get; }

    /// <summary>
    /// Gets or sets the suggestion impact.
    /// </summary>
    public ValidationImpact Impact { get; }

    /// <summary>
    /// Gets or sets the suggestion category.
    /// </summary>
    public ValidationCategory Category { get; }

    /// <summary>
    /// Gets or sets the suggestion scope.
    /// </summary>
    public ValidationScope Scope { get; }

    /// <summary>
    /// Gets or sets when the suggestion was created.
    /// </summary>
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets when the suggestion was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets any additional metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 






