using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Models.LLM;

/// <summary>
/// Represents a code snippet with contextual information.
/// </summary>
public class CodeSnippet
{
    /// <summary>
    /// Gets or sets the unique identifier of the snippet.
    /// </summary>
    public string SnippetId { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the code content.
    /// </summary>
    public string Code { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the file path.
    /// </summary>
    public string FilePath { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the starting line number.
    /// </summary>
    public int StartLine { get; }

    /// <summary>
    /// Gets or sets the ending line number.
    /// </summary>
    public int EndLine { get; }

    /// <summary>
    /// Gets or sets the language of the code.
    /// </summary>
    public string Language { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the code snippet.
    /// </summary>
    public string Description { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the relevance score (0.0 to 1.0).
    /// </summary>
    public double Relevance { get; }

    /// <summary>
    /// Gets or sets the explanation of the snippet.
    /// </summary>
    public string Explanation { get; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this is the problematic code.
    /// </summary>
    public bool IsProblematic { get; }

    /// <summary>
    /// Gets or sets whether this is a suggested fix.
    /// </summary>
    public bool IsSuggestedFix { get; }

    /// <summary>
    /// Gets or sets whether this snippet contains the error.
    /// </summary>
    public bool ContainsError { get; }

    /// <summary>
    /// Gets or sets the timestamp when the snippet was created.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;
} 






