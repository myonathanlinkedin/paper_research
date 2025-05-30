using System;

namespace RuntimeErrorSage.Domain.Models.Validation
{
    /// <summary>
    /// Represents a suggestion for validation improvements.
    /// </summary>
    public class ValidationSuggestion
    {
        /// <summary>
        /// Gets or sets the unique identifier for this suggestion.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the description of the suggestion.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the priority of the suggestion.
        /// </summary>
        public string Priority { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category of the suggestion.
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the impact of implementing the suggestion.
        /// </summary>
        public string Impact { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the effort required to implement the suggestion.
        /// </summary>
        public string Effort { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the code example for implementing the suggestion.
        /// </summary>
        public string CodeExample { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation rule that triggered this suggestion.
        /// </summary>
        public string ValidationRule { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the suggestion has been implemented.
        /// </summary>
        public bool IsImplemented { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the suggestion was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the suggestion was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
} 
