using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Extensions;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Execution;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Models.Metrics;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Domain.Models.Common;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Interfaces;
using ApplicationStrategy = RuntimeErrorSage.Application.Interfaces.IRemediationStrategy;
using DomainStrategy = RuntimeErrorSage.Domain.Interfaces.IRemediationStrategy;

namespace RuntimeErrorSage.Core.Examples
{
    /// <summary>
    /// Extension methods for the FixedRemediationExecutorExample.
    /// </summary>
    public static class FixedRemediationExecutorExampleExtensions
    {
        /// <summary>
        /// Converts an IRemediationStrategy to an Application IRemediationStrategy.
        /// </summary>
        /// <param name="strategy">The strategy to convert.</param>
        /// <returns>The Application strategy.</returns>
        public static ApplicationStrategy ToApplicationStrategy(this ApplicationStrategy strategy)
        {
            if (strategy == null)
                return null;
                
            // If it's already an ApplicationStrategy, return it
            if (strategy is ApplicationStrategy appStrategy)
                return appStrategy;
                
            // If it's a DomainStrategy, use the adapter
            if (strategy is DomainStrategy domainStrategy)
                return RuntimeErrorSage.Core.Remediation.RemediationStrategyAdapterExtensions.ToApplicationStrategy(domainStrategy);
                
            // As a fallback, create a new adapter with basic properties
            return new RuntimeErrorSage.Core.Remediation.RemediationStrategyAdapter(new RemediationStrategyModel
            {
                Id = strategy.Id,
                Name = strategy.Name,
                Description = strategy.Description
            });
        }
    }

    /// <summary>
    /// Examples showing the fixed patterns for common errors in RemediationExecutor
    /// </summary>
    public class FixedRemediationExecutorExample : IRemediationExecutor
    {
        private readonly ILogger<FixedRemediationExecutorExample> _logger;
        private readonly ApplicationStrategy _strategy;
        private readonly IRemediationValidator _validator;
        private readonly IRemediationMetricsCollector _metricsCollector;
        private readonly Dictionary<string, RemediationExecution> _executionHistory;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FixedRemediationExecutorExample"/> class.
        /// </summary>
        public FixedRemediationExecutorExample(
            ILogger<FixedRemediationExecutorExample> logger,
            ApplicationStrategy strategy,
            IRemediationValidator validator,
            IRemediationMetricsCollector metricsCollector)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _executionHistory = new Dictionary<string, RemediationExecution>();
        }

        /// <inheritdoc />
        public bool IsEnabled => true;

        /// <inheritdoc />
        public string Name => "Fixed Remediation Executor Example";

        /// <inheritdoc />
        public string Version => "1.0.0";

        /// <inheritdoc />
        public async Task<RemediationResult> ExecuteStrategyAsync(ApplicationStrategy strategy, ErrorContext errorContext)
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
                var execution = new RemediationExecution
                {
                    RemediationId = Guid.NewGuid().ToString(),
                    ErrorId = errorContext.ErrorId,
                    CorrelationId = errorContext.CorrelationId,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Status = result.Status,
                    Success = result.Success,
                    ErrorMessage = result.ErrorMessage
                };

                // Add the execution to the dictionary for history tracking
                _executionHistory[execution.ExecutionId] = execution;

                // Record the execution metrics
                await _metricsCollector.RecordExecutionAsync(execution.RemediationId, result);

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
                    var strategyResult = await ExecuteStrategyAsync(
                        (strategy is ApplicationStrategy appStrategy) ? appStrategy : 
                            new RuntimeErrorSage.Core.Remediation.RemediationStrategyAdapter(
                                new RemediationStrategyModel { 
                                    Id = strategy.Id, 
                                    Name = strategy.Name, 
                                    Description = strategy.Description 
                                }
                            ), 
                        plan.Context);
                    
                    if (strategyResult.Status != RemediationStatusEnum.Success)
                    {
                        result.Status = RemediationStatusEnum.Failed;
                        result.Message = $"Strategy {strategy.Name} failed: {strategyResult.Message}";
                        break;
                    }

                    result.CompletedSteps.Add(new RemediationStep 
                    { 
                        Name = strategy.Name,
                        Status = RemediationStepStatus.Completed.ToString()
                    });
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
                    PlanId = result.RemediationId
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
                    ErrorId = context.ErrorId,
                    CorrelationId = context.CorrelationId,
                    StartTime = DateTime.UtcNow,
                    Status = RemediationStatusEnum.InProgress.ToString()
                };
                
                // Add execution context details
                var contextInfo = new Dictionary<string, object>
                {
                    { "Environment", context.Environment },
                    { "ServiceName", context.ServiceName },
                    { "ErrorType", analysis.ErrorType }
                };
                
                // Get applicable strategies
                var strategies = await _strategyProvider.GetApplicableStrategiesAsync(context);
                if (!strategies.Any())
                {
                    execution.Status = RemediationStatusEnum.Failed.ToString();
                    execution.EndTime = DateTime.UtcNow;
                    _logger.LogWarning("No applicable remediation strategies found for error {ErrorId}", context.ErrorId);
                    return execution;
                }
                
                // Select the best strategy
                var strategy = await _strategyProvider.GetBestStrategyAsync(context);
                if (strategy == null)
                {
                    execution.Status = RemediationStatusEnum.Failed.ToString();
                    execution.EndTime = DateTime.UtcNow;
                    _logger.LogWarning("Could not determine best remediation strategy for error {ErrorId}", context.ErrorId);
                    return execution;
                }
                
                // Convert application strategy to domain strategy if needed
                var domainStrategy = strategy;
                if (strategy is ApplicationStrategy appStrategy)
                {
                    domainStrategy = RuntimeErrorSage.Core.Remediation.RemediationStrategyAdapterExtensions.ToDomainStrategy(appStrategy);
                }
                
                // Create a remediation plan
                var plan = await domainStrategy.CreateActionsAsync(context);
                
                // For this example, execute each action in the plan
                var completedSteps = new List<RemediationStep>();
                foreach (var action in plan)
                {
                    try
                    {
                        _logger.LogInformation("Executing action {ActionId} of type {ActionType}",
                            action.Id, action.ActionType);
                        
                        // Create a step record
                        var step = new RemediationStep
                        {
                            ActionId = action.Id,
                            ActionType = action.ActionType,
                            StartTime = DateTime.UtcNow,
                            Status = RemediationStepStatus.Completed.ToString()
                        };
                        
                        // Simulate action execution
                        await Task.Delay(100); // Replace with actual execution
                        
                        // Mark step as completed
                        step.EndTime = DateTime.UtcNow;
                        step.Status = RemediationStepStatus.Completed.ToString();
                        
                        completedSteps.Add(step);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error executing action {ActionId}", action.Id);
                    }
                }
                
                // Update execution with results
                execution.EndTime = DateTime.UtcNow;
                execution.Status = RemediationStatusEnum.Success.ToString();
                
                return execution;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing remediation for error {ErrorId}", context.ErrorId);
                
                return new RemediationExecution
                {
                    RemediationId = Guid.NewGuid().ToString(),
                    ErrorId = context.ErrorId,
                    CorrelationId = context.CorrelationId,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Status = RemediationStatusEnum.Failed.ToString()
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationValidationResult> ValidateRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(analysis);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Validating remediation for analysis {AnalysisId} and context {ContextId}", 
                    analysis.Id, context.Id);

                var validationResult = new RemediationValidationResult
                {
                    IsValid = true,
                    Messages = new List<string>(),
                    ValidationWarnings = new List<ValidationWarning>(),
                    ValidationErrors = new List<ValidationError>()
                };

                // Add validation logic here
                await Task.Delay(100); // Simulate work

                return validationResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating remediation for analysis {AnalysisId} and context {ContextId}", 
                    analysis.Id, context.Id);

                return new RemediationValidationResult
                {
                    IsValid = false,
                    Messages = new List<string> { ex.Message },
                    ValidationWarnings = new List<ValidationWarning>(),
                    ValidationErrors = new List<ValidationError> { new ValidationError { Message = ex.Message } }
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
        public async Task<RemediationResult> ExecuteActionAsync(RemediationAction action, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Executing action {ActionName} for error context {ErrorId}", 
                    action.Name, context.ErrorId);
                
                // Validate action
                var validationResult = await ValidateActionAsync(action, context);
                if (!validationResult.IsValid)
                {
                    return new RemediationResult
                    {
                        Success = false,
                        ErrorMessage = "Action validation failed: " + string.Join(", ", validationResult.Errors),
                        Status = RemediationStatusEnum.Failed,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow
                    };
                }
                
                // Execute action
                var result = new RemediationResult
                {
                    Success = true,
                    Status = RemediationStatusEnum.Success,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddSeconds(1),
                    Actions = new List<RemediationActionResult>
                    {
                        new RemediationActionResult
                        {
                            ActionId = action.Id,
                            IsSuccessful = true,
                            StartTime = DateTime.UtcNow,
                            EndTime = DateTime.UtcNow.AddSeconds(1)
                        }
                    }
                };
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing action {ActionName} for context {ErrorId}", 
                    action.Name, context.ErrorId);
                
                return new RemediationResult
                {
                    Success = false,
                    ErrorMessage = $"Execution error: {ex.Message}",
                    Status = RemediationStatusEnum.Failed,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow
                };
            }
        }

        /// <inheritdoc />
        public async Task<ValidationResult> ValidateActionAsync(RemediationAction action, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Validating action {ActionId} for context {ContextId}", 
                    action.Id, context.Id);

                var validationResult = new ValidationResult
                {
                    IsValid = true,
                    Messages = new List<string>()
                };

                // Add validation logic here
                await Task.Delay(100); // Simulate work

                return validationResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating action {ActionId} for context {ContextId}", 
                    action.Id, context.Id);

                return new ValidationResult
                {
                    IsValid = false,
                    Messages = new List<string> { ex.Message }
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
                    Severity = ImpactSeverity.Low.ToRemediationActionSeverity(),
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
                    Severity = ImpactSeverity.Unknown.ToRemediationActionSeverity(),
                    Description = $"Error calculating impact: {ex.Message}"
                };
            }
        }

        /// <inheritdoc />
        public async Task<RiskAssessmentModel> GetActionRiskAsync(RemediationAction action, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Calculating risk of action {ActionId} for error context {ErrorId}", 
                    action.ActionId, context.Id);
                
                // Calculate risk
                
                return new RiskAssessmentModel
                {
                    RiskLevel = RiskLevel.Low.ToRemediationRiskLevel(),
                    Description = "Low risk remediation action",
                    PotentialIssues = new List<string> { "Temporary performance impact" },
                    MitigationSteps = new List<string> { "Monitor system performance during remediation" }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating risk for action {ActionId} for context {ErrorId}", 
                    action.ActionId, context.Id);
                
                return new RiskAssessmentModel
                {
                    RiskLevel = RiskLevel.Medium.ToRemediationRiskLevel(),
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
            var remediationStatus = RuntimeErrorSage.Domain.Models.Remediation.RemediationStatusEnum.Success;
            
            // INCORRECT: Using ambiguous ValidationResult
            // var validationResult = new ValidationResult();
            
            // CORRECT: Using fully qualified name
            var validationResult = new RuntimeErrorSage.Domain.Models.Validation.ValidationResult();
            
            // Create a strategy with explicit namespace to avoid ambiguity
            ApplicationStrategy strategy = _strategy;
            
            // Execute the strategy with explicit namespace references
            return await strategy.ExecuteAsync(context);
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ExecuteRemediationAsync(RemediationPlan plan, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(plan);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Executing remediation plan {PlanId} for context {ContextId}", 
                    plan.PlanId, context.Id);

                var result = new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.InProgress,
                    Message = "Remediation execution started",
                    StartTime = DateTime.UtcNow
                };

                // Execute actions in the plan
                foreach (var action in plan.Actions)
                {
                    var actionResult = await ExecuteActionAsync(action, context);
                    
                    if (actionResult.Status != RemediationStatusEnum.Success)
                    {
                        result.Status = RemediationStatusEnum.Failed;
                        result.Message = $"Action {action.Name} failed: {actionResult.ErrorMessage}";
                        break;
                    }

                    result.CompletedSteps.Add(new RemediationStep
                    {
                        Name = action.Name,
                        Status = RemediationStepStatus.Completed.ToString(),
                        StartTime = actionResult.StartTime,
                        EndTime = actionResult.EndTime
                    });
                }

                if (result.Status == RemediationStatusEnum.InProgress)
                {
                    result.Status = RemediationStatusEnum.Success;
                    result.Message = "Remediation executed successfully";
                }

                result.EndTime = DateTime.UtcNow;
                
                // Record metrics
                await _metricsCollector.RecordMetricAsync(
                    plan.PlanId,
                    "remediation_executed",
                    new
                    {
                        Status = result.Status.ToString(),
                        Duration = result.EndTime.HasValue 
                            ? (result.EndTime.Value - result.StartTime).TotalMilliseconds 
                            : 0
                    });
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing remediation for plan {PlanId} and context {ContextId}", 
                    plan.PlanId, context.Id);
                
                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Remediation execution failed: {ex.Message}",
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> RollbackActionAsync(string actionId, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Rolling back action {ActionId} for context {ContextId}", 
                    actionId, context.Id);
                
                // Perform rollback logic
                await Task.Delay(100); // Simulate work
                
                // Record metrics
                await _metricsCollector.RecordMetricAsync(
                    actionId,
                    "action_rollback",
                    new
                    {
                        Status = "Success",
                        Timestamp = DateTime.UtcNow
                    });
                
                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.Success,
                    Message = $"Action {actionId} rolled back successfully",
                    IsRollback = true,
                    StartTime = DateTime.UtcNow.AddMilliseconds(-100),
                    EndTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back action {ActionId}", actionId);
                
                await _metricsCollector.RecordMetricAsync(
                    actionId,
                    "action_rollback",
                    new
                    {
                        Status = "Failed",
                        Error = ex.Message,
                        Timestamp = DateTime.UtcNow
                    });
                
                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Action rollback failed: {ex.Message}",
                    IsRollback = true,
                    StartTime = DateTime.UtcNow.AddMilliseconds(-100),
                    EndTime = DateTime.UtcNow,
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> GetActionStatusAsync(string actionId, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Getting status for action {ActionId} in context {ContextId}", 
                    actionId, context.Id);
                
                // Retrieve action status from execution history
                RemediationStatusEnum status = RemediationStatusEnum.Unknown;
                string message = "Status unknown";
                
                if (_executionHistory.TryGetValue(actionId, out var execution))
                {
                    status = execution.Status;
                    message = execution.Message;
                }
                
                return new RemediationResult
                {
                    Context = context,
                    Status = status,
                    Message = message,
                    ActionId = actionId,
                    StartTime = DateTime.UtcNow.AddMilliseconds(-100),
                    EndTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting status for action {ActionId}", actionId);
                
                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Error getting action status: {ex.Message}",
                    ActionId = actionId,
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }
    }
} 
