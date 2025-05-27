using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Exceptions;
using RuntimeErrorSage.Core.LLM.Options;
using RuntimeErrorSage.Core.Models.LLM;
using RuntimeErrorSage.Core.LLM.Interfaces;

namespace RuntimeErrorSage.Core.LLM
{
    public class LMStudioClient : ILMStudioClient
    {
        private readonly HttpClient _httpClient;
        private readonly LMStudioOptions _options;
        private readonly ILogger<LMStudioClient> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public LMStudioClient(
            HttpClient httpClient,
            IOptions<LMStudioOptions> options,
            ILogger<LMStudioClient> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
        }

        public async Task<string> AnalyzeErrorAsync(string prompt)
        {
            try
            {
                var request = new LLMRequest
                {
                    Query = prompt,
                    Context = string.Empty
                };
                var content = new StringContent(
                    JsonSerializer.Serialize(request, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("/v1/chat/completions", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LLMResponse>(responseContent, _jsonOptions);

                if (result == null || string.IsNullOrWhiteSpace(result.Content))
                {
                    throw new LMStudioException("Empty response from Qwen model");
                }

                return result.Content.Trim();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with Qwen model");
                throw new LMStudioException("Failed to communicate with Qwen model", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing Qwen model response");
                throw new LMStudioException("Failed to parse Qwen model response", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during error analysis");
                throw new LMStudioException("Unexpected error during error analysis", ex);
            }
        }

        public async Task<bool> IsModelReadyAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/v1/models");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Qwen model health check failed with status code {StatusCode}", response.StatusCode);
                    return false;
                }

                var content = await response.Content.ReadAsStringAsync();
                // Optionally parse and check for model readiness if needed
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking Qwen model status");
                return false;
            }
        }

        public async Task<string> GenerateRemediationAsync(object analysis)
        {
            try
            {
                var prompt = GenerateRemediationPrompt(analysis);
                var request = new LLMRequest
                {
                    Query = prompt,
                    Context = string.Empty
                };
                var content = new StringContent(
                    JsonSerializer.Serialize(request, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("/v1/chat/completions", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LLMResponse>(responseContent, _jsonOptions);

                if (result == null || string.IsNullOrWhiteSpace(result.Content))
                {
                    throw new LMStudioException("Empty response from Qwen model");
                }

                return result.Content.Trim();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating remediation from Qwen model");
                throw new LMStudioException("Failed to generate remediation from Qwen model", ex);
            }
        }

        private string GenerateRemediationPrompt(object analysis)
        {
            return $"Generate remediation steps for the following analysis: {analysis}";
        }
    }
} 
