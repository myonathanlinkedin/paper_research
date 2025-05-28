using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Utilities;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    public class RemediationActionSystem
    {
        private RemediationActionSeverity CalculateImpactSeverity(RemediationSuggestion suggestion)
        {
            // Determine impact severity based on actions
            var severities = suggestion.Actions?.Select(a => a.Severity) ?? Enumerable.Empty<RemediationActionSeverity>();
            return severities.Any() ? severities.Max() : RemediationActionSeverity.Medium;
        }

        private List<string> GetAffectedComponents(RemediationSuggestion suggestion, ErrorContext context)
        {
            // Get list of affected components based on actions and context
            var components = new HashSet<string>();
            
            if (suggestion.Actions != null)
            {
                foreach (var action in suggestion.Actions)
                {
                    if (action.Parameters?.TryGetValue("component", out var component) == true)
                    {
                        components.Add(component.ToString());
                    }
                }
            }

            if (context.ComponentId != null)
            {
                components.Add(context.ComponentId);
            }

            return components.ToList();
        }

        private RemediationRiskLevel CalculateRiskLevel(RemediationSuggestion suggestion)
        {
            // Calculate risk level based on complexity and impact
            var complexityScore = CalculateComplexityScore(suggestion);
            var impactScore = (double)CalculateImpactSeverity(suggestion) / (double)RemediationActionSeverity.Critical;

            var riskScore = (complexityScore + impactScore) / 2.0;
            return riskScore switch
            {
                < 0.3 => RemediationRiskLevel.Low,
                < 0.6 => RemediationRiskLevel.Medium,
                < 0.8 => RemediationRiskLevel.High,
                _ => RemediationRiskLevel.Critical
            };
        }

        private double CalculateComplexityScore(RemediationSuggestion suggestion)
        {
            var factors = new List<double>();

            // Factor 1: Number of actions
            if (suggestion.Actions?.Count > 0)
            {
                factors.Add(Math.Min(suggestion.Actions.Count / 5.0, 1.0));
            }

            // Factor 2: Action complexity
            if (suggestion.Actions != null)
            {
                var actionComplexity = suggestion.Actions.Average(a =>
                {
                    var complexity = 0.0;
                    if (a.Parameters?.Count > 0) complexity += 0.3;
                    if (a.RequiresManualApproval) complexity += 0.2;
                    if (a.CanRollback) complexity += 0.1;
                    return complexity;
                });
                factors.Add(actionComplexity);
            }

            // Factor 3: Context complexity
            if (suggestion.Context?.Count > 0)
            {
                factors.Add(Math.Min(suggestion.Context.Count / 10.0, 1.0));
            }

            return factors.Any() ? factors.Average() : 0.5;
        }
    }
} 