using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Error;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Provides risk assessment functionality for remediation actions.
/// </summary>
public class RemediationRiskAssessment : IRemediationRiskAssessment
{
    /// <summary>
    /// Assesses the risk of a remediation strategy.
    /// </summary>
    /// <param name="strategy">The remediation strategy to assess.</param>
    /// <returns>A risk assessment result.</returns>
    public RiskAssessmentModel AssessRisk(RemediationStrategyModel strategy)
    {
        if (strategy == null)
        {
            throw new ArgumentNullException(nameof(strategy));
        }

        var assessment = new RiskAssessmentModel
        {
            RiskLevel = strategy.RiskLevel.ToRemediationRiskLevel(),
            Description = $"Risk assessment for strategy: {strategy.Name}",
            PotentialIssues = new List<string>(),
            MitigationSteps = new List<string>(),
            ConfidenceLevel = 0.7,
            Timestamp = DateTime.UtcNow,
            ActionId = strategy.Id
        };

        // Add standard potential issues
        assessment.PotentialIssues.Add($"Strategy {strategy.Name} may have unforeseen side effects");
        assessment.PotentialIssues.Add("System dependencies may be affected");

        // Add standard mitigation steps
        assessment.MitigationSteps.Add("Monitor system metrics during execution");
        assessment.MitigationSteps.Add("Have rollback plan ready");

        return assessment;
    }

    /// <summary>
    /// Gets the risk factors for a remediation strategy.
    /// </summary>
    /// <param name="strategy">The remediation strategy to assess.</param>
    /// <returns>A list of risk factors.</returns>
    public List<RiskFactor> GetRiskFactors(RemediationStrategyModel strategy)
    {
        if (strategy == null)
        {
            throw new ArgumentNullException(nameof(strategy));
        }

        var factors = new List<RiskFactor>();

        // Add standard risk factors
        factors.Add(new RiskFactor
        {
            Name = "Complexity",
            Value = strategy.Complexity ?? 0.5,
            Weight = 0.3,
            Description = "Complexity of the strategy implementation"
        });

        factors.Add(new RiskFactor
        {
            Name = "SystemImpact",
            Value = strategy.SystemImpact ?? 0.5,
            Weight = 0.4,
            Description = "Potential impact on system components"
        });

        factors.Add(new RiskFactor
        {
            Name = "RollbackDifficulty",
            Value = strategy.IsRollbackable ? 0.3 : 0.8,
            Weight = 0.3,
            Description = "Difficulty in rolling back changes if needed"
        });

        return factors;
    }

    /// <summary>
    /// Gets the risk metrics for a remediation strategy.
    /// </summary>
    /// <param name="strategy">The remediation strategy to assess.</param>
    /// <returns>The risk metrics.</returns>
    public RiskMetrics GetRiskMetrics(RemediationStrategyModel strategy)
    {
        if (strategy == null)
        {
            throw new ArgumentNullException(nameof(strategy));
        }

        var metrics = new RiskMetrics
        {
            OverallRiskScore = CalculateOverallRiskScore(strategy),
            ConfidenceScore = 0.7,
            PotentialImpactScore = strategy.SystemImpact ?? 0.5,
            ComplexityScore = strategy.Complexity ?? 0.5,
            TimeSensitivityScore = 0.5,
            StrategyId = strategy.Id,
            Timestamp = DateTime.UtcNow
        };

        return metrics;
    }

    /// <summary>
    /// Assesses the risk of a remediation action.
    /// </summary>
    /// <param name="action">The remediation action to assess.</param>
    /// <param name="context">The error context.</param>
    /// <returns>The risk assessment result.</returns>
    public async Task<RiskAssessmentModel> AssessRiskAsync(RemediationAction action, ErrorContext context)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var assessment = new RiskAssessmentModel
        {
            RiskLevel = CalculateRiskLevel(action),
            Description = $"Risk assessment for action: {action.Name}",
            PotentialIssues = GeneratePotentialIssues(action),
            MitigationSteps = GenerateMitigationSteps(action),
            ConfidenceLevel = 0.8,
            Timestamp = DateTime.UtcNow,
            ActionId = action.ActionId,
            Status = AnalysisStatus.Completed,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow
        };

        return assessment;
    }

    /// <summary>
    /// Assesses the risk of a remediation strategy.
    /// </summary>
    /// <param name="strategy">The remediation strategy to assess.</param>
    /// <param name="context">The error context.</param>
    /// <returns>The risk assessment result.</returns>
    public async Task<RiskAssessmentModel> AssessStrategyRiskAsync(IRemediationStrategy strategy, ErrorContext context)
    {
        if (strategy == null)
        {
            throw new ArgumentNullException(nameof(strategy));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var assessment = new RiskAssessmentModel
        {
            RiskLevel = RemediationRiskLevel.Medium, // Default to medium
            Description = $"Risk assessment for strategy: {strategy.Name}",
            PotentialIssues = new List<string>
            {
                $"Strategy {strategy.Name} may have unforeseen side effects",
                "System dependencies may be affected"
            },
            MitigationSteps = new List<string>
            {
                "Monitor system metrics during execution",
                "Have rollback plan ready"
            },
            ConfidenceLevel = 0.7,
            Timestamp = DateTime.UtcNow,
            ActionId = strategy.Id,
            Status = AnalysisStatus.Completed,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow
        };

        return assessment;
    }

    /// <summary>
    /// Assesses the risk of a remediation plan.
    /// </summary>
    /// <param name="plan">The remediation plan to assess.</param>
    /// <returns>The risk assessment result.</returns>
    public async Task<RiskAssessmentModel> AssessPlanRiskAsync(RemediationPlan plan)
    {
        if (plan == null)
        {
            throw new ArgumentNullException(nameof(plan));
        }

        var assessment = new RiskAssessmentModel
        {
            RiskLevel = plan.RiskLevel,
            Description = $"Risk assessment for plan: {plan.Id}",
            PotentialIssues = new List<string>
            {
                "Plan execution may affect system stability",
                "Multiple steps increase complexity"
            },
            MitigationSteps = new List<string>
            {
                "Execute during low-traffic periods",
                "Monitor system metrics during execution",
                "Have rollback plan ready for each step"
            },
            ConfidenceLevel = 0.6,
            Timestamp = DateTime.UtcNow,
            ActionId = plan.Id,
            Status = AnalysisStatus.Completed,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow
        };

        return assessment;
    }

    /// <summary>
    /// Gets the risk level for a given error type.
    /// </summary>
    /// <param name="errorType">The error type to assess.</param>
    /// <returns>The risk level.</returns>
    public async Task<RemediationRiskLevel> GetRiskLevelForErrorTypeAsync(string errorType)
    {
        if (string.IsNullOrEmpty(errorType))
        {
            return RemediationRiskLevel.Medium;
        }

        // Determine risk level based on error type
        if (errorType.Contains("Critical") || errorType.Contains("Fatal"))
        {
            return RemediationRiskLevel.Critical;
        }
        else if (errorType.Contains("High") || errorType.Contains("Severe"))
        {
            return RemediationRiskLevel.High;
        }
        else if (errorType.Contains("Low") || errorType.Contains("Minor"))
        {
            return RemediationRiskLevel.Low;
        }

        return RemediationRiskLevel.Medium;
    }

    /// <summary>
    /// Gets the risk metrics for a remediation operation.
    /// </summary>
    /// <param name="remediationId">The remediation ID.</param>
    /// <returns>The risk metrics.</returns>
    public async Task<RiskMetrics> GetRiskMetricsAsync(string remediationId)
    {
        if (string.IsNullOrEmpty(remediationId))
        {
            throw new ArgumentException("Remediation ID cannot be null or empty", nameof(remediationId));
        }

        var metrics = new RiskMetrics
        {
            OverallRiskScore = 0.6, // Default value
            ConfidenceScore = 0.7,
            PotentialImpactScore = 0.5,
            ComplexityScore = 0.5,
            TimeSensitivityScore = 0.5,
            StrategyId = remediationId,
            Timestamp = DateTime.UtcNow
        };

        return metrics;
    }

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
        var impactAsSeverity = (SeverityLevel)(int)action.Impact;
        var remediationRiskLevel = RiskAssessmentHelper.CalculateRiskLevel(impactAsSeverity, action.ImpactScope);

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
        var riskLevel = RiskLevel.Medium;
        if (action.RiskLevel != null)
        {
            riskLevel = action.RiskLevel;
        }
        
        var remediationRiskLevel = riskLevel.ToRemediationRiskLevel();
        switch (remediationRiskLevel)
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
        var riskLevel = RiskLevel.Medium;
        if (action.RiskLevel != null)
        {
            riskLevel = action.RiskLevel;
        }
        
        var remediationRiskLevel = riskLevel.ToRemediationRiskLevel();
        switch (remediationRiskLevel)
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

        // Add error context-specific steps if available
        if (action.ErrorContext != null)
        {
            // Access properties of ErrorContext directly
            steps.Add($"Validate error context ID: {action.ErrorContext.ErrorId}");
            
            if (!string.IsNullOrEmpty(action.ErrorContext.ErrorType))
            {
                steps.Add($"Validate error type: {action.ErrorContext.ErrorType}");
            }
            
            if (action.ErrorContext.Metadata?.Count > 0)
            {
                steps.Add("Validate error context metadata");
            }
        }

        // Convert RiskLevel to RemediationRiskLevel using the extension method
        if ((int)remediationRiskLevel > 2)
        {
            // High-risk action requires additional considerations
            steps.Add("Prepare system backup before execution");
        }

        return steps;
    }

    private double CalculateOverallRiskScore(RemediationStrategyModel strategy)
    {
        double score = 0.0;

        // Calculate based on complexity
        score += (strategy.Complexity ?? 0.5) * 0.3;

        // Calculate based on system impact
        score += (strategy.SystemImpact ?? 0.5) * 0.4;

        // Calculate based on rollback capability
        score += (strategy.IsRollbackable ? 0.3 : 0.8) * 0.3;

        return Math.Min(Math.Max(score, 0.0), 1.0); // Clamp between 0 and 1
    }
} 
