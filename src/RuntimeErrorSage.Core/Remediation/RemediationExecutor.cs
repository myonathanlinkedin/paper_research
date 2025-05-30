using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Core.Storage.Utilities;

namespace RuntimeErrorSage.Core.Remediation;

public class RemediationExecutor
{
    private readonly ILogger<RemediationExecutor> _logger;
    private readonly IRemediationRiskAssessment _riskAssessment;

    public RemediationExecutor(
        ILogger<RemediationExecutor> logger,
        IRemediationRiskAssessment riskAssessment)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _riskAssessment = riskAssessment ?? throw new ArgumentNullException(nameof(riskAssessment));
    }

    public async Task<RiskAssessmentModel> CreateRiskAssessment(RemediationAction action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var assessment = new RiskAssessmentModel
        {
            StartTime = DateTime.UtcNow
        };
        assessment.CorrelationId = action.Id;

        try
        {
            // Calculate risk level
            var severityLevel = action.Impact.ToSeverityLevel();
            var remediationRiskLevel = RiskAssessmentHelper.CalculateRiskLevel(severityLevel, action.ImpactScope);
            assessment.Level = remediationRiskLevel.ToRiskLevel();

            // Generate potential issues
            assessment.PotentialIssues = GeneratePotentialIssues(remediationRiskLevel);

            // Generate mitigation steps
            assessment.MitigationStrategies = GenerateMitigationSteps(remediationRiskLevel);

            // Set confidence based on available information
            assessment.ConfidenceLevel = CalculateConfidence(action);

            // Add metadata
            assessment.Factors = new Dictionary<string, object>
            {
                ["ErrorType"] = 0.8,
                ["Context"] = 0.6
            };

            // Set affected components
            assessment.Description = $"Risk assessment for {action.ActionType} action";

            // Set timestamp
            assessment.Timestamp = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Risk assessment failed for action {ActionId}", action.Id);
            assessment.Description = $"Risk assessment failed: {ex.Message}";
            assessment.Level = RiskLevel.Unknown;
        }

        return assessment;
    }

    private double CalculateConfidence(RemediationAction action)
    {
        var confidenceFactors = new List<double>();

        // Factor 1: Error type clarity
        if (!string.IsNullOrEmpty(action.ErrorType))
        {
            confidenceFactors.Add(0.8);
        }

        // Factor 2: Stack trace availability
        if (action.Context?.Error?.StackTrace != null)
        {
            confidenceFactors.Add(0.9);
        }

        // Factor 3: Context completeness
        if (action.Parameters?.Count > 0)
        {
            confidenceFactors.Add(0.7);
        }

        // Factor 4: Impact scope clarity
        if (action.ImpactScope != RemediationActionImpactScope.None)
        {
            confidenceFactors.Add(0.6);
        }

        // Calculate average confidence
        return confidenceFactors.Any() ? confidenceFactors.Average() : 0.5;
    }

    private List<string> GetAffectedComponents(RemediationAction action)
    {
        var components = new HashSet<string>();

        // Add components from parameters
        if (action.Parameters?.TryGetValue("component", out var component) == true)
        {
            components.Add(component.ToString());
        }

        // Add components from context's stack trace
        if (action.Context?.Error?.StackTrace != null)
        {
            var stackLines = action.Context.Error.StackTrace.Split('\n');
            foreach (var line in stackLines)
            {
                if (line.Contains("at "))
                {
                    var parts = line.Split(new[] { "at " }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1)
                    {
                        var methodInfo = parts[1].Split('(')[0];
                        components.Add(methodInfo);
                    }
                }
            }
        }

        return components.ToList();
    }

    private TimeSpan EstimateDuration(RemediationAction action)
    {
        // Base duration
        var baseDuration = TimeSpan.FromMinutes(5);

        // Adjust based on risk level
        var riskMultiplier = action.RiskLevel.ToRemediationRiskLevel() switch
        {
            RemediationRiskLevel.Critical => 4.0,
            RemediationRiskLevel.High => 3.0,
            RemediationRiskLevel.Medium => 2.0,
            RemediationRiskLevel.Low => 1.5,
            _ => 1.0
        };

        // Adjust based on context complexity
        var contextMultiplier = action.Parameters?.Count > 10 ? 2.0 : 1.0;

        return TimeSpan.FromTicks((long)(baseDuration.Ticks * riskMultiplier * contextMultiplier));
    }

    private List<string> GeneratePotentialIssues(RemediationRiskLevel riskLevel)
    {
        var issues = new List<string>();
        
        switch (riskLevel)
        {
            case RemediationRiskLevel.Critical:
                issues.Add("Critical risk level may impact system stability");
                issues.Add("May affect multiple dependent systems");
                issues.Add("Potential for data loss or corruption");
                break;
            case RemediationRiskLevel.High:
                issues.Add("High risk level may affect multiple components");
                issues.Add("Performance impact during remediation");
                issues.Add("Temporary service disruption possible");
                break;
            case RemediationRiskLevel.Medium:
                issues.Add("Medium risk level may impact specific functionality");
                issues.Add("Minor performance impact possible");
                break;
            case RemediationRiskLevel.Low:
                issues.Add("Low risk with minimal impact expected");
                break;
            default:
                issues.Add("Unknown risk level - proceed with caution");
                break;
        }
        
        return issues;
    }
    
    private List<string> GenerateMitigationSteps(RemediationRiskLevel riskLevel)
    {
        var steps = new List<string>();
        
        // Add basic validation steps for all risk levels
        steps.Add("Validate all input parameters");
        steps.Add("Ensure proper error handling is in place");
        
        switch (riskLevel)
        {
            case RemediationRiskLevel.Critical:
                steps.Add("Implement comprehensive rollback strategy");
                steps.Add("Schedule during maintenance window");
                steps.Add("Prepare backup of affected components");
                steps.Add("Notify all stakeholders before proceeding");
                break;
            case RemediationRiskLevel.High:
                steps.Add("Implement basic rollback strategy");
                steps.Add("Monitor system metrics during execution");
                steps.Add("Schedule during low-traffic period");
                break;
            case RemediationRiskLevel.Medium:
                steps.Add("Implement basic validation checks");
                steps.Add("Monitor execution progress");
                break;
            case RemediationRiskLevel.Low:
                steps.Add("Implement basic logging");
                break;
            default:
                steps.Add("Implement full logging and monitoring");
                steps.Add("Prepare for manual intervention if needed");
                break;
        }
        
        return steps;
    }
} 
