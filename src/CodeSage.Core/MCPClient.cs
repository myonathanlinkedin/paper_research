using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CodeSage.Core.Interfaces;
using CodeSage.Core.Models;
using CodeSage.Core.Options;
using System.Collections.Concurrent;
using CodeSage.Core.Models.Error;

namespace CodeSage.Core
{
    /// <summary>
    /// Implementation of the IMCPClient for interacting with the Model Context Protocol (MCP) endpoint.
    /// This client is responsible for publishing error context (and analysis), subscribing to updates, retrieving historical context data, and reporting connection status.
    /// </summary>
    public class MCPClient : IMCPClient
    {
        private readonly ILogger<MCPClient> _logger;
        private readonly HttpClient _httpClient;
        private readonly CodeSageOptions _options;
        private readonly ConcurrentDictionary<string, ErrorAnalysisResult> _analysisCache;
        private readonly ConcurrentDictionary<string, DateTime> _lastAnalysisTime;
        private string _endpoint = "http://127.0.0.1:1234/mcp"; // Default MCP endpoint (configurable)
        private Func<ErrorContext, ErrorAnalysisResult, Task>? _subscriptionHandler;

        public MCPClient(
            ILogger<MCPClient> logger,
            HttpClient httpClient,
            IOptions<CodeSageOptions> options)
        {
            _logger = logger;
            _httpClient = httpClient;
            _options = options.Value;
            _analysisCache = new();
            _lastAnalysisTime = new();
        }

        public async Task InitializeAsync(string endpoint)
        {
            _endpoint = endpoint;
            // Perform a health check to verify the endpoint is accessible
            try
            {
                var response = await _httpClient.GetAsync(_endpoint + "/health");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new CodeSageException($"Failed to initialize MCP client: {ex.Message}", ex);
            }
        }

        public async Task PublishContextAsync(ErrorContext context, ErrorAnalysisResult analysis)
        {
            var payload = new { context, analysis };
            var response = await _httpClient.PostAsync(_endpoint + "/publish", new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task SubscribeToContextAsync(Func<ErrorContext, ErrorAnalysisResult, Task> handler)
        {
            _subscriptionHandler = handler;
            // In a real implementation, you might poll or use a streaming endpoint (e.g. SSE or WebSockets) to receive updates.
            // For simplicity, we simulate a subscription by polling (or you can integrate a streaming endpoint).
            // (This is a placeholder; adjust as per your MCP endpoint's subscription mechanism.)
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<ContextHistory>> GetContextHistoryAsync(string correlationId, TimeRange timeRange)
        {
            var query = $"?correlationId={Uri.EscapeDataString(correlationId)}&start={Uri.EscapeDataString(timeRange.StartTime.ToString("o"))}&end={Uri.EscapeDataString(timeRange.EndTime.ToString("o"))}";
            var response = await _httpClient.GetAsync(_endpoint + "/history" + query);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var history = JsonSerializer.Deserialize<IEnumerable<ContextHistory>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return history ?? new List<ContextHistory>();
        }

        public async Task<ConnectionStatus> GetConnectionStatusAsync()
        {
            // In a real implementation, you might ping the endpoint or check a /status endpoint.
            // For simplicity, we assume that if a GET request succeeds, the connection is "Connected."
            try
            {
                var response = await _httpClient.GetAsync(_endpoint + "/status");
                response.EnsureSuccessStatusCode();
                return new ConnectionStatus 
                { 
                    IsConnected = true,
                    Endpoint = _endpoint, 
                    LastConnected = DateTime.UtcNow, 
                    ActiveSubscriptions = (_subscriptionHandler != null) ? 1 : 0,
                    MessagesPublished = 0,
                    MessagesReceived = 0
                };
            }
            catch (Exception ex)
            {
                return new ConnectionStatus 
                { 
                    IsConnected = false,
                    Endpoint = _endpoint, 
                    LastConnected = DateTime.UtcNow, 
                    ActiveSubscriptions = 0,
                    MessagesPublished = 0,
                    MessagesReceived = 0
                };
            }
        }

        public async Task<List<ErrorPattern>> GetErrorPatternsAsync(string serviceName)
        {
            var response = await _httpClient.GetAsync($"{_endpoint}/patterns?service={Uri.EscapeDataString(serviceName)}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var patterns = JsonSerializer.Deserialize<List<ErrorPattern>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return patterns ?? new List<ErrorPattern>();
        }

        public async Task UpdateErrorPatternsAsync(string serviceName, List<ErrorPattern> patterns)
        {
            var payload = new { service = serviceName, patterns };
            var response = await _httpClient.PostAsync($"{_endpoint}/patterns/update", new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteErrorPatternsAsync(string serviceName, List<string> patternIds)
        {
            var payload = new { service = serviceName, patternIds };
            var response = await _httpClient.PostAsync($"{_endpoint}/patterns/delete", new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }
    }
} 