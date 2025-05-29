using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Error
{
    /// <summary>
    /// Represents an HTTP error context.
    /// </summary>
    public class HttpErrorContext : ErrorContext
    {
        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        public string Method { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the request URL.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the request headers.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the request body.
        /// </summary>
        public string RequestBody { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the response body.
        /// </summary>
        public string ResponseBody { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the client IP address.
        /// </summary>
        public string ClientIp { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        public string UserAgent { get; set; } = string.Empty;
    }
} 