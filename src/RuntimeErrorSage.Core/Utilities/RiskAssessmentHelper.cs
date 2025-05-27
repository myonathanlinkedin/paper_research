using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Utilities
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
        public static RemediationRiskLevel CalculateRiskLevel(Models.Enums.RemediationSeverity severity, RemediationActionImpactScope impactScope)
        {
            // Calculate risk level based on severity and impact scope
            // Higher severity and wider impact scope result in higher risk level
            
            if (severity == Models.Enums.RemediationSeverity.Critical)
            {
                return impactScope >= RemediationActionImpactScope.Module ? 
                    RemediationRiskLevel.Critical : RemediationRiskLevel.High;
            }
            
            if (severity == Models.Enums.RemediationSeverity.High)
            {
                return impactScope >= RemediationActionImpactScope.Service ? 
                    RemediationRiskLevel.Critical : RemediationRiskLevel.High;
            }
            
            if (severity == Models.Enums.RemediationSeverity.Medium)
            {
                return impactScope >= RemediationActionImpactScope.System ? 
                    RemediationRiskLevel.High : RemediationRiskLevel.Medium;
            }
            
            if (severity == Models.Enums.RemediationSeverity.Low)
            {
                return impactScope >= RemediationActionImpactScope.Global ? 
                    RemediationRiskLevel.Medium : RemediationRiskLevel.Low;
            }
            
            // If severity is None or not recognized, return None
            return RemediationRiskLevel.None;
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
            
            switch (riskLevel)
            {
                case RemediationRiskLevel.Critical:
                    steps.Add("Create full system backup before proceeding");
                    steps.Add("Schedule maintenance window");
                    steps.Add("Notify all stakeholders");
                    steps.Add("Prepare rollback plan");
                    steps.Add("Have emergency response team on standby");
                    steps.Add("Test in isolated environment first");
                    break;
                    
                case RemediationRiskLevel.High:
                    steps.Add("Create targeted backup of affected components");
                    steps.Add("Notify key stakeholders");
                    steps.Add("Prepare rollback procedure");
                    steps.Add("Monitor system during execution");
                    steps.Add("Test in staging environment first");
                    break;
                    
                case RemediationRiskLevel.Medium:
                    steps.Add("Backup affected configuration");
                    steps.Add("Notify service owners");
                    steps.Add("Document current state");
                    steps.Add("Monitor affected components");
                    break;
                    
                case RemediationRiskLevel.Low:
                    steps.Add("Document changes");
                    steps.Add("Verify functionality after execution");
                    break;
                    
                case RemediationRiskLevel.None:
                    steps.Add("Standard execution without special precautions");
                    break;
                    
                default:
                    steps.Add("Proceed with caution");
                    break;
            }
            
            steps.Add("Validate system state after execution");
            
            return steps;
        }
    }
} 