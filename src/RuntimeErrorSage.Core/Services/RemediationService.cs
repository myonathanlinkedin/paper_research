using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Services.Interfaces;
using RuntimeErrorSage.Core.Interfaces;
using System.Linq;

namespace RuntimeErrorSage.Core.Services;

/// <summary>
/// Service for managing remediation operations.
/// </summary>
public class RemediationService : IRemediationService
{
    private readonly ILogger<RemediationService> _logger;
    private readonly IRemediationStrategySelector _strategySelector;
    private readonly IRemediationExecutor _executor;
    private readonly IRemediationValidator _validator;
    private readonly IRemediationMetricsCollector _metricsCollector;

    public RemediationService(
        ILogger<RemediationService> logger,
        IRemediationStrategySelector strategySelector,
        IRemediationExecutor executor,
        IRemediationValidator validator,
        IRemediationMetricsCollector metricsCollector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _strategySelector = strategySelector ?? throw new ArgumentNullException(nameof(strategySelector));
        _executor = executor ?? throw new ArgumentNullException(nameof(executor));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
    }

    /// <inheritdoc />
    public bool IsEnabled => true;

    /// <inheritdoc />
    public string Name => "RuntimeErrorSage Remediation Service";

    /// <inheritdoc />
    public string Version => "1.0.0";

    /// <inheritdoc />
    public async Task<RemediationResult> ApplyRemediationAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Starting remediation for error context {ErrorId}", context.Id);

            // Create remediation plan
            var plan = await CreatePlanAsync(context);
            if (plan.Status != RemediationStatus.Pending)
            {
                return new RemediationResult
                {
                    Context = context,
                    Status = plan.Status,
                    Message = "Failed to create remediation plan",
                    Validation = new ValidationResult { IsValid = false, Message = "Failed to create remediation plan" }
                };
            }

            // Validate plan
            var validationResult = await ValidatePlanAsync(plan);
            if (!validationResult.IsValid)
            {
                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatus.Failed,
                    Message = "Plan validation failed",
                    Validation = validationResult
                };
            }

            // Execute plan
            var result = await _executor.ExecuteStrategyAsync(plan.Strategies[0], context);
            if (result.Status != RemediationStatus.Completed)
            {
                return new RemediationResult
                {
                    Context = context,
                    Status = RemediationStatus.Failed,
                    Message = result.Message,
                    CompletedSteps = result.CompletedSteps,
                    FailedSteps = result.FailedSteps,
                    Metrics = result.Metrics,
                    Validation = validationResult
                };
            }

            return new RemediationResult
            {
                Context = context,
                Status = RemediationStatus.Completed,
                Message = "Remediation completed successfully",
                CompletedSteps = result.CompletedSteps,
                FailedSteps = result.FailedSteps,
                Metrics = result.Metrics,
                Validation = validationResult
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying remediation for context {ErrorId}", context.Id);
            return new RemediationResult
            {
                Context = context,
                Status = RemediationStatus.Failed,
                Message = $"Remediation failed: {ex.Message}",
                Validation = new ValidationResult { IsValid = false, Message = ex.Message }
            };
        }
    }

    /// <inheritdoc />
    public async Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            var strategy = await _strategySelector.SelectStrategyAsync(context);
            if (strategy == null)
            {
                return new RemediationPlan
                {
                    Context = context,
                    Status = RemediationStatus.Failed,
                    StatusInfo = "No suitable remediation strategy found",
                    RollbackPlan = new RollbackPlan { IsAvailable = false }
                };
            }

            return new RemediationPlan
            {
                Context = context,
                Strategies = new[] { strategy }.ToList(),
                Status = RemediationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                StatusInfo = "Plan created successfully",
                RollbackPlan = new RollbackPlan { IsAvailable = true }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating remediation plan for context {ErrorId}", context.Id);
            return new RemediationPlan
            {
                Context = context,
                Status = RemediationStatus.Failed,
                StatusInfo = $"Failed to create plan: {ex.Message}",
                RollbackPlan = new RollbackPlan { IsAvailable = false }
            };
        }
    }

    /// <inheritdoc />
    public async Task<ValidationResult> ValidatePlanAsync(RemediationPlan plan)
    {
        ArgumentNullException.ThrowIfNull(plan);

        try
        {
            var result = await _validator.ValidatePlanAsync(plan, plan.Context);
            return new ValidationResult { IsValid = result.IsValid, Message = result.Message };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating remediation plan");
            return new ValidationResult { IsValid = false, Message = ex.Message };
        }
    }

    /// <inheritdoc />
    public async Task<Dictionary<string, double>> GetMetricsAsync(string remediationId)
    {
        ArgumentNullException.ThrowIfNull(remediationId);

        try
        {
            var metrics = await _metricsCollector.GetMetricsAsync(remediationId);
            return metrics.Values;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting metrics for remediation {RemediationId}", remediationId);
            return new Dictionary<string, double>();
        }
    }

    /// <inheritdoc />
    public async Task<RemediationStatus> GetStatusAsync(string remediationId)
    {
        ArgumentNullException.ThrowIfNull(remediationId);

        try
        {
            var metrics = await _metricsCollector.GetMetricsAsync(remediationId);
            return metrics.Status;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting status for remediation {RemediationId}", remediationId);
            return RemediationStatus.Unknown;
        }
    }
} 