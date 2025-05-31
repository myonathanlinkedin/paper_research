using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    public interface IRemediationActionManager
    {
        Task<RemediationResult> ExecuteActionAsync(RemediationAction action);
        Task<Domain.Models.Validation.ValidationResult> ValidateActionAsync(RemediationAction action);
        Task<RuntimeErrorSage.Domain.Enums.RollbackStatus> RollbackActionAsync(string actionId);
        Task<RemediationResult> GetActionStatusAsync(string actionId);
    }
} 


