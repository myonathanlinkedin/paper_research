using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.LLM
{
    /// <summary>
    /// Represents a response from the LLM model.
    /// </summary>
    public class ModelResponse
    {
        public string ResponseId { get; set; }
        public string ModelId { get; set; }
        public string Content { get; set; }
        public double ConfidenceScore { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public DateTime Timestamp { get; set; }
        public long ProcessingTimeMs { get; set; }
    }
} 