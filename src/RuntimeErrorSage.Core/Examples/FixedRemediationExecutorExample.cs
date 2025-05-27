using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Extensions;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Execution;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Examples
{
    /// <summary>
    /// Examples showing the fixed patterns for common errors in RemediationExecutor
    /// </summary>
    public class FixedRemediationExecutorExample
    {
        private readonly ILogger<FixedRemediationExecutorExample> _logger;
        private readonly IRemediationStrategy _strategy;
        private readonly IRemediationValidator _validator;
        private readonly IRemediationMetricsCollector _metricsCollector;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FixedRemediationExecutorExample"/> class.
        /// </summary>
        public FixedRemediationExecutorExample(
            ILogger<FixedRemediationExecutorExample> logger,
            IRemediationStrategy strategy,
            IRemediationValidator validator,
            IRemediationMetricsCollector metricsCollector)
        {
            _logger = logger;
            _strategy = strategy;
            _validator = validator;
            _metricsCollector = metricsCollector;
        }
        
        /// <summary>
        /// Example showing fixed RemediationResult Success property usage
        /// </summary>
        public async Task<RemediationResult> FixedSuccessPropertyExample(RemediationPlan plan)
        {
            var result = new RemediationResult
            {
                PlanId = plan.PlanId,
                StartTime = DateTime.UtcNow,
                // INCORRECT: Don't use Success in initializer
                // Success = true
                
                // CORRECT: Use IsSuccessful in initializer
                IsSuccessful = true
            };
            
            foreach (var action in plan.Actions)
            {
                var actionResult = await ExecuteActionAsync(action, plan.Context);
                result.Actions.Add(actionResult);
                
                // INCORRECT: Don't use Success property
                // if (!actionResult.Success)
                // {
                //     result.Success = false;
                //     result.ErrorMessage = actionResult.ErrorMessage;
                //     break;
                // }
                
                // CORRECT: Use IsSuccessful property
                if (!actionResult.IsSuccessful)
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = actionResult.ErrorMessage;
                    break;
                }
            }
            
            result.EndTime = DateTime.UtcNow;
            
            // INCORRECT: TimeSpan? doesn't have TotalMilliseconds directly
            // result.Duration = (result.EndTime - result.StartTime).TotalMilliseconds;
            
            // CORRECT: Use nullable conditional operator
            result.Duration = result.EndTime.HasValue 
                ? (result.EndTime.Value - result.StartTime)
                : TimeSpan.Zero;
            
            await _metricsCollector.RecordExecutionAsync(result.PlanId, result);
            return result;
        }
        
        /// <summary>
        /// Example showing fixed KeyValuePair access patterns
        /// </summary>
        public void FixedKeyValuePairExample(DependencyGraph graph)
        {
            // INCORRECT: Trying to access node properties directly from KeyValuePair
            // foreach (var node in graph.Nodes)
            // {
            //     Console.WriteLine($"Node ID: {node.Id}");
            // }
            
            // CORRECT: Access through Value property
            foreach (var node in graph.Nodes)
            {
                Console.WriteLine($"Node ID: {node.Value.Id}");
                Console.WriteLine($"Node Type: {node.Value.Type}");
                Console.WriteLine($"Error Probability: {node.Value.ErrorProbability}");
            }
            
            // INCORRECT: Trying to access edge properties directly from KeyValuePair
            // foreach (var edge in graph.Edges)
            // {
            //     Console.WriteLine($"Edge from {edge.SourceId} to {edge.TargetId}");
            // }
            
            // CORRECT: Access through Value property
            foreach (var edge in graph.Edges)
            {
                Console.WriteLine($"Edge from {edge.Value.SourceId} to {edge.Value.TargetId}");
            }
        }
        
        /// <summary>
        /// Example showing fixed TimeSpan nullable handling
        /// </summary>
        public void FixedTimeSpanNullableExample(List<RemediationExecution> executions)
        {
            // INCORRECT: Using nullable TimeSpan's TotalMilliseconds directly
            // var avgDuration = executions.Average(e => e.DurationSeconds);
            
            // CORRECT: Filter out nulls and then use Value
            var avgDuration1 = executions
                .Where(e => e.DurationSeconds.HasValue)
                .Average(e => e.DurationSeconds.Value);
            
            // CORRECT: Use extension method
            var avgDuration2 = executions
                .Select(e => {
                    TimeSpan? duration = e.EndTime.HasValue ? e.EndTime.Value - e.StartTime : null;
                    return duration.GetTotalSeconds();
                })
                .Average();
            
            // INCORRECT: Using nullable TimeSpan in calculations directly
            // TimeSpan? totalDuration = executions.Sum(e => e.EndTime - e.StartTime);
            // var avgMs = totalDuration.TotalMilliseconds / executions.Count;
            
            // CORRECT: Handle each nullable value individually
            double totalMs = 0;
            foreach (var execution in executions)
            {
                if (execution.EndTime.HasValue)
                {
                    totalMs += (execution.EndTime.Value - execution.StartTime).TotalMilliseconds;
                }
            }
            var avgMs = totalMs / executions.Count;
        }
        
        /// <summary>
        /// Example showing fixed ambiguous type references
        /// </summary>
        public async Task<RemediationResult> FixedAmbiguousReferencesExample(ErrorContext context)
        {
            // INCORRECT: Using ambiguous AnalysisStatus
            // var analysisStatus = AnalysisStatus.Completed;
            
            // CORRECT: Using fully qualified name
            var analysisStatus = AnalysisStatus.Completed;
            
            // INCORRECT: Using ambiguous RemediationStatusEnum
            // var remediationStatus = RemediationStatusEnum.Completed;
            
            // CORRECT: Using fully qualified name
            var remediationStatus = RuntimeErrorSage.Core.Models.Remediation.RemediationStatusEnum.Completed;
            
            // INCORRECT: Using ambiguous ValidationResult
            // var validationResult = new ValidationResult();
            
            // CORRECT: Using fully qualified name
            var validationResult = new RuntimeErrorSage.Core.Models.Validation.ValidationResult();
            
            // Create a strategy with explicit namespace to avoid ambiguity
            RuntimeErrorSage.Core.Interfaces.IRemediationStrategy strategy = _strategy;
            
            // Execute the strategy with explicit namespace references
            return await strategy.ExecuteAsync(context);
        }
        
        // Helper methods to simulate real functionality
        private async Task<RemediationActionResult> ExecuteActionAsync(RemediationAction action, ErrorContext context)
        {
            await Task.Delay(100); // Simulate work
            
            // CORRECT: Use Success property setter after initialization
            var result = new RemediationActionResult
            {
                ActionId = action.ActionId,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMilliseconds(100),
                IsSuccessful = true // Use IsSuccessful directly in initializer
            };
            
            return result;
        }
    }
} 