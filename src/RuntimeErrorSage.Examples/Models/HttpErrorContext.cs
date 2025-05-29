using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Examples.Models
{
    /// <summary>
    /// Represents the context of an HTTP error.
    /// </summary>
    public class HttpErrorContext
    {
        /// <summary>
        /// Gets or sets the name of the service where the error occurred.
        /// </summary>
        public string ServiceName { get; }

        /// <summary>
        /// Gets or sets the name of the operation that caused the error.
        /// </summary>
        public string OperationName { get; }

        /// <summary>
        /// Gets or sets the correlation ID for tracking the error.
        /// </summary>
        public string CorrelationId { get; }

        /// <summary>
        /// Gets or sets the exception that was thrown.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets or sets the URL of the HTTP request.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Gets or sets the HTTP method used in the request.
        /// </summary>
        public string Method { get; }

        /// <summary>
        /// Gets or sets the HTTP status code received.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Gets or sets the request headers.
        /// </summary>
        public Dictionary<string, string> RequestHeaders { get; set; }

        /// <summary>
        /// Gets or sets the response headers.
        /// </summary>
        public Dictionary<string, string> ResponseHeaders { get; set; }

        /// <summary>
        /// Gets or sets the request body content.
        /// </summary>
        public string RequestBody { get; }

        /// <summary>
        /// Gets or sets the response body content.
        /// </summary>
        public string ResponseBody { get; }
    }
} 






