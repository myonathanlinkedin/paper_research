using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RuntimeErrorSage.Examples.Models;
using RuntimeErrorSage.Examples.Models.Responses;

namespace RuntimeErrorSage.Examples.Services;

/// <summary>
/// Interface for simulating various operations that might fail
/// </summary>
public interface IOperationSimulator
{
    /// <summary>
    /// Simulates a database operation
    /// </summary>
    /// <param name="request">The database operation request</param>
    /// <returns>Operation response with status and error details if any</returns>
    Task<OperationResponse> SimulateDatabaseOperationAsync(ProcessRequest request);

    /// <summary>
    /// Simulates a file upload operation
    /// </summary>
    /// <param name="request">The file upload request</param>
    /// <returns>Operation response with status and error details if any</returns>
    Task<OperationResponse> SimulateFileUploadAsync(FileUploadRequest request);

    /// <summary>
    /// Simulates an external service call
    /// </summary>
    /// <param name="request">The service integration request</param>
    /// <returns>Operation response with status and error details if any</returns>
    Task<OperationResponse> SimulateServiceCallAsync(IntegrationRequest request);

    /// <summary>
    /// Simulates a resource allocation operation
    /// </summary>
    /// <param name="request">The resource allocation request</param>
    /// <returns>Operation response with status and error details if any</returns>
    Task<OperationResponse> SimulateResourceAllocationAsync(ResourceRequest request);
} 





