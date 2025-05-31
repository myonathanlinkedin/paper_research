using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;

namespace RuntimeErrorSage.Core.Health
{
    public class HealthCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HealthCheckMiddleware> _logger;

        public HealthCheckMiddleware(RequestDelegate next, ILogger<HealthCheckMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/health"))
            {
                try
                {
                    // Check system resources
                    var healthStatus = new
                    {
                        Status = "Healthy",
                        Timestamp = DateTime.UtcNow,
                        System = new
                        {
                            Memory = GetMemoryStatus(),
                            CPU = GetCPUStatus(),
                            Disk = GetDiskStatus()
                        }
                    };

                    context.Response.ContentType = "application/json";
                    await WriteJsonResponseAsync(context.Response, 200, healthStatus);
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during health check");
                    context.Response.StatusCode = 500;
                    await WriteJsonResponseAsync(context.Response, 500, new { Status = "Unhealthy", Error = ex.Message });
                    return;
                }
            }

            await _next(context);
        }

        /// <summary>
        /// Writes a JSON response to the HTTP context.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="content">The content to write.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task WriteJsonResponseAsync(HttpResponse response, int statusCode, object content)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json";
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var json = JsonSerializer.Serialize(content, options);
            await response.WriteAsync(json);
        }

        private object GetMemoryStatus()
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            return new
            {
                TotalMemory = process.WorkingSet64,
                PrivateMemory = process.PrivateMemorySize64,
                VirtualMemory = process.VirtualMemorySize64
            };
        }

        private object GetCPUStatus()
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            return new
            {
                ProcessTime = process.TotalProcessorTime,
                UserTime = process.UserProcessorTime,
                PrivilegedTime = process.PrivilegedProcessorTime
            };
        }

        private object GetDiskStatus()
        {
            var drive = new DriveInfo(Path.GetPathRoot(Environment.CurrentDirectory)!);
            return new
            {
                drive.TotalSize,
                AvailableSpace = drive.AvailableFreeSpace,
                UsedSpace = drive.TotalSize - drive.AvailableFreeSpace
            };
        }
    }
} 

