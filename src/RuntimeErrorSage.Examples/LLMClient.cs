using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Application.Options;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.LLM;
using RuntimeErrorSage.Domain.Models.Remediation;

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

    public async Task<LLMAnalysis> AnalyzeContextAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            var prompt = GenerateAnalysisPrompt(context);
            var response = await CallLLMAsync(prompt);
            var analysis = ParseLLMResponse(response);
            ValidateStrategyScores(analysis);
            return analysis;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing context with LLM");
            return new LLMAnalysis
            {
                IsValid = false,
                ErrorMessage = $"LLM analysis failed: {ex.Message}",
                Timestamp = DateTime.UtcNow
            };
        }
    }

    public async Task<LLMResponse> GenerateResponseAsync(LLMRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var prompt = request.Query;
            var response = await CallLLMAsync(prompt);
            
            return new LLMResponse
            {
                ResponseId = request.Query,
                Content = response,
                Confidence = 0.0,
                Metadata = new Dictionary<string, object>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating LLM response");
            return new LLMResponse
            {
                ResponseId = request.Query,
                Content = string.Empty,
                Confidence = 0.0,
                Metadata = new Dictionary<string, object>()
            };
        }
    }

    public async Task<bool> ValidateResponseAsync(LLMResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        if (string.IsNullOrEmpty(response.Content))
        {
            return false;
        }

        try
        {
            var analysis = ParseLLMResponse(response.Content);
            ValidateStrategyScores(analysis);
            return analysis.IsValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating LLM response");
            return false;
        }
    }

    public async Task<LLMAnalysis> AnalyzeErrorAsync(string errorMessage, string context)
    {
        ArgumentNullException.ThrowIfNull(errorMessage);

        try
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Error Analysis Request:");
            prompt.AppendLine($"Error Message: {errorMessage}");
            if (!string.IsNullOrEmpty(context))
            {
                prompt.AppendLine($"Context: {context}");
            }

            var response = await CallLLMAsync(prompt.ToString());
            var analysis = ParseLLMResponse(response);
            ValidateStrategyScores(analysis);
            return analysis;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing error with LLM");
            return new LLMAnalysis
            {
                IsValid = false,
                ErrorMessage = ex.Message,
                Timestamp = DateTime.UtcNow
            };
        }
    }

    public async Task<LLMSuggestion> GetRemediationSuggestionAsync(LLMAnalysis analysis)
    {
        ArgumentNullException.ThrowIfNull(analysis);

        if (!analysis.IsValid)
        {
            return new LLMSuggestion
            {
                Id = string.Empty,
                Description = string.Empty,
                Confidence = 0.0,
                Steps = new List<string>(),
                Metadata = new Dictionary<string, object>()
            };
        }

        try
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Remediation Suggestion Request:");
            prompt.AppendLine($"Analysis: {analysis.Analysis}");
            prompt.AppendLine($"Approach: {analysis.Approach}");
            prompt.AppendLine("\nStrategy Scores:");
            foreach (var (strategy, score) in analysis.StrategyScores)
            {
                prompt.AppendLine($"- {strategy}: {score}");
            }

            var response = await CallLLMAsync(prompt.ToString());
            var suggestion = JsonSerializer.Deserialize<LLMSuggestion>(response, _jsonOptions);
            return suggestion ?? new LLMSuggestion
            {
                Id = string.Empty,
                Description = string.Empty,
                Confidence = 0.0,
                Steps = new List<string>(),
                Metadata = new Dictionary<string, object>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating remediation suggestion");
            return new LLMSuggestion
            {
                Id = string.Empty,
                Description = string.Empty,
                Confidence = 0.0,
                Steps = new List<string>(),
                Metadata = new Dictionary<string, object>()
            };
        }
    }

    private static string GenerateAnalysisPrompt(ErrorContext context)
    {
        var prompt = new StringBuilder();
        prompt.AppendLine("Analyze the following error context and provide remediation strategy recommendations:");
        prompt.AppendLine($"Error Type: {context.ErrorType}");
        prompt.AppendLine($"Error Message: {context.Message}");
        prompt.AppendLine($"Error Source: {context.ErrorSource}");
        prompt.AppendLine($"Severity: {context.Severity}");
        return prompt.ToString();
    }

    private async Task<string> CallLLMAsync(string prompt)
    {
        try
        {
            // TODO: Implement actual LLM API call
            await Task.Delay(100); // Simulate API call

            var response = new
            {
                StrategyScores = new Dictionary<string, double>
                {
                    ["Monitor"] = 0.8,
                    ["Alert"] = 0.6,
                    ["Backup"] = 0.4
                },
                StrategyExplanations = new Dictionary<string, string>
                {
                    ["Monitor"] = "High confidence in monitoring due to clear error patterns",
                    ["Alert"] = "Moderate confidence in alerting due to error severity",
                    ["Backup"] = "Low confidence in backup due to error type"
                },
                Analysis = "Error appears to be related to component communication",
                Approach = "Start with monitoring, then alert if issues persist"
            };

            return JsonSerializer.Serialize(response, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling LLM API");
            throw;
        }
    }

    private LLMAnalysis ParseLLMResponse(string response)
    {
        try
        {
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(response, _jsonOptions);
            if (result == null)
            {
                return new LLMAnalysis
                {
                    IsValid = false,
                    ErrorMessage = "Failed to parse LLM response",
                    Timestamp = DateTime.UtcNow
                };
            }

            var analysis = new LLMAnalysis
            {
                IsValid = true,
                StrategyScores = new Dictionary<string, double>(),
                StrategyExplanations = new Dictionary<string, string>(),
                Timestamp = DateTime.UtcNow
            };

            // Parse strategy scores
            if (result.TryGetValue("strategyScores", out var scoresObj) &&
                scoresObj is JsonElement scoresElement)
            {
                foreach (var score in scoresElement.EnumerateObject())
                {
                    if (score.Value.ValueKind == JsonValueKind.Number)
                    {
                        analysis.StrategyScores[score.Name] = score.Value.GetDouble();
                    }
                }
            }

            // Parse strategy explanations
            if (result.TryGetValue("strategyExplanations", out var explanationsObj) &&
                explanationsObj is JsonElement explanationsElement)
            {
                foreach (var explanation in explanationsElement.EnumerateObject())
                {
                    if (explanation.Value.ValueKind == JsonValueKind.String)
                    {
                        analysis.StrategyExplanations[explanation.Name] = explanation.Value.GetString()!;
                    }
                }
            }

            // Parse analysis
            if (result.TryGetValue("analysis", out var analysisObj) &&
                analysisObj is JsonElement analysisElement &&
                analysisElement.ValueKind == JsonValueKind.String)
            {
                analysis.Analysis = analysisElement.GetString()!;
            }

            // Parse approach
            if (result.TryGetValue("approach", out var approachObj) &&
                approachObj is JsonElement approachElement &&
                approachElement.ValueKind == JsonValueKind.String)
            {
                analysis.Approach = approachElement.GetString()!;
            }

            return analysis;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing LLM response");
            return new LLMAnalysis
            {
                IsValid = false,
                ErrorMessage = $"Failed to parse LLM response: {ex.Message}",
                Timestamp = DateTime.UtcNow
            };
        }
    }

    private void ValidateStrategyScores(LLMAnalysis analysis)
    {
        // Validate score ranges
        foreach (var (strategy, score) in analysis.StrategyScores.ToList())
        {
            if (score < 0.0 || score > 1.0)
            {
                _logger.LogWarning(
                    "Invalid strategy score for {Strategy}: {Score}. Clamping to valid range.",
                    strategy,
                    score);
                analysis.StrategyScores[strategy] = Math.Max(0.0, Math.Min(score, 1.0));
            }
        }

        // Ensure at least one strategy has a score
        if (!analysis.StrategyScores.Any())
        {
            analysis.IsValid = false;
            analysis.ErrorMessage = "No valid strategy scores found in LLM response";
        }
    }
} 
