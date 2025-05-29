using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Models.Classification
{
    /// <summary>
    /// Represents the result of a classification operation.
    /// </summary>
    public class ClassificationResult
    {
        /// <summary>
        /// Gets or sets whether the classification was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the category of the classification.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the confidence level of the classification.
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the error message if the classification failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets additional metadata about the classification.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassificationResult"/> class.
        /// </summary>
        public ClassificationResult()
        {
            Metadata = new Dictionary<string, object>();
        }
    }
} 