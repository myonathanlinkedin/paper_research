namespace RuntimeErrorSage.Core.Models.Validation
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
        /// Gets the warning severity
        /// </summary>
        ValidationSeverity Severity { get; }

        /// <summary>
        /// Gets the warning code
        /// </summary>
        string Code { get; }
    }
} 