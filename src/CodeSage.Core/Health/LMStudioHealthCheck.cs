using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using CodeSage.Core.LLM;
using CodeSage.Core.LLM.Options;
using CodeSage.Core.Options;
using CodeSage.Core.Interfaces;

namespace CodeSage.Core.Health
{
    public class LMStudioHealthCheck : IHealthCheck
    {
        private readonly ILMStudioClient _llmClient;
        private readonly LMStudioOptions _options;
        private readonly IOptions<CodeSageOptions> _codeSageOptions;

        public LMStudioHealthCheck(
            ILMStudioClient llmClient,
            IOptions<LMStudioOptions> options,
            IOptions<CodeSageOptions> codeSageOptions)
        {
            _llmClient = llmClient;
            _options = options.Value;
            _codeSageOptions = codeSageOptions;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if LM Studio is enabled
                if (!_codeSageOptions.Value.EnableErrorAnalysis)
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