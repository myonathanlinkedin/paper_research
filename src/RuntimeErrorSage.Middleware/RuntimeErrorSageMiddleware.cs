using System;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Runtime.Interfaces;
using RuntimeErrorSage.Core.Runtime.Interfaces;

namespace RuntimeErrorSage.Middleware
{
    /// <summary>
    /// ASP.NET Core middleware (conforming to IRuntimeErrorSageMiddleware) that intercepts exceptions, enriches the error context (if enabled), and (optionally) applies automated remediation (using the injected IRuntimeErrorSageService).
    /// </summary>
    public class RuntimeErrorSageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRuntimeErrorSageService _service;
        private readonly ILogger<RuntimeErrorSageMiddleware> _logger;

        public RuntimeErrorSageMiddleware(
            RequestDelegate next,
            IRuntimeErrorSageService service,
            ILogger<RuntimeErrorSageMiddleware> logger)
        {
            ArgumentNullException.ThrowIfNull(next);
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(logger);
            
            _next = next;
            _service = service;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var error = new RuntimeError(
                    message: ex.Message,
                    errorType: ex.GetType().Name,
                    source: "Middleware",
                    stackTrace: ex.StackTrace ?? string.Empty
                );

                var errorContext = new ErrorContext(
                    error: error,
                    context: "Middleware",
                    timestamp: DateTime.UtcNow
                );

                errorContext.AddMetadata("RequestPath", context.Request.Path);
                errorContext.AddMetadata("RequestMethod", context.Request.Method);
                errorContext.AddMetadata("StatusCode", context.Response.StatusCode);

                try
                {
                    var result = await _service.AnalyzeErrorAsync(errorContext);
                    if (result.IsAnalyzed && result.RemediationPlan?.Strategies?.Any() == true)
                    {
                        await _service.RemediateErrorAsync(errorContext);
                    }
                }
                catch (Exception analysisEx)
                {
                    _logger.LogError(analysisEx, "Error analyzing or remediating exception in middleware");
                }

                throw;
            }
        }
    }
} 

