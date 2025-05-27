using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Provides risk assessment functionality for remediation actions.
/// </summary>
public class RemediationRiskAssessment : IRemediationRiskAssessment
{
    /// <summary>
    /// Calculates the risk level for a remediation action.
    /// </summary>
    /// <param name="action">The remediation action to assess.</param>
    /// <returns>The calculated risk level.</returns>
    public RiskLevel CalculateRiskLevel(RemediationAction action)
    {
        if (action == null)
        {
            return RiskLevel.Medium;
        }

        // Convert from RemediationRiskLevel to RiskLevel
        return action.RiskLevel switch
        {
            RemediationRiskLevel.Low => RiskLevel.Low,
            RemediationRiskLevel.Medium => RiskLevel.Medium,
            RemediationRiskLevel.High => RiskLevel.High,
            RemediationRiskLevel.Critical => RiskLevel.Critical,
            _ => RiskLevel.Medium,
        };
    }

    /// <summary>
    /// Generates a list of potential issues based on the remediation action.
    /// </summary>
    /// <param name="action">The remediation action to analyze.</param>
    /// <returns>A list of potential issues.</returns>
    public List<string> GeneratePotentialIssues(RemediationAction action)
    {
        if (action == null)
        {
            return new List<string> { "Unable to assess action - no action provided" };
        }

        var issues = new List<string>();

        // Check for risks based on action properties
        if (action.RequiresManualApproval)
        {
            issues.Add("Requires manual approval which may delay remediation");
        }

        if (action.RiskLevel == RemediationRiskLevel.High || action.RiskLevel == RemediationRiskLevel.Critical)
        {
            issues.Add($"High risk action with potential system impact: {action.Description}");
        }

        if (action.ImpactScope == RemediationActionImpactScope.Global)
        {
            issues.Add("Action has global impact scope which may affect multiple components");
        }

        if (action.Dependencies.Count > 0)
        {
            issues.Add($"Action has {action.Dependencies.Count} dependencies which increases complexity");
        }

        if (issues.Count == 0)
        {
            issues.Add("No significant issues identified");
        }

        return issues;
    }

    /// <summary>
    /// Generates mitigation steps for a remediation action.
    /// </summary>
    /// <param name="action">The remediation action to generate mitigation steps for.</param>
    /// <returns>A list of mitigation steps.</returns>
    public List<string> GenerateMitigationSteps(RemediationAction action)
    {
        if (action == null)
        {
            return new List<string> { "Create backup before proceeding with any remediation" };
        }

        var steps = new List<string>();

        // Generate mitigation steps based on action properties
        if (action.CanRollback)
        {
            steps.Add("Rollback plan is available if remediation fails");
        }
        else
        {
            steps.Add("Create a manual backup before proceeding");
        }

        if (action.RequiresManualApproval)
        {
            steps.Add("Verify changes before approval");
        }

        if (action.RiskLevel == RemediationRiskLevel.High || action.RiskLevel == RemediationRiskLevel.Critical)
        {
            steps.Add("Execute during maintenance window to minimize impact");
            steps.Add("Prepare contingency plan for critical services");
        }

        if (action.ImpactScope != RemediationActionImpactScope.None)
        {
            steps.Add("Notify affected system owners before proceeding");
        }

        if (steps.Count == 0)
        {
            steps.Add("Standard monitoring during execution");
        }

        return steps;
    }
} 