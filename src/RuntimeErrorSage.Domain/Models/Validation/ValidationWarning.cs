using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Validation
{
    /// <summary>
    /// Represents a validation warning.
    /// </summary>
    public sealed class ValidationWarning
    {
        private Dictionary<string, object> _details = new();

        /// <summary>
        /// Gets or sets the warning identifier.
        /// </summary>
        public string WarningId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the warning code.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the warning message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the warning source.
        /// </summary>
        public string Source { get; set; } = "System";

        /// <summary>
        /// Gets or sets the warning severity.
        /// </summary>
        public ValidationSeverity Severity { get; set; } = ValidationSeverity.Warning;

        /// <summary>
        /// Gets or sets the timestamp when this warning was generated.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets additional warning details.
        /// </summary>
        public Dictionary<string, object> Details
        { 
            get => _details;
            set => _details = value ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets additional warning details as read-only dictionary.
        /// </summary>
        public IReadOnlyDictionary<string, object> ReadOnlyDetails => new ReadOnlyDictionary<string, object>(_details);

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationWarning"/> class.
        /// </summary>
        public ValidationWarning()
        {
            Timestamp = DateTime.UtcNow;
            WarningId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationWarning"/> class.
        /// </summary>
        /// <param name="code">The warning code.</param>
        /// <param name="message">The warning message.</param>
        /// <param name="source">The warning source.</param>
        /// <param name="severity">The warning severity.</param>
        /// <param name="details">Optional warning details dictionary.</param>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        public ValidationWarning(
            string code,
            string message,
            string source,
            ValidationSeverity severity,
            Dictionary<string, object> details = null)
        {
            ArgumentNullException.ThrowIfNull(code);
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(source);

            WarningId = Guid.NewGuid().ToString();
            Code = code;
            Message = message;
            Source = source;
            Severity = severity;
            _details = details ?? new Dictionary<string, object>();
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationWarning"/> class with simplified parameters.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="severity">The warning severity.</param>
        public ValidationWarning(string message, ValidationSeverity severity = ValidationSeverity.Warning)
            : this("W" + Guid.NewGuid().ToString().Substring(0, 8), message, "System", severity)
        {
        }

        /// <summary>
        /// Creates a new warning with additional details.
        /// </summary>
        /// <param name="key">The detail key.</param>
        /// <param name="value">The detail value.</param>
        /// <returns>A new warning with the added details.</returns>
        /// <exception cref="ArgumentException">Thrown when key is null or whitespace.</exception>
        public ValidationWarning WithDetail(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Detail key cannot be null or whitespace", nameof(key));
            }

            var newDetails = new Dictionary<string, object>(_details) { [key] = value };
            return new ValidationWarning(
                Code,
                Message,
                Source,
                Severity,
                newDetails);
        }

        /// <summary>
        /// Gets a detail value from the warning.
        /// </summary>
        /// <param name="key">The detail key.</param>
        /// <returns>The detail value, or null if not found.</returns>
        /// <exception cref="ArgumentException">Thrown when key is null or whitespace.</exception>
        public object GetDetail(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Detail key cannot be null or whitespace", nameof(key));
            }

            return _details.TryGetValue(key, out var value) ? value : null;
        }
    }
} 
