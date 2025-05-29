using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    public interface IRemediationPlanManager
    {
        Task<RemediationPlan> CreatePlanAsync(ErrorContext context);
        Task<bool> ValidatePlanAsync(RemediationPlan plan);
        Task<RemediationPlan> UpdatePlanWithBuildResultAsync(RemediationPlan plan, string buildOutput, bool isSuccessful, string conclusion);
    }
} 






