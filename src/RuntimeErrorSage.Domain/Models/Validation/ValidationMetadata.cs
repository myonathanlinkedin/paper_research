using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Validation
{
    /// <summary>
    /// Represents metadata for a validation operation.
    /// </summary>
    public sealed class ValidationMetadata
    {
        private readonly Dictionary<string, object> _metadata = new();
        private double _durationMs;

        /// <summary>
        /// Gets the unique identifier of the validation.
        /// </summary>
        public string ValidationId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the name of the validator.
        /// </summary>
        public string ValidatorName { get; }

        /// <summary>
        /// Gets the version of the validator.
        /// </summary>
        public string ValidatorVersion { get; }

        /// <summary>
        /// Gets the type of validation.
        /// </summary>
        public RuntimeErrorSage.Domain.Enums.ValidationType Type { get; }

        /// <summary>
        /// Gets the scope of validation.
        /// </summary>
        public RuntimeErrorSage.Domain.Enums.ValidationScope Scope { get; }

        /// <summary>
        /// Gets the level of validation.
        /// </summary>
        public RuntimeErrorSage.Domain.Enums.ValidationLevel Level { get; }

        /// <summary>
        /// Gets the category of validation.
        /// </summary>
        public RuntimeErrorSage.Domain.Enums.ValidationCategory Category { get; }

        /// <summary>
        /// Gets the stage of validation.
        /// </summary>
        public RuntimeErrorSage.Domain.Enums.ValidationStage Stage { get; }

        /// <summary>
        /// Gets the timestamp when the validation was performed.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets the duration of the validation in milliseconds.
        /// </summary>
        public double DurationMs => _durationMs;

        /// <summary>
        /// Gets any additional metadata.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata => new ReadOnlyDictionary<string, object>(_metadata);

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMetadata"/> class.
        /// </summary>
        public ValidationMetadata()
            : this(string.Empty, string.Empty, RuntimeErrorSage.Domain.Enums.ValidationType.None, RuntimeErrorSage.Domain.Enums.ValidationScope.None, RuntimeErrorSage.Domain.Enums.ValidationLevel.None, RuntimeErrorSage.Domain.Enums.ValidationCategory.None, RuntimeErrorSage.Domain.Enums.ValidationStage.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMetadata"/> class.
        /// </summary>
        /// <param name="validatorName">The name of the validator.</param>
        /// <param name="validatorVersion">The version of the validator.</param>
        /// <param name="type">The type of validation.</param>
        /// <param name="scope">The scope of validation.</param>
        /// <param name="level">The level of validation.</param>
        /// <param name="category">The category of validation.</param>
        /// <param name="stage">The stage of validation.</param>
        /// <exception cref="ArgumentNullException">Thrown when validatorName or validatorVersion is null.</exception>
        public ValidationMetadata(
            string validatorName,
            string validatorVersion,
            RuntimeErrorSage.Domain.Enums.ValidationType type,
            RuntimeErrorSage.Domain.Enums.ValidationScope scope,
            RuntimeErrorSage.Domain.Enums.ValidationLevel level,
            RuntimeErrorSage.Domain.Enums.ValidationCategory category,
            RuntimeErrorSage.Domain.Enums.ValidationStage stage)
        {
            ValidatorName = validatorName ?? throw new ArgumentNullException(nameof(validatorName));
            ValidatorVersion = validatorVersion ?? throw new ArgumentNullException(nameof(validatorVersion));
            Type = type;
            Scope = scope;
            Level = level;
            Category = category;
            Stage = stage;
        }

        /// <summary>
        /// Sets the duration of the validation in milliseconds.
        /// </summary>
        /// <param name="durationMs">The duration in milliseconds.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when durationMs is negative.</exception>
        public void SetDuration(double durationMs)
        {
            if (durationMs < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(durationMs), "Duration cannot be negative.");
            }

            _durationMs = durationMs;
        }

        /// <summary>
        /// Adds metadata to the validation.
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
        /// Gets a metadata value from the validation.
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
        /// Creates a new instance of <see cref="ValidationMetadata"/> with the specified validator information.
        /// </summary>
        /// <param name="validatorName">The name of the validator.</param>
        /// <param name="validatorVersion">The version of the validator.</param>
        /// <returns>A new instance of <see cref="ValidationMetadata"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when validatorName or validatorVersion is null.</exception>
        public static ValidationMetadata Create(string validatorName, string validatorVersion)
        {
            return new ValidationMetadata(
                validatorName,
                validatorVersion,
                RuntimeErrorSage.Domain.Enums.ValidationType.Data,
                RuntimeErrorSage.Domain.Enums.ValidationScope.Object,
                RuntimeErrorSage.Domain.Enums.ValidationLevel.Basic,
                RuntimeErrorSage.Domain.Enums.ValidationCategory.Input,
                RuntimeErrorSage.Domain.Enums.ValidationStage.PreValidation);
        }
    }
}
