using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Examples.Models;

namespace RuntimeErrorSage.Examples.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorAnalysisController : ControllerBase
    {
        private readonly IErrorAnalyzer _errorAnalyzer;
        private readonly ILogger<ErrorAnalysisController> _logger;

        public ErrorAnalysisController(
            IErrorAnalyzer errorAnalyzer,
            ILogger<ErrorAnalysisController> logger)
        {
            _errorAnalyzer = errorAnalyzer;
            _logger = logger;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeError([FromBody] ErrorContext context)
        {
            try
            {
                _logger.LogInformation(
                    "Analyzing error for service {Service} with correlation ID {CorrelationId}",
                    context.ServiceName,
                    context.CorrelationId);

                var result = await _errorAnalyzer.AnalyzeContextAsync(context);

                _logger.LogInformation(
                    "Error analysis completed with confidence {Confidence} for error type {ErrorType}",
                    result.Confidence,
                    result.ErrorType);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during analysis");
                return StatusCode(500, new
                {
                    error = "Error analysis failed",
                    details = ex.Message
                });
            }
        }

        [HttpPost("analyze-database")]
        public async Task<IActionResult> AnalyzeDatabaseError([FromBody] Models.DatabaseErrorContext context)
        {
            try
            {
                var error = new RuntimeError(
                    message: "Database operation failed",
                    errorType: "DatabaseError",
                    source: "Database",
                    stackTrace: string.Empty
                );

                var errorContext = new ErrorContext(
                    error: error,
                    context: "Database",
                    timestamp: DateTime.UtcNow
                );

                errorContext.AddMetadata("Operation", context.OperationName);
                errorContext.AddMetadata("Database", context.DatabaseName);
                errorContext.AddMetadata("Query", context.Query);

                var result = await _errorAnalyzer.AnalyzeContextAsync(errorContext);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing database error");
                return StatusCode(500, new
                {
                    error = "Database error analysis failed",
                    details = ex.Message
                });
            }
        }

        [HttpPost("analyze-http")]
        public async Task<IActionResult> AnalyzeHttpError([FromBody] Models.HttpErrorContext context)
        {
            try
            {
                var error = new RuntimeError(
                    message: $"HTTP request failed with status {context.StatusCode}",
                    errorType: "HttpError",
                    source: "HttpClient",
                    stackTrace: string.Empty
                );

                var errorContext = new ErrorContext(
                    error: error,
                    context: "HttpClient",
                    timestamp: DateTime.UtcNow
                );

                errorContext.AddMetadata("Method", context.Method);
                errorContext.AddMetadata("Url", context.Url);
                errorContext.AddMetadata("StatusCode", context.StatusCode);
                errorContext.AddMetadata("ResponseContent", context.ResponseBody);

                var result = await _errorAnalyzer.AnalyzeContextAsync(errorContext);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing HTTP error");
                return StatusCode(500, new
                {
                    error = "HTTP error analysis failed",
                    details = ex.Message
                });
            }
        }
    }
} 

