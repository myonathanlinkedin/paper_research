using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Model.Remediation.Interfaces
{
    /// <summary>
    /// Interface for remediation validation warnings.
    /// </summary>
    public interface IRemediationValidationWarning
    {
        /// <summary>
        /// Gets the unique identifier for this warning.
        /// </summary>
        string WarningId { get; }

        /// <summary>
        /// Gets the message describing the warning.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets the severity level of this warning.
        /// </summary>
        ValidationSeverity Severity { get; }

        /// <summary>
        /// Gets the error context associated with this warning.
        /// </summary>
        ErrorContext ErrorContext { get; }

        /// <summary>
        /// Gets the validation rule that generated this warning.
        /// </summary>
        IRemediationValidationRule Rule { get; }

        /// <summary>
        /// Gets additional context data for this warning.
        /// </summary>
        Dictionary<string, object> ContextData { get; }
    }
} 
