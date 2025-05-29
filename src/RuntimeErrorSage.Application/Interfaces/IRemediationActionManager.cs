using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Validation;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    public interface IRemediationActionManager
    {
        Task<RemediationResult> ExecuteActionAsync(RemediationAction action);
        Task<ValidationResult> ValidateActionAsync(RemediationAction action);
        Task<RollbackStatus> RollbackActionAsync(string actionId);
        Task<RemediationResult> GetActionStatusAsync(string actionId);
    }
} 


