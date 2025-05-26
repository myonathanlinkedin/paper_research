using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Exceptions;
using RuntimeErrorSage.Core.LLM.Models;
using RuntimeErrorSage.Core.LLM.Options;

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

            // Validate Qwen model configuration
            if (_options.ModelId != "Qwen-2.5-7B-Instruct-1M")
            {
                throw new LMStudioException("Invalid model ID. Must use Qwen-2.5-7B-Instruct-1M for research compliance.");
            }

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
                    throw new LMStudioException("Qwen model is not ready");
                }

                // Prepare the request with Qwen-specific format
                var request = new
                {
                    model = _options.ModelId,
                    messages = new[]
                    {
                        new { role = "system", content = _options.SystemPromptTemplate },
                        new { role = "user", content = prompt }
                    },
                    max_tokens = _options.MaxTokens,
                    temperature = _options.Temperature,
                    top_p = _options.TopP,
                    frequency_penalty = _options.FrequencyPenalty,
                    presence_penalty = _options.PresencePenalty,
                    stop = _options.StopSequences,
                    stream = _options.EnableStreaming
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                // Send request to LM Studio
                var response = await _httpClient.PostAsync("/v1/chat/completions", content);
                response.EnsureSuccessStatusCode();

                // Parse response
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LMStudioResponse>(responseContent, _jsonOptions);

                if (result?.Choices == null || result.Choices.Count == 0)
                {
                    throw new LMStudioException("Empty response from Qwen model");
                }

                // Extract the assistant's message
                var assistantMessage = result.Choices[0].Message?.Content;
                if (string.IsNullOrWhiteSpace(assistantMessage))
                {
                    throw new LMStudioException("Empty assistant message from Qwen model");
                }

                return assistantMessage.Trim();
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
                var models = JsonSerializer.Deserialize<LMStudioModelsResponse>(content, _jsonOptions);

                // Verify Qwen model is loaded and ready
                var qwenModel = models?.Data?.FirstOrDefault(m => 
                    m.Id == _options.ModelId && 
                    m.Status == "ready" &&
                    m.IsLoaded);

                if (qwenModel == null)
                {
                    _logger.LogWarning("Qwen model not found or not ready");
                    return false;
                }

                // Verify model parameters
                if (qwenModel.ContextWindowSize < _options.ContextWindowSize)
                {
                    _logger.LogWarning("Qwen model context window size ({ModelSize}) is less than required ({RequiredSize})",
                        qwenModel.ContextWindowSize, _options.ContextWindowSize);
                    return false;
                }

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
                var request = new
                {
                    model = _options.ModelId,
                    messages = new[]
                    {
                        new { role = "system", content = _options.SystemPromptTemplate },
                        new { role = "user", content = prompt }
                    },
                    max_tokens = _options.MaxTokens,
                    temperature = _options.Temperature,
                    top_p = _options.TopP,
                    frequency_penalty = _options.FrequencyPenalty,
                    presence_penalty = _options.PresencePenalty,
                    stop = _options.StopSequences,
                    stream = _options.EnableStreaming
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("/v1/chat/completions", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LMStudioResponse>(responseContent, _jsonOptions);

                if (result?.Choices == null || result.Choices.Count == 0)
                {
                    throw new LMStudioException("Empty response from Qwen model");
                }

                var assistantMessage = result.Choices[0].Message?.Content;
                if (string.IsNullOrWhiteSpace(assistantMessage))
                {
                    throw new LMStudioException("Empty assistant message from Qwen model");
                }

                return assistantMessage.Trim();
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
                _logger.LogError(ex, "Unexpected error during remediation generation");
                throw new LMStudioException("Unexpected error during remediation generation", ex);
            }
        }

        private string GenerateRemediationPrompt(object analysis)
        {
            return $@"Based on the following error analysis, provide detailed remediation steps:

Analysis:
{JsonSerializer.Serialize(analysis, _jsonOptions)}

Please provide:
1. Step-by-step remediation instructions
2. Validation checks for each step
3. Rollback procedures if needed
4. Prevention strategies
5. Confidence score (0-1) for the remediation plan

Format your response in a clear, technical manner suitable for implementation.";
        }
    }
} 
