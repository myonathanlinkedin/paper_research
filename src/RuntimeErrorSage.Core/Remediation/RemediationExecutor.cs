using System.Collections.ObjectModel;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    public class RemediationExecutor
    {
        private readonly IRemediationRiskAssessment _riskAssessment;

        public RemediationExecutor(IRemediationRiskAssessment riskAssessment)
        {
            _riskAssessment = riskAssessment ?? ArgumentNullException.ThrowIfNull(nameof(riskAssessment));
        }

        public async Task<RiskAssessment> CreateRiskAssessment(RemediationAction action)
        {
            if (action == null)
            {
                ArgumentNullException.ThrowIfNull(nameof(action));
            }

            var assessment = new RiskAssessment
            {
                CorrelationId = action.ActionId,
                StartTime = DateTime.UtcNow
            };

            try
            {
                // Calculate risk level
                var remediationRiskLevel = RiskAssessmentHelper.CalculateRiskLevel(action.Impact, action.ImpactScope);
                assessment.RiskLevel = remediationRiskLevel;

                // Generate potential issues
                assessment.PotentialIssues = RiskAssessmentHelper.GeneratePotentialIssues(remediationRiskLevel);

                // Generate mitigation steps
                assessment.MitigationSteps = RiskAssessmentHelper.GenerateMitigationSteps(remediationRiskLevel);

                // Set confidence based on available information
                assessment.Confidence = CalculateConfidence(action);

                // Add metadata
                assessment.Metadata = new Dictionary<string, object>
                {
                    ["ErrorType"] = action.ErrorType,
                    ["StackTrace"] = action.StackTrace,
                    ["ContextRiskLevel"] = action.Context?.RiskLevel ?? RemediationRiskLevel.Medium
                };

                // Set affected components
                assessment.AffectedComponents = GetAffectedComponents(action);

                // Set estimated duration
                assessment.EstimatedDuration = EstimateDuration(action);

                // Set status
                assessment.Status = AnalysisStatus.Completed;
            }
            catch (Exception ex)
            {
                assessment.Status = AnalysisStatus.Failed;
                assessment.Notes = $"Risk assessment failed: {ex.Message}";
                assessment.Warnings.Add($"Error during assessment: {ex.Message}");
            }
            finally
            {
                assessment.EndTime = DateTime.UtcNow;
            }

            return assessment;
        }

        private double CalculateConfidence(RemediationAction action)
        {
            var confidenceFactors = new Collection<double>();

            // Factor 1: Error type clarity
            if (!string.IsNullOrEmpty(action.ErrorType))
            {
                confidenceFactors.Add(0.8);
            }

            // Factor 2: Stack trace availability
            if (!string.IsNullOrEmpty(action.StackTrace))
            {
                confidenceFactors.Add(0.9);
            }

            // Factor 3: Context completeness
            if (action.Context?.Count > 0)
            {
                confidenceFactors.Add(0.7);
            }

            // Factor 4: Impact scope clarity
            if (action.ImpactScope != RemediationActionImpactScope.None)
            {
                confidenceFactors.Add(0.6);
            }

            // Calculate average confidence
            return confidenceFactors.Any() ? confidenceFactors.Average() * 100 : 50.0;
        }

        private Collection<string> GetAffectedComponents(RemediationAction action)
        {
            var components = new HashSet<string>();

            // Add components from context
            if (action.Context?.TryGetValue("component", out var component) == true)
            {
                components.Add(component.ToString());
            }

            // Add components from stack trace
            if (!string.IsNullOrEmpty(action.StackTrace))
            {
                var stackLines = action.StackTrace.Split('\n');
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
            var riskMultiplier = action.RiskLevel switch
            {
                RemediationRiskLevel.Critical => 4.0,
                RemediationRiskLevel.High => 3.0,
                RemediationRiskLevel.Medium => 2.0,
                RemediationRiskLevel.Low => 1.5,
                _ => 1.0
            };

            // Adjust based on context complexity
            var contextMultiplier = action.Context?.Count > 10 ? 2.0 : 1.0;

            return TimeSpan.FromTicks((long)(baseDuration.Ticks * riskMultiplier * contextMultiplier));
        }
    }
} 






