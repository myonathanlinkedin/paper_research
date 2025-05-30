using RuntimeErrorSage.Domain.Models.Error;

namespace RuntimeErrorSage.Domain.Models.Error.Factories
{
    /// <summary>
    /// Interface for creating RuntimeError instances.
    /// </summary>
    public interface IRuntimeErrorFactory
    {
        /// <summary>
        /// Creates a new RuntimeError instance.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errorType">The error type.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>A new RuntimeError instance.</returns>
        RuntimeError Create(string message, string errorType = null, string stackTrace = null);
    }
} 
