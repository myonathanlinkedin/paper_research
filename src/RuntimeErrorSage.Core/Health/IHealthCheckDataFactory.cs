using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Health
{
    /// <summary>
    /// Interface for creating health check data.
    /// </summary>
    public interface IHealthCheckDataFactory
    {
        /// <summary>
        /// Creates a dictionary with model information.
        /// </summary>
        /// <param name="modelId">The model ID.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns>A dictionary containing model information.</returns>
        Dictionary<string, object> CreateModelInfo(string modelId, string baseUrl);

        /// <summary>
        /// Creates a dictionary with model information and response length.
        /// </summary>
        /// <param name="modelId">The model ID.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="responseLength">The response length.</param>
        /// <returns>A dictionary containing model information and response length.</returns>
        Dictionary<string, object> CreateModelInfoWithResponse(string modelId, string baseUrl, int responseLength);

        /// <summary>
        /// Creates a dictionary with model information and error details.
        /// </summary>
        /// <param name="modelId">The model ID.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="error">The error message.</param>
        /// <returns>A dictionary containing model information and error details.</returns>
        Dictionary<string, object> CreateModelInfoWithError(string modelId, string baseUrl, string error);
    }
} 






