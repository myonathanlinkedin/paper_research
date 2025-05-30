using System;
using System.Runtime.Serialization;

namespace RuntimeErrorSage.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when there is an error in pattern recognition.
    /// </summary>
    [Serializable]
    public class PatternRecognitionException : Exception
    {
        /// <summary>
        /// Gets or sets the pattern that caused the error.
        /// </summary>
        public string Pattern { get; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRecognitionException"/> class.
        /// </summary>
        public PatternRecognitionException()
            : base("An error occurred during pattern recognition.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRecognitionException"/> class with a message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public PatternRecognitionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRecognitionException"/> class with a message and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PatternRecognitionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRecognitionException"/> class with a message, pattern, and error code.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="pattern">The pattern that caused the error.</param>
        /// <param name="errorCode">The error code.</param>
        public PatternRecognitionException(string message, string pattern, string errorCode)
            : base(message)
        {
            Pattern = pattern;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRecognitionException"/> class with a message, pattern, error code, and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="pattern">The pattern that caused the error.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="innerException">The inner exception.</param>
        public PatternRecognitionException(string message, string pattern, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            Pattern = pattern;
            ErrorCode = errorCode;
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
