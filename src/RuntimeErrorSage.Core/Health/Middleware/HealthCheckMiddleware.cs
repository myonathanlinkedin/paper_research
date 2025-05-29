using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;

namespace RuntimeErrorSage.Model.Health.Middleware
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
                    await context.Response.WriteAsJsonAsync(healthStatus);
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during health check");
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new { Status = "Unhealthy", Error = ex.Message });
                    return;
                }
            }

            await _next(context);
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

