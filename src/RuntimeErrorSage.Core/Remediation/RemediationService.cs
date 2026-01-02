using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Common;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Models.Execution;
using RuntimeErrorSage.Domain.Models.Metrics;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Options;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Application.Validation.Interfaces;
using RuntimeErrorSage.Application.Extensions;

using ApplicationStrategy = RuntimeErrorSage.Application.Interfaces.IRemediationStrategy;
using DomainStrategy = RuntimeErrorSage.Domain.Interfaces.IRemediationStrategy;
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;
using RuntimeErrorSage.Core.Remediation.Mapping;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Service for managing remediation operations.
    /// Follows DDD principles by using mappers for type conversions and maintaining layer separation.
    /// </summary>
    public class RemediationService : IRemediationService
    {
        private readonly ILogger<RemediationService> _logger;
        private readonly IRemediationPlanManager _planManager;
        private readonly IRemediationExecutor _executor;
        private readonly IRemediationMetricsCollector _metricsCollector;
        private readonly IRemediationSuggestionManager _suggestionManager;
        private readonly IRemediationActionManager _actionManager;
        private readonly IRemediationStrategySelector _strategySelector;
        private readonly IRemediationValidator _validator;
        private readonly Dictionary<string, ApplicationStrategy> _registeredStrategies;
        private readonly IValidationRuleProvider _validationRuleProvider;
        private readonly IRemediationStrategyMapper _strategyMapper;
        private readonly IRemediationModelMapper _modelMapper;

        public RemediationService(
            ILogger<RemediationService> logger,
            IRemediationPlanManager planManager,
            IRemediationExecutor executor,
            IRemediationMetricsCollector metricsCollector,
            IRemediationSuggestionManager suggestionManager,
            IRemediationActionManager actionManager,
            IRemediationStrategySelector strategySelector,
            IRemediationValidator validator,
            IValidationRuleProvider validationRuleProvider,
            IRemediationStrategyMapper strategyMapper,
            IRemediationModelMapper modelMapper)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(planManager);
            ArgumentNullException.ThrowIfNull(executor);
            ArgumentNullException.ThrowIfNull(metricsCollector);
            ArgumentNullException.ThrowIfNull(suggestionManager);
            ArgumentNullException.ThrowIfNull(actionManager);
            ArgumentNullException.ThrowIfNull(strategySelector);
            ArgumentNullException.ThrowIfNull(validator);
            ArgumentNullException.ThrowIfNull(validationRuleProvider);
            ArgumentNullException.ThrowIfNull(strategyMapper);
            ArgumentNullException.ThrowIfNull(modelMapper);

            _logger = logger;
            _planManager = planManager;
            _executor = executor;
            _metricsCollector = metricsCollector;
            _suggestionManager = suggestionManager;
            _actionManager = actionManager;
            _strategySelector = strategySelector;
            _validator = validator;
            _validationRuleProvider = validationRuleProvider;
            _strategyMapper = strategyMapper;
            _modelMapper = modelMapper;
            _registeredStrategies = new Dictionary<string, ApplicationStrategy>();
        }

        /// <inheritdoc/>
        public bool IsEnabled => true;

        /// <inheritdoc/>
        public string Name => "RuntimeErrorSage Remediation Service";

        /// <inheritdoc/>
        public string Version => "1.0.0";

        /// <inheritdoc />
        public void RegisterStrategy(IRemediationStrategy strategy)
        {
            ArgumentNullException.ThrowIfNull(strategy);

            try
            {
                _logger.LogInformation("Registering remediation strategy: {StrategyName}", strategy.Name);
                
                // Cannot assign to read-only property, so skip if Id is empty
                if (!string.IsNullOrEmpty(strategy.Id))
                {
                    _registeredStrategies[strategy.Id] = strategy;
                    
                    _logger.LogInformation("Strategy registered successfully: {StrategyName} ({StrategyId})", 
                        strategy.Name, strategy.Id);
                }
                else
                {
                    _logger.LogWarning("Strategy {StrategyName} not registered - ID is required", strategy.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register strategy {StrategyName}", strategy.Name);
                throw;
            }
        }

        /// <inheritdoc />
        public void Configure(RuntimeErrorSageOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            try
            {
                _logger.LogInformation("Configuring remediation service with options");
                
                // Apply configuration options here
                // This could include configuring strategy selectors, executors, etc.
                
                _logger.LogInformation("Remediation service configured");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to configure remediation service");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> RemediateAsync(ErrorAnalysisResult analysis, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(analysis);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Remediating error for context {ContextId}", context.Id);
                
                // Create remediation plan based on analysis
                var plan = await CreatePlanAsync(context);
                
                // Apply the remediation
                return await ApplyRemediationAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemediateAsync for context {ContextId}", context.Id);
                
                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Remediation failed: {ex.Message}",
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ExecuteActionAsync(RemediationAction action)
        {
            ArgumentNullException.ThrowIfNull(action);

            try
            {
                _logger.LogInformation("Executing remediation action: {ActionId} - {ActionName}", action.Id, action.Name);
                
                // Validate the action first
                var validationResult = await ValidateActionAsync(action);
                if (!validationResult.IsValid)
                {
                    return new RemediationResult
                    {
                        Status = RemediationStatusEnum.Failed,
                        Message = "Action validation failed",
                        Validation = validationResult
                    };
                }
                
                // Execute the action if validation passed
                var executionResult = await ExecuteActionAsync(action, action.Context);
                
                // Collect metrics about the execution
                await _metricsCollector.RecordMetricAsync(
                    action.Id,
                    "action_executed",
                    new
                    {
                        ActionType = action.ActionType,
                        ExecutionTime = DateTime.UtcNow,
                        Status = executionResult.Status.ToString()
                    });
                
                // Parse Status string to ExecutionStatus enum for comparison
                var statusString = executionResult.Status;
                var isCompleted = statusString == Domain.Enums.ExecutionStatus.Completed.ToString() || 
                                 statusString == "Completed" ||
                                 statusString == "Success";
                
                return new RemediationResult
                {
                    Status = isCompleted 
                        ? RemediationStatusEnum.Success 
                        : RemediationStatusEnum.Failed,
                    Message = executionResult.ErrorMessage ?? "Execution completed",
                    CompletedSteps = new List<RemediationStep> 
                    { 
                        new RemediationStep 
                        { 
                            Name = action.Name,
                            Status = isCompleted 
                                ? RemediationStatusEnum.Success.ToString() 
                                : RemediationStatusEnum.Failed.ToString(),
                            Message = executionResult.ErrorMessage ?? "Step completed",
                            StartTime = executionResult.StartTime,
                            EndTime = executionResult.EndTime
                        } 
                    },
                    Success = isCompleted,
                    Error = !isCompleted ? executionResult.ErrorMessage : null,
                    Metadata = new Dictionary<string, object>
                    {
                        { "ActionId", action.Id },
                        { "ErrorId", action.Context?.ErrorId ?? "Unknown" },
                        { "Status", executionResult.Status.ToString() }
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing action {ActionId}", action.Id);
                return new RemediationResult
                {
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Action execution failed: {ex.Message}",
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ApplyRemediationAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Starting remediation for error context {ErrorId}", context.Id);

                // Create remediation plan
                var plan = await CreatePlanAsync(context);
                if (plan.Status != RemediationStatusEnum.NotStarted)
                {
                    return new RemediationResult
                    {
                        Context = context,
                        Status = plan.Status,
                        Message = "Failed to create remediation plan",
                        Validation = new ValidationResult { IsValid = false, Messages = new List<string> { "Failed to create remediation plan" } }
                    };
                }

                // Validate plan
                var validationResult = await ValidatePlanAsync(plan);
                if (!validationResult)
                {
                    return new RemediationResult
                    {
                        Context = context,
                        Status = RemediationStatusEnum.Failed,
                        Message = "Plan validation failed",
                        Validation = new ValidationResult { IsValid = false, Messages = new List<string> { "Plan validation failed" } }
                    };
                }

                // Get strategy from plan - it's already a Domain strategy
                var domainStrategy = plan.Strategies.FirstOrDefault();
                if (domainStrategy == null)
                {
                    return new RemediationResult
                    {
                        Context = context,
                        Status = RemediationStatusEnum.Failed,
                        Message = "No strategy found in plan",
                        Validation = new ValidationResult { IsValid = false, Messages = new List<string> { "No strategy found in plan" } }
                    };
                }

                // Create action using the domain strategy directly
                var action = new RemediationAction(_validationRuleProvider)
                {
                    Name = domainStrategy.Name ?? "Unknown Strategy",
                    Description = domainStrategy.Description ?? string.Empty,
                    Strategy = domainStrategy,
                    Context = context
                };
                var result = await _executor.ExecuteActionAsync(action, context);
                if (result.Status != RemediationStatusEnum.Success)
                {
                    return new RemediationResult
                    {
                        Context = context,
                        Status = RemediationStatusEnum.Failed,
                        Message = result.Message,
                        CompletedSteps = result.CompletedSteps,
                        FailedSteps = result.FailedSteps,
                        Metrics = result.Metrics,
                        Validation = new ValidationResult { IsValid = false, Messages = new List<string> { result.Message } }
                    };
                }

                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.Success,
                    Message = "Remediation completed successfully",
                    CompletedSteps = result.CompletedSteps,
                    FailedSteps = result.FailedSteps,
                    Metrics = result.Metrics,
                    Validation = new ValidationResult { IsValid = true, Messages = new List<string> { "Remediation completed successfully" } }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying remediation for context {ErrorId}", context.Id);
                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Remediation failed: {ex.Message}",
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Creating remediation plan for context {ContextId}", context.Id);

                var plan = new RemediationPlan(
                    "Default Plan",
                    "Default remediation plan",
                    new List<RemediationAction>(),
                    new Dictionary<string, object>(),
                    TimeSpan.FromMinutes(5)
                );

                // Add plan creation logic here
                return plan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating remediation plan for context {ContextId}", context.Id);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> ValidatePlanAsync(RemediationPlan plan)
        {
            ArgumentNullException.ThrowIfNull(plan);

            try
            {
                var result = await _validator.ValidatePlanAsync(plan, plan.Context);
                return result.IsValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating remediation plan");
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationMetrics> GetMetricsAsync(string remediationId)
        {
            ArgumentNullException.ThrowIfNull(remediationId);

            try
            {
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
                    EndTime = DateTime.UtcNow,
                    Metadata = new Dictionary<string, object>()
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationStatusEnum> GetStatusAsync(string remediationId)
        {
            ArgumentNullException.ThrowIfNull(remediationId);

            try
            {
                // Get status using GetActionStatusAsync
                var statusResult = await _executor.GetActionStatusAsync(remediationId, new ErrorContext(
                    error: new RuntimeError("Status check", "StatusCheck", "System", ""),
                    context: "Status check",
                    timestamp: DateTime.UtcNow));
                var status = statusResult.Status;
                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting status for remediation {RemediationId}", remediationId);
                return RemediationStatusEnum.Unknown;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationSuggestion> GetRemediationSuggestionsAsync(ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                _logger.LogInformation("Getting remediation suggestions for error context {ErrorId}", errorContext.Id);
                var strategy = await _strategySelector.SelectStrategyAsync(errorContext);
                
                if (strategy == null)
                {
                    return new RemediationSuggestion
                    {
                        ErrorContext = errorContext,
                        Status = SuggestionStatus.Failed,
                        Strategies = new List<string>(),
                        Message = "No applicable strategies found"
                    };
                }

                return new RemediationSuggestion
                {
                    ErrorContext = errorContext,
                    Status = SuggestionStatus.Available,
                    Strategies = new List<string> { strategy.Name },
                    ConfidenceScore = 0.8, // TODO: Get actual confidence score
                    Message = $"Suggested strategy: {strategy.Name}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting remediation suggestions for error context {ErrorId}", errorContext.Id);
                
                return new RemediationSuggestion
                {
                    ErrorContext = errorContext,
                    Status = SuggestionStatus.Failed,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        /// <inheritdoc />
        public async Task<ValidationResult> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                if (string.IsNullOrEmpty(suggestion.StrategyName))
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Messages = new List<string> { "No strategy found in suggestion" }
                    };
                }

                // Create a mock context with the strategy name to use with SelectStrategyAsync
                var error = new RuntimeError(
                    message: $"Mock error for strategy {suggestion.StrategyName}",
                    errorType: suggestion.StrategyName,
                    source: "RemediationService",
                    stackTrace: string.Empty
                );

                var mockContext = new ErrorContext(
                    error: error,
                    context: "RemediationService",
                    timestamp: DateTime.UtcNow
                );

                mockContext.CorrelationId = errorContext.CorrelationId;
                
                var strategy = await _strategySelector.SelectStrategyAsync(mockContext);
                if (strategy == null)
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Messages = new List<string> { "Strategy not found" }
                    };
                }

                // Validator expects Application strategy, not Domain strategy
                var result = await _validator.ValidateStrategyAsync(strategy, errorContext);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating suggestion for error context {ErrorId}", errorContext.Id);
                return new ValidationResult
                {
                    IsValid = false,
                    Messages = new List<string> { $"Error validating suggestion: {ex.Message}" }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                _logger.LogInformation("Executing remediation suggestion for error context {ErrorId}", errorContext.Id);
                
                // Validate the suggestion first
                var validationResult = await ValidateSuggestionAsync(suggestion, errorContext);
                if (!validationResult.IsValid)
                {
                    return new RemediationResult
                    {
                        Context = errorContext,
                        Status = RemediationStatusEnum.Failed,
                        Message = "Suggestion validation failed",
                        Validation = validationResult
                    };
                }

                // Use mapper to create action from suggestion
                var action = _modelMapper.ToAction(suggestion, errorContext, _strategyMapper);
                
                // Set strategy if available (mapper doesn't handle this yet, so we do it here)
                if (suggestion.Strategies?.Any() == true)
                {
                    // Note: This assumes strategies is a list of strategy IDs or names
                    // Actual implementation may need to lookup the strategy
                    action.Strategy = null; // Will be set by strategy lookup if needed
                }

                // Execute the action
                var executionResult = await ExecuteActionAsync(action, errorContext);
                
                // Use mapper to convert execution result to remediation result
                var result = _modelMapper.ToResult(executionResult, errorContext);
                
                await _metricsCollector.RecordRemediationMetricsAsync(new RemediationMetrics
                {
                    ExecutionId = action.ActionId,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Success = result.Status == RemediationStatusEnum.Success,
                    Error = result.Status == RemediationStatusEnum.Failed ? (result.ErrorMessage ?? result.Message) : null,
                    Metadata = new Dictionary<string, object>
                    {
                        { "Suggestion", suggestion.Id },
                        { "ErrorId", errorContext.Id },
                        { "Status", result.Status.ToString() }
                    }
                });
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing remediation suggestion for context {ErrorId}", errorContext.Id);
                return new RemediationResult
                {
                    Context = errorContext,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Error executing remediation suggestion: {ex.Message}",
                    Validation = new ValidationResult { IsValid = false, Messages = new List<string> { ex.Message } }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            return await ExecuteSuggestionAsync(suggestion, suggestion.ErrorContext);
        }

        /// <inheritdoc />
        public async Task<RemediationImpact> GetSuggestionImpactAsync(RemediationSuggestion suggestion, ErrorContext errorContext)
        {
            ArgumentNullException.ThrowIfNull(suggestion);
            ArgumentNullException.ThrowIfNull(errorContext);

            try
            {
                if (string.IsNullOrEmpty(suggestion.StrategyName))
                {
                    return new RemediationImpact
                    {
                        Severity = SeverityLevel.Unknown.ToImpactSeverity().ToRemediationActionSeverity(),
                        Scope = ImpactScope.Component.ToRemediationActionImpactScope(),
                        AffectedComponents = new List<string>(),
                        EstimatedRecoveryTime = TimeSpan.Zero
                    };
                }

                _logger.LogInformation("Getting impact for remediation suggestion for error context {ErrorId}", errorContext.Id);
                
                // Create a mock context with the strategy name to use with SelectStrategyAsync
                var error = new RuntimeError(
                    message: $"Mock error for strategy {suggestion.StrategyName}",
                    errorType: suggestion.StrategyName,
                    source: "RemediationService",
                    stackTrace: string.Empty
                );

                var mockContext = new ErrorContext(
                    error: error,
                    context: "RemediationService",
                    timestamp: DateTime.UtcNow
                );

                mockContext.CorrelationId = errorContext.CorrelationId;
                
                var strategy = await _strategySelector.SelectStrategyAsync(mockContext);
                if (strategy == null)
                {
                    return new RemediationImpact
                    {
                        Severity = SeverityLevel.Unknown.ToImpactSeverity().ToRemediationActionSeverity(),
                        Scope = ImpactScope.Component.ToRemediationActionImpactScope(),
                        AffectedComponents = new List<string>(),
                        EstimatedRecoveryTime = TimeSpan.Zero
                    };
                }

                // Use mapper to convert application strategy to domain strategy
                var domainStrategy = _strategyMapper.ToDomain(strategy);
                
                // Try to get impact from the domain strategy if it's a RemediationStrategy base class
                if (domainStrategy is RuntimeErrorSage.Core.Remediation.Base.RemediationStrategy baseStrategy)
                {
                    var impact = await baseStrategy.GetImpactAsync(errorContext);
                    if (impact != null)
                    {
                        return impact;
                    }
                }
                
                // Fallback: create default impact
                return new RemediationImpact
                {
                    Severity = RemediationActionSeverity.Medium,
                    Scope = RemediationActionImpactScope.Module,
                    AffectedComponents = new List<string>(),
                    EstimatedRecoveryTime = TimeSpan.FromMinutes(30)
                };
                return new RemediationImpact
                {
                    Severity = SeverityLevel.Unknown.ToImpactSeverity().ToRemediationActionSeverity(),
                    Scope = ImpactScope.Component.ToRemediationActionImpactScope(),
                    AffectedComponents = new List<string>(),
                    EstimatedRecoveryTime = TimeSpan.Zero
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting impact for suggestion for error context {ErrorId}", errorContext.Id);
                return new RemediationImpact
                {
                    Severity = SeverityLevel.Unknown.ToImpactSeverity().ToRemediationActionSeverity(),
                    Scope = ImpactScope.Component.ToRemediationActionImpactScope(),
                    AffectedComponents = new List<string>(),
                    EstimatedRecoveryTime = TimeSpan.Zero
                };
            }
        }

        /// <inheritdoc />
        public async Task<RuntimeErrorSage.Domain.Models.Execution.RemediationActionExecution> ExecuteActionAsync(RemediationAction action, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Executing action {ActionId} for context {ContextId}", 
                    action.Id, context.Id);
                
                // Track execution start
                await _metricsCollector.RecordMetricAsync(
                    action.Id,
                    "action_execution_start",
                    new Dictionary<string, object>
                    {
                        { "ActionType", action.ActionType },
                        { "StartTime", DateTime.UtcNow }
                    });
                
                // Create the execution record
                var execution = new RuntimeErrorSage.Domain.Models.Execution.RemediationActionExecution
                {
                    ActionId = action.Id,
                    StartTime = DateTime.UtcNow,
                    Status = RemediationStatusEnum.InProgress.ToString(),
                    ErrorMessage = string.Empty
                };
                
                try
                {
                    // Execute the action's logic here
                    // For now, this is just a simulation
                    await Task.Delay(100); // Simulate work
                    
                    // Update execution record
                    execution.EndTime = DateTime.UtcNow;
                    execution.Status = RemediationStatusEnum.Success.ToString();
                    
                    // Track execution completion
                    var metrics = new Dictionary<string, string>
                    {
                        { "ActionType", action.ActionType },
                        { "EndTime", DateTime.UtcNow.ToString("o") },
                        { "Duration", (execution.EndTime - execution.StartTime).GetTotalMilliseconds().ToString() },
                        { "Status", execution.Status }
                    };
                    
                    return execution;
                }
                catch (Exception ex)
                {
                    execution.EndTime = DateTime.UtcNow;
                    execution.Status = RemediationStatusEnum.Failed.ToString();
                    execution.ErrorMessage = ex.Message;
                    
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing action {ActionId}", action.Id);
                
                var execution = new RuntimeErrorSage.Domain.Models.Execution.RemediationActionExecution
                {
                    ActionId = action.Id,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Status = RemediationStatusEnum.Failed.ToString(),
                    ErrorMessage = ex.Message
                };
                
                return execution;
            }
        }

        /// <inheritdoc />
        public async Task<ValidationResult> ValidateActionAsync(RemediationAction action)
        {
            ArgumentNullException.ThrowIfNull(action);

            try
            {
                _logger.LogInformation("Validating remediation action {ActionId}", action.ActionId);
                
                if (action.Context == null)
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Messages = new List<string> { "No context provided for action validation" }
                    };
                }
                
                return await _validator.ValidateActionAsync(action, action.Context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating action {ActionId}", action.ActionId);
                
                return new ValidationResult
                {
                    IsValid = false,
                    Messages = new List<string> { $"Error validating action: {ex.Message}" }
                };
            }
        }

        /// <inheritdoc />
        public async Task<RuntimeErrorSage.Domain.Enums.RollbackStatus> RollbackActionAsync(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);

            try
            {
                _logger.LogInformation("Rolling back action {ActionId}", actionId);
                
                // Implement rollback logic
                var result = await Task.FromResult(new RemediationResult
                {
                    Status = RemediationStatusEnum.Success,
                    Message = $"Rollback for action {actionId} completed successfully"
                });
                
                return result.Status == RemediationStatusEnum.Success ? RuntimeErrorSage.Domain.Enums.RollbackStatus.Completed : RuntimeErrorSage.Domain.Enums.RollbackStatus.Failed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back action {ActionId}", actionId);
                return RuntimeErrorSage.Domain.Enums.RollbackStatus.Failed;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> GetActionStatusAsync(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);

            try
            {
                _logger.LogInformation("Getting status for remediation action {ActionId}", actionId);
                
                // Get execution status instead of history
                var executionResult = await _executor.GetActionStatusAsync(actionId, new ErrorContext(
                    error: new RuntimeError("Status check", "StatusCheck", "System", ""),
                    context: "Status check",
                    timestamp: DateTime.UtcNow));
                var execution = executionResult; // Use result as execution info
                if (execution == null)
                {
                    return new RemediationResult
                    {
                        Status = RemediationStatusEnum.Unknown,
                        Message = "No execution history found"
                    };
                }
                
                return new RemediationResult
                {
                    Status = execution.Status,
                    Message = execution.ErrorMessage ?? "Remediation execution details retrieved",
                    IsSuccessful = execution.Status == RemediationStatusEnum.Success,
                    StartTime = execution.StartTime,
                    EndTime = execution.EndTime,
                    ActionId = actionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting status for action {ActionId}", actionId);
                
                return new RemediationResult
                {
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Error getting action status: {ex.Message}"
                };
            }
        }

        private RemediationImpact CreateDefaultImpact()
        {
            return new RemediationImpact
            {
                Severity = SeverityLevel.Unknown.ToImpactSeverity().ToRemediationActionSeverity(),
                Scope = ImpactScope.Component.ToRemediationActionImpactScope(),
                AffectedComponents = new List<string>(),
                EstimatedRecoveryTime = TimeSpan.Zero
            };
        }

        private RemediationImpact CreateErrorImpact(string errorMessage)
        {
            return new RemediationImpact
            {
                Severity = SeverityLevel.Unknown.ToImpactSeverity().ToRemediationActionSeverity(),
                Scope = ImpactScope.Component.ToRemediationActionImpactScope(),
                AffectedComponents = new List<string>(),
                EstimatedRecoveryTime = TimeSpan.Zero,
                Description = errorMessage
            };
        }

        private RemediationImpact CreateTimeoutImpact()
        {
            return new RemediationImpact
            {
                Severity = SeverityLevel.Unknown.ToImpactSeverity().ToRemediationActionSeverity(),
                Scope = ImpactScope.Unknown.ToRemediationActionImpactScope(),
                AffectedComponents = new List<string>(),
                EstimatedRecoveryTime = TimeSpan.Zero,
                Description = "Operation timed out"
            };
        }

        private RemediationImpact CreateCancelledImpact()
        {
            return new RemediationImpact
            {
                Severity = SeverityLevel.Unknown.ToImpactSeverity().ToRemediationActionSeverity(),
                Scope = ImpactScope.Unknown.ToRemediationActionImpactScope(),
                AffectedComponents = new List<string>(),
                EstimatedRecoveryTime = TimeSpan.Zero,
                Description = "Operation was cancelled"
            };
        }

        public async Task<RemediationAction> CreateActionAsync(ErrorContext context, string actionType)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentException.ThrowIfNullOrEmpty(actionType);

            try
            {
                _logger.LogInformation("Creating remediation action of type {ActionType} for context {ContextId}",
                    actionType, context.Id);

                var action = new RemediationAction(_validationRuleProvider)
                {
                    Id = Guid.NewGuid().ToString(),
                    ActionType = actionType,
                    Name = "Default action",
                    Severity = RemediationActionSeverity.Medium,
                    Parameters = new Dictionary<string, object>()
                };

                // Add action creation logic here
                return action;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating remediation action for context {ContextId}", context.Id);
                throw;
            }
        }

        public async Task<RemediationResult> GetResultAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                _logger.LogInformation("Getting remediation result for context {ContextId}", context.Id);

                var result = new RemediationResult(
                    context,
                    RemediationStatusEnum.NotStarted,
                    "No remediation actions executed yet",
                    string.Empty
                );

                // Add result retrieval logic here
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting remediation result for context {ContextId}", context.Id);
                throw;
            }
        }

        public async Task<RemediationResult> ExecuteRemediationAsync(ErrorContext context, RemediationPlan plan)
        {
            try
            {
                var result = await _executor.ExecuteRemediationAsync(plan, context);
                if (result.Status == RemediationStatusEnum.Success)
                {
                    await _metricsCollector.RecordRemediationMetricsAsync(new RemediationMetrics
                    {
                        ExecutionId = plan.PlanId,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow,
                        Success = result.Success,
                        Metadata = new Dictionary<string, object>
                        {
                            { "PlanId", plan.PlanId },
                            { "ErrorId", context.ErrorId }
                        }
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                return RemediationResult.CreateFailure(context, ex.Message);
            }
        }

        public async Task<RemediationAction> CreateActionFromStrategyAsync(IRemediationStrategy strategy, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(strategy);
            ArgumentNullException.ThrowIfNull(context);

            var action = new RemediationAction(_validationRuleProvider)
            {
                Id = Guid.NewGuid().ToString(),
                Name = strategy.Name,
                Description = strategy.Description,
                Context = context,
                ActionType = strategy.Name ?? "Unknown",
                ErrorType = context.ErrorType,
                Severity = RemediationActionSeverity.Medium, // Default severity based on strategy
                ImpactScope = RemediationActionImpactScope.Module, // Default scope (Component doesn't exist, use Module)
                Parameters = strategy.Parameters ?? new Dictionary<string, object>(),
                Strategy = strategy != null ? _strategyMapper.ToDomain(strategy) : null,
                RiskLevel = strategy.RiskLevel
            };

            return action;
        }

        public async Task<RemediationAction> CreateGenericActionAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var action = new RemediationAction(_validationRuleProvider)
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Generic Remediation Action",
                Description = "A generic remediation action for handling errors",
                Context = context,
                ActionType = "Generic",
                ErrorType = context.ErrorType,
                Severity = ImpactSeverity.Warning.ToRemediationActionSeverity(),
                ImpactScope = ImpactScope.Component.ToRemediationActionImpactScope(),
                Parameters = new Dictionary<string, object>(),
                RiskLevel = RiskLevel.Low
            };

            return action;
        }

        public async Task<RemediationAction> CreateSystemActionAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var action = new RemediationAction(_validationRuleProvider)
            {
                Id = Guid.NewGuid().ToString(),
                Name = "System Remediation Action",
                Description = "A system-generated remediation action",
                Context = context,
                ActionType = "System",
                ErrorType = context.ErrorType,
                Severity = ImpactSeverity.Info.ToRemediationActionSeverity(),
                ImpactScope = ImpactScope.Component.ToRemediationActionImpactScope(),
                Parameters = new Dictionary<string, object>(),
                RiskLevel = RiskLevel.Low
            };

            return action;
        }

        public async Task<RemediationAction> CreateAutomaticActionAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var action = new RemediationAction(_validationRuleProvider)
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Automatic Remediation Action",
                Description = "An automatically generated remediation action",
                Context = context,
                ActionType = "Automatic",
                ErrorType = context.ErrorType,
                Severity = ImpactSeverity.Info.ToRemediationActionSeverity(),
                ImpactScope = ImpactScope.Component.ToRemediationActionImpactScope(),
                Parameters = new Dictionary<string, object>(),
                RiskLevel = RiskLevel.Low
            };

            return action;
        }

        /// <inheritdoc />
        public async Task<Application.Interfaces.IRemediationStrategy> GetStrategyAsync(string strategyId)
        {
            try
            {
                _logger.LogInformation("Getting remediation strategy {StrategyId}", strategyId);

                if (string.IsNullOrEmpty(strategyId))
                    throw new ArgumentNullException(nameof(strategyId));

                // Try to get from registered strategies
                if (_registeredStrategies.TryGetValue(strategyId, out var strategy))
                    return strategy;

                // Try to get from strategy selector
                strategy = await _strategySelector.GetAvailableStrategiesAsync()
                    .ContinueWith(t => t.Result.FirstOrDefault(s => s.Id == strategyId));

                if (strategy != null)
                    return strategy;

                // Strategy not found
                throw new KeyNotFoundException($"Remediation strategy with ID {strategyId} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting remediation strategy {StrategyId}", strategyId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ApplyStrategyAsync(Domain.Interfaces.IRemediationStrategy strategy, ErrorContext errorContext)
        {
            try
            {
                _logger.LogInformation("Applying remediation strategy {StrategyId} to error {ErrorId}", 
                    strategy.Id, errorContext.ErrorId);

                // Convert domain strategy to application strategy
                var applicationStrategy = RemediationStrategyAdapterExtensions.FromDomainStrategy(strategy);
                
                // Execute the strategy using the executor - create action first
                var action = new RemediationAction
                {
                    Name = applicationStrategy.Name,
                    Description = applicationStrategy.Description,
                    Strategy = _strategyMapper.ToDomain(applicationStrategy),
                    Context = errorContext
                };
                var result = await _executor.ExecuteActionAsync(action, errorContext);

                // Track the remediation metrics
                await _metricsCollector.RecordRemediationMetricsAsync(new RemediationMetrics
                {
                    ExecutionId = strategy.Id,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Success = result.Success,
                    Metadata = result.Metadata ?? new Dictionary<string, object>()
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying remediation strategy {StrategyId} to error {ErrorId}",
                    strategy.Id, errorContext.ErrorId);
                throw;
            }
        }
    }
} 
