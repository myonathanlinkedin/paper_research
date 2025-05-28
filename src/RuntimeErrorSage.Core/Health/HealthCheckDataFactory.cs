using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Health
{
    /// <summary>
    /// Default implementation of IHealthCheckDataFactory.
    /// </summary>
    public class HealthCheckDataFactory : IHealthCheckDataFactory
    {
        /// <summary>
        /// Creates a dictionary with model information.
        /// </summary>
        /// <param name="modelId">The model ID.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns>A dictionary containing model information.</returns>
        public Dictionary<string, object> CreateModelInfo(string modelId, string baseUrl)
        {
            return new Dictionary<string, object>
            {
                ["ModelId"] = modelId,
                ["BaseUrl"] = baseUrl
            };
        }

        /// <summary>
        /// Creates a dictionary with model information and response length.
        /// </summary>
        /// <param name="modelId">The model ID.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="responseLength">The response length.</param>
        /// <returns>A dictionary containing model information and response length.</returns>
        public Dictionary<string, object> CreateModelInfoWithResponse(string modelId, string baseUrl, int responseLength)
        {
            var data = CreateModelInfo(modelId, baseUrl);
            data["ResponseLength"] = responseLength;
            return data;
        }

        /// <summary>
        /// Creates a dictionary with model information and error details.
        /// </summary>
        /// <param name="modelId">The model ID.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="error">The error message.</param>
        /// <returns>A dictionary containing model information and error details.</returns>
        public Dictionary<string, object> CreateModelInfoWithError(string modelId, string baseUrl, string error)
        {
            var data = CreateModelInfo(modelId, baseUrl);
            data["Error"] = error;
            return data;
        }
    }
} 
