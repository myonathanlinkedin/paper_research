using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Domain.Models.Common;

namespace RuntimeErrorSage.Application.Remediation
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

                var statusInfo = new Models.Common.RemediationStatusInfo
                {
                    Status = RemediationStatusEnum.Created,
                    Message = "Remediation plan created",
                    LastUpdated = DateTime.UtcNow
                };

                var rollbackPlan = new RemediationPlan
                {
                    PlanId = Guid.NewGuid().ToString(),
                    Context = context,
                    CreatedAt = DateTime.UtcNow,
                    Status = RemediationStatusEnum.Created,
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
                    Status = RemediationStatusEnum.Created,
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
                var statusInfo = new RemediationStatusInfo
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
                    statusInfo.History.Add(new StatusHistoryEntry
                    {
                        Status = plan.Status,
                        Message = plan.StatusInfo.Message,
                        Timestamp = plan.StatusInfo.LastUpdated
                    });
                }

                // Update the plan with new status info
                plan.Status = status;
                plan.StatusInfo = statusInfo;
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
