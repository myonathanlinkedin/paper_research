using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Core.Remediation
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
                SeverityLevel.None => RemediationRiskLevel.None,
                _ => RemediationRiskLevel.Medium
            };
        }

        /// <summary>
        /// Calculates the risk level based on severity and impact scope.
        /// </summary>
        /// <param name="severity">The severity level.</param>
        /// <param name="impactScope">The impact scope.</param>
        /// <returns>The calculated risk level.</returns>
        public static RemediationRiskLevel CalculateRiskLevel(SeverityLevel severity, ImpactScope impactScope)
        {
            // Convert ImpactScope to RemediationActionImpactScope and call the other overload
            return CalculateRiskLevel(severity, impactScope.ToRemediationActionImpactScope());
        }

        /// <summary>
        /// Generates a list of recommended mitigation steps based on risk level.
        /// </summary>
        /// <param name="riskLevel">The risk level.</param>
        /// <returns>A list of mitigation steps.</returns>
        public static List<string> GenerateMitigationSteps(RemediationRiskLevel riskLevel)
        {
            var steps = new List<string>();

            // Add common steps for all risk levels
            steps.Add("Monitor system metrics during execution");
            steps.Add("Have rollback plan ready");

            // Add specific steps based on risk level
            switch (riskLevel)
            {
                case RemediationRiskLevel.Critical:
                    steps.Add("Perform in maintenance window");
                    steps.Add("Notify all stakeholders");
                    steps.Add("Prepare contingency plan");
                    steps.Add("Have backup systems ready");
                    break;
                    
                case RemediationRiskLevel.High:
                    steps.Add("Perform in low-traffic period");
                    steps.Add("Notify key stakeholders");
                    steps.Add("Test in staging environment first");
                    break;
                    
                case RemediationRiskLevel.Medium:
                    steps.Add("Consider performing in low-traffic period");
                    steps.Add("Test in staging environment if possible");
                    break;
                    
                case RemediationRiskLevel.Low:
                    steps.Add("Proceed with standard precautions");
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