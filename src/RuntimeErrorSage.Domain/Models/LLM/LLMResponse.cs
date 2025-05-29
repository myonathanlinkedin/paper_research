using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.LLM
{
    /// <summary>
    /// Represents a response from the LLM.
    /// </summary>
    public class LLMResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for this response.
        /// </summary>
        public string ResponseId { get; }
        
        /// <summary>
        /// Gets or sets the content returned from the LLM.
        /// </summary>
        public string Content { get; }
        
        /// <summary>
        /// Gets or sets the confidence score (0-1) for this response.
        /// </summary>
        public double Confidence { get; }
        
        /// <summary>
        /// Gets or sets additional metadata about the response.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        public string RequestId { get; } = string.Empty;
        public string CorrelationId { get; } = string.Empty;
        public DateTime Timestamp { get; }
        public string Model { get; } = string.Empty;
        public string Response { get; } = string.Empty;
        public bool IsSuccessful { get; }
        public string ErrorMessage { get; } = string.Empty;
    }
} 






