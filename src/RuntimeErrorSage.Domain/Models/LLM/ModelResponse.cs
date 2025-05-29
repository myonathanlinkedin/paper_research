using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.LLM
{
    /// <summary>
    /// Represents a response from the LLM model.
    /// </summary>
    public class ModelResponse
    {
        /// <summary>
        /// Gets or sets the response text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets or sets the confidence score (0-1).
        /// </summary>
        public double Confidence { get; }

        /// <summary>
        /// Gets or sets the response metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the response tokens.
        /// </summary>
        public IReadOnlyCollection<Tokens> Tokens { get; } = new();

        /// <summary>
        /// Gets or sets the response latency in milliseconds.
        /// </summary>
        public double Latency { get; }

        /// <summary>
        /// Gets or sets whether the response was successful.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets or sets the error message if any.
        /// </summary>
        public string ErrorMessage { get; }
    }
} 






