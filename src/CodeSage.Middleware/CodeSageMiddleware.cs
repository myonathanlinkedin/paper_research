using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CodeSage.Core;

namespace CodeSage.Middleware
{
    /// <summary>
    /// ASP.NET Core middleware (conforming to ICodeSageMiddleware) that intercepts exceptions, enriches the error context (if enabled), and (optionally) applies automated remediation (using the injected ICodeSageService).
    /// </summary>
    public class CodeSageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICodeSageService _codeSageService;
        private readonly ILogger<CodeSageMiddleware> _logger;

        public CodeSageMiddleware(RequestDelegate next, ICodeSageService codeSageService, ILogger<CodeSageMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _codeSageService = codeSageService ?? throw new ArgumentNullException(nameof(codeSageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred in the request pipeline.");
                // Enrich the error context (if enabled) and (optionally) apply automated remediation.
                var errorContext = new ErrorContext { CorrelationId = context.TraceIdentifier, Timestamp = DateTime.UtcNow };
                var analysis = await _codeSageService.ProcessExceptionAsync(ex, errorContext);
                if (analysis != null)
                {
                    // Optionally, apply remediation (if enabled) based on the analysis.
                    await _codeSageService.ApplyRemediationAsync(analysis);
                }
                // Re-throw (or handle) the exception as needed.
                throw;
            }
        }
    }
} 