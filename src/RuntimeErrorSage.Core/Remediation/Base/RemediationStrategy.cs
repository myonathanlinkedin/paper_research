using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Remediation;
using RuntimeErrorSage.Model.Models.Validation;
using RuntimeErrorSage.Model.Models.Remediation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RuntimeErrorSage.Model.Remediation.Base;

/// <summary>
/// Base class for remediation strategies.
/// </summary>
public abstract class RemediationStrategy : Models.Remediation.Interfaces.IRemediationStrategy
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
    public RemediationPriority Priority { get; set; }

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
    public RemediationStatusEnum Status { get; protected set; }

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
                    var actionResult = await action.ExecuteAsync(context);
                    result.Actions.Add(actionResult);

                    if (!actionResult.IsSuccessful)
                    {
                        result.Success = false;
                        result.ErrorMessage = $"Action {action.Name} failed: {actionResult.ErrorMessage}";
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
            result.Success = result.Actions.All(a => a.IsSuccessful);
            result.Status = result.Success ? RemediationStatusEnum.Success : RemediationStatusEnum.Failed;

            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing strategy {StrategyName}", Name);
            throw;
        }
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
    public virtual Task<RiskAssessment> GetRiskAsync(ErrorContext context)
    {
        // Default implementation returns a medium risk
        return Task.FromResult(new RiskAssessment
        {
            Level = RiskLevel.Medium,
            Description = "Default risk assessment",
            Factors = new Dictionary<string, double>(),
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
        return Task.FromResult(new RemediationPlan
        {
            StrategyId = StrategyId,
            StrategyName = Name,
            ErrorContext = context,
            Steps = new List<RemediationStep>()
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

    protected RemediationAction CreateAction(string name, string description, RemediationActionType type)
    {
        return new RemediationAction
        {
            Name = name,
            Description = description,
            Type = type,
            Severity = SeverityLevel.Medium.ToRemediationActionSeverity(),
            Status = RemediationActionStatus.Pending
        };
    }
} 
