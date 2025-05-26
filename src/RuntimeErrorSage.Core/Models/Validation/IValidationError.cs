namespace RuntimeErrorSage.Core.Models.Validation
{
    /// <summary>
    /// Represents a validation error
    /// </summary>
    public interface IValidationError
    {
        /// <summary>
        /// Gets the error message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets the error code
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Gets the error details
        /// </summary>
        string Details { get; }
    }
} 