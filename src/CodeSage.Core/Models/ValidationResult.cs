using System.Collections.Generic;

namespace CodeSage.Core.Models
{
    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets or sets whether the validation was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional validation details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();
    }
} 