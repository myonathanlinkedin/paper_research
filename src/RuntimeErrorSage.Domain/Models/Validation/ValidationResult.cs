using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Validation
{
    /// <summary>
    /// Represents the result of a validation.
    /// </summary>
    public sealed class ValidationResult
    {
        private readonly List<string> _errors = new();
        private readonly List<string> _warnings = new();
        private readonly List<string> _validationRules = new();
        private List<string> _messages = new();
        private readonly Dictionary<string, object> _metadata = new();
        private Dictionary<string, object> _details = new();
        private readonly List<ValidationSuggestion> _suggestions = new();
        private string _actionId;
        private DateTime _timestamp = DateTime.UtcNow;
        private bool _isValid;
        private long _durationMs;
        private ValidationSeverity _severity;
        private ValidationStatus _status;
        private readonly ValidationContext _context;
        private readonly MetricsValidation _metrics;
        private string _correlationId;
        private string _message;
        private DateTime _startTime = DateTime.UtcNow;
        private DateTime? _endTime;
        private string _strategyId;
        private string _strategyName;

        /// <summary>
        /// Gets or sets the ID of the action that was validated.
        /// </summary>
        public string ActionId
        {
            get => _actionId;
            set => _actionId = value;
        }

        /// <summary>
        /// Gets or sets the timestamp of the validation.
        /// </summary>
        public DateTime Timestamp
        {
            get => _timestamp;
            set => _timestamp = value;
        }

        /// <summary>
        /// Gets or sets whether the validation was successful.
        /// </summary>
        public bool IsValid
        {
            get => _isValid;
            set => _isValid = value;
        }

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
        /// Gets or sets the validation duration in milliseconds.
        /// </summary>
        public long DurationMs
        {
            get => _durationMs;
            set => _durationMs = value;
        }

        /// <summary>
        /// Gets or sets the validation messages.
        /// </summary>
        public List<string> Messages
        {
            get => _messages;
            set => _messages = value ?? new List<string>();
        }

        /// <summary>
        /// Gets the additional metadata.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata => new ReadOnlyDictionary<string, object>(_metadata);

        /// <summary>
        /// Gets or sets the additional validation details.
        /// </summary>
        public Dictionary<string, object> Details
        {
            get => _details;
            set => _details = value ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the severity of the validation result.
        /// </summary>
        public ValidationSeverity Severity
        {
            get => _severity;
            set => _severity = value;
        }

        /// <summary>
        /// Gets or sets the validation status.
        /// </summary>
        public ValidationStatus Status
        {
            get => _status;
            set => _status = value;
        }

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
        /// Gets or sets the validation message.
        /// </summary>
        public string Message
        {
            get => _message;
            set => _message = value;
        }

        /// <summary>
        /// Gets or sets the start time of validation.
        /// </summary>
        public DateTime StartTime
        {
            get => _startTime;
            set => _startTime = value;
        }

        /// <summary>
        /// Gets or sets the end time of validation.
        /// </summary>
        public DateTime? EndTime
        {
            get => _endTime;
            set => _endTime = value;
        }

        /// <summary>
        /// Gets or sets the strategy ID associated with the validation.
        /// </summary>
        public string StrategyId
        {
            get => _strategyId;
            set => _strategyId = value;
        }

        /// <summary>
        /// Gets or sets the strategy name associated with the validation.
        /// </summary>
        public string StrategyName
        {
            get => _strategyName;
            set => _strategyName = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class with default values.
        /// </summary>
        public ValidationResult()
            : this(new ValidationContext(), true, ValidationSeverity.Info, ValidationStatus.Pending, null, null)
        {
        }

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
            _message = string.Empty;
        }

        /// <summary>
        /// Adds an error message to the validation result.
        /// </summary>
        /// <param name="error">The error message to add.</param>
        public void AddError(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                _errors.Add(error);
                _isValid = false;
            }
        }

        /// <summary>
        /// Adds a validation error to the validation result.
        /// </summary>
        /// <param name="error">The validation error to add.</param>
        public void AddError(ValidationError error)
        {
            ArgumentNullException.ThrowIfNull(error);
            _errors.Add(error.Message);
            _isValid = false;
        }

        /// <summary>
        /// Adds an error to the validation result with specified message and severity.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="severity">The error severity.</param>
        public void AddError(string message, ValidationSeverity severity)
        {
            var error = new ValidationError(message, severity);
            _errors.Add(error.Message);
            _isValid = false;
        }

        /// <summary>
        /// Adds a warning message to the validation result.
        /// </summary>
        /// <param name="warning">The warning message to add.</param>
        public void AddWarning(string warning)
        {
            if (!string.IsNullOrEmpty(warning))
            {
                _warnings.Add(warning);
            }
        }

        /// <summary>
        /// Adds a validation warning to the validation result.
        /// </summary>
        /// <param name="warning">The validation warning to add.</param>
        public void AddWarning(ValidationWarning warning)
        {
            ArgumentNullException.ThrowIfNull(warning);
            _warnings.Add(warning.Message);
        }

        /// <summary>
        /// Adds a warning to the validation result with specified message and severity.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="severity">The warning severity.</param>
        public void AddWarning(string message, ValidationSeverity severity)
        {
            var warning = new ValidationWarning(message, severity);
            _warnings.Add(warning.Message);
        }

        /// <summary>
        /// Adds a validation rule to the result.
        /// </summary>
        /// <param name="rule">The validation rule to add.</param>
        public void AddValidationRule(string rule)
        {
            if (!string.IsNullOrEmpty(rule))
            {
                _validationRules.Add(rule);
            }
        }

        /// <summary>
        /// Adds a message to the validation result.
        /// </summary>
        /// <param name="message">The message to add.</param>
        public void AddMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            _messages.Add(message);
        }

        /// <summary>
        /// Adds multiple messages to the validation result.
        /// </summary>
        /// <param name="messages">The messages to add.</param>
        public void AddMessages(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            foreach (var message in messages)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    _messages.Add(message);
                }
            }
        }

        /// <summary>
        /// Clears all messages from the validation result.
        /// </summary>
        public void ClearMessages()
        {
            _messages.Clear();
        }

        /// <summary>
        /// Sets the messages collection to a new list of messages.
        /// </summary>
        /// <param name="messages">The messages to set.</param>
        public void SetMessages(IEnumerable<string> messages)
        {
            ArgumentNullException.ThrowIfNull(messages);
            
            _messages.Clear();
            foreach (var message in messages)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    _messages.Add(message);
                }
            }
        }

        /// <summary>
        /// Adds metadata to the validation result.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        public void AddMetadata(string key, object value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _metadata[key] = value;
            }
        }

        /// <summary>
        /// Adds a validation suggestion.
        /// </summary>
        /// <param name="suggestion">The suggestion to add.</param>
        public void AddSuggestion(ValidationSuggestion suggestion)
        {
            if (suggestion != null)
            {
                _suggestions.Add(suggestion);
            }
        }

        /// <summary>
        /// Merges another validation result into this one.
        /// </summary>
        /// <param name="other">The validation result to merge.</param>
        /// <returns>This validation result instance for chaining.</returns>
        public ValidationResult Merge(ValidationResult other)
        {
            ArgumentNullException.ThrowIfNull(other);
            
            // Merge properties
            _isValid = _isValid && other.IsValid;
            _severity = other.Severity > _severity ? other.Severity : _severity;
            _status = other.Status == ValidationStatus.Failed ? ValidationStatus.Failed : _status;
            
            // Merge collections
            foreach (var error in other.Errors)
            {
                _errors.Add(error);
            }
            
            foreach (var warning in other.Warnings)
            {
                _warnings.Add(warning);
            }
            
            foreach (var message in other.Messages)
            {
                _messages.Add(message);
            }
            
            foreach (var rule in other.ValidationRules)
            {
                _validationRules.Add(rule);
            }
            
            foreach (var suggestion in other.Suggestions)
            {
                _suggestions.Add(suggestion);
            }
            
            foreach (var kvp in other.Metadata)
            {
                _metadata[kvp.Key] = kvp.Value;
            }
            
            return this;
        }

        /// <summary>
        /// Creates a successful validation result.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <param name="message">Optional success message.</param>
        /// <returns>A successful validation result.</returns>
        public static ValidationResult Success(ValidationContext context, string message = null)
        {
            var result = new ValidationResult(context, true, ValidationSeverity.Info, ValidationStatus.Completed);
            if (!string.IsNullOrEmpty(message))
            {
                result.AddMessage(message);
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
                result.AddError(errorMessage);
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
                results.Any(r => r._status == ValidationStatus.Failed) ? ValidationStatus.Failed : ValidationStatus.Completed
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

        /// <summary>
        /// Sets the correlation ID for tracking related validations.
        /// </summary>
        /// <param name="correlationId">The correlation ID to set.</param>
        public void SetCorrelationId(string correlationId)
        {
            _correlationId = correlationId ?? Guid.NewGuid().ToString();
        }
    }
} 


