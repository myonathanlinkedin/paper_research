using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RuntimeErrorSage.Domain.Interfaces;

namespace RuntimeErrorSage.Core.Remediation.Base;

/// <summary>
/// Abstract base class for remediation strategies.
/// </summary>
public abstract class RemediationStrategy : RuntimeErrorSage.Application.Interfaces.IRemediationStrategy
{
    protected readonly ILogger<RemediationStrategy> Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemediationStrategy"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    protected RemediationStrategy(ILogger<RemediationStrategy> logger)
    {
        Logger = logger;
        Id = Guid.NewGuid().ToString();
        Parameters = new Dictionary<string, object>();
        SupportedErrorTypes = new HashSet<string>();
        Actions = new List<RemediationAction>();
        CreatedAt = DateTime.UtcNow;
        Status = RemediationStatusEnum.NotStarted;
        Priority = RemediationPriority.Medium;
    }

    /// <inheritdoc/>
    public string Id { get; set; }

    /// <inheritdoc/>
    public string StrategyId { get; }

    /// <inheritdoc/>
    public abstract string Name { get; set; }

    /// <inheritdoc/>
    public RemediationPriority Priority { get; set; } = RemediationPriority.Medium;

    /// <inheritdoc/>
    public int? PriorityValue 
    { 
        get => (int)Priority; 
        set => Priority = value.HasValue ? (RemediationPriority)value.Value : RemediationPriority.Medium; 
    }

    /// <inheritdoc/>
    public RiskLevel RiskLevel { get; set; } = RiskLevel.Medium;

    /// <inheritdoc/>
    public abstract string Description { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, object> Parameters { get; set; } = new();

    /// <inheritdoc/>
    public ISet<string> SupportedErrorTypes { get; protected set; }

    /// <inheritdoc/>
    public List<RemediationAction> Actions { get; protected set; }

    /// <inheritdoc/>
    public DateTime CreatedAt { get; protected set; }

    /// <inheritdoc/>
    public RemediationStatusEnum Status { get; set; }

    /// <inheritdoc/>
    public virtual string Version { get; } = "1.0.0";

    /// <inheritdoc/>
    public virtual bool IsEnabled { get; } = true;

    /// <inheritdoc/>
    public virtual async Task<RemediationResult> ExecuteAsync(ErrorContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            Logger.LogInformation("Executing remediation strategy {StrategyName} for error type {ErrorType}", 
                Name, context.ErrorType);

            var result = new RemediationResult
            {
                StartTime = DateTime.UtcNow,
                CorrelationId = context.CorrelationId,
                ErrorId = context.ErrorId,
                ErrorType = context.ErrorType,
                StrategyId = StrategyId,
                StrategyName = Name
            };

            // Validate context
            if (!await CanHandleErrorAsync(context))
            {
                result.Success = false;
                result.ErrorMessage = "Strategy cannot handle this error type";
                result.EndTime = DateTime.UtcNow;
                return result;
            }

            // Execute actions
            foreach (var action in Actions.OrderBy(a => a.Priority))
            {
                try
                {
                    var actionResult = await action.ExecuteAsync();
                    // Create a RemediationActionResult from the actionResult and add it to the list
                    if (result.Actions == null)
                    {
                        result.Actions = new List<RemediationActionResult>();
                    }
                    
                    // Add a simple representation of the action result
                    result.Actions.Add(new RemediationActionResult
                    {
                        ActionId = action.Id,
                        Name = action.Name,
                        Success = actionResult.IsSuccessful,
                        Message = actionResult.Message,
                        StartTime = actionResult.StartTime != null && actionResult.GetType().GetProperty("StartTime")?.PropertyType == typeof(DateTime?) 
                            ? ((DateTime?)actionResult.StartTime).Value 
                            : (DateTime)actionResult.StartTime,
                        EndTime = actionResult.EndTime != null && actionResult.GetType().GetProperty("EndTime")?.PropertyType == typeof(DateTime?) 
                            ? ((DateTime?)actionResult.EndTime).Value 
                            : (DateTime)(actionResult.EndTime ?? DateTime.UtcNow)
                    });

                    if (!actionResult.IsSuccessful)
                    {
                        result.Success = false;
                        result.ErrorMessage = $"Action {action.Name} failed: {actionResult.Message}";
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error executing action {ActionName}", action.Name);
                    result.Success = false;
                    result.ErrorMessage = $"Action {action.Name} failed: {ex.Message}";
                    break;
                }
            }

            result.EndTime = DateTime.UtcNow;
            result.Success = result.Actions.All(a => a.Success);
            result.Status = result.Success ? RemediationStatusEnum.Success : RemediationStatusEnum.Failed;

            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing strategy {StrategyName}", Name);
            throw;
        }
    }

    /// <summary>
    /// Executes this strategy with optional parameters.
    /// </summary>
    public virtual async Task<RemediationResult> ExecuteAsync(ErrorContext context, object parameters)
    {
        // Store parameters if provided
        if (parameters != null)
        {
            if (parameters is Dictionary<string, object> paramsDict)
            {
                foreach (var param in paramsDict)
                {
                    Parameters[param.Key] = param.Value;
                }
            }
        }
        
        // Call the main execution method
        return await ExecuteAsync(context);
    }

    /// <inheritdoc/>
    public virtual Task<bool> CanApplyAsync(ErrorContext context)
    {
        return Task.FromResult(CanHandleErrorAsync(context).Result);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> CanHandleErrorAsync(ErrorContext context)
    {
        if (context == null)
        {
            return false;
        }

        try
        {
            // Check if the error type is supported
            if (!SupportedErrorTypes.Contains(context.ErrorType))
            {
                return false;
            }

            // Validate that all required parameters are present
            foreach (var param in Parameters)
            {
                if (param.Value == null)
                {
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error checking if strategy can handle error type {ErrorType}", context.ErrorType);
            return false;
        }
    }

    /// <inheritdoc/>
    public virtual Task<double> GetSuccessProbabilityAsync(ErrorContext context)
    {
        // Default implementation returns a medium probability
        return Task.FromResult(0.5);
    }

    /// <inheritdoc/>
    public virtual Task<RemediationImpact> GetImpactAsync(ErrorContext context)
    {
        // Default implementation returns a medium impact
        return Task.FromResult(new RemediationImpact
        {
            Severity = RemediationActionSeverity.Medium,
            Description = "Default impact assessment"
        });
    }

    /// <inheritdoc/>
    public virtual Task<RemediationPriority> GetPriorityAsync()
    {
        return Task.FromResult(Priority);
    }

    /// <inheritdoc/>
    public virtual Task<RiskAssessmentModel> GetRiskAsync(ErrorContext context)
    {
        // Default implementation returns a medium risk
        return Task.FromResult(new RiskAssessmentModel
        {
            Level = RiskLevel.Medium,
            Description = "Default risk assessment",
            Factors = new Dictionary<string, object>(),
            Timestamp = DateTime.UtcNow
        });
    }

    /// <inheritdoc/>
    public virtual Task<RiskLevel> GetRiskLevelAsync(ErrorContext context)
    {
        // Default implementation returns a medium risk level
        return Task.FromResult(RiskLevel.Medium);
    }

    /// <inheritdoc/>
    public virtual Task<TimeSpan> GetEstimatedDurationAsync(ErrorContext context)
    {
        // Default implementation returns a 5-minute duration
        return Task.FromResult(TimeSpan.FromMinutes(5));
    }

    /// <inheritdoc/>
    public virtual Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
    {
        // Default implementation creates an empty plan
        return Task.FromResult(new RemediationPlan(
            Name,
            $"Default plan for {Name} strategy",
            new List<RemediationAction>(),
            new Dictionary<string, object>(),
            TimeSpan.FromMinutes(5))
        {
            Context = context
        });
    }

    /// <inheritdoc/>
    public virtual Task<ValidationResult> ValidateAsync(ErrorContext context)
    {
        // Default implementation returns a success validation result
        return Task.FromResult(new ValidationResult
        {
            IsValid = true,
            StrategyId = StrategyId,
            StrategyName = Name
        });
    }

    /// <inheritdoc/>
    public virtual Task<bool> ValidateConfigurationAsync()
    {
        // Default implementation returns true
        return Task.FromResult(true);
    }

    /// <inheritdoc/>
    public virtual Task<RemediationPriority> GetPriorityAsync(ErrorContext context)
    {
        // Default implementation returns a medium priority
        return Task.FromResult(RemediationPriority.Medium);
    }

    /// <inheritdoc/>
    public virtual Task<bool> CanHandleAsync(ErrorContext context)
    {
        return CanHandleErrorAsync(context);
    }

    /// <inheritdoc/>
    public virtual Task<double> GetConfidenceAsync(ErrorContext context)
    {
        // Default implementation returns a medium confidence
        return Task.FromResult(0.6);
    }

    /// <inheritdoc/>
    public virtual Task<List<RemediationSuggestion>> GetSuggestionsAsync(ErrorContext context)
    {
        // Default implementation returns an empty list of suggestions
        return Task.FromResult(new List<RemediationSuggestion>());
    }

    protected RemediationAction CreateAction(string name, string description, RemediationActionType type)
    {
        return new RemediationAction
        {
            Name = name,
            Description = description,
            Type = type.ToString(),
            Severity = SeverityLevel.Medium.ToRemediationActionSeverity(),
            Status = RemediationStatusEnum.Pending
        };
    }
}

// Extension methods for RemediationStrategy
public static class RemediationStrategyExtensions
{
    /// <summary>
    /// Converts a RemediationStrategy to a Domain IRemediationStrategy
    /// </summary>
    public static Domain.Interfaces.IRemediationStrategy ToDomainStrategy(this RemediationStrategy strategy)
    {
        if (strategy == null)
            return null;

        return new DomainStrategyWrapper(strategy);
    }

    /// <summary>
    /// Wrapper class that adapts a RemediationStrategy to a Domain IRemediationStrategy
    /// </summary>
    private class DomainStrategyWrapper : Domain.Interfaces.IRemediationStrategy
    {
        private readonly RemediationStrategy _strategy;

        public DomainStrategyWrapper(RemediationStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public string Id => _strategy.Id;
        
        public string Name { get => _strategy.Name; set => _strategy.Name = value; }
        
        public string Description { get => _strategy.Description; set => _strategy.Description = value; }
        
        public string Version => _strategy.Version;
        
        public bool IsEnabled => _strategy.IsEnabled;
        
        public RemediationPriority Priority { get => _strategy.Priority; set => _strategy.Priority = value; }
        
        public int? PriorityValue { get => _strategy.PriorityValue; set => _strategy.PriorityValue = value; }
        
        public RiskLevel RiskLevel { get => _strategy.RiskLevel; set => _strategy.RiskLevel = value; }
        
        public Dictionary<string, object> Parameters { get => _strategy.Parameters; set => _strategy.Parameters = value; }
        
        public ISet<string> SupportedErrorTypes => _strategy.SupportedErrorTypes;
        
        public List<RemediationAction> Actions => _strategy.Actions;
        
        public DateTime CreatedAt => _strategy.CreatedAt;
        
        public RemediationStatusEnum Status { get => _strategy.Status; set => _strategy.Status = value; }
        
        public Task<RemediationPriority> GetPriorityAsync()
        {
            return _strategy.GetPriorityAsync();
        }
        
        public bool AppliesTo(ErrorContext context)
        {
            return _strategy.CanHandleErrorAsync(context).GetAwaiter().GetResult();
        }
        
        public Task<IEnumerable<RemediationAction>> CreateActionsAsync(ErrorContext context)
        {
            return Task.FromResult<IEnumerable<RemediationAction>>(_strategy.Actions);
        }
    }
} 
