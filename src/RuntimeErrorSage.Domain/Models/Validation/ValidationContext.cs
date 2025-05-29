using System.Collections.ObjectModel;
using RuntimeErrorSage.Domain.Enums;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Validation
{
    /// <summary>
    /// Represents the context for validation operations.
    /// </summary>
    public class ValidationContext
    {
        /// <summary>
        /// Gets or sets the unique identifier for this context.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the object being validated.
        /// </summary>
        public object Target { get; }

        /// <summary>
        /// Gets or sets the validation type.
        /// </summary>
        public ValidationType Type { get; }

        /// <summary>
        /// Gets or sets the validation scope.
        /// </summary>
        public ValidationScope Scope { get; }

        /// <summary>
        /// Gets or sets the validation level.
        /// </summary>
        public ValidationLevel Level { get; }

        /// <summary>
        /// Gets or sets the validation category.
        /// </summary>
        public ValidationCategory Category { get; }

        /// <summary>
        /// Gets or sets the validation stage.
        /// </summary>
        public ValidationStage Stage { get; }

        /// <summary>
        /// Gets or sets additional validation parameters.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation timestamp.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets whether to stop on first error.
        /// </summary>
        public bool StopOnFirstError { get; }

        /// <summary>
        /// Gets or sets whether to validate recursively.
        /// </summary>
        public bool ValidateRecursively { get; }

        /// <summary>
        /// Gets or sets the validation timeout in seconds.
        /// </summary>
        public int TimeoutSeconds { get; } = 30;

        /// <summary>
        /// Gets or sets the validation metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 






