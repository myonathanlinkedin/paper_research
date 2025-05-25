using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeSage.Core
{
    /// <summary>
    /// Implementation of the ILMStudioClient for interacting with the LM Studio API.
    /// </summary>
    public class LMStudioClient : ILMStudioClient
    {
        private readonly HttpClient _httpClient;
        private string _modelName = "qwen2.5-7b-instruct-1m";
        private string _endpoint = "http://127.0.0.1:1234/v1";
        private ModelInfo? _modelInfo;

        public LMStudioClient(HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task InitializeAsync(string modelName, string endpoint)
        {
            _modelName = modelName;
            _endpoint = endpoint;
            // Optionally, fetch model info
            _modelInfo = await GetModelInfoAsync();
        }

        public async Task<string> AnalyzeErrorAsync(string prompt)
        {
            var request = new
            {
                model = _modelName,
                messages = new[]
                {
                    new { role = "system", content = "You are an expert .NET runtime error analyzer." },
                    new { role = "user", content = prompt }
                },
                stream = false
            };

            var response = await _httpClient.PostAsync(
                _endpoint + "/chat/completions",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            );
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
            return content ?? string.Empty;
        }

        public async Task<ModelInfo> GetModelInfoAsync()
        {
            // This assumes LM Studio exposes a /models endpoint (adjust if needed)
            var response = await _httpClient.GetAsync(_endpoint + "/models");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            foreach (var model in doc.RootElement.EnumerateArray())
            {
                if (model.GetProperty("id").GetString() == _modelName)
                {
                    return new ModelInfo
                    {
                        Name = model.GetProperty("id").GetString() ?? _modelName,
                        Version = model.TryGetProperty("version", out var v) ? v.GetString() ?? "" : "",
                        SizeGB = model.TryGetProperty("size_gb", out var s) ? s.GetDouble() : 0,
                        ContextWindowSize = model.TryGetProperty("context_length", out var c) ? c.GetInt32() : 0,
                        IsLoaded = model.TryGetProperty("loaded", out var l) && l.GetBoolean(),
                        LastUpdated = DateTime.UtcNow // No info, so use now
                    };
                }
            }
            return new ModelInfo { Name = _modelName, IsLoaded = false };
        }

        public async Task<bool> UpdateModelAsync()
        {
            // Placeholder: LM Studio may not support hot model updates via API
            _modelInfo = await GetModelInfoAsync();
            return _modelInfo.IsLoaded;
        }
    }
} 