using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Validation
{
    /// <summary>
    /// Represents the result of a validation.
    /// </summary>
    public sealed class ValidationResult
    {
        private readonly List<string> _errors = new();
        private readonly List<string> _warnings = new();
        private readonly List<string> _validationRules = new();
        private readonly List<string> _messages = new();
        private readonly Dictionary<string, object> _metadata = new();
        private readonly List<ValidationSuggestion> _suggestions = new();
        private readonly string _actionId;
        private readonly DateTime _timestamp = DateTime.UtcNow;
        private readonly bool _isValid;
        private readonly long _durationMs;
        private readonly ValidationSeverity _severity;
        private readonly ValidationStatus _status;
        private readonly ValidationContext _context;
        private readonly MetricsValidation _metrics;
        private readonly string _correlationId;

        /// <summary>
        /// Gets the ID of the action that was validated.
        /// </summary>
        public string ActionId => _actionId;

        /// <summary>
        /// Gets the timestamp of the validation.
        /// </summary>
        public DateTime Timestamp => _timestamp;

        /// <summary>
        /// Gets whether the validation was successful.
        /// </summary>
        public bool IsValid => _isValid;

        /// <summary>
        /// Gets the list of validation errors.
        /// </summary>
        public IReadOnlyList<string> Errors => new ReadOnlyCollection<string>(_errors);

        /// <summary>
        /// Gets the list of validation warnings.
        /// </summary>
        public IReadOnlyList<string> Warnings => new ReadOnlyCollection<string>(_warnings);

        /// <summary>
        /// Gets the list of validation rules that were applied.
        /// </summary>
        public IReadOnlyList<string> ValidationRules => new ReadOnlyCollection<string>(_validationRules);

        /// <summary>
        /// Gets the validation duration in milliseconds.
        /// </summary>
        public long DurationMs => _durationMs;

        /// <summary>
        /// Gets the validation messages.
        /// </summary>
        public IReadOnlyList<string> Messages => new ReadOnlyCollection<string>(_messages);

        /// <summary>
        /// Gets the additional metadata.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata => new ReadOnlyDictionary<string, object>(_metadata);

        /// <summary>
        /// Gets the severity of the validation result.
        /// </summary>
        public ValidationSeverity Severity => _severity;

        /// <summary>
        /// Gets the validation status.
        /// </summary>
        public ValidationStatus Status => _status;

        /// <summary>
        /// Gets the validation context.
        /// </summary>
        public ValidationContext Context => _context;

        /// <summary>
        /// Gets the validation suggestions.
        /// </summary>
        public IReadOnlyList<ValidationSuggestion> Suggestions => new ReadOnlyCollection<ValidationSuggestion>(_suggestions);

        /// <summary>
        /// Gets the validation metrics.
        /// </summary>
        public MetricsValidation Metrics => _metrics;

        /// <summary>
        /// Gets the correlation ID for tracking related validations.
        /// </summary>
        public string CorrelationId => _correlationId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <param name="isValid">Whether the validation was successful.</param>
        /// <param name="severity">The validation severity.</param>
        /// <param name="status">The validation status.</param>
        /// <param name="metrics">The validation metrics.</param>
        /// <param name="correlationId">The correlation ID.</param>
        /// <exception cref="ArgumentNullException">Thrown when context is null.</exception>
        public ValidationResult(
            ValidationContext context,
            bool isValid = true,
            ValidationSeverity severity = ValidationSeverity.Info,
            ValidationStatus status = ValidationStatus.Pending,
            MetricsValidation metrics = null,
            string correlationId = null)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _isValid = isValid;
            _severity = severity;
            _status = status;
            _metrics = metrics ?? new MetricsValidation();
            _correlationId = correlationId ?? Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Creates a successful validation result.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <param name="message">Optional success message.</param>
        /// <returns>A successful validation result.</returns>
        public static ValidationResult Success(ValidationContext context, string message = null)
        {
            var result = new ValidationResult(context, true, ValidationSeverity.Info, ValidationStatus.Success);
            if (!string.IsNullOrEmpty(message))
            {
                result._messages.Add(message);
            }
            return result;
        }

        /// <summary>
        /// Creates a failed validation result.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="severity">The validation severity.</param>
        /// <returns>A failed validation result.</returns>
        public static ValidationResult Failure(
            ValidationContext context,
            string errorMessage,
            ValidationSeverity severity = ValidationSeverity.Error)
        {
            var result = new ValidationResult(context, false, severity, ValidationStatus.Failed);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                result._errors.Add(errorMessage);
            }
            return result;
        }

        /// <summary>
        /// Combines multiple validation results.
        /// </summary>
        /// <param name="results">The validation results to combine.</param>
        /// <returns>A combined validation result.</returns>
        public static ValidationResult Combine(IEnumerable<ValidationResult> results)
        {
            if (results == null || !results.Any())
            {
                throw new ArgumentException("At least one validation result is required", nameof(results));
            }

            var firstResult = results.First();
            var combinedResult = new ValidationResult(
                firstResult._context,
                results.All(r => r._isValid),
                results.Max(r => r._severity),
                results.Any(r => r._status == ValidationStatus.Failed) ? ValidationStatus.Failed : ValidationStatus.Success
            );

            foreach (var result in results)
            {
                combinedResult._errors.AddRange(result._errors);
                combinedResult._warnings.AddRange(result._warnings);
                combinedResult._messages.AddRange(result._messages);
                combinedResult._validationRules.AddRange(result._validationRules);
                combinedResult._suggestions.AddRange(result._suggestions);

                foreach (var kvp in result._metadata)
                {
                    combinedResult._metadata[kvp.Key] = kvp.Value;
                }
            }

            return combinedResult;
        }
    }
} 


