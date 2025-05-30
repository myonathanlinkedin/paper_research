using RuntimeErrorSage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Models.Remediation;
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
                CorrelationId = errorId,
                Timestamp = DateTime.UtcNow,
                ComponentHealth = new Dictionary<string, double>(),
                ComponentRelationships = new List<ComponentRelationship>(),
                Metrics = new Dictionary<string, double>()
            };

            try
            {
                // Get the graph - note that GetGraphAsync already returns a GraphAnalysis
                var existingGraph = await _graphService.GetGraphAsync(errorId);
                if (existingGraph == null)
                {
                    analysis.IsValid = false;
                    analysis.ErrorMessage = "Graph not found";
                    return analysis;
                }

                // We can just use the existing graph and enhance it
                analysis = existingGraph;

                // Add additional metrics and analysis
                var confidenceScore = CalculateConfidenceScore(analysis);
                analysis.Metrics["ConfidenceScore"] = confidenceScore;
                
                analysis.IsValid = true;
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
                    .Count(h => h > 0) / (double)analysis.ComponentHealth.Count;
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

            // Factor 3: Metrics completeness
            if (analysis.Metrics.Count > 0)
            {
                score += 0.5;
                factors++;
            }

            return factors > 0 ? score / factors : 0;
        }

        public async Task<RuntimeErrorSage.Domain.Models.Graph.ComponentHealth> AnalyzeComponentHealthAsync(string componentId)
        {
            return new RuntimeErrorSage.Domain.Models.Graph.ComponentHealth
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

