using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Core.Storage.Utilities
{
    /// <summary>
    /// Helper class for risk assessment operations.
    /// </summary>
    public static class RiskAssessmentHelper
    {
        /// <summary>
        /// Calculates the risk level based on severity and impact scope.
        /// </summary>
        /// <param name="severity">The severity level.</param>
        /// <param name="impactScope">The impact scope.</param>
        /// <returns>The calculated risk level.</returns>
        public static RemediationRiskLevel CalculateRiskLevel(SeverityLevel severity, RemediationActionImpactScope impactScope)
        {
            // Critical severity always results in Critical risk for global/system impact
            if (severity == SeverityLevel.Critical && (impactScope == RemediationActionImpactScope.Global || impactScope == RemediationActionImpactScope.System))
            {
                return RemediationRiskLevel.Critical;
            }

            // High severity with global/system impact results in Critical risk
            if (severity == SeverityLevel.High && (impactScope == RemediationActionImpactScope.Global || impactScope == RemediationActionImpactScope.System))
            {
                return RemediationRiskLevel.Critical;
            }

            // Medium severity with global/system impact results in High risk
            if (severity == SeverityLevel.Medium && (impactScope == RemediationActionImpactScope.Global || impactScope == RemediationActionImpactScope.System))
            {
                return RemediationRiskLevel.High;
            }

            // Low severity with global/system impact results in Medium risk
            if (severity == SeverityLevel.Low && (impactScope == RemediationActionImpactScope.Global || impactScope == RemediationActionImpactScope.System))
            {
                return RemediationRiskLevel.Medium;
            }

            // For other combinations, map severity directly to risk level
            return severity switch
            {
                SeverityLevel.Critical => RemediationRiskLevel.Critical,
                SeverityLevel.High => RemediationRiskLevel.High,
                SeverityLevel.Medium => RemediationRiskLevel.Medium,
                SeverityLevel.Low => RemediationRiskLevel.Low,
                _ => RemediationRiskLevel.None
            };
        }

        /// <summary>
        /// Generates potential issues based on the risk level.
        /// </summary>
        /// <param name="riskLevel">The risk level.</param>
        /// <returns>A list of potential issues.</returns>
        public static List<string> GeneratePotentialIssues(RemediationRiskLevel riskLevel)
        {
            var issues = new List<string>();
            
            switch (riskLevel)
            {
                case RemediationRiskLevel.Critical:
                    issues.Add("May cause significant system downtime");
                    issues.Add("Could affect multiple dependent services");
                    issues.Add("May require manual intervention to recover");
                    issues.Add("Data loss or corruption is possible");
                    issues.Add("May affect system stability");
                    break;
                    
                case RemediationRiskLevel.High:
                    issues.Add("May cause temporary service disruption");
                    issues.Add("Could affect related components");
                    issues.Add("May require additional monitoring after execution");
                    issues.Add("Performance degradation is possible");
                    break;
                    
                case RemediationRiskLevel.Medium:
                    issues.Add("May cause minor service disruption");
                    issues.Add("Could affect specific functionality");
                    issues.Add("May require validation after execution");
                    break;
                    
                case RemediationRiskLevel.Low:
                    issues.Add("Minimal impact expected");
                    issues.Add("Limited to specific component");
                    break;
                    
                case RemediationRiskLevel.None:
                    issues.Add("No significant impact expected");
                    break;
                    
                default:
                    issues.Add("Unknown potential issues");
                    break;
            }
            
            return issues;
        }

        /// <summary>
        /// Generates mitigation steps based on the risk level.
        /// </summary>
        /// <param name="riskLevel">The risk level.</param>
        /// <returns>A list of mitigation steps.</returns>
        public static List<string> GenerateMitigationSteps(RemediationRiskLevel riskLevel)
        {
            var steps = new List<string>();
            
            // Common steps for all risk levels
            steps.Add("Verify system state before execution");
            steps.Add("Validate system state after execution");
            
            switch (riskLevel)
            {
                case RemediationRiskLevel.Critical:
                    steps.Add("Schedule during maintenance window");
                    steps.Add("Prepare rollback plan");
                    steps.Add("Coordinate with dependent service teams");
                    steps.Add("Set up additional monitoring");
                    steps.Add("Prepare incident response plan");
                    break;
                    
                case RemediationRiskLevel.High:
                    steps.Add("Schedule during low-traffic period");
                    steps.Add("Prepare rollback plan");
                    steps.Add("Notify dependent service teams");
                    steps.Add("Set up monitoring");
                    break;
                    
                case RemediationRiskLevel.Medium:
                    steps.Add("Schedule during off-peak hours");
                    steps.Add("Prepare basic rollback plan");
                    steps.Add("Set up basic monitoring");
                    break;
                    
                case RemediationRiskLevel.Low:
                    steps.Add("Schedule during normal business hours");
                    steps.Add("Prepare simple rollback plan");
                    break;
                    
                case RemediationRiskLevel.None:
                    steps.Add("Execute during normal operations");
                    break;
                    
                default:
                    steps.Add("Execute with caution");
                    break;
            }
            
            return steps;
        }

        /// <summary>
        /// Calculates the confidence level for a risk assessment.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <returns>A confidence value between 0 and 100.</returns>
        public static double CalculateConfidence(RemediationAction action)
        {
            if (action == null)
            {
                return 0.0;
            }

            // Simple confidence calculation based on available data
            double confidence = 50.0; // Base confidence
            
            // Adjust based on available data
            if (!string.IsNullOrEmpty(action.Description))
            {
                confidence += 10.0;
            }
            
            if (action.ValidationResults?.Count > 0)
            {
                confidence += 15.0;
            }
            
            if (action.Context != null)
            {
                confidence += 15.0;

                // Additional confidence for context details
                if (!string.IsNullOrEmpty(action.Context.ErrorType))
                {
                    confidence += 5.0;
                }
                
                if (!string.IsNullOrEmpty(action.Context.ErrorSource))
                {
                    confidence += 5.0;
                }
                
                if (action.Context.AffectedComponents?.Count > 0)
                {
                    confidence += 5.0;
                }
            }
            
            return Math.Min(100.0, confidence);
        }
    }
} 
