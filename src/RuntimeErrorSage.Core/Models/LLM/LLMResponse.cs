using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.LLM
{
    /// <summary>
    /// Represents a response from the LLM.
    /// </summary>
    public class LLMResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for this response.
        /// </summary>
        public string ResponseId { get; set; }
        
        /// <summary>
        /// Gets or sets the content returned from the LLM.
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// Gets or sets the confidence score (0-1) for this response.
        /// </summary>
        public double Confidence { get; set; }
        
        /// <summary>
        /// Gets or sets additional metadata about the response.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 