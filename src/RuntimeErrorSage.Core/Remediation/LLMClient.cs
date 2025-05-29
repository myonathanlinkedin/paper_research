using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.LLM;
using System.Text.Json;
using System.Text;
using RuntimeErrorSage.Model.Models.Remediation;
using RuntimeErrorSage.Model.LLM.Interfaces;

namespace RuntimeErrorSage.Model.Remediation
{
    /// <summary>
    /// Client for interacting with the Qwen 2.5 7B Instruct 1M language model.
    /// </summary>
    public class LLMClient : ILLMClient
    {
        private readonly ILogger<LLMClient> _logger;
        private readonly string _modelEndpoint;
        private readonly string _apiKey;
        private readonly JsonSerializerOptions _jsonOptions;

        public bool IsEnabled => true;

        public string Name => "Qwen2.5-7B-Instruct-1M";

        public string Version => "1.0.0";

        public bool IsConnected => true;

        public LLMClient(
            ILogger<LLMClient> logger,
            string modelEndpoint,
            string apiKey)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _modelEndpoint = modelEndpoint ?? throw new ArgumentNullException(nameof(modelEndpoint));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<LLMAnalysis> AnalyzeContextAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                // Prepare prompt
                var prompt = GenerateAnalysisPrompt(context);

                // Call LLM
                var response = await CallLLMAsync(prompt);

                // Parse and validate response
                var analysis = ParseLLMResponse(response);
                if (!analysis.IsValid)
                {
                    return analysis;
                }

                // Validate strategy scores
                ValidateStrategyScores(analysis);

                _logger.LogInformation(
                    "LLM analysis completed for error type '{ErrorType}' with {Count} strategy recommendations",
                    context.ErrorType,
                    analysis.StrategyScores.Count);

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

        private string GenerateAnalysisPrompt(ErrorContext context)
        {
            var prompt = new System.Text.StringBuilder();

            // Add error context
            prompt.AppendLine("Analyze the following error context and provide remediation strategy recommendations:");
            prompt.AppendLine($"Error Type: {context.ErrorType}");
            prompt.AppendLine($"Error Message: {context.Message}");
            prompt.AppendLine($"Error Source: {context.ErrorSource}");
            prompt.AppendLine($"Severity: {context.Severity}");

            // Add component graph
            if (context.ComponentGraph != null && context.ComponentGraph.Any())
            {
                prompt.AppendLine("\nComponent Graph:");
                foreach (var (source, targets) in context.ComponentGraph)
                {
                    prompt.AppendLine($"- {source} -> {string.Join(", ", targets)}");
                }
            }

            // Add system state
            if (context.SystemState != null && context.SystemState.Any())
            {
                prompt.AppendLine("\nSystem State:");
                foreach (var (key, value) in context.SystemState)
                {
                    prompt.AppendLine($"- {key}: {value}");
                }
            }

            // Add request for analysis
            prompt.AppendLine("\nPlease provide:");
            prompt.AppendLine("1. Strategy scores (0-1) for each applicable strategy");
            prompt.AppendLine("2. Brief explanation for each strategy recommendation");
            prompt.AppendLine("3. Overall analysis of the error context");
            prompt.AppendLine("4. Suggested remediation approach");

            return prompt.ToString();
        }

        private async Task<string> CallLLMAsync(string prompt)
        {
            try
            {
                // TODO: Implement actual LLM API call
                // For now, return a simulated response
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
                    SuggestionId = string.Empty,
                    Content = string.Empty,
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
                    SuggestionId = string.Empty,
                    Content = string.Empty,
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
                    SuggestionId = string.Empty,
                    Content = string.Empty,
                    Confidence = 0.0,
                    Steps = new List<string>(),
                    Metadata = new Dictionary<string, object>()
                };
            }
        }
    }
} 
