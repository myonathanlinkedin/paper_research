using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Validation
{
    /// <summary>
    /// Represents the result of a validation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets or sets the ID of the action that was validated.
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the validation.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets whether the validation was successful.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the list of validation errors.
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the list of validation warnings.
        /// </summary>
        public List<string> Warnings { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the list of validation rules that were applied.
        /// </summary>
        public List<string> ValidationRules { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the validation duration in milliseconds.
        /// </summary>
        public long DurationMs { get; set; }

        /// <summary>
        /// Gets or sets the validation messages.
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets additional metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the severity of the validation result.
        /// </summary>
        public ValidationSeverity Severity { get; set; } = ValidationSeverity.Info;

        /// <summary>
        /// Gets or sets the validation status.
        /// </summary>
        public ValidationStatus Status { get; set; } = ValidationStatus.Pending;

        /// <summary>
        /// Gets or sets the validation context.
        /// </summary>
        public ValidationContext Context { get; set; }

        /// <summary>
        /// Gets or sets the validation suggestions.
        /// </summary>
        public List<ValidationSuggestion> Suggestions { get; set; } = new List<ValidationSuggestion>();

        /// <summary>
        /// Gets or sets the validation metrics.
        /// </summary>
        public MetricsValidation Metrics { get; set; }

        /// <summary>
        /// Gets or sets the correlation ID for tracking related validations.
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        public ValidationResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="isValid">Whether the validation was successful.</param>
        public ValidationResult(bool isValid)
        {
            IsValid = isValid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="isValid">Whether the validation was successful.</param>
        /// <param name="message">The validation message.</param>
        public ValidationResult(bool isValid, string message)
        {
            IsValid = isValid;
            if (!string.IsNullOrEmpty(message))
            {
                Messages.Add(message);
            }
        }

        /// <summary>
        /// Adds an error to the validation result.
        /// </summary>
        /// <param name="error">The error to add.</param>
        public void AddError(ValidationError error)
        {
            ArgumentNullException.ThrowIfNull(error);
            Errors.Add(error.Message);
            IsValid = false;
        }

        /// <summary>
        /// Adds a warning to the validation result.
        /// </summary>
        /// <param name="warning">The warning to add.</param>
        public void AddWarning(ValidationWarning warning)
        {
            ArgumentNullException.ThrowIfNull(warning);
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

            Metadata[key] = value;
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
            foreach (var metadata in other.Metadata)
            {
                Metadata[metadata.Key] = metadata.Value;
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
            return new ValidationResult { IsValid = true, Messages = new List<string> { message } };
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

                foreach (var metadata in result.Metadata)
                {
                    combinedResult.Metadata[metadata.Key] = metadata.Value;
                }
            }
            return combinedResult;
        }
    }
} 


