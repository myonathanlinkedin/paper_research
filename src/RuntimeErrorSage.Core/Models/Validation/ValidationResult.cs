using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Models.Validation
{
    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for this validation result.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets whether the validation passed.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the validation type.
        /// </summary>
        public ValidationType Type { get; set; }

        /// <summary>
        /// Gets or sets the validation scope.
        /// </summary>
        public ValidationScope Scope { get; set; }

        /// <summary>
        /// Gets or sets the validation level.
        /// </summary>
        public ValidationLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the validation category.
        /// </summary>
        public ValidationCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the validation stage.
        /// </summary>
        public ValidationStage Stage { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        public ReadOnlyCollection<ValidationError> Errors { get; }

        /// <summary>
        /// Gets the validation warnings.
        /// </summary>
        public ReadOnlyCollection<ValidationWarning> Warnings { get; }

        /// <summary>
        /// Gets or sets when the validation was performed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the duration of validation in seconds.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Gets or sets the validator that performed the validation.
        /// </summary>
        public string ValidatorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional validation details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();

        /// <summary>
        /// Gets or sets whether manual review is required.
        /// </summary>
        public bool RequiresManualReview { get; set; }

        /// <summary>
        /// Gets or sets any recommendations for fixing validation issues.
        /// </summary>
        public List<string> Recommendations { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation context.
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets additional validation metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        public ValidationResult(IList<ValidationError> errors = null, IList<ValidationWarning> warnings = null)
        {
            Errors = new ReadOnlyCollection<ValidationError>(errors ?? new List<ValidationError>());
            Warnings = new ReadOnlyCollection<ValidationWarning>(warnings ?? new List<ValidationWarning>());
            IsValid = Errors.Count == 0;
        }

        public void AddError(string error)
        {
            Errors.Add(new ValidationError(error));
            IsValid = false;
        }

        public void AddWarning(string warning)
        {
            Warnings.Add(new ValidationWarning { Code = warning, Message = warning });
        }

        public void AddMetadata(string key, object value)
        {
            Metadata[key] = value;
        }

        public static ValidationResult Success()
        {
            return new ValidationResult { IsValid = true };
        }

        public static ValidationResult Failure(string error)
        {
            var result = new ValidationResult { IsValid = false };
            result.AddError(error);
            return result;
        }
    }

    /// <summary>
    /// Represents a validation warning.
    /// </summary>
    public class ValidationWarning
    {
        /// <summary>
        /// Gets or sets the warning code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the warning message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the warning source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets additional warning details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();
    }

    /// <summary>
    /// Represents the severity of a validation error.
    /// </summary>
    public enum ValidationSeverity
    {
        /// <summary>
        /// The validation error is informational.
        /// </summary>
        Info,

        /// <summary>
        /// The validation error is a warning.
        /// </summary>
        Warning,

        /// <summary>
        /// The validation error is an error.
        /// </summary>
        Error,

        /// <summary>
        /// The validation error is critical.
        /// </summary>
        Critical
    }
} 
