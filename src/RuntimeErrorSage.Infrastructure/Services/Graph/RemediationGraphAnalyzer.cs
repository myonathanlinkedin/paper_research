using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Graph;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Domain.Enums;
using System.Linq;

namespace RuntimeErrorSage.Application.Services.Graph
{
    /// <summary>
    /// Analyzes the graph for remediation purposes.
    /// </summary>
    public class RemediationGraphAnalyzer : IRemediationGraphAnalyzer
    {
        private readonly IGraphService _graphService;
        private readonly IRemediationService _remediationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationGraphAnalyzer"/> class.
        /// </summary>
        public RemediationGraphAnalyzer(
            IGraphService graphService,
            IRemediationService remediationService)
        {
            _graphService = graphService;
            _remediationService = remediationService;
        }

        /// <summary>
        /// Analyzes the graph for remediation.
        /// </summary>
        public async Task<GraphAnalysis> AnalyzeGraphAsync(string errorId)
        {
            var analysis = new GraphAnalysis
            {
                ErrorId = errorId,
                Timestamp = DateTime.UtcNow,
                ComponentHealth = new Dictionary<string, Models.Graph.ComponentHealth>(),
                ComponentRelationships = new Collection<ComponentRelationship>(),
                Metadata = new Dictionary<string, object>()
            };

            try
            {
                // Get the graph
                var graph = await _graphService.GetGraphAsync(errorId);
                if (graph == null)
                {
                    analysis.IsValid = false;
                    analysis.ErrorMessage = "Graph not found";
                    return analysis;
                }

                // Analyze component health
                foreach (var component in graph.Components)
                {
                    var health = new Models.Graph.ComponentHealth
                    {
                        ComponentId = component.Id,
                        ComponentName = component.Name,
                        IsHealthy = true,
                        HealthScore = 100,
                        Metrics = new Dictionary<string, object>()
                    };

                    // Calculate health score based on metrics
                    if (component.Metrics != null)
                    {
                        foreach (var metric in component.Metrics)
                        {
                            health.Metrics[metric.Key] = metric.Value;
                        }
                    }

                    analysis.ComponentHealth[component.Id] = health;
                }

                // Analyze relationships
                foreach (var relationship in graph.Relationships)
                {
                    var componentRelationship = new ComponentRelationship
                    {
                        SourceComponent = relationship.SourceId,
                        TargetComponent = relationship.TargetId,
                        RelationshipType = relationship.Type,
                        Strength = relationship.Weight,
                        Metadata = new Dictionary<string, object>(),
                        Timestamp = DateTime.UtcNow
                    };

                    analysis.ComponentRelationships.Add(componentRelationship);
                }

                analysis.IsValid = true;
                analysis.ConfidenceScore = CalculateConfidenceScore(analysis);
            }
            catch (Exception ex)
            {
                analysis.IsValid = false;
                analysis.ErrorMessage = ex.Message;
            }

            return analysis;
        }

        /// <summary>
        /// Calculates the confidence score for the analysis.
        /// </summary>
        private double CalculateConfidenceScore(GraphAnalysis analysis)
        {
            if (!analysis.IsValid)
                return 0;

            var score = 0.0;
            var factors = 0;

            // Factor 1: Component health data completeness
            if (analysis.ComponentHealth.Count > 0)
            {
                var healthDataScore = analysis.ComponentHealth.Values
                    .Count(h => h.Metrics.Count > 0) / (double)analysis.ComponentHealth.Count;
                score += healthDataScore;
                factors++;
            }

            // Factor 2: Relationship data completeness
            if (analysis.ComponentRelationships.Count > 0)
            {
                var relationshipScore = analysis.ComponentRelationships
                    .Count(r => r.Strength > 0) / (double)analysis.ComponentRelationships.Count;
                score += relationshipScore;
                factors++;
            }

            // Factor 3: Metadata completeness
            if (analysis.Metadata.Count > 0)
            {
                score += 0.5;
                factors++;
            }

            return factors > 0 ? score / factors : 0;
        }

        public async Task<RuntimeErrorSage.Application.Models.Graph.ComponentHealth> AnalyzeComponentHealthAsync(string componentId)
        {
            return new RuntimeErrorSage.Application.Models.Graph.ComponentHealth
            {
                ComponentId = componentId,
                ComponentName = componentId,
                IsHealthy = true,
                HealthScore = 100,
                Metrics = new Dictionary<string, object>()
            };
        }
    }
} 







