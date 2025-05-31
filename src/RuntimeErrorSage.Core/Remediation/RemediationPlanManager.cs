using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Common;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Core.Remediation
{
    public class RemediationPlanManager : IRemediationPlanManager
    {
        private readonly ILogger<RemediationPlanManager> _logger;
        private readonly IErrorContextAnalyzer _errorContextAnalyzer;
        private readonly IRemediationRegistry _registry;
        private readonly IRemediationValidator _validator;

        public RemediationPlanManager(
            ILogger<RemediationPlanManager> logger,
            IErrorContextAnalyzer errorContextAnalyzer,
            IRemediationRegistry registry,
            IRemediationValidator validator)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(errorContextAnalyzer);
            ArgumentNullException.ThrowIfNull(registry);
            ArgumentNullException.ThrowIfNull(validator);

            _logger = logger;
            _errorContextAnalyzer = errorContextAnalyzer;
            _registry = registry;
            _validator = validator;
        }

        public async Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                var analysis = await _errorContextAnalyzer.AnalyzeContextAsync(context);
                var strategies = await _registry.GetStrategiesForErrorAsync(context);

                // Create a new ErrorAnalysisResult from the RemediationAnalysis
                var errorAnalysisResult = new Domain.Models.Error.ErrorAnalysisResult
                {
                    ErrorId = context.ErrorId,
                    CorrelationId = context.CorrelationId,
                    Timestamp = DateTime.UtcNow,
                    Status = Domain.Enums.AnalysisStatus.Completed,
                    ErrorType = context.ErrorType,
                    Context = context,
                    Message = "Analysis completed successfully"
                };

                var statusInfo = new Domain.Models.Common.RemediationStatusInfo
                {
                    Status = RemediationStatusEnum.Pending,
                    Message = "Remediation plan created",
                    LastUpdated = DateTime.UtcNow
                };

                var rollbackPlan = new RemediationPlan(
                    name: "Rollback Plan",
                    description: $"Rollback plan for error {context.ErrorId}",
                    actions: new List<RemediationAction>(),
                    parameters: new Dictionary<string, object>(),
                    estimatedDuration: TimeSpan.FromMinutes(5))
                {
                    PlanId = Guid.NewGuid().ToString(),
                    Context = context,
                    CreatedAt = DateTime.UtcNow,
                    Status = RemediationStatusEnum.Pending,
                    StatusInfo = statusInfo.Message,
                    RollbackPlan = null
                };

                // Convert application strategies to domain strategies
                var domainStrategies = strategies
                    .Select(s => new RuntimeErrorSage.Domain.Models.Remediation.RemediationStrategyModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description
                    })
                    .Cast<RuntimeErrorSage.Domain.Interfaces.IRemediationStrategy>()
                    .ToList();

                return new RemediationPlan(
                    name: $"Remediation for {context.ErrorType}",
                    description: $"Automated remediation plan for error {context.ErrorId}",
                    actions: new List<RemediationAction>(),
                    parameters: new Dictionary<string, object>(),
                    estimatedDuration: TimeSpan.FromMinutes(10))
                {
                    PlanId = Guid.NewGuid().ToString(),
                    Analysis = errorAnalysisResult,
                    Context = context,
                    Strategies = domainStrategies,
                    CreatedAt = DateTime.UtcNow,
                    Status = RemediationStatusEnum.Pending,
                    StatusInfo = statusInfo.Message,
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

        public async Task<RemediationPlan> UpdatePlanWithBuildResultAsync(RemediationPlan plan, string buildOutput, bool isSuccessful, string conclusion)
        {
            ArgumentNullException.ThrowIfNull(plan);
            ArgumentNullException.ThrowIfNull(buildOutput);
            ArgumentNullException.ThrowIfNull(conclusion);

            try
            {
                // Update plan status based on build success
                var status = isSuccessful ? RemediationStatusEnum.Success : RemediationStatusEnum.Failed;
                
                // Create new status info
                var statusInfo = new Domain.Models.Common.RemediationStatusInfo
                {
                    Status = status,
                    Message = isSuccessful ? "Remediation completed successfully" : "Remediation failed",
                    LastUpdated = DateTime.UtcNow,
                    Metadata = new Dictionary<string, object>
                    {
                        { "BuildOutput", buildOutput },
                        { "Conclusion", conclusion },
                        { "CompletedAt", DateTime.UtcNow }
                    }
                };

                // Add previous status to history if it exists
                if (plan.StatusInfo != null)
                {
                    statusInfo.History.Add(new Domain.Models.Common.StatusHistoryEntry
                    {
                        Status = plan.Status,
                        Message = plan.StatusInfo,
                        Timestamp = DateTime.UtcNow // Can't use LastUpdated since it's a string
                    });
                }

                // Update the plan with new status info
                plan.Status = status;
                plan.StatusInfo = statusInfo.Message;
                plan.Description = conclusion;

                _logger.LogInformation(
                    "Updated remediation plan {PlanId} with build result. Status: {Status}",
                    plan.PlanId,
                    status);

                return plan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating remediation plan {PlanId} with build result", plan.PlanId);
                throw;
            }
        }
    }
} 
