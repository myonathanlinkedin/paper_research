using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Validation.Models
{
    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for this validation.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp when the validation was performed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets whether the validation was successful.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the validation status.
        /// </summary>
        public ValidationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the collection of validation errors.
        /// </summary>
        public List<ValidationError> Errors { get; set; } = new();

        /// <summary>
        /// Gets or sets the collection of validation warnings.
        /// </summary>
        public List<ValidationWarning> Warnings { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation score (0-1).
        /// </summary>
        public float Score { get; set; }

        /// <summary>
        /// Gets or sets the processing time in milliseconds.
        /// </summary>
        public long ProcessingTimeMs { get; set; }

        /// <summary>
        /// Gets or sets any additional metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Represents a validation rule.
    /// </summary>
    public class ValidationRule
    {
        /// <summary>
        /// Gets or sets the unique identifier of the rule.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the rule.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the severity level of the rule.
        /// </summary>
        public SeverityLevel Severity { get; set; }

        /// <summary>
        /// Gets or sets the validation function.
        /// </summary>
        public Func<object, Task<bool>>? ValidationFunction { get; set; }

        /// <summary>
        /// Gets or sets whether the rule is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the rule category.
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rule version.
        /// </summary>
        public string Version { get; set; } = "1.0.0";
    }

    /// <summary>
    /// Represents a validation error.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rule ID that generated this error.
        /// </summary>
        public string RuleId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the severity level of the error.
        /// </summary>
        public SeverityLevel Severity { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the error occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Represents a validation warning.
    /// </summary>
    public class ValidationWarning
    {
        /// <summary>
        /// Gets or sets the warning code.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the warning message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rule ID that generated this warning.
        /// </summary>
        public string RuleId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the warning occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Defines the validation status.
    /// </summary>
    public enum ValidationStatus
    {
        Valid,
        Invalid,
        Warning,
        Unknown
    }
} 