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
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Examples
{
    /// <summary>
    /// Examples showing the fixed patterns for common errors in RemediationExecutor
    /// </summary>
    public class FixedRemediationExecutorExample : IRemediationExecutor
    {
        private readonly ILogger<FixedRemediationExecutorExample> _logger;
        private readonly Models.Remediation.Interfaces.IRemediationStrategy _strategy;
        private readonly IRemediationValidator _validator;
        private readonly IRemediationMetricsCollector _metricsCollector;
        private readonly Dictionary<string, RemediationExecution> _executionHistory;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FixedRemediationExecutorExample"/> class.
        /// </summary>
        public FixedRemediationExecutorExample(
            ILogger<FixedRemediationExecutorExample> logger,
            Models.Remediation.Interfaces.IRemediationStrategy strategy,
            IRemediationValidator validator,
            IRemediationMetricsCollector metricsCollector)
        {
            _logger = logger;
            _strategy = strategy;
            _validator = validator;
            _metricsCollector = metricsCollector;
            _executionHistory = new Dictionary<string, RemediationExecution>();
        }

        /// <inheritdoc />
        public bool IsEnabled => true;

        /// <inheritdoc />
        public string Name => "Fixed Remediation Executor Example";

        /// <inheritdoc />
        public string Version => "1.0.0";

        /// <inheritdoc />
        public async Task<RemediationResult> ExecuteStrategyAsync(Models.Remediation.Interfaces.IRemediationStrategy strategy, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(strategy);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                _logger.LogInformation("Executing strategy {StrategyName} for error context {ErrorId}", 
                    strategy.Name, errorContext.Id);

                // Validate strategy against the error context
                var validationResult = await _validator.ValidateStrategyAsync(strategy, errorContext);
                if (!validationResult.IsValid)
                {
                    return new RemediationResult
                    {
                        Context = errorContext,
                        Status = RemediationStatusEnum.Failed,
                        Message = "Strategy validation failed: " + string.Join(", ", validationResult.Messages),
                        Validation = validationResult
                    };
                }

                // Execute the strategy
                var result = await strategy.ExecuteAsync(errorContext);

                // Track execution
                await _metricsCollector.TrackExecutionAsync(new RemediationExecution
                {
                    Strategy = strategy.Name,
                    ErrorContext = errorContext,
                    Result = result,
                    ExecutionTime = DateTime.UtcNow
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing strategy {StrategyName} for context {ErrorId}", 
                    strategy.Name, errorContext.Id);
                
                return new RemediationResult
                {
                    Context = errorContext,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Execution error: {ex.Message}",
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ExecutePlanAsync(RemediationPlan plan)
        {
            ArgumentNullException.ThrowIfNull(plan);

            try
            {
                _logger.LogInformation("Executing remediation plan {PlanId}", plan.PlanId);

                var result = new RemediationResult
                {
                    Context = plan.Context,
                    Status = RemediationStatusEnum.InProgress,
                    Message = "Plan execution started",
                    StartTime = DateTime.UtcNow
                };

                // Execute each strategy in the plan
                foreach (var strategy in plan.Strategies)
                {
                    var strategyResult = await ExecuteStrategyAsync(strategy, plan.Context);
                    
                    if (strategyResult.Status != RemediationStatusEnum.Success)
                    {
                        result.Status = RemediationStatusEnum.Failed;
                        result.Message = $"Strategy {strategy.Name} failed: {strategyResult.Message}";
                        break;
                    }

                    result.CompletedSteps.Add(strategy.Name);
                }

                if (result.Status == RemediationStatusEnum.InProgress)
                {
                    result.Status = RemediationStatusEnum.Success;
                    result.Message = "Plan executed successfully";
                }

                result.EndTime = DateTime.UtcNow;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing plan {PlanId}", plan.PlanId);
                return new RemediationResult
                {
                    Context = plan.Context,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Plan execution failed: {ex.Message}",
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> RollbackAsync(RemediationResult result)
        {
            ArgumentNullException.ThrowIfNull(result);

            try
            {
                _logger.LogInformation("Rolling back remediation {RemediationId}", result.RemediationId);
                
                // Perform rollback logic
                await Task.Delay(100); // Simulate work
                
                return new RemediationResult
                {
                    Context = result.Context,
                    Status = RemediationStatusEnum.Success,
                    Message = "Rollback completed successfully",
                    IsRollback = true,
                    OriginalRemediationId = result.RemediationId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back remediation {RemediationId}", result.RemediationId);
                
                return new RemediationResult
                {
                    Context = result.Context,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Rollback failed: {ex.Message}",
                    IsRollback = true
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationStatusEnum> GetRemediationStatusAsync(string remediationId)
        {
            ArgumentNullException.ThrowIfNull(remediationId);

            try
            {
                _logger.LogInformation("Getting status for remediation {RemediationId}", remediationId);
                
                // Check execution history
                if (_executionHistory.TryGetValue(remediationId, out var execution))
                {
                    return execution.Status;
                }
                
                return RemediationStatusEnum.Unknown;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting status for remediation {RemediationId}", remediationId);
                return RemediationStatusEnum.Unknown;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationExecution> GetExecutionHistoryAsync(string remediationId)
        {
            ArgumentNullException.ThrowIfNull(remediationId);

            try
            {
                _logger.LogInformation("Getting execution history for remediation {RemediationId}", remediationId);
                
                if (_executionHistory.TryGetValue(remediationId, out var execution))
                {
                    return execution;
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting execution history for remediation {RemediationId}", remediationId);
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<bool> CancelRemediationAsync(string remediationId)
        {
            ArgumentNullException.ThrowIfNull(remediationId);

            try
            {
                _logger.LogInformation("Cancelling remediation {RemediationId}", remediationId);
                
                // Cancel logic would go here
                await Task.Delay(100); // Simulate work
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling remediation {RemediationId}", remediationId);
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationExecution> ExecuteRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(analysis);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Executing remediation for analysis {AnalysisId}", analysis.ErrorId);
                
                // Create execution record
                var execution = new RemediationExecution
                {
                    RemediationId = Guid.NewGuid().ToString(),
                    ErrorContext = context,
                    StartTime = DateTime.UtcNow,
                    Status = RemediationStatusEnum.InProgress
                };
                
                // Store in history
                _executionHistory[execution.RemediationId] = execution;
                
                // Check if the strategy can handle this error context
                var canHandle = await _strategy.CanHandleErrorAsync(context);
                if (!canHandle)
                {
                    execution.Status = RemediationStatusEnum.Failed;
                    execution.ErrorMessage = $"Strategy {_strategy.Name} cannot handle error type {context.ErrorType}";
                    execution.EndTime = DateTime.UtcNow;
                    return execution;
                }
                
                // Execute the strategy
                var result = await _strategy.ExecuteAsync(context);
                execution.RemediationPlanId = result.PlanId;
                execution.CompletedActions = result.Actions.Count;
                execution.TotalActions = result.Actions.Count;
                execution.EndTime = DateTime.UtcNow;
                execution.Status = result.Status;
                execution.ErrorMessage = result.ErrorMessage;
                
                // Store in history
                _executionHistory[execution.RemediationId] = execution;
                
                return execution;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing remediation for analysis {AnalysisId}", analysis.ErrorId);
                
                var execution = new RemediationExecution
                {
                    RemediationId = Guid.NewGuid().ToString(),
                    ErrorContext = context,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Status = RemediationStatusEnum.Failed,
                    ErrorMessage = ex.Message
                };
                
                // Store in history
                _executionHistory[execution.RemediationId] = execution;
                
                return execution;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationValidationResult> ValidateRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(analysis);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Validating remediation for analysis {AnalysisId}", analysis.ErrorId);
                
                // Check if the strategy can handle this error context
                var validationResult = await _validator.ValidateStrategyAsync(_strategy, context);
                
                return new RemediationValidationResult
                {
                    IsValid = validationResult.IsValid,
                    ErrorContext = context,
                    Messages = validationResult.Messages,
                    Timestamp = DateTime.UtcNow,
                    ValidationResults = new List<ValidationResult> { validationResult }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating remediation for analysis {AnalysisId}", analysis.ErrorId);
                
                return new RemediationValidationResult
                {
                    IsValid = false,
                    ErrorContext = context,
                    Messages = new List<string> { $"Validation error: {ex.Message}" },
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationMetrics> GetExecutionMetricsAsync(string remediationId)
        {
            ArgumentNullException.ThrowIfNull(remediationId);

            try
            {
                _logger.LogInformation("Getting metrics for remediation {RemediationId}", remediationId);
                
                var metrics = await _metricsCollector.GetMetricsAsync(remediationId);
                return metrics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting metrics for remediation {RemediationId}", remediationId);
                return new RemediationMetrics
                {
                    ExecutionId = remediationId,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationAction> ExecuteActionAsync(RemediationAction action, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Executing action {ActionId} for error context {ErrorId}", 
                    action.ActionId, context.Id);
                
                action.StartTime = DateTime.UtcNow;
                
                // Validate
                var validationResult = await ValidateActionAsync(action, context);
                if (!validationResult.IsValid)
                {
                    action.Status = RemediationStatus.Failed;
                    action.ErrorMessage = $"Action validation failed: {string.Join(", ", validationResult.Messages)}";
                    action.EndTime = DateTime.UtcNow;
                    return action;
                }
                
                // Execute
                await Task.Delay(100); // Simulate work
                
                action.Status = RemediationStatus.Success;
                action.EndTime = DateTime.UtcNow;
                
                return action;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing action {ActionId} for context {ErrorId}", 
                    action.ActionId, context.Id);
                
                action.Status = RemediationStatus.Failed;
                action.ErrorMessage = ex.Message;
                action.EndTime = DateTime.UtcNow;
                
                return action;
            }
        }

        /// <inheritdoc />
        public async Task<ValidationResult> ValidateActionAsync(RemediationAction action, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Validating action {ActionId} for error context {ErrorId}", 
                    action.ActionId, context.Id);
                
                // Perform validation logic
                
                return new ValidationResult 
                { 
                    IsValid = true,
                    Messages = new List<string> { "Action validation successful" }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating action {ActionId} for context {ErrorId}", 
                    action.ActionId, context.Id);
                
                return new ValidationResult
                {
                    IsValid = false,
                    Messages = new List<string> { $"Validation error: {ex.Message}" }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationImpact> GetActionImpactAsync(RemediationAction action, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Calculating impact of action {ActionId} for error context {ErrorId}", 
                    action.ActionId, context.Id);
                
                // Calculate impact
                
                return new RemediationImpact
                {
                    Severity = ImpactSeverity.Low,
                    Description = "Minimal system impact expected",
                    AffectedComponents = new List<string> { "Component1", "Component2" }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating impact for action {ActionId} for context {ErrorId}", 
                    action.ActionId, context.Id);
                
                return new RemediationImpact
                {
                    Severity = ImpactSeverity.Unknown,
                    Description = $"Error calculating impact: {ex.Message}"
                };
            }
        }

        /// <inheritdoc />
        public async Task<RiskAssessment> GetActionRiskAsync(RemediationAction action, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Calculating risk of action {ActionId} for error context {ErrorId}", 
                    action.ActionId, context.Id);
                
                // Calculate risk
                
                return new RiskAssessment
                {
                    RiskLevel = RiskLevel.Low,
                    Description = "Low risk remediation action",
                    PotentialIssues = new List<string> { "Temporary performance impact" },
                    MitigationSteps = new List<string> { "Monitor system performance during remediation" }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating risk for action {ActionId} for context {ErrorId}", 
                    action.ActionId, context.Id);
                
                return new RiskAssessment
                {
                    RiskLevel = RiskLevel.Unknown,
                    Description = $"Error calculating risk: {ex.Message}"
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> RollbackRemediationAsync(string remediationId)
        {
            ArgumentNullException.ThrowIfNull(remediationId);

            try
            {
                _logger.LogInformation("Rolling back remediation {RemediationId}", remediationId);
                
                // Get the previous execution
                if (_executionHistory.TryGetValue(remediationId, out var execution))
                {
                    return await RollbackAsync(execution.Result);
                }
                
                return new RemediationResult
                {
                    Status = RemediationStatusEnum.Failed,
                    Message = $"No execution found for remediation ID {remediationId}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back remediation {RemediationId}", remediationId);
                
                return new RemediationResult
                {
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Rollback error: {ex.Message}"
                };
            }
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
                    result.Message = actionResult.ErrorMessage;
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
            // var remediationStatus = RemediationStatusEnum.Success;
            
            // CORRECT: Using fully qualified name
            var remediationStatus = RuntimeErrorSage.Core.Models.Remediation.RemediationStatusEnum.Success;
            
            // INCORRECT: Using ambiguous ValidationResult
            // var validationResult = new ValidationResult();
            
            // CORRECT: Using fully qualified name
            var validationResult = new RuntimeErrorSage.Core.Models.Validation.ValidationResult();
            
            // Create a strategy with explicit namespace to avoid ambiguity
            Models.Remediation.Interfaces.IRemediationStrategy strategy = _strategy;
            
            // Execute the strategy with explicit namespace references
            return await strategy.ExecuteAsync(context);
        }
    }
} 
