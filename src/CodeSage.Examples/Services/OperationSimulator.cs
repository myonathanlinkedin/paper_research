using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CodeSage.Examples.Models;
using CodeSage.Examples.Models.Responses;
using CodeSage.Examples.Exceptions;

namespace CodeSage.Examples.Services;

/// <summary>
/// Simulates various operations that might fail for testing CodeSage's error handling
/// </summary>
public class OperationSimulator : IOperationSimulator
{
    private readonly Random _random = new Random();

    public async Task<OperationResponse> SimulateDatabaseOperationAsync(ProcessRequest request)
    {
        var startTime = DateTime.UtcNow;
        try
        {
            // Simulate operation delay
            await Task.Delay(_random.Next(100, 500));

            if (_random.NextDouble() < 0.2) // 20% chance of failure
            {
                throw new DatabaseOperationException(
                    "Database operation failed",
                    new Dictionary<string, object>
                    {
                        { "Operation", request.Operation },
                        { "DataLength", request.Data?.Length ?? 0 },
                        { "Priority", request.Priority },
                        { "TimeoutSeconds", request.TimeoutSeconds }
                    }
                );
            }

            return new OperationResponse
            {
                Status = OperationStatus.Success,
                DurationMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds
            };
        }
        catch (Exception ex)
        {
            return new OperationResponse
            {
                Status = OperationStatus.Failed,
                DurationMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds,
                Error = new ErrorDetails
                {
                    Code = "DB_OPERATION_FAILED",
                    Message = ex.Message,
                    Details = "Database operation failed due to simulated error",
                    StackTrace = ex.StackTrace,
                    Context = new Dictionary<string, object>
                    {
                        { "Operation", request.Operation },
                        { "DataLength", request.Data?.Length ?? 0 },
                        { "Priority", request.Priority },
                        { "TimeoutSeconds", request.TimeoutSeconds }
                    }
                }
            };
        }
    }

    public async Task<OperationResponse> SimulateFileUploadAsync(FileUploadRequest request)
    {
        var startTime = DateTime.UtcNow;
        try
        {
            await Task.Delay(_random.Next(200, 1000));

            if (_random.NextDouble() < 0.2)
            {
                throw new FileOperationException(
                    "File upload failed",
                    new Dictionary<string, object>
                    {
                        { "FileName", request.FileName },
                        { "FileSize", request.FileSize },
                        { "ContentType", request.ContentType }
                    }
                );
            }

            return new OperationResponse
            {
                Status = OperationStatus.Success,
                DurationMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds
            };
        }
        catch (Exception ex)
        {
            return new OperationResponse
            {
                Status = OperationStatus.Failed,
                DurationMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds,
                Error = new ErrorDetails
                {
                    Code = "FILE_UPLOAD_FAILED",
                    Message = ex.Message,
                    Details = "File upload failed due to simulated error",
                    StackTrace = ex.StackTrace,
                    Context = new Dictionary<string, object>
                    {
                        { "FileName", request.FileName },
                        { "FileSize", request.FileSize },
                        { "ContentType", request.ContentType }
                    }
                }
            };
        }
    }

    public async Task<OperationResponse> SimulateServiceCallAsync(IntegrationRequest request)
    {
        var startTime = DateTime.UtcNow;
        try
        {
            await Task.Delay(_random.Next(300, 1500));

            if (_random.NextDouble() < 0.2)
            {
                throw new ServiceIntegrationException(
                    "External service call failed",
                    new Dictionary<string, object>
                    {
                        { "ServiceUrl", request.ServiceUrl },
                        { "Method", request.Method },
                        { "TimeoutSeconds", request.TimeoutSeconds }
                    }
                );
            }

            return new OperationResponse
            {
                Status = OperationStatus.Success,
                DurationMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds
            };
        }
        catch (Exception ex)
        {
            return new OperationResponse
            {
                Status = OperationStatus.Failed,
                DurationMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds,
                Error = new ErrorDetails
                {
                    Code = "SERVICE_CALL_FAILED",
                    Message = ex.Message,
                    Details = "External service call failed due to simulated error",
                    StackTrace = ex.StackTrace,
                    Context = new Dictionary<string, object>
                    {
                        { "ServiceUrl", request.ServiceUrl },
                        { "Method", request.Method },
                        { "TimeoutSeconds", request.TimeoutSeconds }
                    }
                }
            };
        }
    }

    public async Task<OperationResponse> SimulateResourceAllocationAsync(ResourceRequest request)
    {
        var startTime = DateTime.UtcNow;
        try
        {
            await Task.Delay(_random.Next(100, 300));

            if (_random.NextDouble() < 0.2)
            {
                throw new ResourceAllocationException(
                    "Resource allocation failed",
                    new Dictionary<string, object>
                    {
                        { "ResourceType", request.ResourceType },
                        { "RequestedAmount", request.RequestedAmount },
                        { "Priority", request.Priority }
                    }
                );
            }

            return new OperationResponse
            {
                Status = OperationStatus.Success,
                DurationMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds
            };
        }
        catch (Exception ex)
        {
            return new OperationResponse
            {
                Status = OperationStatus.Failed,
                DurationMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds,
                Error = new ErrorDetails
                {
                    Code = "RESOURCE_ALLOCATION_FAILED",
                    Message = ex.Message,
                    Details = "Resource allocation failed due to simulated error",
                    StackTrace = ex.StackTrace,
                    Context = new Dictionary<string, object>
                    {
                        { "ResourceType", request.ResourceType },
                        { "RequestedAmount", request.RequestedAmount },
                        { "Priority", request.Priority }
                    }
                }
            };
        }
    }
} 