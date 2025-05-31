using System;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Validation
{
    /// <summary>
    /// Represents a validation message with severity, code, and timestamp.
    /// </summary>
    public class ValidationMessage
    {
        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the severity of the validation message.
        /// </summary>
        public ValidationSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the validation message code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the message was created.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the additional context for the message.
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        public ValidationMessage()
        {
            Message = string.Empty;
            Code = string.Empty;
            Context = string.Empty;
            Timestamp = DateTime.UtcNow;
            Severity = ValidationSeverity.Info;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="code">The message code.</param>
        public ValidationMessage(string message, ValidationSeverity severity, string code)
        {
            Message = message ?? string.Empty;
            Severity = severity;
            Code = code ?? string.Empty;
            Context = string.Empty;
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="code">The message code.</param>
        /// <param name="context">The additional context.</param>
        public ValidationMessage(string message, ValidationSeverity severity, string code, string context)
            : this(message, severity, code)
        {
            Context = context ?? string.Empty;
        }

        /// <summary>
        /// Creates a new warning message.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="code">The warning code.</param>
        /// <returns>A new validation message with warning severity.</returns>
        public static ValidationMessage CreateWarning(string message, string code = "")
        {
            return new ValidationMessage(message, ValidationSeverity.Warning, code);
        }

        /// <summary>
        /// Creates a new error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="code">The error code.</param>
        /// <returns>A new validation message with error severity.</returns>
        public static ValidationMessage CreateError(string message, string code = "")
        {
            return new ValidationMessage(message, ValidationSeverity.Error, code);
        }

        /// <summary>
        /// Creates a new info message.
        /// </summary>
        /// <param name="message">The info message.</param>
        /// <param name="code">The info code.</param>
        /// <returns>A new validation message with info severity.</returns>
        public static ValidationMessage CreateInfo(string message, string code = "")
        {
            return new ValidationMessage(message, ValidationSeverity.Info, code);
        }
    }
} 