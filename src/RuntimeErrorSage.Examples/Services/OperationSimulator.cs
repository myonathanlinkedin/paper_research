using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Examples.Models;
using RuntimeErrorSage.Examples.Models.Responses;
using RuntimeErrorSage.Examples.Exceptions;
using RuntimeErrorSage.Examples.Services.Interfaces;

namespace RuntimeErrorSage.Examples.Services;

/// <summary>
/// Simulates various operations that might fail for testing RuntimeErrorSage's error handling
/// </summary>
public class OperationSimulator : IOperationSimulator
{
    private readonly Random _random = new Random();
    private readonly IContextDataFactory _contextFactory;
    private readonly IKeyValueDataFactory _keyValueFactory;

    public OperationSimulator(
        IContextDataFactory contextFactory,
        IKeyValueDataFactory keyValueFactory)
    {
        _contextFactory = contextFactory ?? ArgumentNullException.ThrowIfNull(nameof(contextFactory));
        _keyValueFactory = keyValueFactory ?? ArgumentNullException.ThrowIfNull(nameof(keyValueFactory));
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
                var context = _contextFactory.CreateDatabaseContext(request);
                var errorData = _keyValueFactory.CreateWithMetadata(
                    "DatabaseError",
                    "Database operation failed",
                    context
                );
                throw new DatabaseOperationException(
                    "Database operation failed",
                    errorData.Metadata
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
            var context = _contextFactory.CreateDatabaseContext(request);
            var errorData = _keyValueFactory.CreateWithMetadata(
                "DatabaseError",
                ex.Message,
                context
            );
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
                    Context = errorData.Metadata
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
                var context = _contextFactory.CreateFileUploadContext(request);
                var errorData = _keyValueFactory.CreateWithMetadata(
                    "FileError",
                    "File upload failed",
                    context
                );
                throw new FileOperationException(
                    "File upload failed",
                    errorData.Metadata
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
            var context = _contextFactory.CreateFileUploadContext(request);
            var errorData = _keyValueFactory.CreateWithMetadata(
                "FileError",
                ex.Message,
                context
            );
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
                    Context = errorData.Metadata
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
                var context = _contextFactory.CreateServiceCallContext(request);
                var errorData = _keyValueFactory.CreateWithMetadata(
                    "ServiceError",
                    "External service call failed",
                    context
                );
                throw new ServiceIntegrationException(
                    "External service call failed",
                    errorData.Metadata
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
            var context = _contextFactory.CreateServiceCallContext(request);
            var errorData = _keyValueFactory.CreateWithMetadata(
                "ServiceError",
                ex.Message,
                context
            );
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
                    Context = errorData.Metadata
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
                var context = _contextFactory.CreateResourceContext(request);
                var errorData = _keyValueFactory.CreateWithMetadata(
                    "ResourceError",
                    "Resource allocation failed",
                    context
                );
                throw new ResourceAllocationException(
                    "Resource allocation failed",
                    errorData.Metadata
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
            var context = _contextFactory.CreateResourceContext(request);
            var errorData = _keyValueFactory.CreateWithMetadata(
                "ResourceError",
                ex.Message,
                context
            );
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
                    Context = errorData.Metadata
                }
            };
        }
    }
} 








