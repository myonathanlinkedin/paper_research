using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Application.LLM;
using RuntimeErrorSage.Application.LLM.Options;
using RuntimeErrorSage.Application.Options;
using RuntimeErrorSage.Application.LLM.Interfaces;

namespace RuntimeErrorSage.Application.Health
{
    public class LMStudioHealthCheck : IHealthCheck
    {
        private readonly ILMStudioClient _llmClient;
        private readonly LMStudioOptions _options;
        private readonly IOptions<RuntimeErrorSageOptions> _RuntimeErrorSageOptions;
        private readonly IHealthCheckDataFactory _dataFactory;

        public LMStudioHealthCheck(
            ILMStudioClient llmClient,
            IOptions<LMStudioOptions> options,
            IOptions<RuntimeErrorSageOptions> RuntimeErrorSageOptions,
            IHealthCheckDataFactory dataFactory)
        {
            _llmClient = llmClient;
            _options = options.Value;
            _RuntimeErrorSageOptions = RuntimeErrorSageOptions;
            _dataFactory = dataFactory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if LM Studio is enabled
                if (!_RuntimeErrorSageOptions.Value.EnableErrorAnalysis)
                {
                    return HealthCheckResult.Healthy("LM Studio integration is disabled");
                }

                // Check if model is ready
                var isReady = await _llmClient.IsModelReadyAsync();
                if (!isReady)
                {
                    var data = _dataFactory.CreateModelInfo(_options.ModelId, _options.BaseUrl);
                    return new HealthCheckResult(
                        Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
                        "LM Studio model is not ready",
                        null, 
                        data);
                }

                // Perform a simple test analysis
                var testPrompt = "Test prompt for health check";
                var response = await _llmClient.AnalyzeErrorAsync(testPrompt);

                if (string.IsNullOrWhiteSpace(response))
                {
                    var data = _dataFactory.CreateModelInfo(_options.ModelId, _options.BaseUrl);
                    return new HealthCheckResult(
                        Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                        "LM Studio returned empty response",
                        null,
                        data);
                }

                var healthData = _dataFactory.CreateModelInfoWithResponse(_options.ModelId, _options.BaseUrl, response.Length);
                return new HealthCheckResult(
                    Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy,
                    "LM Studio is healthy",
                    null,
                    healthData);
            }
            catch (Exception ex)
            {
                var errorData = _dataFactory.CreateModelInfoWithError(_options.ModelId, _options.BaseUrl, ex.Message);
                return new HealthCheckResult(
                    Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
                    "LM Studio health check failed",
                    ex,
                    errorData);
            }
        }
    }
} 

