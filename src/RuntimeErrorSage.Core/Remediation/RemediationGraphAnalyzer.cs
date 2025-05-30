using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using System.Linq;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Analyzes error context graphs to determine component health and relationships for remediation purposes.
    /// </summary>
    public class RemediationGraphAnalyzer : IRemediationGraphAnalyzer
    {
        private readonly ILogger<RemediationGraphAnalyzer> _logger;

        public RemediationGraphAnalyzer(ILogger<RemediationGraphAnalyzer> logger)
        {
            ArgumentNullException.ThrowIfNull(logger);
            _logger = logger;
        }

        public async Task<GraphAnalysis> AnalyzeContextGraphAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                // Validate context has required graph data
                if (context.ComponentGraph == null || !context.ComponentGraph.Any())
                {
                    return new GraphAnalysis
                    {
                        IsValid = false,
                        ErrorMessage = "Context does not contain component graph data",
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Analyze component health
                var componentHealth = await AnalyzeComponentHealthAsync(context);

                // Analyze component relationships
                var relationships = await AnalyzeComponentRelationshipsAsync(context);

                // Analyze error propagation
                var propagation = await AnalyzeErrorPropagationAsync(context);

                // Create analysis result
                var analysis = new GraphAnalysis
                {
                    IsValid = true,
                    ComponentHealth = componentHealth,
                    ComponentRelationships = relationships,
                    ErrorPropagation = propagation,
                    Timestamp = DateTime.UtcNow
                };

                _logger.LogInformation(
                    "Graph analysis completed for {Count} components with {RelationshipCount} relationships",
                    componentHealth.Count,
                    relationships.Count);

                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing context graph");
                return new GraphAnalysis
                {
                    IsValid = false,
                    ErrorMessage = $"Graph analysis failed: {ex.Message}",
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        private async Task<Dictionary<string, double>> AnalyzeComponentHealthAsync(ErrorContext context)
        {
            var health = new Dictionary<string, double>();

            foreach (var component in context.ComponentGraph.Keys)
            {
                // Calculate component health based on metrics
                var metrics = context.ComponentMetrics.TryGetValue(component, out var m) ? m : new Dictionary<string, double>();
                var healthScore = CalculateComponentHealthScore(component, metrics, context);
                health[component] = healthScore;
            }

            await Task.CompletedTask;
            return health;
        }

        private async Task<List<ComponentRelationship>> AnalyzeComponentRelationshipsAsync(ErrorContext context)
        {
            var relationships = new List<ComponentRelationship>();

            foreach (var (source, targets) in context.ComponentGraph)
            {
                foreach (var target in targets)
                {
                    // Calculate relationship strength based on metrics and context
                    var strength = CalculateRelationshipStrength(source, target, context);
                    var type = DetermineRelationshipType(source, target, context);

                    relationships.Add(new Domain.Models.Graph.ComponentRelationship
                    {
                        SourceComponent = source,
                        TargetComponent = target,
                        RelationshipType = type,
                        Strength = strength
                    });
                }
            }

            await Task.CompletedTask;
            return relationships;
        }

        private async Task<Domain.Models.Error.ErrorPropagation> AnalyzeErrorPropagationAsync(ErrorContext context)
        {
            var affectedComponents = new HashSet<string>();
            var propagationPaths = new List<List<string>>();
            var severity = new Dictionary<string, double>();

            // Start from the error source
            var source = context.ErrorSource;
            if (!string.IsNullOrEmpty(source))
            {
                // Find all affected components through graph traversal
                var visited = new HashSet<string>();
                var queue = new Queue<(string Component, List<string> Path)>();
                queue.Enqueue((source, new List<string> { source }));

                while (queue.Count > 0)
                {
                    var (component, path) = queue.Dequeue();
                    if (visited.Contains(component))
                    {
                        continue;
                    }

                    visited.Add(component);
                    affectedComponents.Add(component);

                    // Calculate component severity
                    severity[component] = CalculateComponentSeverity(component, path, context);

                    // Add propagation path if it's a leaf node or has high severity
                    if (!context.ComponentGraph.ContainsKey(component) || severity[component] > 0.7)
                    {
                        propagationPaths.Add(path);
                    }

                    // Enqueue neighbors
                    if (context.ComponentGraph.TryGetValue(component, out var neighbors))
                    {
                        foreach (var neighbor in neighbors)
                        {
                            if (!visited.Contains(neighbor))
                            {
                                var newPath = new List<string>(path) { neighbor };
                                queue.Enqueue((neighbor, newPath));
                            }
                        }
                    }
                }
            }

            await Task.CompletedTask;
            return new Domain.Models.Error.ErrorPropagation
            {
                AffectedComponents = affectedComponents.ToList(),
                PropagationPaths = propagationPaths,
                ComponentSeverity = severity
            };
        }

        private double CalculateComponentHealthScore(
            string component,
            Dictionary<string, double> metrics,
            ErrorContext context)
        {
            var score = 1.0;

            // Consider error rate
            if (metrics.TryGetValue("error_rate", out var errorRate))
            {
                score *= (1.0 - Math.Min(errorRate, 1.0));
            }

            // Consider response time
            if (metrics.TryGetValue("response_time_ms", out var responseTime))
            {
                var normalizedTime = Math.Min(responseTime / 1000.0, 1.0); // Normalize to 1 second
                score *= (1.0 - normalizedTime);
            }

            // Consider resource utilization
            if (metrics.TryGetValue("resource_utilization", out var utilization))
            {
                score *= (1.0 - Math.Min(utilization, 1.0));
            }

            // Consider error context
            if (context.ErrorSource == component)
            {
                score *= 0.5; // Reduce health if component is error source
            }

            return Math.Max(0.0, Math.Min(score, 1.0)); // Clamp between 0 and 1
        }

        private double CalculateRelationshipStrength(
            string source,
            string target,
            ErrorContext context)
        {
            var strength = 0.5; // Default strength

            // Consider error propagation
            if (context.ErrorSource == source && context.ComponentGraph[source].Contains(target))
            {
                strength += 0.3; // Stronger if part of error propagation
            }

            // Consider metrics
            if (context.ComponentMetrics.TryGetValue(source, out var sourceMetrics) &&
                context.ComponentMetrics.TryGetValue(target, out var targetMetrics))
            {
                // Consider correlation of metrics
                if (sourceMetrics.TryGetValue("error_rate", out var sourceErrorRate) &&
                    targetMetrics.TryGetValue("error_rate", out var targetErrorRate))
                {
                    var correlation = Math.Abs(sourceErrorRate - targetErrorRate);
                    strength += (1.0 - correlation) * 0.2;
                }
            }

            return Math.Max(0.0, Math.Min(strength, 1.0)); // Clamp between 0 and 1
        }

        private RelationshipType DetermineRelationshipType(
            string source,
            string target,
            ErrorContext context)
        {
            // Check if it's a direct dependency
            if (context.ComponentDependencies.Any(dep => dep.Source == source && dep.Target == target))
            {
                return RelationshipType.DirectDependency;
            }

            // Check if it's a service call
            if (context.ServiceCalls.Any(call => call.Source == source && call.Target == target))
            {
                return RelationshipType.ServiceCall;
            }

            // Check if it's a data flow
            if (context.DataFlows.Any(flow => flow.Source == source && flow.Target == target))
            {
                return RelationshipType.DataFlow;
            }

            return RelationshipType.Indirect;
        }

        private double CalculateComponentSeverity(
            string component,
            List<string> path,
            ErrorContext context)
        {
            var severity = 0.0;

            // Consider path length
            severity += Math.Max(0.0, 1.0 - (path.Count - 1) * 0.2); // Reduce severity with distance

            // Consider component health
            if (context.ComponentMetrics.TryGetValue(component, out var metrics))
            {
                if (metrics.TryGetValue("error_rate", out var errorRate))
                {
                    severity += errorRate * 0.4;
                }

                if (metrics.TryGetValue("response_time_ms", out var responseTime))
                {
                    severity += Math.Min(responseTime / 1000.0, 1.0) * 0.3;
                }
            }

            // Consider if component is error source
            if (context.ErrorSource == component)
            {
                severity += 0.3;
            }

            return Math.Max(0.0, Math.Min(severity, 1.0)); // Clamp between 0 and 1
        }
    }
} 
