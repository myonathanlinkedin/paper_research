using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.LLM;

/// <summary>
/// Represents an analysis performed by a large language model.
/// </summary>
public class LLMAnalysis
{
    /// <summary>
    /// Gets or sets the unique identifier of the analysis.
    /// </summary>
    public string AnalysisId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the model name.
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the model version.
    /// </summary>
    public string ModelVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the analysis was performed.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the status of the analysis.
    /// </summary>
    public AnalysisStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the confidence level of the analysis (0.0 to 1.0).
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the error explanation.
    /// </summary>
    public string ErrorExplanation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the root cause analysis.
    /// </summary>
    public string RootCauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the suggested fixes.
    /// </summary>
    public List<string> SuggestedFixes { get; set; } = new();

    /// <summary>
    /// Gets or sets the code snippets.
    /// </summary>
    public List<CodeSnippet> CodeSnippets { get; set; } = new();

    /// <summary>
    /// Gets or sets the prevention suggestions.
    /// </summary>
    public List<string> PreventionSuggestions { get; set; } = new();

    /// <summary>
    /// Gets or sets the references.
    /// </summary>
    public List<string> References { get; set; } = new();

    /// <summary>
    /// Gets or sets the metrics of the analysis.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new();

    /// <summary>
    /// Gets or sets the metadata of the analysis.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 