using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Execution;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Service for managing remediation operations.
    /// </summary>
    public class RemediationService : IRemediationService
    {
        private readonly ILogger<RemediationService> _logger;
        private readonly IErrorContextAnalyzer _errorContextAnalyzer;
        private readonly IRemediationRegistry _registry;
        private readonly IRemediationExecutor _executor;
        private readonly IRemediationValidator _validator;
        private readonly IRemediationMetricsCollector _metricsCollector;

        public RemediationService(
            ILogger<RemediationService> logger,
            IErrorContextAnalyzer errorContextAnalyzer,
            IRemediationRegistry registry,
            IRemediationExecutor executor,
            IRemediationValidator validator,
            IRemediationMetricsCollector metricsCollector)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _errorContextAnalyzer = errorContextAnalyzer ?? throw new ArgumentNullException(nameof(errorContextAnalyzer));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
        }

        /// <inheritdoc/>
        public bool IsEnabled => true;

        /// <inheritdoc/>
        public string Name => "RuntimeErrorSage Remediation Service";

        /// <inheritdoc/>
        public string Version => "1.0.0";

        public async Task<RemediationResult> ApplyRemediationAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                // Create remediation plan
                var plan = await CreatePlanAsync(context);

                // Validate plan
                var validationResult = await ValidatePlanAsync(plan);
                if (!validationResult)
                {
                    return new RemediationResult
                    {
                        Success = false,
                        ErrorMessage = "Plan validation failed",
                        Status = RemediationStatus.Failed
                    };
                }

                // Record initial metrics
                var metrics = new RemediationMetrics
                {
                    MetricsId = Guid.NewGuid().ToString(),
                    RemediationId = Guid.NewGuid().ToString(),
                    StartTime = DateTime.UtcNow,
                    Status = RemediationStatus.Analyzing,
                    ErrorType = context.ErrorType,
                    ServiceName = context.ServiceName
                };

                await _metricsCollector.RecordRemediationMetricsAsync(metrics);

                // Execute remediation
                metrics.Status = RemediationStatus.Executing;
                await _metricsCollector.RecordRemediationMetricsAsync(metrics);

                var execution = await _executor.ExecuteRemediationAsync(plan, context);

                // Record final metrics
                metrics = new RemediationMetrics
                {
                    MetricsId = Guid.NewGuid().ToString(),
                    RemediationId = execution.RemediationId,
                    EndTime = DateTime.UtcNow,
                    Status = execution.Status,
                    ErrorType = context.ErrorType,
                    ServiceName = context.ServiceName,
                    Success = execution.Success,
                    ErrorMessage = execution.ErrorMessage
                };

                await _metricsCollector.RecordRemediationMetricsAsync(metrics);

                return new RemediationResult
                {
                    RemediationId = execution.RemediationId,
                    Success = execution.Success,
                    ErrorMessage = execution.ErrorMessage,
                    Status = execution.Status,
                    Metrics = metrics
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying remediation for error {ErrorType}", context.ErrorType);
                return new RemediationResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Status = RemediationStatus.Failed
                };
            }
        }

        public async Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                // Analyze error
                var analysis = await _errorContextAnalyzer.AnalyzeErrorAsync(context);

                // Get applicable strategies
                var strategies = await _registry.GetStrategiesForErrorAsync(context);

                var statusInfo = new RemediationStatusInfo
                {
                    Status = RemediationStatus.Created,
                    Message = "Remediation plan created",
                    LastUpdated = DateTime.UtcNow
                };

                var rollbackPlan = new RemediationPlan
                {
                    PlanId = Guid.NewGuid().ToString(),
                    Context = context,
                    CreatedAt = DateTime.UtcNow,
                    Status = RemediationStatus.Created,
                    StatusInfo = statusInfo,
                    RollbackPlan = null
                };

                return new RemediationPlan
                {
                    PlanId = Guid.NewGuid().ToString(),
                    Analysis = analysis,
                    Context = context,
                    Strategies = strategies.ToList(),
                    CreatedAt = DateTime.UtcNow,
                    Status = RemediationStatus.Created,
                    StatusInfo = statusInfo,
                    RollbackPlan = rollbackPlan
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating remediation plan for error {ErrorType}", context.ErrorType);
                throw;
            }
        }

        public async Task<bool> ValidatePlanAsync(RemediationPlan plan)
        {
            ArgumentNullException.ThrowIfNull(plan);

            try
            {
                var validationResult = await _validator.ValidatePlanAsync(plan, plan.Context);
                return validationResult.IsValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating remediation plan {PlanId}", plan.PlanId);
                return false;
            }
        }

        public async Task<RemediationStatus> GetStatusAsync(string remediationId)
        {
            ArgumentNullException.ThrowIfNull(remediationId);

            try
            {
                return await _executor.GetRemediationStatusAsync(remediationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting remediation status for {RemediationId}", remediationId);
                return RemediationStatus.Unknown;
            }
        }

        public async Task<RemediationMetrics> GetMetricsAsync(string remediationId)
        {
            ArgumentNullException.ThrowIfNull(remediationId);

            try
            {
                var metrics = await _metricsCollector.GetMetricsHistoryAsync(remediationId);
                var values = metrics.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.FirstOrDefault()?.Value
                );

                bool success = false;
                if (values.TryGetValue("Success", out var successValue) && successValue is double doubleValue)
                {
                    success = doubleValue != 0;
                }

                return new RemediationMetrics
                {
                    MetricsId = Guid.NewGuid().ToString(),
                    RemediationId = remediationId,
                    StrategyName = values.GetValueOrDefault("StrategyName")?.ToString(),
                    ErrorType = values.GetValueOrDefault("ErrorType")?.ToString(),
                    ServiceName = values.GetValueOrDefault("ServiceName")?.ToString(),
                    Success = success,
                    ErrorMessage = values.GetValueOrDefault("ErrorMessage")?.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting remediation metrics for {RemediationId}", remediationId);
                throw;
            }
        }
    }
} 