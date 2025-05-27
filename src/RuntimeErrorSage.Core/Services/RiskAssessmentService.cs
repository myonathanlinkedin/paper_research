using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Utilities;

namespace RuntimeErrorSage.Core.Services
{
    /// <summary>
    /// Service for assessing risks associated with remediation actions.
    /// </summary>
    public class RiskAssessmentService : IRiskAssessmentService
    {
        /// <summary>
        /// Assesses the risk of a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to assess.</param>
        /// <returns>A risk assessment for the action.</returns>
        public async Task<RiskAssessment> AssessRiskAsync(RemediationAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // Use the constructor to ensure proper initialization
            var assessment = new RiskAssessment
            {
                CorrelationId = action.ActionId,
                Status = AnalysisStatus.InProgress,
                StartTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                // Calculate risk level
                assessment.RiskLevel = CalculateRiskLevel(action);
                
                // Generate potential issues
                assessment.PotentialIssues = GeneratePotentialIssues(action);
                
                // Generate mitigation steps
                assessment.MitigationSteps = GenerateMitigationSteps(action);
                
                // Set impact scope
                assessment.ImpactScope = action.ImpactScope;
                
                // Set confidence based on available data
                assessment.Confidence = CalculateConfidence(action);
                
                // Set estimated duration
                assessment.EstimatedDuration = TimeSpan.FromSeconds(action.TimeoutSeconds);
                
                // Set description
                assessment.Description = $"Risk assessment for action: {action.Name}";
                
                // Set affected components based on impact scope and context
                assessment.AffectedComponents = DetermineAffectedComponents(action);
                
                // Set risk factors
                assessment.RiskFactors = DetermineRiskFactors(action);
                
                // Set notes
                assessment.Notes = GenerateAssessmentNotes(action);
                
                // Initialize metadata if null
                assessment.Metadata ??= new Dictionary<string, object>();
                
                // Set metadata
                assessment.Metadata["ActionType"] = action.Type;
                assessment.Metadata["StrategyName"] = action.StrategyName;
                assessment.Metadata["RequiresManualApproval"] = action.RequiresManualApproval;
                assessment.Metadata["CanRollback"] = action.CanRollback;
                assessment.Metadata["MaxRetries"] = action.MaxRetries;
                assessment.Metadata["RetryDelaySeconds"] = action.RetryDelaySeconds;

                // Add context-specific metadata if available
                if (action.Context != null)
                {
                    assessment.Metadata["ServiceName"] = action.Context.ServiceName;
                    assessment.Metadata["ErrorType"] = action.Context.ErrorType;
                    assessment.Metadata["ErrorSource"] = action.Context.ErrorSource;
                    assessment.Metadata["OperationName"] = action.Context.OperationName;
                    assessment.Metadata["OperationType"] = action.Context.OperationType;
                    assessment.Metadata["ContextId"] = action.Context.ContextId;
                    assessment.Metadata["ContextName"] = action.Context.Name;
                    assessment.Metadata["ContextType"] = action.Context.Type;
                    assessment.Metadata["ContextVersion"] = action.Context.Version;
                    assessment.Metadata["ContextSource"] = action.Context.Source;
                    assessment.Metadata["ContextPriority"] = action.Context.Priority;
                    assessment.Metadata["ContextIsActive"] = action.Context.IsActive;
                    assessment.Metadata["ContextIsRequired"] = action.Context.IsRequired;
                    assessment.Metadata["ContextIsOptional"] = action.Context.IsOptional;
                    assessment.Metadata["ContextIsConditional"] = action.Context.IsConditional;
                    assessment.Metadata["ContextCondition"] = action.Context.Condition;
                    assessment.Metadata["ContextSeverity"] = action.Context.Severity;
                    assessment.Metadata["ContextRiskLevel"] = action.Context.RiskLevel;
                    assessment.Metadata["ContextImpactScope"] = action.Context.ImpactScope;
                    assessment.Metadata["ContextStrategyName"] = action.Context.StrategyName;
                    assessment.Metadata["ContextOrder"] = action.Context.Order;
                    assessment.Metadata["ContextIsEnabled"] = action.Context.IsEnabled;
                    assessment.Metadata["ContextIsParallel"] = action.Context.IsParallel;
                    assessment.Metadata["ContextIsSequential"] = action.Context.IsSequential;
                    assessment.Metadata["ContextIsAsynchronous"] = action.Context.IsAsynchronous;
                    assessment.Metadata["ContextIsSynchronous"] = action.Context.IsSynchronous;
                    assessment.Metadata["ContextIsIdempotent"] = action.Context.IsIdempotent;
                    assessment.Metadata["ContextIsReversible"] = action.Context.IsReversible;
                    assessment.Metadata["ContextIsAtomic"] = action.Context.IsAtomic;
                    assessment.Metadata["ContextIsTransactional"] = action.Context.IsTransactional;
                    
                    // Add context data
                    if (action.Context.Data != null)
                    {
                        foreach (var kvp in action.Context.Data)
                        {
                            assessment.Metadata[$"ContextData_{kvp.Key}"] = kvp.Value;
                        }
                    }
                    
                    // Add context tags
                    if (action.Context.Tags?.Count > 0)
                    {
                        assessment.Metadata["ContextTags"] = string.Join(",", action.Context.Tags);
                    }
                    
                    // Add context dependencies
                    if (action.Context.Dependencies?.Count > 0)
                    {
                        assessment.Metadata["ContextDependencies"] = string.Join(",", action.Context.Dependencies);
                    }
                    
                    // Add context prerequisites
                    if (action.Context.Prerequisites?.Count > 0)
                    {
                        assessment.Metadata["ContextPrerequisites"] = string.Join(",", action.Context.Prerequisites);
                    }
                    
                    // Add context warnings
                    if (action.Context.Warnings?.Count > 0)
                    {
                        assessment.Metadata["ContextWarnings"] = string.Join(",", action.Context.Warnings);
                    }
                    
                    // Add any additional context metadata
                    if (action.Context.AdditionalContext != null)
                    {
                        foreach (var kvp in action.Context.AdditionalContext)
                        {
                            assessment.Metadata[$"Context_{kvp.Key}"] = kvp.Value;
                        }
                    }
                }
                
                // Set status to completed
                assessment.Status = AnalysisStatus.Completed;
                assessment.EndTime = DateTime.UtcNow;
                
                return assessment;
            }
            catch (Exception ex)
            {
                assessment.Status = AnalysisStatus.Failed;
                assessment.EndTime = DateTime.UtcNow;
                assessment.Warnings ??= new List<string>();
                assessment.Warnings.Add(ex.Message);
                return assessment;
            }
        }

        /// <summary>
        /// Calculates the risk level for a remediation action.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <returns>The calculated risk level.</returns>
        private RemediationRiskLevel CalculateRiskLevel(RemediationAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // Extract severity and impact scope from the action
            var severity = DetermineSeverity(action);
            var impactScope = action.ImpactScope;
            
            // Calculate base risk level from severity
            var baseRiskLevel = severity switch
            {
                RemediationSeverity.Critical => RemediationRiskLevel.Critical,
                RemediationSeverity.High => RemediationRiskLevel.High,
                RemediationSeverity.Medium => RemediationRiskLevel.Medium,
                RemediationSeverity.Low => RemediationRiskLevel.Low,
                _ => RemediationRiskLevel.None
            };

            // Adjust risk level based on impact scope
            baseRiskLevel = impactScope switch
            {
                RemediationActionImpactScope.System => IncreaseRiskLevel(baseRiskLevel, 2),
                RemediationActionImpactScope.Service => IncreaseRiskLevel(baseRiskLevel, 1),
                _ => baseRiskLevel
            };

            // Adjust risk level based on action properties
            if (action.RequiresManualApproval)
            {
                baseRiskLevel = IncreaseRiskLevel(baseRiskLevel, 1);
            }

            if (!action.CanRollback)
            {
                baseRiskLevel = IncreaseRiskLevel(baseRiskLevel, 1);
            }

            if (action.MaxRetries == 0)
            {
                baseRiskLevel = IncreaseRiskLevel(baseRiskLevel, 1);
            }

            // Adjust based on context if available
            if (action.Context != null)
            {
                if (action.Context.PreviousErrors?.Count > 0)
                {
                    baseRiskLevel = IncreaseRiskLevel(baseRiskLevel, 1);
                }

                if (!action.Context.IsIdempotent)
                {
                    baseRiskLevel = IncreaseRiskLevel(baseRiskLevel, 1);
                }

                if (!action.Context.IsReversible)
                {
                    baseRiskLevel = IncreaseRiskLevel(baseRiskLevel, 1);
                }
            }

            return baseRiskLevel;
        }

        /// <summary>
        /// Increases the risk level by the specified number of levels.
        /// </summary>
        private RemediationRiskLevel IncreaseRiskLevel(RemediationRiskLevel currentLevel, int levels)
        {
            var newValue = (int)currentLevel + levels;
            return (RemediationRiskLevel)Math.Min(newValue, (int)RemediationRiskLevel.Critical);
        }

        /// <summary>
        /// Determines the severity of a remediation action.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <returns>The determined severity level.</returns>
        private RemediationSeverity DetermineSeverity(RemediationAction action)
        {
            // Map the action's impact to a severity level
            return action.Impact switch
            {
                RemediationActionSeverity.Critical => RemediationSeverity.Critical,
                RemediationActionSeverity.High => RemediationSeverity.High,
                RemediationActionSeverity.Medium => RemediationSeverity.Medium,
                RemediationActionSeverity.Low => RemediationSeverity.Low,
                _ => RemediationSeverity.None
            };
        }

        /// <summary>
        /// Generates potential issues for a remediation action.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <returns>A list of potential issues.</returns>
        private List<string> GeneratePotentialIssues(RemediationAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var issues = new List<string>();

            // Add severity-based issues
            switch (action.Impact)
            {
                case RemediationActionSeverity.Critical:
                    issues.Add("Critical severity action - high risk of system impact");
                    issues.Add("May require immediate rollback if issues occur");
                    break;
                case RemediationActionSeverity.High:
                    issues.Add("High severity action - significant system impact possible");
                    issues.Add("Careful monitoring required during execution");
                    break;
                case RemediationActionSeverity.Medium:
                    issues.Add("Medium severity action - moderate system impact possible");
                    break;
                case RemediationActionSeverity.Low:
                    issues.Add("Low severity action - minimal system impact expected");
                    break;
            }

            // Add scope-based issues
            switch (action.ImpactScope)
            {
                case RemediationActionImpactScope.System:
                    issues.Add("System-wide impact - affects entire application");
                    issues.Add("Requires comprehensive testing before execution");
                    break;
                case RemediationActionImpactScope.Service:
                    issues.Add("Service-level impact - affects multiple components");
                    issues.Add("Service dependencies should be verified");
                    break;
                case RemediationActionImpactScope.Module:
                    issues.Add("Module-level impact - affects specific functionality");
                    break;
                case RemediationActionImpactScope.Component:
                    issues.Add("Component-level impact - localized effect");
                    break;
            }

            // Add action-specific issues
            if (action.RequiresManualApproval)
            {
                issues.Add("Action requires manual approval before execution");
            }

            if (!action.CanRollback)
            {
                issues.Add("No automatic rollback available - manual recovery may be needed");
            }

            if (action.MaxRetries == 0)
            {
                issues.Add("No retry mechanism available - single attempt only");
            }

            // Add context-specific issues
            if (action.Context != null)
            {
                if (!string.IsNullOrEmpty(action.Context.ErrorType))
                {
                    issues.Add($"Addresses {action.Context.ErrorType} error type");
                }

                if (action.Context.PreviousErrors?.Count > 0)
                {
                    issues.Add($"Has {action.Context.PreviousErrors.Count} previous related errors");
                }

                if (!action.Context.IsIdempotent)
                {
                    issues.Add("Action is not idempotent - multiple executions may cause issues");
                }

                if (!action.Context.IsReversible)
                {
                    issues.Add("Action is not reversible - changes cannot be undone");
                }

                if (action.Context.Dependencies?.Count > 0)
                {
                    issues.Add($"Has {action.Context.Dependencies.Count} dependencies that must be satisfied");
                }
            }

            return issues;
        }

        /// <summary>
        /// Generates mitigation steps for a remediation action.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <returns>A list of mitigation steps.</returns>
        private List<string> GenerateMitigationSteps(RemediationAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var steps = new List<string>();

            // Add severity-based mitigation steps
            switch (action.Impact)
            {
                case RemediationActionSeverity.Critical:
                    steps.Add("Prepare immediate rollback plan");
                    steps.Add("Schedule maintenance window if needed");
                    steps.Add("Ensure all stakeholders are notified");
                    break;
                case RemediationActionSeverity.High:
                    steps.Add("Prepare rollback plan");
                    steps.Add("Notify affected teams");
                    steps.Add("Schedule execution during low-traffic period");
                    break;
                case RemediationActionSeverity.Medium:
                    steps.Add("Monitor system metrics during execution");
                    steps.Add("Have rollback plan ready");
                    break;
                case RemediationActionSeverity.Low:
                    steps.Add("Monitor execution progress");
                    break;
            }

            // Add scope-based mitigation steps
            switch (action.ImpactScope)
            {
                case RemediationActionImpactScope.System:
                    steps.Add("Perform full system backup before execution");
                    steps.Add("Verify all system components are healthy");
                    steps.Add("Prepare system-wide rollback procedure");
                    break;
                case RemediationActionImpactScope.Service:
                    steps.Add("Backup service configuration");
                    steps.Add("Verify service dependencies");
                    steps.Add("Prepare service-specific rollback");
                    break;
                case RemediationActionImpactScope.Module:
                    steps.Add("Backup module state");
                    steps.Add("Verify module dependencies");
                    break;
                case RemediationActionImpactScope.Component:
                    steps.Add("Backup component configuration");
                    break;
            }

            // Add action-specific mitigation steps
            if (action.CanRollback)
            {
                steps.Add("Automatic rollback will be attempted if issues occur");
            }
            else
            {
                steps.Add("Prepare manual recovery steps");
            }

            if (action.MaxRetries > 0)
            {
                steps.Add($"System will attempt up to {action.MaxRetries} retries with {action.RetryDelaySeconds}s delay");
            }

            // Add context-specific mitigation steps
            if (action.Context != null)
            {
                if (!string.IsNullOrEmpty(action.Context.OperationName))
                {
                    steps.Add($"Monitor operation: {action.Context.OperationName}");
                }

                if (action.Context.AffectedComponents?.Count > 0)
                {
                    steps.Add($"Monitor {action.Context.AffectedComponents.Count} affected components");
                }

                if (action.Context.Dependencies?.Count > 0)
                {
                    steps.Add("Verify all dependencies are satisfied before execution");
                }

                if (!action.Context.IsIdempotent)
                {
                    steps.Add("Ensure action is not executed multiple times");
                }
            }

            return steps;
        }

        /// <summary>
        /// Calculates the confidence level for a risk assessment.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <returns>A confidence value between 0 and 100.</returns>
        private double CalculateConfidence(RemediationAction action)
        {
            // Simple confidence calculation based on available data
            double confidence = 50.0; // Base confidence
            
            // Adjust based on available data
            if (!string.IsNullOrEmpty(action.Description))
            {
                confidence += 10.0;
            }
            
            if (action.ValidationResults?.Count > 0)
            {
                confidence += 15.0;
            }
            
            if (action.Context != null)
            {
                confidence += 15.0;

                // Additional confidence for context details
                if (!string.IsNullOrEmpty(action.Context.ErrorType))
                {
                    confidence += 5.0;
                }
                
                if (!string.IsNullOrEmpty(action.Context.ErrorSource))
                {
                    confidence += 5.0;
                }
                
                if (action.Context.AffectedComponents?.Count > 0)
                {
                    confidence += 5.0;
                }

                // Additional confidence for context properties
                if (!string.IsNullOrEmpty(action.Context.Name))
                {
                    confidence += 2.0;
                }
                
                if (!string.IsNullOrEmpty(action.Context.Description))
                {
                    confidence += 2.0;
                }
                
                if (!string.IsNullOrEmpty(action.Context.Type))
                {
                    confidence += 2.0;
                }
                
                if (action.Context.Data?.Count > 0)
                {
                    confidence += 3.0;
                }
                
                if (action.Context.Tags?.Count > 0)
                {
                    confidence += 2.0;
                }
                
                if (!string.IsNullOrEmpty(action.Context.Version))
                {
                    confidence += 2.0;
                }
                
                if (!string.IsNullOrEmpty(action.Context.Source))
                {
                    confidence += 2.0;
                }
                
                if (action.Context.Dependencies?.Count > 0)
                {
                    confidence += 2.0;
                }
                
                if (action.Context.Prerequisites?.Count > 0)
                {
                    confidence += 2.0;
                }
                
                if (action.Context.IsRequired)
                {
                    confidence += 3.0;
                }
                
                if (action.Context.IsIdempotent)
                {
                    confidence += 2.0;
                }
                
                if (action.Context.IsReversible)
                {
                    confidence += 2.0;
                }
                
                if (action.Context.IsAtomic)
                {
                    confidence += 2.0;
                }
                
                if (action.Context.IsTransactional)
                {
                    confidence += 2.0;
                }
            }
            
            // Cap at 100
            return Math.Min(confidence, 100.0);
        }

        /// <summary>
        /// Determines the affected components based on the action's impact scope.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <returns>A list of affected components.</returns>
        private List<string> DetermineAffectedComponents(RemediationAction action)
        {
            var components = new List<string>();
            
            // Add components based on impact scope
            switch (action.ImpactScope)
            {
                case RemediationActionImpactScope.System:
                    components.Add("Entire System");
                    break;
                case RemediationActionImpactScope.Service:
                    components.Add("Service Layer");
                    if (action.Context?.ServiceName != null)
                    {
                        components.Add($"Service: {action.Context.ServiceName}");
                    }
                    break;
                case RemediationActionImpactScope.Module:
                    components.Add("Module Layer");
                    if (action.Context?.ModuleName != null)
                    {
                        components.Add($"Module: {action.Context.ModuleName}");
                    }
                    break;
                case RemediationActionImpactScope.Component:
                    if (action.Context?.ComponentName != null)
                    {
                        components.Add($"Component: {action.Context.ComponentName}");
                    }
                    break;
            }

            // Add context-specific components if available
            if (action.Context?.AffectedComponents != null)
            {
                foreach (var component in action.Context.AffectedComponents)
                {
                    if (!string.IsNullOrEmpty(component.Name))
                    {
                        components.Add($"Affected Component: {component.Name}");
                    }
                }
            }
            
            return components;
        }

        /// <summary>
        /// Determines the risk factors for a remediation action.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <returns>A list of risk factors.</returns>
        private List<RiskFactor> DetermineRiskFactors(RemediationAction action)
        {
            var factors = new List<RiskFactor>();
            
            // Add severity-based risk factor
            factors.Add(new RiskFactor
            {
                Name = "Action Severity",
                Description = $"Action has {action.Impact} severity level",
                Impact = (int)action.Impact
            });
            
            // Add scope-based risk factor
            factors.Add(new RiskFactor
            {
                Name = "Impact Scope",
                Description = $"Action affects {action.ImpactScope} scope",
                Impact = (int)action.ImpactScope
            });
            
            // Add manual approval risk factor if required
            if (action.RequiresManualApproval)
            {
                factors.Add(new RiskFactor
                {
                    Name = "Manual Approval Required",
                    Description = "Action requires manual approval before execution",
                    Impact = 2
                });
            }
            
            // Add rollback risk factor
            factors.Add(new RiskFactor
            {
                Name = "Rollback Capability",
                Description = action.CanRollback ? "Action has rollback capability" : "Action has no rollback capability",
                Impact = action.CanRollback ? 1 : 3
            });

            // Add context-specific risk factors if available
            if (action.Context != null)
            {
                if (!string.IsNullOrEmpty(action.Context.ErrorType))
                {
                    factors.Add(new RiskFactor
                    {
                        Name = "Error Type",
                        Description = $"Action addresses {action.Context.ErrorType} error",
                        Impact = 2
                    });
                }

                if (action.Context.PreviousErrors?.Count > 0)
                {
                    factors.Add(new RiskFactor
                    {
                        Name = "Previous Errors",
                        Description = $"Action has {action.Context.PreviousErrors.Count} previous related errors",
                        Impact = action.Context.PreviousErrors.Count
                    });
                }

                // Add context-specific risk factors
                if (action.Context.IsRequired)
                {
                    factors.Add(new RiskFactor
                    {
                        Name = "Required Context",
                        Description = "Action requires specific context to be present",
                        Impact = 2
                    });
                }

                if (action.Context.IsConditional)
                {
                    factors.Add(new RiskFactor
                    {
                        Name = "Conditional Context",
                        Description = $"Action has conditional context: {action.Context.Condition}",
                        Impact = 2
                    });
                }

                if (action.Context.Dependencies?.Count > 0)
                {
                    factors.Add(new RiskFactor
                    {
                        Name = "Context Dependencies",
                        Description = $"Action has {action.Context.Dependencies.Count} context dependencies",
                        Impact = action.Context.Dependencies.Count
                    });
                }

                if (action.Context.Prerequisites?.Count > 0)
                {
                    factors.Add(new RiskFactor
                    {
                        Name = "Context Prerequisites",
                        Description = $"Action has {action.Context.Prerequisites.Count} context prerequisites",
                        Impact = action.Context.Prerequisites.Count
                    });
                }

                if (!action.Context.IsIdempotent)
                {
                    factors.Add(new RiskFactor
                    {
                        Name = "Non-Idempotent Context",
                        Description = "Action context is not idempotent",
                        Impact = 2
                    });
                }

                if (!action.Context.IsReversible)
                {
                    factors.Add(new RiskFactor
                    {
                        Name = "Non-Reversible Context",
                        Description = "Action context is not reversible",
                        Impact = 2
                    });
                }

                if (!action.Context.IsAtomic)
                {
                    factors.Add(new RiskFactor
                    {
                        Name = "Non-Atomic Context",
                        Description = "Action context is not atomic",
                        Impact = 2
                    });
                }

                if (!action.Context.IsTransactional)
                {
                    factors.Add(new RiskFactor
                    {
                        Name = "Non-Transactional Context",
                        Description = "Action context is not transactional",
                        Impact = 2
                    });
                }
            }
            
            return factors;
        }

        /// <summary>
        /// Generates assessment notes for a remediation action.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <returns>Assessment notes.</returns>
        private string GenerateAssessmentNotes(RemediationAction action)
        {
            var notes = new List<string>
            {
                $"Risk assessment performed for action: {action.Name}",
                $"Action type: {action.Type}",
                $"Impact level: {action.Impact}",
                $"Impact scope: {action.ImpactScope}",
                $"Manual approval required: {action.RequiresManualApproval}",
                $"Rollback capability: {action.CanRollback}",
                $"Timeout: {action.TimeoutSeconds} seconds",
                $"Max retries: {action.MaxRetries}"
            };
            
            if (!string.IsNullOrEmpty(action.StrategyName))
            {
                notes.Add($"Strategy: {action.StrategyName}");
            }
            
            if (action.ValidationResults?.Count > 0)
            {
                notes.Add($"Validation results: {action.ValidationResults.Count} items");
            }

            // Add context-specific notes if available
            if (action.Context != null)
            {
                if (!string.IsNullOrEmpty(action.Context.ServiceName))
                {
                    notes.Add($"Service: {action.Context.ServiceName}");
                }

                if (!string.IsNullOrEmpty(action.Context.ErrorType))
                {
                    notes.Add($"Error type: {action.Context.ErrorType}");
                }

                if (!string.IsNullOrEmpty(action.Context.ErrorSource))
                {
                    notes.Add($"Error source: {action.Context.ErrorSource}");
                }

                if (action.Context.AffectedComponents?.Count > 0)
                {
                    notes.Add($"Affected components: {action.Context.AffectedComponents.Count}");
                }

                if (action.Context.PreviousErrors?.Count > 0)
                {
                    notes.Add($"Previous errors: {action.Context.PreviousErrors.Count}");
                }
            }
            
            return string.Join(Environment.NewLine, notes);
        }
    }
} 