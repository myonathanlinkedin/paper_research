using System;

namespace RuntimeErrorSage.Core.Models.Validation
{
    /// <summary>
    /// Exception thrown when remediation validation fails.
    /// </summary>
    public class RemediationValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the RemediationValidationException class.
        /// </summary>
        public RemediationValidationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RemediationValidationException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public RemediationValidationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RemediationValidationException class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public RemediationValidationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
} 

