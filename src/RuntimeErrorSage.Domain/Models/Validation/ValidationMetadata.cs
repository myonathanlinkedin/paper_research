using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Validation
{
    /// <summary>
    /// Represents metadata for a validation operation.
    /// </summary>
    public class ValidationMetadata
    {
        /// <summary>
        /// Gets or sets the unique identifier of the validation.
        /// </summary>
        public string ValidationId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the validator.
        /// </summary>
        public string ValidatorName { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the version of the validator.
        /// </summary>
        public string ValidatorVersion { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of validation.
        /// </summary>
        public ValidationType Type { get; }

        /// <summary>
        /// Gets or sets the scope of validation.
        /// </summary>
        public ValidationScope Scope { get; }

        /// <summary>
        /// Gets or sets the level of validation.
        /// </summary>
        public ValidationLevel Level { get; }

        /// <summary>
        /// Gets or sets the category of validation.
        /// </summary>
        public ValidationCategory Category { get; }

        /// <summary>
        /// Gets or sets the stage of validation.
        /// </summary>
        public ValidationStage Stage { get; }

        /// <summary>
        /// Gets or sets the timestamp when the validation was performed.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the duration of the validation in milliseconds.
        /// </summary>
        public double DurationMs { get; }

        /// <summary>
        /// Gets or sets any additional metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}






