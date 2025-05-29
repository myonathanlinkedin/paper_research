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
                    return HealthCheckResult.Unhealthy(
                        "LM Studio model is not ready",
                        _dataFactory.CreateModelInfo(_options.ModelId, _options.BaseUrl));
                }

                // Perform a simple test analysis
                var testPrompt = "Test prompt for health check";
                var response = await _llmClient.AnalyzeErrorAsync(testPrompt);

                if (string.IsNullOrWhiteSpace(response))
                {
                    return HealthCheckResult.Degraded(
                        "LM Studio returned empty response",
                        _dataFactory.CreateModelInfo(_options.ModelId, _options.BaseUrl));
                }

                return HealthCheckResult.Healthy(
                    "LM Studio is healthy",
                    _dataFactory.CreateModelInfoWithResponse(_options.ModelId, _options.BaseUrl, response.Length));
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    "LM Studio health check failed",
                    ex,
                    _dataFactory.CreateModelInfoWithError(_options.ModelId, _options.BaseUrl, ex.Message));
            }
        }
    }
} 

