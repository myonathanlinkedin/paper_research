using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Validation
{
    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets or sets whether the validation passed.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        public string ValidationMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start time of validation.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time of validation.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the validation status.
        /// </summary>
        public AnalysisValidationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the validation details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of validation errors.
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of validation warnings.
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation metadata.
        /// </summary>
        public ValidationMetadata ValidationMetadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation code.
        /// </summary>
        public string Code { get; set; } = string.Empty;

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
        /// Gets or sets the validation duration.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Gets or sets the validator identifier.
        /// </summary>
        public string ValidatorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the validation requires manual review.
        /// </summary>
        public bool RequiresManualReview { get; set; }

        /// <summary>
        /// Gets or sets the validation recommendations.
        /// </summary>
        public List<string> Recommendations { get; set; } = new();

        /// <summary>
        /// Gets or sets the error ID this validation is associated with.
        /// </summary>
        public string ErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the remediation ID this validation is associated with.
        /// </summary>
        public string RemediationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation context.
        /// </summary>
        public string Context { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation severity.
        /// </summary>
        public string Severity { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        public ValidationResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="isValid">Whether the validation passed.</param>
        public ValidationResult(bool isValid)
        {
            IsValid = isValid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="isValid">Whether the validation passed.</param>
        /// <param name="errors">The validation errors.</param>
        public ValidationResult(bool isValid, List<ValidationError> errors)
        {
            IsValid = isValid;
            foreach (var error in errors)
            {
                Errors.Add(error.Message);
            }
        }

        /// <summary>
        /// Adds an error to the validation result.
        /// </summary>
        /// <param name="error">The error to add.</param>
        public void AddError(ValidationError error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            Errors.Add(error.Message);
            IsValid = false;
        }

        /// <summary>
        /// Adds a warning to the validation result.
        /// </summary>
        /// <param name="warning">The warning to add.</param>
        public void AddWarning(ValidationWarning warning)
        {
            if (warning == null)
            {
                throw new ArgumentNullException(nameof(warning));
            }

            Warnings.Add(warning.Message);
        }

        /// <summary>
        /// Adds metadata to the validation result.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        public void AddMetadata(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Metadata key cannot be null or whitespace", nameof(key));
            }

            Details[key] = value;
        }

        /// <summary>
        /// Merges another validation result into this one.
        /// </summary>
        /// <param name="other">The other validation result.</param>
        public void Merge(ValidationResult other)
        {
            if (other == null)
            {
                return;
            }

            IsValid = IsValid && other.IsValid;
            foreach (var error in other.Errors)
            {
                Errors.Add(error);
            }
            foreach (var warning in other.Warnings)
            {
                Warnings.Add(warning);
            }
            foreach (var metadata in other.Details)
            {
                Details[metadata.Key] = metadata.Value;
            }
        }

        /// <summary>
        /// Creates a successful validation result.
        /// </summary>
        /// <returns>A successful validation result.</returns>
        public static ValidationResult Success()
        {
            return new ValidationResult(true);
        }

        /// <summary>
        /// Creates a successful validation result with a message.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <returns>A successful validation result.</returns>
        public static ValidationResult Success(string message)
        {
            return new ValidationResult { IsValid = true, ValidationMessage = message };
        }

        /// <summary>
        /// Creates a failed validation result with the specified error.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>A failed validation result.</returns>
        public static ValidationResult Failure(string errorMessage)
        {
            var result = new ValidationResult(false);
            result.AddError(new ValidationError(errorMessage));
            return result;
        }

        /// <summary>
        /// Combines multiple validation results into a single result.
        /// </summary>
        /// <param name="results">The validation results to combine.</param>
        /// <returns>A combined validation result.</returns>
        public static ValidationResult Combine(IEnumerable<ValidationResult> results)
        {
            var combinedResult = new ValidationResult();
            foreach (var result in results)
            {
                if (result == null)
                {
                    continue;
                }

                if (!result.IsValid)
                {
                    combinedResult.IsValid = false;
                }

                foreach (var error in result.Errors)
                {
                    combinedResult.Errors.Add(error);
                }

                foreach (var warning in result.Warnings)
                {
                    combinedResult.Warnings.Add(warning);
                }

                foreach (var metadata in result.Details)
                {
                    combinedResult.Details[metadata.Key] = metadata.Value;
                }
            }
            return combinedResult;
        }
    }
} 
