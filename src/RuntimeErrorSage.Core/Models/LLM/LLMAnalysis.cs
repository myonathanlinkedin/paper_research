using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.LLM
{
    /// <summary>
    /// Represents the result of an LLM analysis.
    /// </summary>
    public class LLMAnalysis
    {
        /// <summary>
        /// Gets or sets whether the analysis is valid.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the error message if analysis failed.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the analysis text.
        /// </summary>
        public string Analysis { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the suggested approach.
        /// </summary>
        public string Approach { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the strategy scores.
        /// </summary>
        public Dictionary<string, double> StrategyScores { get; set; } = new();

        /// <summary>
        /// Gets or sets the strategy explanations.
        /// </summary>
        public Dictionary<string, string> StrategyExplanations { get; set; } = new();

        /// <summary>
        /// Gets or sets additional metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class LLMRequest
    {
        public string Query { get; set; }
        public string Context { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
    }

    public class LLMResponse
    {
        public string ResponseId { get; set; }
        public string Content { get; set; }
        public double Confidence { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class LLMSuggestion
    {
        public string SuggestionId { get; set; }
        public string Content { get; set; }
        public double Confidence { get; set; }
        public List<string> Steps { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 