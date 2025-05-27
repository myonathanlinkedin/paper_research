using System;

namespace RuntimeErrorSage.Core.Models.LLM;

/// <summary>
/// Represents a code snippet with contextual information.
/// </summary>
public class CodeSnippet
{
    /// <summary>
    /// Gets or sets the unique identifier of the snippet.
    /// </summary>
    public string SnippetId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the code content.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file path.
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the starting line number.
    /// </summary>
    public int StartLine { get; set; }

    /// <summary>
    /// Gets or sets the ending line number.
    /// </summary>
    public int EndLine { get; set; }

    /// <summary>
    /// Gets or sets the language of the code.
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the code snippet.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the relevance score (0.0 to 1.0).
    /// </summary>
    public double Relevance { get; set; }

    /// <summary>
    /// Gets or sets the explanation of the snippet.
    /// </summary>
    public string Explanation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this is the problematic code.
    /// </summary>
    public bool IsProblematic { get; set; }

    /// <summary>
    /// Gets or sets whether this is a suggested fix.
    /// </summary>
    public bool IsSuggestedFix { get; set; }

    /// <summary>
    /// Gets or sets whether this snippet contains the error.
    /// </summary>
    public bool ContainsError { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the snippet was created.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
} 