using System.Collections.ObjectModel;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Models.LLM;

namespace RuntimeErrorSage.Application.LLM;

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
        _httpClient = httpClient ?? ArgumentNullException.ThrowIfNull(nameof(httpClient));
        _logger = logger ?? ArgumentNullException.ThrowIfNull(nameof(logger));
        _baseUrl = baseUrl ?? ArgumentNullException.ThrowIfNull(nameof(baseUrl));
        _apiKey = apiKey ?? ArgumentNullException.ThrowIfNull(nameof(apiKey));

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
            var request = new LLMRequest
            {
                Query = prompt,
                Context = string.Empty
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<LLMResponse>(responseContent);

            return responseObject?.Content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating response from Qwen LLM");
            throw;
        }
    }
} 






