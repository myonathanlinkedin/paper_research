using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Utilities;

namespace RuntimeErrorSage.Application.Models.Remediation;

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
    public RemediationRiskLevel CalculateRiskLevel(RemediationAction action)
    {
        if (action == null)
        {
            return RemediationRiskLevel.Medium;
        }

        // Use the helper to calculate risk level
        var remediationRiskLevel = RiskAssessmentHelper.CalculateRiskLevel(action.Impact, action.ImpactScope);

        // Return the calculated risk level
        return remediationRiskLevel;
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

        // Add issues based on error type
        if (!string.IsNullOrEmpty(action.ErrorType))
        {
            issues.Add($"Potential {action.ErrorType} recurrence");
        }

        // Add issues based on stack trace
        if (!string.IsNullOrEmpty(action.StackTrace))
        {
            var stackLines = action.StackTrace.Split('\n');
            if (stackLines.Length > 5)
            {
                issues.Add("Deep call stack may indicate complex error propagation");
            }
        }

        // Add issues based on context
        if (action.Context?.Count > 0)
        {
            if (action.Context.Count > 5)
            {
                issues.Add("Complex context may lead to unexpected side effects");
            }
        }

        // Add risk-based issues
        switch (action.RiskLevel)
        {
            case RemediationRiskLevel.Critical:
                issues.Add("Critical risk level may impact system stability");
                break;
            case RemediationRiskLevel.High:
                issues.Add("High risk level may affect multiple components");
                break;
            case RemediationRiskLevel.Medium:
                issues.Add("Medium risk level may impact specific functionality");
                break;
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
            return new List<string> { "Unable to generate mitigation steps - no action provided" };
        }

        var steps = new List<string>();

        // Add basic validation steps
        steps.Add("Validate all input parameters");
        steps.Add("Ensure proper error handling is in place");
        steps.Add("Verify system state before execution");

        // Add risk-specific steps
        switch (action.RiskLevel)
        {
            case RemediationRiskLevel.Critical:
                steps.Add("Implement comprehensive rollback strategy");
                steps.Add("Schedule during maintenance window");
                steps.Add("Prepare backup of affected components");
                break;
            case RemediationRiskLevel.High:
                steps.Add("Implement basic rollback strategy");
                steps.Add("Monitor system metrics during execution");
                break;
            case RemediationRiskLevel.Medium:
                steps.Add("Implement basic validation checks");
                steps.Add("Monitor execution progress");
                break;
            case RemediationRiskLevel.Low:
                steps.Add("Implement basic logging");
                break;
        }

        // Add context-specific steps
        if (action.Context?.Count > 0)
        {
            foreach (var kvp in action.Context)
            {
                steps.Add($"Validate {kvp.Key} before use");
            }
        }

        return steps;
    }
} 
