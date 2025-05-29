using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Validation.Interfaces
{
    /// <summary>
    /// Represents a validation warning
    /// </summary>
    public interface IValidationWarning
    {
        /// <summary>
        /// Gets the warning message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets the severity level of the warning.
        /// </summary>
        SeverityLevel Severity { get; }

        /// <summary>
        /// Gets the warning code
        /// </summary>
        string Code { get; }
    }
} 
