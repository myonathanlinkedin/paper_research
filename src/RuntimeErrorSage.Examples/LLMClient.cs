using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using RuntimeErrorSage.Core.Options;

namespace RuntimeErrorSage.Examples;

/// <summary>
/// Implementation of ILLMClient that uses LMStudio for LLM interactions
/// </summary>
public class LLMClient : ILLMClient
{
    private readonly ILogger<LLMClient> _logger;
    private readonly string _modelEndpoint;
    private readonly string _apiKey;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly RuntimeErrorSageOptions _options;

    public bool IsEnabled => true;

    public string Name => _options.LLMModel;

    public string Version => "1.0.0";

    public bool IsConnected => true;

    public LLMClient(
        ILogger<LLMClient> logger,
        IOptions<RuntimeErrorSageOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _modelEndpoint = _options.LLMEndpoint;
        _apiKey = string.Empty; // API key not needed for local LM Studio
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    // ... existing code ...
} 