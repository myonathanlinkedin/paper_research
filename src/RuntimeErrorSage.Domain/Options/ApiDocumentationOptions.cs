using System;

namespace RuntimeErrorSage.Application.Options
{
    /// <summary>
    /// Configuration options for API documentation.
    /// Required by research for implementation completeness.
    /// </summary>
    public class ApiDocumentationOptions
    {
        /// <summary>
        /// Gets or sets whether to generate API documentation.
        /// Required by research for implementation completeness.
        /// </summary>
        public bool GenerateApiDocs { get; set; }

        /// <summary>
        /// Gets or sets whether to generate integration patterns.
        /// Required by research for implementation completeness.
        /// </summary>
        public bool GenerateIntegrationPatterns { get; set; }

        /// <summary>
        /// Gets or sets whether to include code examples.
        /// Required by research for implementation completeness.
        /// </summary>
        public bool IncludeCodeExamples { get; set; }

        /// <summary>
        /// Gets or sets whether to include performance guidelines.
        /// Required by research for implementation completeness.
        /// </summary>
        public bool IncludePerformanceGuidelines { get; set; }
    }
} 
