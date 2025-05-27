namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Default implementation of IErrorFactory.
    /// </summary>
    public class ErrorFactory : IErrorFactory
    {
        /// <summary>
        /// Creates a new error instance.
        /// </summary>
        /// <param name="type">The error type.</param>
        /// <param name="message">The error message.</param>
        /// <param name="source">The error source.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>A new error instance.</returns>
        public Error CreateError(string type, string message, string source, string stackTrace)
        {
            return new Error(type, message, source, stackTrace);
        }
    }
} 