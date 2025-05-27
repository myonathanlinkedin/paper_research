using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.LLM
{
    /// <summary>
    /// Represents a request to the LLM for processing.
    /// </summary>
    public class LLMRequest
    {
        /// <summary>
        /// Gets or sets the query text to send to the LLM.
        /// </summary>
        public string Query { get; set; }
        
        /// <summary>
        /// Gets or sets the context information to help the LLM understand the query.
        /// </summary>
        public string Context { get; set; }
        
        /// <summary>
        /// Gets or sets additional parameters for the LLM request.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
} 