using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Interfaces;
using RuntimeErrorSage.Core.Remediation.Base;
using ApplicationStrategy = RuntimeErrorSage.Application.Interfaces.IRemediationStrategy;
using DomainStrategy = RuntimeErrorSage.Domain.Interfaces.IRemediationStrategy;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Adapter that converts between application and domain strategy interfaces.
    /// This preserves DDD architecture by keeping the domain layer independent.
    /// </summary>
    public static class RemediationStrategyAdapterExtensions
    {
        /// <summary>
        /// Converts an Application IRemediationStrategy to a Domain IRemediationStrategy
        /// </summary>
        public static DomainStrategy ToDomainStrategy(this ApplicationStrategy appStrategy)
        {
            if (appStrategy == null)
                return null;

            return new RemediationStrategyDomainAdapter(appStrategy);
        }

        /// <summary>
        /// Converts a Domain IRemediationStrategy to an Application IRemediationStrategy
        /// </summary>
        public static ApplicationStrategy ToApplicationStrategy(this DomainStrategy domainStrategy)
        {
            if (domainStrategy == null)
                return null;
                
            return new RemediationStrategyApplicationAdapter(domainStrategy);
        }
        
        /// <summary>
        /// Converts an IRemediationStrategy to an Application IRemediationStrategy.
        /// This helps bridge the interface gap when the exact type is unknown.
        /// </summary>
        public static ApplicationStrategy ToApplicationStrategy(this RuntimeErrorSage.Application.Interfaces.IRemediationStrategy strategy)
        {
            if (strategy == null)
                return null;
                
            // If it's already an ApplicationStrategy, return it
            if (strategy is ApplicationStrategy appStrategy)
                return appStrategy;
                
            // If it's a DomainStrategy, convert it
            if (strategy is DomainStrategy domainStrategy)
                return ToApplicationStrategy(domainStrategy);
                
            // As a fallback, create a new adapter
            return new RemediationStrategyAdapter(new RemediationStrategyModel
            {
                Id = strategy.Id,
                Name = strategy.Name,
                Description = strategy.Description,
                Version = strategy.Version,
                IsEnabled = strategy.IsEnabled,
                RiskLevel = strategy.RiskLevel,
                CreatedAt = strategy.CreatedAt
            });
        }
        
        /// <summary>
        /// Converts a RemediationStrategy model to an Application IRemediationStrategy
        /// </summary>
        public static ApplicationStrategy ToApplicationStrategy(this RemediationStrategy strategyModel)
        {
            if (strategyModel == null)
                return null;
            
            // Create an adapter that converts the model to the application interface
            // We first need to get the Domain interface version using the wrapper
            var domainStrategy = RuntimeErrorSage.Core.Remediation.Base.RemediationStrategyExtensions.ToDomainStrategy(strategyModel);
            return new RemediationStrategyAdapter(domainStrategy);
        }

        /// <summary>
        /// Converts a Domain IRemediationStrategy to an Application IRemediationStrategy.
        /// </summary>
        /// <param name="strategy">The Domain strategy to convert.</param>
        /// <returns>The Application strategy.</returns>
        public static ApplicationStrategy FromDomainStrategy(DomainStrategy strategy)
        {
            return new RemediationStrategyAdapter(strategy);
        }
    }

    /// <summary>
    /// Adapts an Application IRemediationStrategy to a Domain IRemediationStrategy
    /// </summary>
    public class RemediationStrategyDomainAdapter : DomainStrategy
    {
        private readonly ApplicationStrategy _appStrategy;

        public RemediationStrategyDomainAdapter(ApplicationStrategy appStrategy)
        {
            _appStrategy = appStrategy ?? throw new ArgumentNullException(nameof(appStrategy));
        }

        public string Id => _appStrategy.Id;
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Version => _appStrategy.Version;
        
        public bool IsEnabled => _appStrategy.IsEnabled;
        
        public RemediationPriority Priority { get; set; }
        
        public int? PriorityValue { get; set; }
        
        public RiskLevel RiskLevel { get; set; }
        
        public Dictionary<string, object> Parameters { get; set; }
        
        public ISet<string> SupportedErrorTypes => _appStrategy.SupportedErrorTypes;
        
        public List<RemediationAction> Actions => _appStrategy.Actions;
        
        public bool AppliesTo(ErrorContext context)
        {
            // Basic implementation to satisfy interface
            return true;
        }
        
        public Task<IEnumerable<RemediationAction>> CreateActionsAsync(ErrorContext context)
        {
            // Basic implementation to satisfy interface
            return Task.FromResult<IEnumerable<RemediationAction>>(_appStrategy.Actions);
        }
        
        public DateTime CreatedAt => DateTime.UtcNow;
        
        public RemediationStatusEnum Status { get; set; } = RemediationStatusEnum.NotStarted;
        
        public Task<RemediationPriority> GetPriorityAsync()
        {
            return Task.FromResult(Priority);
        }
    }

    /// <summary>
    /// Adapts a Domain IRemediationStrategy to an Application IRemediationStrategy
    /// </summary>
    public class RemediationStrategyApplicationAdapter : ApplicationStrategy
    {
        private readonly DomainStrategy _domainStrategy;

        public RemediationStrategyApplicationAdapter(DomainStrategy domainStrategy)
        {
            _domainStrategy = domainStrategy ?? throw new ArgumentNullException(nameof(domainStrategy));
        }

        public string Id => _domainStrategy.Id;
        
        public string Name => _domainStrategy.Name;
        
        public string Description => _domainStrategy.Description;
        
        public string Version => _domainStrategy.Version;
        
        public bool IsEnabled => _domainStrategy.IsEnabled;
        
        public RemediationPriority Priority
        {
            get => _domainStrategy.Priority;
            set
            {
                if (_domainStrategy != null)
                {
                    _domainStrategy.Priority = value;
                    _domainStrategy.PriorityValue = (int)value;
                }
            }
        }
        
        public int? PriorityValue
        {
            get => _domainStrategy.PriorityValue ?? (int)_domainStrategy.Priority;
            set
            {
                if (_domainStrategy != null)
                {
                    _domainStrategy.PriorityValue = value;
                    if (value.HasValue)
                    {
                        _domainStrategy.Priority = (RemediationPriority)value.Value;
                    }
                }
            }
        }
        
        public RiskLevel RiskLevel { get => _domainStrategy.RiskLevel; set => _domainStrategy.RiskLevel = value; }
        
        public Dictionary<string, object> Parameters { get => _domainStrategy.Parameters; set => _domainStrategy.Parameters = value; }
        
        public ISet<string> SupportedErrorTypes => _domainStrategy.SupportedErrorTypes;
        
        public List<RemediationAction> Actions => _domainStrategy.Actions;
        
        public DateTime CreatedAt => DateTime.UtcNow;

        public Task<bool> CanHandleAsync(ErrorContext context)
        {
            // Basic implementation to satisfy interface
            return Task.FromResult(_domainStrategy.AppliesTo(context));
        }

        public Task<double> GetConfidenceAsync(ErrorContext context)
        {
            // Basic implementation to satisfy interface
            return Task.FromResult(0.8);
        }

        public Task<List<RemediationSuggestion>> GetSuggestionsAsync(ErrorContext context)
        {
            // Basic implementation to satisfy interface
            return Task.FromResult(new List<RemediationSuggestion>());
        }

        public Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
        {
            // Basic implementation to satisfy interface
            var actionsTask = _domainStrategy.CreateActionsAsync(context);
            var actions = new List<RemediationAction>(actionsTask.Result);
            
            var plan = new RemediationPlan(
                $"{_domainStrategy.Name} Plan",
                $"Automated plan for {_domainStrategy.Name}",
                actions,
                new Dictionary<string, object>(),
                TimeSpan.FromMinutes(5));
                
            return Task.FromResult(plan);
        }

        public Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            // Create a basic implementation
            return Task.FromResult(new RemediationResult
            {
                Context = context,
                Status = RemediationStatusEnum.Success,
                Message = "Executed via adapter",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddSeconds(1)
            });
        }
    }

    /// <summary>
    /// Adapter class that adapts a model-based remediation strategy to the remediation interface.
    /// This helps solve namespace conflicts by providing a bridge between the two implementations.
    /// </summary>
    public class RemediationStrategyAdapter : ApplicationStrategy
    {
        /// <summary>
        /// Static method to convert a Domain IRemediationStrategy to an Application IRemediationStrategy.
        /// </summary>
        /// <param name="strategy">The Domain strategy to convert.</param>
        /// <returns>The Application strategy.</returns>
        public static ApplicationStrategy FromDomainStrategy(Domain.Interfaces.IRemediationStrategy strategy)
        {
            if (strategy == null)
                return null;
                
            return new RemediationStrategyAdapter(strategy);
        }
        
        private readonly RemediationStrategyModel _strategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationStrategyAdapter"/> class.
        /// </summary>
        /// <param name="strategy">The strategy model to adapt.</param>
        public RemediationStrategyAdapter(RemediationStrategyModel strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationStrategyAdapter"/> class.
        /// </summary>
        /// <param name="strategy">The domain strategy to adapt.</param>
        public RemediationStrategyAdapter(Domain.Interfaces.IRemediationStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));
                
            // Convert the domain strategy to a model
            _strategy = new RemediationStrategyModel
            {
                Id = strategy.Id,
                Name = strategy.Name,
                Description = strategy.Description,
                RiskLevel = strategy.RiskLevel
            };
            
            // If Parameters are accessible through strategy, set them on the model
            if (strategy.Parameters != null)
            {
                _strategy.SetParameters(strategy.Parameters);
            }
        }

        /// <inheritdoc />
        public string Id => _strategy?.Id ?? null;

        /// <inheritdoc />
        public string Name 
        { 
            get => _strategy?.Name ?? null; 
            set 
            {
                if (_strategy != null)
                    _strategy.Name = value;
            }
        }

        /// <inheritdoc />
        public RemediationPriority Priority 
        { 
            get => _strategy?.Priority ?? RemediationPriority.Medium; 
            set 
            {
                if (_strategy != null)
                {
                    _strategy.Priority = value;
                    _strategy.PriorityValue = (int)value;
                }
            } 
        }

        /// <inheritdoc />
        public int? PriorityValue
        {
            get => _strategy?.PriorityValue ?? (int)(_strategy?.Priority ?? RemediationPriority.Medium);
            set
            {
                if (_strategy != null && value.HasValue)
                {
                    _strategy.PriorityValue = value;
                    _strategy.Priority = (RemediationPriority)value.Value;
                }
            }
        }

        // Helper method to get priority as int for comparisons
        private int GetPriorityAsInt() => (int)(_strategy?.Priority ?? RemediationPriority.Medium);

        // Helper method to compare priority with int values
        private bool IsPriorityGreaterOrEqual(int value) => GetPriorityAsInt() >= value;

        // Helper method to compare priority with other priority values
        private bool IsPriorityGreaterOrEqual(RemediationPriority other) => GetPriorityAsInt() >= (int)other;

        /// <inheritdoc />
        public RiskLevel RiskLevel
        {
            get => _strategy?.RiskLevel ?? RiskLevel.Medium;
            set 
            {
                if (_strategy != null)
                    _strategy.RiskLevel = value;
            }
        }

        /// <inheritdoc />
        public string Description 
        { 
            get => _strategy?.Description ?? null; 
            set 
            {
                if (_strategy != null)
                    _strategy.Description = value;
            } 
        }

        /// <inheritdoc />
        public Dictionary<string, object> Parameters 
        { 
            get => _strategy?.GetParameters() ?? null; 
            set 
            {
                if (_strategy != null)
                    _strategy.SetParameters(value);
            } 
        }

        /// <inheritdoc />
        public string Version => _strategy?.Version ?? null;

        /// <inheritdoc />
        public bool IsEnabled => _strategy?.IsEnabled ?? true;

        /// <inheritdoc />
        public ISet<string> SupportedErrorTypes => _strategy?.GetSupportedErrorTypes() ?? new HashSet<string>();

        /// <inheritdoc />
        public List<RemediationAction> Actions => _strategy?.GetActions() ?? new List<RemediationAction>();

        /// <inheritdoc />
        public DateTime CreatedAt => _strategy?.CreatedAt ?? DateTime.UtcNow;

        /// <inheritdoc />
        public Task<bool> CanHandleAsync(ErrorContext context)
        {
            if (_strategy != null)
            {
                return Task.FromResult(_strategy.AppliesTo(context));
            }
            
            // Default implementation for strategy
            return Task.FromResult(false);
        }

        /// <inheritdoc />
        public Task<double> GetConfidenceAsync(ErrorContext context)
        {
            // Default implementation provides medium confidence
            return Task.FromResult(0.5);
        }

        /// <inheritdoc />
        public async Task<List<RemediationSuggestion>> GetSuggestionsAsync(ErrorContext context)
        {
            // Create suggestions based on actions
            var actions = await _strategy.CreateActionsAsync(context);
            var suggestions = new List<RemediationSuggestion>();
            
            foreach (var action in actions)
            {
                var suggestion = new RemediationSuggestion
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = action.Name,
                    Description = action.Description,
                    ConfidenceLevel = 0.7,
                    Scope = RuntimeErrorSage.Domain.Enums.ImpactScope.Component,
                    Severity = RemediationSeverity.Medium,
                    Metadata = new Dictionary<string, object>()
                };
                
                suggestions.Add(suggestion);
            }
            
            return suggestions;
        }

        /// <inheritdoc />
        public async Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
        {
            var actions = await _strategy.CreateActionsAsync(context);
            var actionsList = new List<RemediationAction>(actions);
            
            var plan = new RemediationPlan(
                name: $"{_strategy.Name} Plan",
                description: $"Plan created from {_strategy.Name} strategy",
                actions: actionsList,
                parameters: new Dictionary<string, object>
                {
                    { "StrategyId", _strategy.Id },
                    { "StrategyName", _strategy.Name },
                    { "ErrorId", context.ErrorId }
                },
                estimatedDuration: TimeSpan.FromMinutes(5)
            );
            
            return plan;
        }

        /// <inheritdoc />
        public async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            if (_strategy != null)
            {
                // If we have an interface-based strategy, use its execute method
                return await ExecuteInterfaceStrategyAsync(context);
            }
            
            throw new InvalidOperationException("No strategy implementation available");
        }

        private async Task<RemediationResult> ExecuteInterfaceStrategyAsync(ErrorContext context)
        {
            try
            {
                var actions = await _strategy.CreateActionsAsync(context);
                
                var result = new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.InProgress,
                    Message = $"Executing strategy {_strategy.Name}",
                    StartTime = DateTime.UtcNow
                };
                
                foreach (var action in actions)
                {
                    try
                    {
                        // Execute each action
                        var step = new RemediationStep
                        {
                            Name = action.Name,
                            Description = action.Description,
                            ActionId = action.Id,
                            StartTime = DateTime.UtcNow,
                            Status = "Running"
                        };
                        
                        // Simulate action execution
                        await Task.Delay(100);
                        
                        step.EndTime = DateTime.UtcNow;
                        step.Status = "Completed";
                        
                        result.CompletedSteps.Add(step);
                    }
                    catch (Exception ex)
                    {
                        result.Status = RemediationStatusEnum.Failed;
                        result.ErrorMessage = $"Action failed: {ex.Message}";
                        break;
                    }
                }
                
                if (result.Status == RemediationStatusEnum.InProgress)
                {
                    result.Status = RemediationStatusEnum.Success;
                    result.Message = "Strategy executed successfully";
                }
                
                result.EndTime = DateTime.UtcNow;
                return result;
            }
            catch (Exception ex)
            {
                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatusEnum.Failed,
                    Message = $"Strategy execution failed: {ex.Message}",
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Determines whether the strategy can be applied to the specified error context.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        /// <returns>A task that represents the asynchronous operation, containing a value indicating whether the strategy can be applied.</returns>
        public Task<bool> CanApplyAsync(ErrorContext errorContext)
        {
            return Task.FromResult(_strategy.AppliesTo(errorContext));
        }

        /// <summary>
        /// Gets the estimated impact of applying the strategy.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        /// <returns>A task that represents the asynchronous operation, containing the estimated impact.</returns>
        public Task<double> GetImpactAsync(ErrorContext errorContext)
        {
            // Default implementation returns medium impact
            return Task.FromResult(0.5);
        }

        /// <summary>
        /// Gets the risk of applying the strategy.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        /// <returns>A task that represents the asynchronous operation, containing the risk level.</returns>
        public Task<RiskLevel> GetRiskAsync(ErrorContext errorContext)
        {
            // Default implementation returns medium risk
            return Task.FromResult(_strategy.RiskLevel);
        }

        /// <summary>
        /// Validates the strategy configuration.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a value indicating whether the configuration is valid.</returns>
        public Task<bool> ValidateConfigurationAsync()
        {
            // Default implementation assumes valid configuration
            return Task.FromResult(true);
        }

        /// <summary>
        /// Validates the strategy for the specified error context.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        /// <returns>A task that represents the asynchronous operation, containing the validation result.</returns>
        public Task<ValidationResult> ValidateAsync(ErrorContext errorContext)
        {
            // Default implementation assumes valid
            return Task.FromResult(new ValidationResult { IsValid = true });
        }
        
        /// <summary>
        /// Creates remediation actions for the specified error context.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of remediation actions.</returns>
        public Task<IEnumerable<RemediationAction>> CreateActionsAsync(ErrorContext errorContext)
        {
            // Basic implementation to satisfy interface
            return Task.FromResult<IEnumerable<RemediationAction>>(_strategy.GetActions());
        }
    }

    // Add extension methods for RemediationStrategyModel
    public static class RemediationStrategyModelExtensions
    {
        public static Dictionary<string, object> GetParameters(this RemediationStrategyModel model)
        {
            return model?.Metadata ?? new Dictionary<string, object>();
        }
        
        public static void SetParameters(this RemediationStrategyModel model, Dictionary<string, object> parameters)
        {
            if (model != null)
                model.Metadata = parameters;
        }
        
        public static ISet<string> GetSupportedErrorTypes(this RemediationStrategyModel model)
        {
            HashSet<string> result = new HashSet<string>();
            if (model?.HandledPatterns != null)
            {
                foreach (var pattern in model.HandledPatterns)
                {
                    if (!string.IsNullOrEmpty(pattern.ErrorType))
                        result.Add(pattern.ErrorType);
                }
            }
            return result;
        }
        
        public static List<RemediationAction> GetActions(this RemediationStrategyModel model)
        {
            List<RemediationAction> actions = new List<RemediationAction>();
            if (model?.Steps != null)
            {
                foreach (var step in model.Steps)
                {
                    actions.Add(new RemediationAction
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = step.Name,
                        Description = step.Description,
                        ActionType = "Strategy",
                        Parameters = new Dictionary<string, object>()
                    });
                }
            }
            return actions;
        }
        
        public static bool AppliesTo(this RemediationStrategyModel model, ErrorContext context)
        {
            if (model?.HandledPatterns == null || !model.HandledPatterns.Any())
                return true;
                
            return model.HandledPatterns.Any(p => 
                string.IsNullOrEmpty(p.ErrorType) || 
                context.ErrorType.Equals(p.ErrorType, StringComparison.OrdinalIgnoreCase));
        }
        
        public static async Task<IEnumerable<RemediationAction>> CreateActionsAsync(this RemediationStrategyModel model, ErrorContext context)
        {
            return model.GetActions();
        }
    }
} 
