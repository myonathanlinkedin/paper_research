using System;
using Microsoft.AspNetCore.Http;
using RuntimeErrorSage.Domain.Models.Error;

namespace RuntimeErrorSage.Core.Extensions
{
    /// <summary>
    /// Extension methods for creating ErrorContext from exceptions and HTTP context.
    /// </summary>
    public static class ErrorContextExtensions
    {
        /// <summary>
        /// Creates an ErrorContext from an exception and HTTP context information.
        /// </summary>
        /// <param name="exception">The exception that occurred</param>
        /// <param name="source">The source of the error</param>
        /// <param name="requestPath">The HTTP request path</param>
        /// <param name="requestMethod">The HTTP request method</param>
        /// <param name="statusCode">The HTTP status code</param>
        /// <returns>An ErrorContext instance</returns>
        public static ErrorContext CreateFromException(
            Exception exception,
            string source,
            PathString requestPath,
            string requestMethod,
            int statusCode)
        {
            ArgumentNullException.ThrowIfNull(exception);

            var error = new RuntimeError(
                message: exception.Message,
                errorType: exception.GetType().Name,
                source: source,
                stackTrace: exception.StackTrace ?? string.Empty
            );

            var errorContext = new ErrorContext(
                error: error,
                context: source,
                timestamp: DateTime.UtcNow
            );

            errorContext.AddMetadata("RequestPath", requestPath.ToString());
            errorContext.AddMetadata("RequestMethod", requestMethod);
            errorContext.AddMetadata("StatusCode", statusCode);

            return errorContext;
        }
    }
}








