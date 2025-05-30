using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    public interface IRemediationPlanManager
    {
        Task<RemediationPlan> CreatePlanAsync(ErrorContext context);
        Task<bool> ValidatePlanAsync(RemediationPlan plan);
        Task<RemediationPlan> UpdatePlanWithBuildResultAsync(RemediationPlan plan, string buildOutput, bool isSuccessful, string conclusion);
    }
} 
