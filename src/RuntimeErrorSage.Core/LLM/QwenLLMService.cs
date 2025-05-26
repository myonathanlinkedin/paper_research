using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RuntimeErrorSage.Core.LLM;

/// <summary>
/// Implements the LLM service using Qwen 2.5 7B Instruct 1M.
/// </summary>
public class QwenLLMService : ILLMService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<QwenLLMService> _logger;
    private readonly string _baseUrl;
    private readonly string _apiKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="QwenLLMService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="baseUrl">The base URL.</param>
    /// <param name="apiKey">The API key.</param>
    public QwenLLMService(
        HttpClient httpClient,
        ILogger<QwenLLMService> logger,
        string baseUrl,
        string apiKey)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    /// <summary>
    /// Generates a response from the LLM.
    /// </summary>
    /// <param name="prompt">The prompt.</param>
    /// <returns>The response.</returns>
    public async Task<string> GenerateAsync(string prompt)
    {
        if (string.IsNullOrEmpty(prompt))
            throw new ArgumentException("Prompt cannot be null or empty.", nameof(prompt));

        try
        {
            var request = new
            {
                model = "qwen-7b-instruct",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant that analyzes runtime errors and provides remediation steps." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.7,
                max_tokens = 1000
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<JsonElement>(responseContent);

            return responseObject
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating response from Qwen LLM");
            throw;
        }
    }
} 