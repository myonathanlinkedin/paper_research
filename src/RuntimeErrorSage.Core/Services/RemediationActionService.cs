using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Services
{
    public class RemediationActionService
    {
        private RemediationRiskLevel CalculateRiskLevel(RemediationAction action)
        {
            var riskFactors = new List<int>();

            // Analyze error type severity
            if (action.ErrorType?.Contains("Critical", StringComparison.OrdinalIgnoreCase) == true)
                riskFactors.Add(3);
            else if (action.ErrorType?.Contains("Error", StringComparison.OrdinalIgnoreCase) == true)
                riskFactors.Add(2);
            else if (action.ErrorType?.Contains("Warning", StringComparison.OrdinalIgnoreCase) == true)
                riskFactors.Add(1);

            // Analyze stack trace depth
            var stackTraceDepth = action.StackTrace?.Split('\n').Length ?? 0;
            if (stackTraceDepth > 10) riskFactors.Add(2);
            else if (stackTraceDepth > 5) riskFactors.Add(1);

            // Analyze context complexity
            var contextComplexity = action.Context?.Count ?? 0;
            if (contextComplexity > 10) riskFactors.Add(2);
            else if (contextComplexity > 5) riskFactors.Add(1);

            // Calculate average risk factor
            var averageRisk = riskFactors.Any() ? riskFactors.Average() : 1;

            // Map to RemediationRiskLevel
            return averageRisk switch
            {
                var r when r >= 2.5 => RemediationRiskLevel.Critical,
                var r when r >= 1.75 => RemediationRiskLevel.High,
                var r when r >= 1.25 => RemediationRiskLevel.Medium,
                _ => RemediationRiskLevel.Low
            };
        }

        private List<string> GeneratePotentialIssues(RemediationAction action)
        {
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
            if (action.Context?.Any() == true)
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

        private List<string> GenerateMitigationSteps(RemediationAction action)
        {
            var steps = new List<string>();

            // Add basic validation steps
            steps.Add("Validate input parameters and context before execution");
            steps.Add("Ensure proper error handling is in place");

            // Add risk-specific mitigation steps
            switch (action.RiskLevel)
            {
                case RemediationRiskLevel.Critical:
                    steps.Add("Implement rollback mechanism");
                    steps.Add("Add comprehensive logging");
                    steps.Add("Schedule immediate monitoring after execution");
                    break;
                case RemediationRiskLevel.High:
                    steps.Add("Add detailed logging");
                    steps.Add("Implement partial rollback if needed");
                    break;
                case RemediationRiskLevel.Medium:
                    steps.Add("Add basic logging");
                    steps.Add("Monitor execution results");
                    break;
                case RemediationRiskLevel.Low:
                    steps.Add("Verify execution results");
                    break;
            }

            // Add context-specific steps
            if (action.Context?.Any() == true)
            {
                steps.Add("Validate all context values before use");
                if (action.Context.Count > 5)
                {
                    steps.Add("Consider simplifying context structure");
                }
            }

            return steps;
        }
    }
} 