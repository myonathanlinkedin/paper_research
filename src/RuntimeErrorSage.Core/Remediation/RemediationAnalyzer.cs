using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.LLM;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Analysis.Interfaces;

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

        public async Task<RiskAssessment> GetRiskAssessmentAsync(ErrorContext context)
        {
            var analysis = await AnalyzeErrorAsync(context);
            return new RiskAssessment
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
                Severity = analysis.ApplicableStrategies.Max(s => s.Priority),
                AffectedComponents = analysis.GraphAnalysis.AffectedComponents,
                EstimatedDuration = TimeSpan.FromMinutes(analysis.ApplicableStrategies.Count * 5),
                RiskLevel = CalculateRiskLevel(analysis),
                Timestamp = DateTime.UtcNow,
                CorrelationId = context.CorrelationId
            };
        }

        public async Task<RemediationStrategyModel> GetRecommendedStrategyAsync(ErrorContext context)
        {
            var analysis = await AnalyzeErrorAsync(context);
            var bestStrategy = analysis.ApplicableStrategies.OrderByDescending(s => s.Confidence).FirstOrDefault();
            return bestStrategy != null ? await _strategyProvider.GetStrategyAsync(bestStrategy.StrategyName) : null;
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
                var graphAnalysis = await _errorContextAnalyzer.AnalyzeContextAsync(context);
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

        private RemediationRiskLevel CalculateRiskLevel(RemediationAnalysis analysis)
        {
            if (!analysis.IsValid) return RemediationRiskLevel.Critical;
            
            var maxSeverity = analysis.ApplicableStrategies.Max(s => s.Priority);
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
    }
} 