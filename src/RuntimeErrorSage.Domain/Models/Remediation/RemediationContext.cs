using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Error;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents the context for remediation operations.
    /// </summary>
    public class RemediationContext
    {
        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public ErrorContext ErrorContext { get; set; }

        /// <summary>
        /// Gets or sets the remediation options.
        /// </summary>
        public Dictionary<string, object> Options { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the source identifier.
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationContext"/> class.
        /// </summary>
        public RemediationContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationContext"/> class.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        public RemediationContext(ErrorContext errorContext)
        {
            ErrorContext = errorContext ?? throw new ArgumentNullException(nameof(errorContext));
        }

        /// <summary>
        /// Sets an option in the remediation context.
        /// </summary>
        /// <param name="key">The option key.</param>
        /// <param name="value">The option value.</param>
        public void SetOption(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            Options[key] = value;
        }

        /// <summary>
        /// Gets an option from the remediation context.
        /// </summary>
        /// <typeparam name="T">The option type.</typeparam>
        /// <param name="key">The option key.</param>
        /// <returns>The option value.</returns>
        public T GetOption<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (Options.TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;

            return default;
        }

        /// <summary>
        /// Creates a copy of the remediation context.
        /// </summary>
        /// <returns>A copy of the remediation context.</returns>
        public RemediationContext Copy()
        {
            return new RemediationContext
            {
                ErrorContext = ErrorContext,
                Options = new Dictionary<string, object>(Options),
                SourceId = SourceId,
                Timestamp = Timestamp
            };
        }
    }
} 