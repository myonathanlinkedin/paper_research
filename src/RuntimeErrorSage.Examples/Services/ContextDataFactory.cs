using System.Collections.ObjectModel;
using System.Collections.Generic;
using RuntimeErrorSage.Examples.Models;

namespace RuntimeErrorSage.Examples.Services;

/// <summary>
/// Factory for creating context data for error reporting and logging.
/// </summary>
public class ContextDataFactory : IContextDataFactory
{
    /// <inheritdoc />
    public Dictionary<string, object> CreateDatabaseContext(ProcessRequest request)
    {
        return new Dictionary<string, object>
        {
            { "Operation", request.Operation },
            { "DataLength", request.Data?.Length ?? 0 },
            { "Priority", request.Priority },
            { "TimeoutSeconds", request.TimeoutSeconds }
        };
    }

    /// <inheritdoc />
    public Dictionary<string, object> CreateFileUploadContext(FileUploadRequest request)
    {
        return new Dictionary<string, object>
        {
            { "FileName", request.FileName },
            { "FileSize", request.FileSize },
            { "ContentType", request.ContentType }
        };
    }

    /// <inheritdoc />
    public Dictionary<string, object> CreateServiceCallContext(IntegrationRequest request)
    {
        return new Dictionary<string, object>
        {
            { "ServiceUrl", request.ServiceUrl },
            { "Method", request.Method },
            { "TimeoutSeconds", request.TimeoutSeconds }
        };
    }

    /// <inheritdoc />
    public Dictionary<string, object> CreateResourceContext(ResourceRequest request)
    {
        return new Dictionary<string, object>
        {
            { "ResourceType", request.ResourceType },
            { "RequestedAmount", request.RequestedAmount },
            { "Priority", request.Priority }
        };
    }
} 






