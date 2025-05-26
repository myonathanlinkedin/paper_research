using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Remediation.Models.Analysis;
using RuntimeErrorSage.Core.Remediation.Models.Common;
using RuntimeErrorSage.Core.Remediation.Models.Validation;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Analyzes error contexts to determine appropriate remediation strategies.
    /// </summary>
    public class RemediationAnalyzer : IRemediationAnalyzer
    {
        private readonly ILogger<RemediationAnalyzer> _logger;
        private readonly IRemediationValidator _validator;
        private readonly IRemediationRegistry _registry;
        private readonly IGraphAnalyzer _graphAnalyzer;
        private readonly ILLMClient _llmClient;

        public RemediationAnalyzer(
            ILogger<RemediationAnalyzer> logger,
            IRemediationValidator validator,
            IRemediationRegistry registry,
            IGraphAnalyzer graphAnalyzer,
            ILLMClient llmClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _graphAnalyzer = graphAnalyzer ?? throw new ArgumentNullException(nameof(graphAnalyzer));
            _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
        }

        public async Task<RemediationAnalysis> AnalyzeErrorAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                // Validate context
                var validationResult = await _validator.ValidateRemediationAsync(context);
                if (!validationResult.IsValid)
                {
                    return new RemediationAnalysis
                    {
                        IsValid = false,
                        ErrorMessage = $"Context validation failed: {string.Join(", ", validationResult.Errors)}",
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Analyze context graph
                var graphAnalysis = await _graphAnalyzer.AnalyzeContextGraphAsync(context);
                if (!graphAnalysis.IsValid)
                {
                    return new RemediationAnalysis
                    {
                        IsValid = false,
                        ErrorMessage = $"Graph analysis failed: {graphAnalysis.ErrorMessage}",
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Analyze with LLM
                var llmAnalysis = await _llmClient.AnalyzeContextAsync(context);
                if (!llmAnalysis.IsValid)
                {
                    return new RemediationAnalysis
                    {
                        IsValid = false,
                        ErrorMessage = $"LLM analysis failed: {llmAnalysis.ErrorMessage}",
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Get applicable strategies
                var strategies = await _registry.GetStrategiesForErrorTypeAsync(context.ErrorType);
                if (!strategies.Any())
                {
                    return new RemediationAnalysis
                    {
                        IsValid = false,
                        ErrorMessage = $"No strategies found for error type '{context.ErrorType}'",
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Create analysis result
                var analysis = new RemediationAnalysis
                {
                    IsValid = true,
                    ErrorContext = context,
                    GraphAnalysis = graphAnalysis,
                    LLMAnalysis = llmAnalysis,
                    ApplicableStrategies = strategies
                        .Select(s => new StrategyRecommendation
                        {
                            StrategyName = s.Name,
                            Priority = s.Priority,
                            Confidence = CalculateStrategyConfidence(s, graphAnalysis, llmAnalysis),
                            Reasoning = GenerateStrategyReasoning(s, graphAnalysis, llmAnalysis)
                        })
                        .OrderByDescending(r => r.Confidence)
                        .ThenByDescending(r => r.Priority)
                        .ToList(),
                    Timestamp = DateTime.UtcNow
                };

                _logger.LogInformation(
                    "Analysis completed for error type '{ErrorType}' with {Count} applicable strategies",
                    context.ErrorType,
                    analysis.ApplicableStrategies.Count);

                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing error context");
                return new RemediationAnalysis
                {
                    IsValid = false,
                    ErrorMessage = $"Analysis failed: {ex.Message}",
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        private double CalculateStrategyConfidence(
            IRemediationStrategy strategy,
            GraphAnalysis graphAnalysis,
            LLMAnalysis llmAnalysis)
        {
            var confidence = 0.0;

            // Consider graph analysis
            if (graphAnalysis.ComponentHealth.TryGetValue(strategy.Name, out var health))
            {
                confidence += health * 0.4; // 40% weight for graph analysis
            }

            // Consider LLM analysis
            if (llmAnalysis.StrategyScores.TryGetValue(strategy.Name, out var score))
            {
                confidence += score * 0.6; // 60% weight for LLM analysis
            }

            // Consider strategy priority
            confidence *= (6 - strategy.Priority) / 5.0; // Higher priority = higher confidence

            return Math.Min(Math.Max(confidence, 0.0), 1.0); // Clamp between 0 and 1
        }

        private string GenerateStrategyReasoning(
            IRemediationStrategy strategy,
            GraphAnalysis graphAnalysis,
            LLMAnalysis llmAnalysis)
        {
            var reasons = new List<string>();

            // Add graph analysis reasoning
            if (graphAnalysis.ComponentHealth.TryGetValue(strategy.Name, out var health))
            {
                reasons.Add($"Component health: {health:P0}");
            }

            // Add LLM analysis reasoning
            if (llmAnalysis.StrategyScores.TryGetValue(strategy.Name, out var score))
            {
                reasons.Add($"LLM confidence: {score:P0}");
            }

            // Add priority reasoning
            reasons.Add($"Strategy priority: {strategy.Priority}/5");

            // Add LLM explanation if available
            if (llmAnalysis.StrategyExplanations.TryGetValue(strategy.Name, out var explanation))
            {
                reasons.Add($"LLM explanation: {explanation}");
            }

            return string.Join("; ", reasons);
        }
    }
} 