using System.Collections.ObjectModel;
using System.Collections.Generic;
using RuntimeErrorSage.Examples.Models;

namespace RuntimeErrorSage.Examples.Services.Interfaces;

/// <summary>
/// Interface for creating context data for error reporting and logging.
/// </summary>
public interface IContextDataFactory
{
    /// <summary>
    /// Creates context data for database operations.
    /// </summary>
    Dictionary<string, object> CreateDatabaseContext(ProcessRequest request);

    /// <summary>
    /// Creates context data for file upload operations.
    /// </summary>
    Dictionary<string, object> CreateFileUploadContext(FileUploadRequest request);

    /// <summary>
    /// Creates context data for service integration operations.
    /// </summary>
    Dictionary<string, object> CreateServiceCallContext(IntegrationRequest request);

    /// <summary>
    /// Creates context data for resource allocation operations.
    /// </summary>
    Dictionary<string, object> CreateResourceContext(ResourceRequest request);
} 






