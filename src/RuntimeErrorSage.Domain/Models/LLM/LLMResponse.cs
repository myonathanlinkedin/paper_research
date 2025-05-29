using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Models.LLM
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

        public string RequestId { get; set; } = string.Empty;
        public string CorrelationId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
} 
