using System;
using System.Runtime.Serialization;

namespace RuntimeErrorSage.Model.Exceptions
{
    /// <summary>
    /// Exception thrown when an error occurs during Model Context Protocol operations.
    /// </summary>
    [Serializable]
    public class MCPException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MCPException"/> class.
        /// </summary>
        public MCPException()
            : base("An error occurred during Model Context Protocol operation.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCPException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MCPException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCPException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MCPException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCPException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected MCPException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
} 
