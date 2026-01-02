using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Runtime.Interfaces;
using RuntimeErrorSage.Core.Extensions;
using RuntimeErrorSage.Domain.Enums;

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
                var errorContext = ErrorContextExtensions.CreateFromException(
                    ex,
                    "Middleware",
                    context.Request.Path,
                    context.Request.Method,
                    context.Response.StatusCode
                );

                try
                {
                    var result = await _service.AnalyzeErrorAsync(errorContext);
                    // Check if analysis is complete and has suggested actions for remediation
                    if (result.IsComplete && result.Status == AnalysisStatus.Completed && 
                        (result.SuggestedActions?.Any() == true || result.CanAutoRemediate))
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

