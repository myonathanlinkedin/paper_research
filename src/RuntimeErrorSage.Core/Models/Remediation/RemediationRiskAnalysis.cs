using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Provides risk analysis functionality for remediation actions.
    /// </summary>
    public class RemediationRiskAnalysis
    {
        /// <summary>
        /// Calculates the risk level for a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to analyze.</param>
        /// <returns>The calculated risk level.</returns>
        public RiskLevel CalculateRiskLevel(RemediationAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // Analyze action properties to determine risk level
            if (action.Impact == RemediationActionSeverity.Critical)
            {
                return RiskLevel.High;
            }
            else if (action.Impact == RemediationActionSeverity.High)
            {
                return RiskLevel.Medium;
            }
            else if (action.Impact == RemediationActionSeverity.Medium)
            {
                return RiskLevel.Low;
            }
            
            return RiskLevel.None;
        }

        /// <summary>
        /// Generates a list of potential issues for a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to analyze.</param>
        /// <returns>A list of potential issues.</returns>
        public List<string> GeneratePotentialIssues(RemediationAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var issues = new List<string>();
            
            // Analyze action to identify potential issues
            if (action.Impact == RemediationActionSeverity.Critical || action.Impact == RemediationActionSeverity.High)
            {
                issues.Add("This action has high severity and may impact system stability");
            }
            
            if (action.ImpactScope == RemediationActionImpactScope.Global)
            {
                issues.Add("This action has global impact scope and should be executed with caution");
            }
            
            return issues;
        }

        /// <summary>
        /// Generates mitigation steps for a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to analyze.</param>
        /// <returns>A list of mitigation steps.</returns>
        public List<string> GenerateMitigationSteps(RemediationAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var steps = new List<string>();
            
            // Generate mitigation steps based on action properties
            if (action.Impact == RemediationActionSeverity.Critical || action.Impact == RemediationActionSeverity.High)
            {
                steps.Add("Backup affected components before executing this action");
                steps.Add("Verify system stability after execution");
            }
            
            if (action.ImpactScope == RemediationActionImpactScope.Global)
            {
                steps.Add("Execute this action during low-traffic periods");
                steps.Add("Have a rollback plan ready before proceeding");
            }
            
            return steps;
        }
    }
} 