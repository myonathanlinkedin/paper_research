using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    public interface IRemediationPlanManager
    {
        Task<RemediationPlan> CreatePlanAsync(ErrorContext context);
        Task<bool> ValidatePlanAsync(RemediationPlan plan);
        Task<RemediationPlan> UpdatePlanWithBuildResultAsync(RemediationPlan plan, string buildOutput, bool isSuccessful, string conclusion);
    }
} 
