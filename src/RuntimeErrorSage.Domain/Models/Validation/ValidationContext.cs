using RuntimeErrorSage.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RuntimeErrorSage.Domain.Models.Validation
{
    /// <summary>
    /// Represents the context for validation operations.
    /// </summary>
    public class ValidationContext
    {
        /// <summary>
        /// Gets or sets the unique identifier for this validation context.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp when the validation was performed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the validation rules that were applied.
        /// </summary>
        public List<string> AppliedRules { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation parameters.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation scope.
        /// </summary>
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation level.
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the validation was successful.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the validation errors.
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation warnings.
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Gets or sets additional metadata about the validation.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();

        /// <summary>
        /// Gets the object being validated.
        /// </summary>
        public object Target => _target;

        /// <summary>
        /// Gets the validation type.
        /// </summary>
        public ValidationType Type => _type;

        /// <summary>
        /// Gets the validation category.
        /// </summary>
        public ValidationCategory Category => _category;

        /// <summary>
        /// Gets the validation stage.
        /// </summary>
        public ValidationStage Stage => _stage;

        /// <summary>
        /// Gets the additional validation parameters.
        /// </summary>
        public IReadOnlyDictionary<string, object> ParametersReadOnly => new ReadOnlyDictionary<string, object>(Parameters);

        /// <summary>
        /// Gets whether to stop on first error.
        /// </summary>
        public bool StopOnFirstError => _stopOnFirstError;

        /// <summary>
        /// Gets whether to validate recursively.
        /// </summary>
        public bool ValidateRecursively => _validateRecursively;

        /// <summary>
        /// Gets the validation timeout in seconds.
        /// </summary>
        public int TimeoutSeconds => _timeoutSeconds;

        /// <summary>
        /// Gets the validation metadata.
        /// </summary>
        public IReadOnlyDictionary<string, string> MetadataReadOnly => new ReadOnlyDictionary<string, string>(Metadata);

        private readonly Dictionary<string, object> _parameters = new();
        private readonly Dictionary<string, object> _metadata = new();
        private object _target;
        private ValidationType _type;
        private ValidationScope _scope;
        private ValidationLevel _level;
        private ValidationCategory _category;
        private ValidationStage _stage;
        private bool _stopOnFirstError;
        private bool _validateRecursively;
        private int _timeoutSeconds = 30;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationContext"/> class.
        /// </summary>
        public ValidationContext()
        {
        }

        /// <summary>
        /// Sets the target object to validate.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <exception cref="ArgumentNullException">Thrown when target is null.</exception>
        public void SetTarget(object target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Sets the validation type.
        /// </summary>
        /// <param name="type">The validation type.</param>
        public void SetType(ValidationType type)
        {
            _type = type;
        }

        /// <summary>
        /// Sets the validation scope.
        /// </summary>
        /// <param name="scope">The validation scope.</param>
        public void SetScope(ValidationScope scope)
        {
            _scope = scope;
        }

        /// <summary>
        /// Sets the validation level.
        /// </summary>
        /// <param name="level">The validation level.</param>
        public void SetLevel(ValidationLevel level)
        {
            _level = level;
        }

        /// <summary>
        /// Sets the validation category.
        /// </summary>
        /// <param name="category">The validation category.</param>
        public void SetCategory(ValidationCategory category)
        {
            _category = category;
        }

        /// <summary>
        /// Sets the validation stage.
        /// </summary>
        /// <param name="stage">The validation stage.</param>
        public void SetStage(ValidationStage stage)
        {
            _stage = stage;
        }

        /// <summary>
        /// Sets whether to stop on first error.
        /// </summary>
        /// <param name="stopOnFirstError">Whether to stop on first error.</param>
        public void SetStopOnFirstError(bool stopOnFirstError)
        {
            _stopOnFirstError = stopOnFirstError;
        }

        /// <summary>
        /// Sets whether to validate recursively.
        /// </summary>
        /// <param name="validateRecursively">Whether to validate recursively.</param>
        public void SetValidateRecursively(bool validateRecursively)
        {
            _validateRecursively = validateRecursively;
        }

        /// <summary>
        /// Sets the validation timeout in seconds.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when timeoutSeconds is less than or equal to 0.</exception>
        public void SetTimeout(int timeoutSeconds)
        {
            if (timeoutSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(timeoutSeconds), "Timeout must be greater than 0.");
            }
            _timeoutSeconds = timeoutSeconds;
        }

        /// <summary>
        /// Adds a parameter to the validation context.
        /// </summary>
        /// <param name="key">The parameter key.</param>
        /// <param name="value">The parameter value.</param>
        /// <exception cref="ArgumentException">Thrown when key is null or whitespace.</exception>
        public void AddParameter(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Parameter key cannot be null or whitespace", nameof(key));
            }

            _parameters[key] = value;
        }

        /// <summary>
        /// Adds metadata to the validation context.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        /// <exception cref="ArgumentException">Thrown when key is null or whitespace.</exception>
        public void AddMetadata(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Metadata key cannot be null or whitespace", nameof(key));
            }

            _metadata[key] = value;
        }

        /// <summary>
        /// Gets a parameter value from the validation context.
        /// </summary>
        /// <param name="key">The parameter key.</param>
        /// <returns>The parameter value, or null if not found.</returns>
        /// <exception cref="ArgumentException">Thrown when key is null or whitespace.</exception>
        public object GetParameter(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Parameter key cannot be null or whitespace", nameof(key));
            }

            return _parameters.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Gets a metadata value from the validation context.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <returns>The metadata value, or null if not found.</returns>
        /// <exception cref="ArgumentException">Thrown when key is null or whitespace.</exception>
        public object GetMetadata(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Metadata key cannot be null or whitespace", nameof(key));
            }

            return _metadata.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationContext"/> with the specified target.
        /// </summary>
        /// <param name="target">The target object to validate.</param>
        /// <returns>A new instance of <see cref="ValidationContext"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when target is null.</exception>
        public static ValidationContext Create(object target)
        {
            var context = new ValidationContext();
            context.SetTarget(target);
            return context;
        }
    }
} 
