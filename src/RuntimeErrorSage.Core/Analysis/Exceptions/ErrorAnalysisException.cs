using System;
using System.Runtime.Serialization;

namespace RuntimeErrorSage.Model.Analysis.Exceptions
{
    /// <summary>
    /// Exception thrown when an error occurs during error analysis operations.
    /// </summary>
    [Serializable]
    public class ErrorAnalysisException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorAnalysisException"/> class.
        /// </summary>
        public ErrorAnalysisException()
            : base("An error occurred during error analysis.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorAnalysisException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ErrorAnalysisException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorAnalysisException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ErrorAnalysisException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorAnalysisException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected ErrorAnalysisException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
} 
