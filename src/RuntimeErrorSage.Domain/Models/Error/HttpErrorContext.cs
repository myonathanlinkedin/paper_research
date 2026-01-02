using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Flow;

namespace RuntimeErrorSage.Domain.Models.Error
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

        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        public string RequestId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the request start time.
        /// </summary>
        public DateTime RequestStartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the request duration.
        /// </summary>
        public TimeSpan RequestDuration { get; set; }

        /// <summary>
        /// Gets or sets the request status.
        /// </summary>
        public string RequestStatus { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the request type.
        /// </summary>
        public string RequestType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the request version.
        /// </summary>
        public string RequestVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the request result.
        /// </summary>
        public string RequestResult { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the request target.
        /// </summary>
        public string RequestTarget { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the request metrics.
        /// </summary>
        public Dictionary<string, double> RequestMetrics { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Gets or sets the request dependencies.
        /// </summary>
        public List<string> RequestDependencies { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the request tags.
        /// </summary>
        public List<string> RequestTags { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the service calls.
        /// </summary>
        public new List<ServiceCall> ServiceCalls { get; set; } = new List<ServiceCall>();

        /// <summary>
        /// Gets or sets the data flows.
        /// </summary>
        public new List<DataFlow> DataFlows { get; set; } = new List<DataFlow>();

        /// <summary>
        /// Gets or sets the component metrics.
        /// </summary>
        public new Dictionary<string, Dictionary<string, double>> ComponentMetrics { get; set; } = new Dictionary<string, Dictionary<string, double>>();

        /// <summary>
        /// Gets or sets the component dependencies.
        /// </summary>
        public new List<ComponentDependency> ComponentDependencies { get; set; } = new List<ComponentDependency>();

        /// <summary>
        /// Gets or sets the context data.
        /// </summary>
        public new Dictionary<string, object> ContextData { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the inner error.
        /// </summary>
        public new HttpErrorContext? InnerError { get; set; }

        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public new string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public new string Context { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the error is actionable.
        /// </summary>
        public new bool IsActionable { get; set; }

        /// <summary>
        /// Gets or sets whether the error is transient.
        /// </summary>
        public new bool IsTransient { get; set; }

        /// <summary>
        /// Gets or sets whether the error is resolved.
        /// </summary>
        public new bool IsResolved { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpErrorContext"/> class.
        /// </summary>
        /// <param name="error">The runtime error.</param>
        /// <param name="context">The context information.</param>
        /// <param name="timestamp">The timestamp when the error occurred.</param>
        public HttpErrorContext(RuntimeError error, string context, DateTime timestamp) 
            : base(error, context, timestamp)
        {
            Category = Domain.Enums.ErrorCategory.Network;
        }

        /// <summary>
        /// Validates the HTTP error context.
        /// </summary>
        /// <returns>True if the HTTP error context is valid; otherwise, false.</returns>
        public new bool Validate()
        {
            if (!base.Validate())
                return false;

            if (StatusCode <= 0)
                return false;

            if (string.IsNullOrEmpty(Method))
                return false;

            if (string.IsNullOrEmpty(Url))
                return false;

            if (string.IsNullOrEmpty(RequestId))
                return false;

            if (string.IsNullOrEmpty(RequestStatus))
                return false;

            if (string.IsNullOrEmpty(RequestType))
                return false;

            if (string.IsNullOrEmpty(RequestVersion))
                return false;

            if (string.IsNullOrEmpty(RequestResult))
                return false;

            if (string.IsNullOrEmpty(RequestTarget))
                return false;

            if (string.IsNullOrEmpty(ServiceName))
                return false;

            if (string.IsNullOrEmpty(Context))
                return false;

            return true;
        }

        /// <summary>
        /// Converts the HTTP error context to a dictionary.
        /// </summary>
        /// <returns>The dictionary representation of the HTTP error context.</returns>
        public new Dictionary<string, object> ToDictionary()
        {
            var dict = base.ToDictionary();

            dict.Add("StatusCode", StatusCode);
            dict.Add("Method", Method);
            dict.Add("Url", Url);
            dict.Add("Headers", Headers);
            dict.Add("RequestBody", RequestBody);
            dict.Add("ResponseBody", ResponseBody);
            dict.Add("ClientIp", ClientIp);
            dict.Add("UserAgent", UserAgent);
            dict.Add("RequestId", RequestId);
            dict.Add("RequestStartTime", RequestStartTime);
            dict.Add("RequestDuration", RequestDuration);
            dict.Add("RequestStatus", RequestStatus);
            dict.Add("RequestType", RequestType);
            dict.Add("RequestVersion", RequestVersion);
            dict.Add("RequestResult", RequestResult);
            dict.Add("RequestTarget", RequestTarget);
            dict.Add("RequestMetrics", RequestMetrics);
            dict.Add("RequestDependencies", RequestDependencies);
            dict.Add("RequestTags", RequestTags);
            dict.Add("ServiceCalls", ServiceCalls);
            dict.Add("DataFlows", DataFlows);
            dict.Add("ComponentMetrics", ComponentMetrics);
            dict.Add("ComponentDependencies", ComponentDependencies);
            dict.Add("ContextData", ContextData);
            dict.Add("ServiceName", ServiceName);
            dict.Add("Context", Context);
            dict.Add("IsActionable", IsActionable);
            dict.Add("IsTransient", IsTransient);
            dict.Add("IsResolved", IsResolved);

            if (InnerError != null)
            {
                dict.Add("InnerError", InnerError);
            }

            return dict;
        }
    }
} 
