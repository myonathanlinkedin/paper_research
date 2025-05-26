using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CodeSage.Core.Interfaces;
using CodeSage.Core.Exceptions;
using CodeSage.Core.LLM.Models;
using CodeSage.Core.LLM.Options;

namespace CodeSage.Core.LLM
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
                // Check if model is ready
                if (!await IsModelReadyAsync())
                {
                    throw new LMStudioException("LM Studio model is not ready");
                }

                // Prepare the request
                var request = new
                {
                    prompt = prompt,
                    max_tokens = _options.MaxTokens,
                    temperature = _options.Temperature,
                    top_p = _options.TopP,
                    frequency_penalty = _options.FrequencyPenalty,
                    presence_penalty = _options.PresencePenalty,
                    stop = _options.StopSequences
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                // Send request to LM Studio
                var response = await _httpClient.PostAsync("/v1/completions", content);
                response.EnsureSuccessStatusCode();

                // Parse response
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LMStudioResponse>(responseContent, _jsonOptions);

                if (result?.Choices == null || result.Choices.Count == 0)
                {
                    throw new LMStudioException("Empty response from LM Studio");
                }

                return result.Choices[0].Text.Trim();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with LM Studio");
                throw new LMStudioException("Failed to communicate with LM Studio", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing LM Studio response");
                throw new LMStudioException("Failed to parse LM Studio response", ex);
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
                    _logger.LogWarning("LM Studio health check failed with status code {StatusCode}", response.StatusCode);
                    return false;
                }

                var content = await response.Content.ReadAsStringAsync();
                var models = JsonSerializer.Deserialize<LMStudioModelsResponse>(content, _jsonOptions);

                return models?.Data != null && 
                       models.Data.Any(m => m.Id == _options.ModelId && m.Status == "ready");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking LM Studio model status");
                return false;
            }
        }

        public async Task<string> GenerateRemediationAsync(object analysis)
        {
            try
            {
                var prompt = GenerateRemediationPrompt(analysis);
                var response = await _httpClient.PostAsync("/v1/completions", new StringContent(prompt, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LMStudioResponse>(responseContent, _jsonOptions);

                if (result?.Choices == null || result.Choices.Count == 0)
                {
                    throw new LMStudioException("Empty response from LM Studio");
                }

                return result.Choices[0].Text.Trim();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with LM Studio");
                throw new LMStudioException("Failed to communicate with LM Studio", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing LM Studio response");
                throw new LMStudioException("Failed to parse LM Studio response", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during remediation generation");
                throw new LMStudioException("Unexpected error during remediation generation", ex);
            }
        }

        private string GenerateRemediationPrompt(object analysis)
        {
            return $@"System: You are an expert .NET runtime error remediation generator. Based on the following analysis, provide:
1. Step-by-step remediation steps
2. Validation checks for each step
3. Rollback procedures if needed
4. Prevention strategies

Analysis:
{JsonSerializer.Serialize(analysis, _jsonOptions)}";
        }
    }
} 