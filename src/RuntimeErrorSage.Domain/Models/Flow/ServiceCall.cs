using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Flow
{
    /// <summary>
    /// Represents a service call in the system.
    /// </summary>
    public class ServiceCall
    {
        /// <summary>
        /// Gets or sets the unique identifier for this service call.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the service being called.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the endpoint being called.
        /// </summary>
        public string Endpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HTTP method used for the call.
        /// </summary>
        public string Method { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the call was made.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the duration of the call.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the status code of the response.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets whether the call was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the request payload.
        /// </summary>
        public string RequestPayload { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the response payload.
        /// </summary>
        public string ResponsePayload { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional metadata about the call.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the correlation ID for tracking related calls.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the parent call ID if this is a nested call.
        /// </summary>
        public string? ParentCallId { get; set; }

        /// <summary>
        /// Gets or sets the list of child calls if this call spawned other calls.
        /// </summary>
        public List<string> ChildCallIds { get; set; } = new();

        /// <summary>
        /// Gets or sets the error message if the call failed.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the stack trace if the call failed.
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the retry count if the call was retried.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets whether the call was retried.
        /// </summary>
        public bool WasRetried { get; set; }

        /// <summary>
        /// Gets or sets the timeout value for the call.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets whether the call timed out.
        /// </summary>
        public bool TimedOut { get; set; }

        /// <summary>
        /// Gets or sets the circuit breaker state at the time of the call.
        /// </summary>
        public string? CircuitBreakerState { get; set; }

        /// <summary>
        /// Gets or sets the fallback used if the call failed.
        /// </summary>
        public string? FallbackUsed { get; set; }

        /// <summary>
        /// Gets or sets the metrics collected during the call.
        /// </summary>
        public Dictionary<string, double> Metrics { get; set; } = new();
    }
} 
