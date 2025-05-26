using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Analysis;
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

                var result = await _errorAnalyzer.AnalyzeErrorAsync(context);

                _logger.LogInformation(
                    "Error analysis completed with confidence {Confidence} and accuracy {Accuracy}",
                    result.Confidence,
                    result.Accuracy);

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
        public async Task<IActionResult> AnalyzeDatabaseError([FromBody] DatabaseErrorContext context)
        {
            try
            {
                var errorContext = new ErrorContext
                {
                    ErrorType = "DatabaseError",
                    ErrorMessage = "Database operation failed",
                    Source = "Database",
                    Timestamp = DateTime.UtcNow,
                    AdditionalContext = new Dictionary<string, object>
                    {
                        { "Operation", context.Operation },
                        { "Table", context.Table },
                        { "Query", context.Query }
                    }
                };

                var result = await _errorAnalyzer.AnalyzeErrorAsync(errorContext);
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
        public async Task<IActionResult> AnalyzeHttpError([FromBody] HttpErrorContext context)
        {
            try
            {
                var errorContext = new ErrorContext
                {
                    ErrorType = "HttpError",
                    ErrorMessage = $"HTTP request failed with status {context.StatusCode}",
                    Source = "HttpClient",
                    Timestamp = DateTime.UtcNow,
                    AdditionalContext = new Dictionary<string, object>
                    {
                        { "Method", context.Method },
                        { "Url", context.Url },
                        { "StatusCode", context.StatusCode },
                        { "ResponseContent", context.ResponseContent }
                    }
                };

                var result = await _errorAnalyzer.AnalyzeErrorAsync(errorContext);
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

    public class DatabaseErrorContext
    {
        public string ServiceName { get; set; }
        public string OperationName { get; set; }
        public string CorrelationId { get; set; }
        public Exception Exception { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string Query { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public TimeSpan Timeout { get; set; }
    }

    public class HttpErrorContext
    {
        public string ServiceName { get; set; }
        public string OperationName { get; set; }
        public string CorrelationId { get; set; }
        public Exception Exception { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }
        public Dictionary<string, string> ResponseHeaders { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
    }
} 
