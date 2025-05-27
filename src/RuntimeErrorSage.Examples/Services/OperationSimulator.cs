using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Examples.Models;
using RuntimeErrorSage.Examples.Models.Responses;
using RuntimeErrorSage.Examples.Exceptions;

namespace RuntimeErrorSage.Examples.Services;

/// <summary>
/// Simulates various operations that might fail for testing RuntimeErrorSage's error handling
/// </summary>
public class OperationSimulator : IOperationSimulator
{
    private readonly Random _random = new Random();
    private readonly Func<Dictionary<string, object>> _dictFactory;

    public OperationSimulator(Func<Dictionary<string, object>>? dictFactory = null)
    {
        _dictFactory = dictFactory ?? (() => new Dictionary<string, object>());
    }

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
                    _dictFactory().AddRange(new[]
                    {
                        new KeyValuePair<string, object>("Operation", request.Operation),
                        new KeyValuePair<string, object>("DataLength", request.Data?.Length ?? 0),
                        new KeyValuePair<string, object>("Priority", request.Priority),
                        new KeyValuePair<string, object>("TimeoutSeconds", request.TimeoutSeconds)
                    })
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
                    Context = _dictFactory().AddRange(new[]
                    {
                        new KeyValuePair<string, object>("Operation", request.Operation),
                        new KeyValuePair<string, object>("DataLength", request.Data?.Length ?? 0),
                        new KeyValuePair<string, object>("Priority", request.Priority),
                        new KeyValuePair<string, object>("TimeoutSeconds", request.TimeoutSeconds)
                    })
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
                    _dictFactory().AddRange(new[]
                    {
                        new KeyValuePair<string, object>("FileName", request.FileName),
                        new KeyValuePair<string, object>("FileSize", request.FileSize),
                        new KeyValuePair<string, object>("ContentType", request.ContentType)
                    })
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
                    Context = _dictFactory().AddRange(new[]
                    {
                        new KeyValuePair<string, object>("FileName", request.FileName),
                        new KeyValuePair<string, object>("FileSize", request.FileSize),
                        new KeyValuePair<string, object>("ContentType", request.ContentType)
                    })
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
                    _dictFactory().AddRange(new[]
                    {
                        new KeyValuePair<string, object>("ServiceUrl", request.ServiceUrl),
                        new KeyValuePair<string, object>("Method", request.Method),
                        new KeyValuePair<string, object>("TimeoutSeconds", request.TimeoutSeconds)
                    })
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
                    Context = _dictFactory().AddRange(new[]
                    {
                        new KeyValuePair<string, object>("ServiceUrl", request.ServiceUrl),
                        new KeyValuePair<string, object>("Method", request.Method),
                        new KeyValuePair<string, object>("TimeoutSeconds", request.TimeoutSeconds)
                    })
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
                    _dictFactory().AddRange(new[]
                    {
                        new KeyValuePair<string, object>("ResourceType", request.ResourceType),
                        new KeyValuePair<string, object>("RequestedAmount", request.RequestedAmount),
                        new KeyValuePair<string, object>("Priority", request.Priority)
                    })
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
                    Context = _dictFactory().AddRange(new[]
                    {
                        new KeyValuePair<string, object>("ResourceType", request.ResourceType),
                        new KeyValuePair<string, object>("RequestedAmount", request.RequestedAmount),
                        new KeyValuePair<string, object>("Priority", request.Priority)
                    })
                }
            };
        }
    }
}

public static class DictionaryExtensions
{
    public static Dictionary<string, object> AddRange(this Dictionary<string, object> dict, IEnumerable<KeyValuePair<string, object>> items)
    {
        foreach (var item in items)
        {
            dict[item.Key] = item.Value;
        }
        return dict;
    }
} 

