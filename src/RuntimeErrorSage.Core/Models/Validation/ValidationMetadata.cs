using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Validation
{
    /// <summary>
    /// Represents metadata for a validation operation.
    /// </summary>
    public class ValidationMetadata
    {
        /// <summary>
        /// Gets or sets the unique identifier of the validation.
        /// </summary>
        public string ValidationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the validator.
        /// </summary>
        public string ValidatorName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version of the validator.
        /// </summary>
        public string ValidatorVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of validation.
        /// </summary>
        public ValidationType Type { get; set; }

        /// <summary>
        /// Gets or sets the scope of validation.
        /// </summary>
        public ValidationScope Scope { get; set; }

        /// <summary>
        /// Gets or sets the level of validation.
        /// </summary>
        public ValidationLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the category of validation.
        /// </summary>
        public ValidationCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the stage of validation.
        /// </summary>
        public ValidationStage Stage { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the validation was performed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the duration of the validation in milliseconds.
        /// </summary>
        public double DurationMs { get; set; }

        /// <summary>
        /// Gets or sets any additional metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}