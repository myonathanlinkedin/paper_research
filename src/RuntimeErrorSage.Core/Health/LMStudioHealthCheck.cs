using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.LLM;
using RuntimeErrorSage.Core.LLM.Options;
using RuntimeErrorSage.Core.Options;
using RuntimeErrorSage.Core.Interfaces;

namespace RuntimeErrorSage.Core.Health
{
    public class LMStudioHealthCheck : IHealthCheck
    {
        private readonly ILMStudioClient _llmClient;
        private readonly LMStudioOptions _options;
        private readonly IOptions<RuntimeErrorSageOptions> _RuntimeErrorSageOptions;

        public LMStudioHealthCheck(
            ILMStudioClient llmClient,
            IOptions<LMStudioOptions> options,
            IOptions<RuntimeErrorSageOptions> RuntimeErrorSageOptions)
        {
            _llmClient = llmClient;
            _options = options.Value;
            _RuntimeErrorSageOptions = RuntimeErrorSageOptions;
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
                        new Dictionary<string, object>
                        {
                            ["ModelId"] = _options.ModelId,
                            ["BaseUrl"] = _options.BaseUrl
                        });
                }

                // Perform a simple test analysis
                var testPrompt = "Test prompt for health check";
                var response = await _llmClient.AnalyzeErrorAsync(testPrompt);

                if (string.IsNullOrWhiteSpace(response))
                {
                    return HealthCheckResult.Degraded(
                        "LM Studio returned empty response",
                        new Dictionary<string, object>
                        {
                            ["ModelId"] = _options.ModelId,
                            ["BaseUrl"] = _options.BaseUrl
                        });
                }

                return HealthCheckResult.Healthy(
                    "LM Studio is healthy",
                    new Dictionary<string, object>
                    {
                        ["ModelId"] = _options.ModelId,
                        ["BaseUrl"] = _options.BaseUrl,
                        ["ResponseLength"] = response.Length
                    });
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    "LM Studio health check failed",
                    ex,
                    new Dictionary<string, object>
                    {
                        ["ModelId"] = _options.ModelId,
                        ["BaseUrl"] = _options.BaseUrl,
                        ["Error"] = ex.Message
                    });
            }
        }
    }
} 
