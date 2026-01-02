using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Models.LLM;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Analyzes errors and determines appropriate remediation actions.
    /// </summary>
    public class RemediationAnalyzer : IRemediationAnalyzer
    {
        private readonly ILogger<RemediationAnalyzer> _logger;
        private readonly IErrorContextAnalyzer _errorContextAnalyzer;
        private readonly IRemediationRegistry _registry;
        private readonly IRemediationValidator _validator;
        private readonly IRemediationStrategyProvider _strategyProvider;
        private readonly IRemediationMetricsCollector _metricsCollector;
        private readonly ILLMClient _llmClient;

        public bool IsEnabled { get; } = true;
        public string Name { get; } = "RemediationAnalyzer";
        public string Version { get; } = "1.0.0";

        public RemediationAnalyzer(
            ILogger<RemediationAnalyzer> logger,
            IErrorContextAnalyzer errorContextAnalyzer,
            IRemediationRegistry registry,
            IRemediationValidator validator,
            IRemediationStrategyProvider strategyProvider,
            IRemediationMetricsCollector metricsCollector,
            ILLMClient llmClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _errorContextAnalyzer = errorContextAnalyzer ?? throw new ArgumentNullException(nameof(errorContextAnalyzer));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _strategyProvider = strategyProvider ?? throw new ArgumentNullException(nameof(strategyProvider));
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
        }

        public async Task<RiskAssessmentModel> GetRiskAssessmentAsync(ErrorContext context)
        {
            var analysis = await AnalyzeErrorAsync(context);
            return new RiskAssessmentModel
            {
                RiskLevel = CalculateRiskLevel(analysis),
                PotentialIssues = analysis.ApplicableStrategies.Select(s => s.Reasoning).ToList(),
                MitigationSteps = GenerateMitigationSteps(analysis),
                Timestamp = DateTime.UtcNow,
                CorrelationId = context.CorrelationId
            };
        }

        public async Task<RemediationImpact> GetImpactAsync(ErrorContext context)
        {
            var analysis = await AnalyzeErrorAsync(context);
            return new RemediationImpact
            {
                Severity = (RemediationActionSeverity)analysis.ApplicableStrategies.Max(s => (int)s.Priority),
                AffectedComponents = analysis.GraphAnalysis.ErrorPropagation?.AffectedComponents,
                EstimatedDuration = TimeSpan.FromMinutes(analysis.ApplicableStrategies.Count * 5),
                RiskLevel = CalculateRiskLevel(analysis).ToRiskLevel(),
                Timestamp = DateTime.UtcNow,
                CorrelationId = context.CorrelationId
            };
        }

        public async Task<RemediationStrategyModel> GetRecommendedStrategyAsync(ErrorContext context)
        {
            var analysisResult = await AnalyzeErrorAsync(context);
            var bestStrategy = analysisResult.ApplicableStrategies.OrderByDescending(s => s.Confidence).FirstOrDefault();
            
            // Use the available method in IRemediationStrategyProvider
            var strategy = bestStrategy != null ? await _strategyProvider.GetStrategyByIdAsync(bestStrategy.StrategyName) : null;
            return strategy != null ? new RemediationStrategyModel
            {
                Id = strategy.Id,
                Name = strategy.Name,
                Description = strategy.Description,
                Priority = strategy.Priority
            } : null;
        }

        public async Task<double> GetConfidenceLevelAsync(ErrorContext context)
        {
            var analysis = await AnalyzeErrorAsync(context);
            return analysis.ApplicableStrategies.Max(s => s.Confidence);
        }

        public async Task<RemediationAnalysis> AnalyzeErrorAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                // Validate context
                var validationResult = await _validator.ValidateRemediationAsync(await GetErrorAnalysisResultAsync(context), context);
                if (!validationResult.IsValid)
                {
                    return CreateInvalidAnalysis($"Context validation failed: {string.Join(", ", validationResult.Errors)}");
                }

                // Analyze context - this returns RemediationAnalysis from Domain.Models.Remediation namespace
                var errorAnalysis = await _errorContextAnalyzer.AnalyzeContextAsync(context);
                
                // Check if analysis was successful - Remediation.RemediationAnalysis has IsValid and SuggestedActions
                bool isGraphValid = errorAnalysis != null && errorAnalysis.IsValid &&
                    (errorAnalysis.SuggestedActions?.Any() == true || errorAnalysis.GraphAnalysis != null);
                
                if (!isGraphValid)
                {
                    return CreateInvalidAnalysis("Graph analysis failed");
                }

                // Create GraphAnalysis from the error analysis data
                var graphAnalysis = new GraphAnalysis
                {
                    IsValid = true,
                    Timestamp = DateTime.UtcNow,
                    CorrelationId = context.CorrelationId,
                    ComponentHealth = new Dictionary<string, double>()
                };

                // Analyze with LLM
                var llmAnalysis = await _llmClient.AnalyzeContextAsync(context);
                
                // Check if LLM analysis was successful
                bool isLlmValid = llmAnalysis != null;
                
                if (!isLlmValid)
                {
                    return CreateInvalidAnalysis("LLM analysis failed");
                }

                // Get applicable strategies
                var strategies = await _registry.GetStrategiesForErrorAsync(context);
                if (!strategies.Any())
                {
                    return CreateInvalidAnalysis($"No strategies found for error type '{context.ErrorType}'");
                }

                // Create analysis result
                var validAnalysis = new RemediationAnalysis
                {
                    IsValid = true,
                    ErrorContext = context,
                    GraphAnalysis = graphAnalysis,
                    LLMAnalysis = llmAnalysis,
                    ApplicableStrategies = strategies
                        .Select(s => new StrategyRecommendation
                        {
                            StrategyName = s.Name,
                            Priority = (int)s.Priority,
                            Confidence = CalculateStrategyConfidence(s, graphAnalysis, llmAnalysis),
                            Reasoning = GenerateStrategyReasoning(s, graphAnalysis, llmAnalysis)
                        })
                        .OrderByDescending(r => r.Confidence)
                        .ThenByDescending(r => (int)r.Priority) // Cast RemediationPriority to int for ordering
                        .ToList(),
                    Timestamp = DateTime.UtcNow
                };

                _logger.LogInformation(
                    "Analysis completed for error type '{ErrorType}' with {Count} applicable strategies",
                    context.ErrorType,
                    validAnalysis.ApplicableStrategies.Count);

                return validAnalysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing error context");
                return CreateInvalidAnalysis($"Analysis failed: {ex.Message}");
            }
        }

        private double CalculateStrategyConfidence(
            IRemediationStrategy strategy,
            GraphAnalysis graphAnalysis,
            LLMAnalysis llmAnalysis)
        {
            var confidence = 0.0;

            // Consider graph analysis
            if (graphAnalysis != null && graphAnalysis.ComponentHealth != null)
            {
                double health;
                if (graphAnalysis.ComponentHealth.TryGetValue(strategy.Name, out health))
                {
                    confidence += health * 0.4; // 40% weight for graph analysis
                }
            }

            // Consider LLM analysis
            if (llmAnalysis != null && llmAnalysis.StrategyScores != null)
            {
                double score;
                if (llmAnalysis.StrategyScores.TryGetValue(strategy.Name, out score))
                {
                    confidence += score * 0.6; // 60% weight for LLM analysis
                }
            }

            // Consider strategy priority - convert to int first, then to double
            confidence *= (6 - (int)strategy.Priority) / 5.0; // Higher priority = higher confidence

            return Math.Min(Math.Max(confidence, 0.0), 1.0); // Clamp between 0 and 1
        }

        private string GenerateStrategyReasoning(
            IRemediationStrategy strategy,
            GraphAnalysis graphAnalysis,
            LLMAnalysis llmAnalysis)
        {
            var reasons = new List<string>();

            // Add graph analysis reasoning
            if (graphAnalysis != null && graphAnalysis.ComponentHealth != null)
            {
                double health;
                if (graphAnalysis.ComponentHealth.TryGetValue(strategy.Name, out health))
                {
                    reasons.Add($"Component health: {health:P0}");
                }
            }

            // Add LLM analysis reasoning
            if (llmAnalysis != null && llmAnalysis.StrategyScores != null)
            {
                double score;
                if (llmAnalysis.StrategyScores.TryGetValue(strategy.Name, out score))
                {
                    reasons.Add($"LLM confidence: {score:P0}");
                }
            }

            // Add priority reasoning
            reasons.Add($"Strategy priority: {strategy.Priority}/5");

            // Add LLM explanation if available
            if (llmAnalysis != null && llmAnalysis.StrategyExplanations != null)
            {
                string explanation;
                if (llmAnalysis.StrategyExplanations.TryGetValue(strategy.Name, out explanation))
                {
                    reasons.Add($"LLM explanation: {explanation}");
                }
            }

            return string.Join("; ", reasons);
        }

        private RemediationRiskLevel CalculateRiskLevel(RemediationAnalysis analysis)
        {
            if (!analysis.IsValid) return RemediationRiskLevel.Critical;
            
            // Convert the Priority to int before using it in the switch
            var maxSeverity = analysis.ApplicableStrategies.Max(s => (int)s.Priority);
            return maxSeverity switch
            {
                1 => RemediationRiskLevel.Low,
                2 => RemediationRiskLevel.Medium,
                3 => RemediationRiskLevel.High,
                _ => RemediationRiskLevel.Critical
            };
        }

        private List<string> GenerateMitigationSteps(RemediationAnalysis analysis)
        {
            var steps = new List<string>();
            
            foreach (var strategy in analysis.ApplicableStrategies.OrderBy(s => s.Priority))
            {
                steps.Add($"Strategy: {strategy.StrategyName}");
                steps.Add($"- Priority: {strategy.Priority}");
                steps.Add($"- Confidence: {strategy.Confidence:P0}");
                steps.Add($"- Reasoning: {strategy.Reasoning}");
            }

            return steps;
        }

        // Helper method to create an invalid analysis result
        private RemediationAnalysis CreateInvalidAnalysis(string errorMessage)
        {
            return new RemediationAnalysis
            {
                IsValid = false,
                ErrorMessage = errorMessage,
                Timestamp = DateTime.UtcNow,
                ApplicableStrategies = new List<StrategyRecommendation>()
            };
        }
        
        // Helper method to get ErrorAnalysisResult from ErrorContext
        private async Task<ErrorAnalysisResult> GetErrorAnalysisResultAsync(ErrorContext context)
        {
            // Create a simple ErrorAnalysisResult based on the context
            return new ErrorAnalysisResult
            {
                ErrorId = context.ErrorId,
                CorrelationId = context.CorrelationId,
                Timestamp = DateTime.UtcNow,
                Status = Domain.Enums.AnalysisStatus.Completed,
                ErrorType = context.ErrorType
            };
        }
    }
} 
