using System.Collections.ObjectModel;
using System;
using System.Runtime.Serialization;

namespace RuntimeErrorSage.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when an error occurs during pattern recognition operations.
    /// </summary>
    [Serializable]
    public class PatternRecognitionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRecognitionException"/> class.
        /// </summary>
        public PatternRecognitionException()
            : base("An error occurred during pattern recognition.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRecognitionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PatternRecognitionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRecognitionException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public PatternRecognitionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRecognitionException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected PatternRecognitionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
} 






