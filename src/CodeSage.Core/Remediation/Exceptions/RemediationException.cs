using System;

namespace CodeSage.Core.Remediation.Exceptions
{
    /// <summary>
    /// Base exception class for remediation-related exceptions.
    /// </summary>
    public class RemediationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationException"/> class.
        /// </summary>
        public RemediationException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RemediationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RemediationException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when a remediation validation fails.
    /// </summary>
    public class RemediationValidationException : RemediationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationValidationException"/> class.
        /// </summary>
        public RemediationValidationException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationValidationException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RemediationValidationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationValidationException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RemediationValidationException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when a remediation execution fails.
    /// </summary>
    public class RemediationExecutionException : RemediationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationExecutionException"/> class.
        /// </summary>
        public RemediationExecutionException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationExecutionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RemediationExecutionException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationExecutionException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RemediationExecutionException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when a remediation strategy fails.
    /// </summary>
    public class RemediationStrategyException : RemediationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationStrategyException"/> class.
        /// </summary>
        public RemediationStrategyException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationStrategyException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RemediationStrategyException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationStrategyException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RemediationStrategyException(string message, Exception innerException) : base(message, innerException) { }
    }
} 