using System.Threading.Tasks;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Remediation;

namespace RuntimeErrorSage.Model.Remediation.Interfaces
{
    public interface IRemediationPlanManager
    {
        Task<RemediationPlan> CreatePlanAsync(ErrorContext context);
        Task<bool> ValidatePlanAsync(RemediationPlan plan);
        Task<RemediationPlan> UpdatePlanWithBuildResultAsync(RemediationPlan plan, string buildOutput, bool isSuccessful, string conclusion);
    }
} 
