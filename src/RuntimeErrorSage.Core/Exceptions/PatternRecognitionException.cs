using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when pattern recognition operations fail.
    /// </summary>
    public class PatternRecognitionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRecognitionException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public PatternRecognitionException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRecognitionException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PatternRecognitionException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
} 
