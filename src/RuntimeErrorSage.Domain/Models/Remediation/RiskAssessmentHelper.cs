using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation;

/// <summary>
/// Helper class for risk assessment operations.
/// </summary>
public static class RiskAssessmentHelper
{
    /// <summary>
    /// Calculates the risk level based on severity and impact scope.
    /// </summary>
    /// <param name="severity">The remediation severity.</param>
    /// <param name="impactScope">The impact scope.</param>
    /// <returns>The calculated risk level.</returns>
    public static RemediationRiskLevel CalculateRiskLevel(RemediationActionSeverity severity, RemediationActionImpactScope impactScope)
    {
        // Higher severity and wider scope means higher risk
        if (severity == RemediationActionSeverity.Critical && impactScope >= RemediationActionImpactScope.System)
        {
            return RemediationRiskLevel.Critical;
        }
        else if (severity >= RemediationActionSeverity.High && impactScope >= RemediationActionImpactScope.Service)
        {
            return RemediationRiskLevel.High;
        }
        else if (severity >= RemediationActionSeverity.Medium || impactScope >= RemediationActionImpactScope.Module)
        {
            return RemediationRiskLevel.Medium;
        }
        else if (severity >= RemediationActionSeverity.Low)
        {
            return RemediationRiskLevel.Low;
        }

        return RemediationRiskLevel.None;
    }

    /// <summary>
    /// Generates standard mitigation steps based on the risk level.
    /// </summary>
    /// <param name="riskLevel">The risk level.</param>
    /// <returns>A list of recommended mitigation steps.</returns>
    public static List<string> GenerateMitigationSteps(RemediationRiskLevel riskLevel)
    {
        var steps = new List<string>();

        // Add common steps for all risk levels
        steps.Add("Review the remediation plan before execution");

        // Add risk-level-specific steps
        switch (riskLevel)
        {
            case RemediationRiskLevel.Critical:
                steps.Add("Create a full system backup before proceeding");
                steps.Add("Schedule downtime and notify all stakeholders");
                steps.Add("Prepare a detailed rollback plan");
                steps.Add("Have senior engineers on standby during execution");
                break;

            case RemediationRiskLevel.High:
                steps.Add("Backup affected components before execution");
                steps.Add("Notify key stakeholders before proceeding");
                steps.Add("Ensure rollback capability is available");
                steps.Add("Schedule execution during low-traffic periods");
                break;

            case RemediationRiskLevel.Medium:
                steps.Add("Backup affected components before execution");
                steps.Add("Test in a staging environment if possible");
                steps.Add("Monitor system during execution");
                break;

            case RemediationRiskLevel.Low:
                steps.Add("Monitor system during execution");
                break;
        }

        return steps;
    }

    /// <summary>
    /// Generates potential issues based on the risk level.
    /// </summary>
    /// <param name="riskLevel">The risk level.</param>
    /// <returns>A list of potential issues.</returns>
    public static List<string> GeneratePotentialIssues(RemediationRiskLevel riskLevel)
    {
        var issues = new List<string>();

        // Add risk-level-specific issues
        switch (riskLevel)
        {
            case RemediationRiskLevel.Critical:
                issues.Add("May cause significant system downtime");
                issues.Add("Could affect multiple dependent services");
                issues.Add("May require extensive coordination across teams");
                issues.Add("Rollback might be complex if issues occur");
                break;

            case RemediationRiskLevel.High:
                issues.Add("May cause service disruption");
                issues.Add("Could affect dependent components");
                issues.Add("Might require coordination with other teams");
                break;

            case RemediationRiskLevel.Medium:
                issues.Add("May cause brief service degradation");
                issues.Add("Could affect specific functionality");
                break;

            case RemediationRiskLevel.Low:
                issues.Add("Minimal impact expected");
                break;
        }

        return issues;
    }
} 
