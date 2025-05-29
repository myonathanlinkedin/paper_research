using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Models.Metrics
{
    /// <summary>
    /// Exception thrown when there is an error collecting metrics.
    /// </summary>
    public class MetricsCollectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the MetricsCollectionException class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public MetricsCollectionException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the MetricsCollectionException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public MetricsCollectionException(string message) : base(message) { }
    }
} 






