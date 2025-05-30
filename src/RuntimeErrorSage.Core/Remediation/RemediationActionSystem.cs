using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Interfaces;

namespace RuntimeErrorSage.Core.Remediation
{
    public class RemediationActionSystem
    {
        private readonly ILogger<RemediationActionSystem> _logger;
        private readonly IRemediationActionTracker _tracker;

        public RemediationActionSystem(
            ILogger<RemediationActionSystem> logger,
            IRemediationActionTracker tracker)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
        }

        private RemediationActionSeverity CalculateImpactSeverity(RemediationSuggestion suggestion)
        {
            // Determine impact severity based on actions
            var severities = suggestion.Actions?.Select(a => a.Impact.ToRemediationActionSeverity()) ?? Enumerable.Empty<RemediationActionSeverity>();
            return severities.Any() ? severities.Max() : RemediationActionSeverity.Medium;
        }

        private List<string> GetAffectedComponents(RemediationSuggestion suggestion, ErrorContext context)
        {
            // Get list of affected components based on actions and context
            var components = new HashSet<string>();
            
            if (suggestion.Actions != null)
            {
                foreach (var action in suggestion.Actions)
                {
                    if (action.Parameters?.TryGetValue("component", out var component) == true)
                    {
                        components.Add(component.ToString());
                    }
                }
            }

            if (context.ComponentId != null)
            {
                components.Add(context.ComponentId);
            }

            return components.ToList();
        }

        private RemediationRiskLevel CalculateRiskLevel(RemediationSuggestion suggestion)
        {
            // Calculate risk level based on complexity and impact
            var complexityScore = CalculateComplexityScore(suggestion);
            var impactScore = (double)CalculateImpactSeverity(suggestion) / (double)RemediationActionSeverity.Critical;

            var riskScore = (complexityScore + impactScore) / 2.0;
            return riskScore switch
            {
                < 0.3 => RemediationRiskLevel.Low,
                < 0.6 => RemediationRiskLevel.Medium,
                < 0.8 => RemediationRiskLevel.High,
                _ => RemediationRiskLevel.Critical
            };
        }

        private double CalculateComplexityScore(RemediationSuggestion suggestion)
        {
            var factors = new List<double>();

            // Factor 1: Number of actions
            if (suggestion.Actions?.Count > 0)
            {
                factors.Add(Math.Min(suggestion.Actions.Count / 5.0, 1.0));
            }

            // Factor 2: Action complexity
            if (suggestion.Actions != null)
            {
                var actionComplexity = suggestion.Actions.Average(a =>
                {
                    var complexity = 0.0;
                    if (a.Parameters?.Count > 0) complexity += 0.3;
                    if (a.RequiresManualApproval) complexity += 0.2;
                    if (a.CanRollback) complexity += 0.1;
                    return complexity;
                });
                factors.Add(actionComplexity);
            }

            // Factor 3: Context complexity
            if (suggestion.Parameters?.Count > 0)
            {
                factors.Add(Math.Min(suggestion.Parameters.Count / 10.0, 1.0));
            }

            return factors.Any() ? factors.Average() : 0.5;
        }

        public async Task<RemediationResult> ExecuteActionAsync(RemediationAction action, ErrorContext context)
        {
            try
            {
                _logger.LogInformation("Executing remediation action {ActionId} for context {ContextId}", 
                    action.Id, context.Id);
                    
                var result = await action.ExecuteAsync();
                if (result.Status == RemediationStatusEnum.Success)
                {
                    await _tracker.TrackActionCompletionAsync(action.Id, true);
                }
                else
                {
                    await _tracker.TrackActionCompletionAsync(action.Id, false, result.Message);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing action {ActionId} for context {ContextId}", 
                    action.Id, context.Id);
                    
                await _tracker.TrackActionCompletionAsync(action.Id, false, ex.Message);
                
                return new RemediationResult 
                { 
                    Context = context,
                    Status = RemediationStatusEnum.Failed,
                    Message = ex.Message
                };
            }
        }
    }
} 


